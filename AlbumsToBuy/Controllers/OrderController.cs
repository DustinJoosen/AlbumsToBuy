using AlbumsToBuy.Dtos;
using AlbumsToBuy.Helpers;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Controllers
{
	[Authorize(Roles = "Customer")]
	public class OrderController : Controller
	{
		private UserService _userService;
		private PaymentService _paymentService;
		private OrderService _orderService;
		private AlbumOrderService _albumOrderService;
		private ShoppingListItemService _shoppingListItemService;
		private INotyfService _notyf;


		public OrderController(UserService userService, PaymentService paymentService, OrderService orderService, 
			AlbumOrderService albumOrderService, ShoppingListItemService shoppingListItemService, INotyfService notyfService)
		{
			_userService = userService;
			_paymentService = paymentService;
			_orderService = orderService;
			_albumOrderService = albumOrderService;
			_shoppingListItemService = shoppingListItemService;
			_notyf = notyfService;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _userService.GetById(Convert.ToInt32(User.Identity.Name));
			if (user == null)
			{
				return Unauthorized();
			}

			var order = new Order()
			{
				Country = user.Country,
				Street = user.Street,
				City = user.City,
				ZipCode = user.ZipCode
			};

			return View(new OrderUserDto() 
			{ 
				User = user , 
				Order = order, 
				TotalPrice = PriceCalculator.CalculateShoppingList(user.ShoppingListItems) 
			});
		}

		//creates an order
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(OrderUserDto orderUser)
		{
			//sets the order and give it an orderdate
			var order = orderUser.Order;
			order.OrderDate = DateTime.Now;

			//retrieves the currently logged in user
			var user = await _userService.GetById(Convert.ToInt32(User.Identity.Name));
			if (user == null)
			{
				return Unauthorized();
			}

			//creates the payment that will be stored on the order
			var payment = new Payment() {
				UserId = user.Id,
				Status = PaymentStatus.NotPayed,
				Amount = PriceCalculator.CalculateShoppingList(user.ShoppingListItems)
			};
			await _paymentService.Create(payment);

			//set the user and the payment to the order, and save the order to the database
			order.UserId = user.Id;
			order.PaymentId = payment.Id;

			await _orderService.Create(order);

			//loop through all the items on the shoppingList, and add it to the order
			foreach(var shoppingListItem in user.ShoppingListItems)
			{
				await _albumOrderService.Create(new AlbumOrder()
				{
					AlbumId = shoppingListItem.AlbumId,
					OrderId = order.Id,
					Quantity = shoppingListItem.Quantity
				});
			}

			//empty out the shoppingList and go back to the homepage
			await _shoppingListItemService.RemoveFromUser(user.Id);
			_notyf.Information("Your order has been send");
			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
