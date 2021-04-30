using AlbumsToBuy.Dtos;
using AlbumsToBuy.Helpers;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlbumsToBuy.Controllers
{
	[Authorize(Roles = "Admin, Customer")]
	[Route("[controller]/{action=ShoppingCart}/{id?}")]
	public class CheckoutController : Controller
	{
		private UserService _userService;
		private PaymentService _paymentService;
		private OrderService _orderService;
		private AlbumService _albumService;
		private AlbumOrderService _albumOrderService;
		private ShoppingListItemService _shoppingListItemService;
		private INotyfService _notyf;
		
		public CheckoutController(UserService userService, PaymentService paymentService, OrderService orderService, AlbumService albumService,
			AlbumOrderService albumOrderService, ShoppingListItemService shoppingListItemService, INotyfService notyfService)
		{
			_userService = userService;
			_paymentService = paymentService;
			_orderService = orderService;
			_albumService = albumService;
			_albumOrderService = albumOrderService;
			_shoppingListItemService = shoppingListItemService;
			_notyf = notyfService;
		}
		public async Task<IActionResult> ShoppingCart()
		{
			var user = await _userService.GetById(User.Identity.Name);
			if (user == null)
			{
				return Unauthorized();
			}

			return View(user);
		}


		[HttpPost]
		public async Task<IActionResult> RemoveFromShoppingList(int id)
		{
			var shoppingListItem = await _shoppingListItemService.GetById(id);
			if (shoppingListItem == null)
			{
				return NotFound();
			}

			await _shoppingListItemService.Remove(shoppingListItem);
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> IncrementQuantity(int id)
		{
			var shoppingListItem = await _shoppingListItemService.GetById(id);
			if (shoppingListItem == null)
			{
				return NotFound();
			}

			if (shoppingListItem.Quantity < shoppingListItem.Album.Stock)
			{
				shoppingListItem.Quantity++;
			}
			else
			{
				ModelState.AddModelError("Input", "This is the amount of stock this album currently has.");
				return RedirectToAction(nameof(Index));
			}

			await _shoppingListItemService.Update(shoppingListItem);
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> DecrementQuantity(int id)
		{
			var shoppingListItem = await _shoppingListItemService.GetById(id);
			if (shoppingListItem == null)
			{
				return NotFound();
			}

			shoppingListItem.Quantity--;

			if (shoppingListItem.Quantity > 0)
			{
				await _shoppingListItemService.Update(shoppingListItem);
			}
			else
			{
				await _shoppingListItemService.Remove(shoppingListItem);
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Address()
		{
			var user = await _userService.GetById(User.Identity.Name);
			if (user == null)
			{
				return Unauthorized();
			}

			if (user.ShoppingListItems.Count <= 0)
			{
				return RedirectToAction(nameof(ShoppingCart));
			}
			

			return View(new AddressDto()
			{
				Country = user.Country,
				Street = user.Street,
				City = user.City,
				ZipCode = user.ZipCode
			});
		}

		[HttpPost]
		public IActionResult Address([Bind("Street", "City", "ZipCode", "Country")] AddressDto address)
		{
			TempData["address"] = JsonSerializer.Serialize(address);
			return RedirectToAction(nameof(Confirm));
		}

		public async Task<IActionResult> Confirm()
		{
			//get the serialized address
			var serialized_address = TempData["address"];
			if(serialized_address == null)
			{
				return RedirectToAction(nameof(Address));
			}

			//deserialize the address into an AddressDto object
			var address = JsonSerializer.Deserialize<AddressDto>(serialized_address.ToString());
			if(address == null)
			{
				return RedirectToAction(nameof(Address));
			}

			//retrieve the currently logged in user
			var user = await _userService.GetById(User.Identity.Name);
			if (user == null)
			{
				return Unauthorized();
			}

			return View(new Order()
			{
				Street = address.Street,
				City = address.City,
				ZipCode = address.ZipCode,
				Country = address.Country,
				User = user,
				Payment = new Payment() { Amount = PriceCalculator.CalculateShoppingList(user.ShoppingListItems) }
			});
		}

		public async Task<IActionResult> Order(Order order)
		{
			//retrieves the currently logged in user
			var user = await _userService.GetById(User.Identity.Name);
			if (user == null)
			{
				return Unauthorized();
			}

			//creates the payment that will be stored on the order
			var payment = new Payment()
			{
				UserId = user.Id,
				Status = PaymentStatus.NotPayed,
				Amount = PriceCalculator.CalculateShoppingList(user.ShoppingListItems)
			};
			await _paymentService.Create(payment);

			//set the user and the payment to the order, and save the order to the database
			order.UserId = user.Id;
			order.PaymentId = payment.Id;
			order.OrderDate = DateTime.Now;

			await _orderService.Create(order);

			//loop through all the items on the shoppingList, and add it to the order + handle the stock
			foreach (var item in user.ShoppingListItems)
			{
				//if there are more items in the quantity, then there is stock, even it out
				if (item.Album.Stock < item.Quantity)
				{
					_notyf.Information($"There were more items of album {item.Album.Name} then there was stock of it, Your order only includes {item.Album.Stock} items", 20);
					item.Quantity = item.Album.Stock;
				}

				await _albumOrderService.Create(new AlbumOrder()
				{
					AlbumId = item.AlbumId,
					OrderId = order.Id,
					Quantity = item.Quantity
				});

				//decrement the stock with the quantities of the album orders
				item.Album.Stock -= item.Quantity;
				await _albumService.Update(item.Album);
			}

			//empty out the shoppingList and go back to the homepage
			await _shoppingListItemService.RemoveFromUser(user.Id);

			_notyf.Information("Your order has been send. you will shortly recieve a confirmation email", 10);
			MailHelper.OrderConfirmation(order);

			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
