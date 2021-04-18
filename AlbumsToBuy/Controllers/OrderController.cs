using AlbumsToBuy.Dtos;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;
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
		private AddressService _addressService;
		private PaymentService _paymentService;
		private OrderService _orderService;
		private AlbumOrderService _albumOrderService;
		private ShoppingListItemService _shoppingListItemService;

		public OrderController(UserService userService, AddressService addressService, PaymentService paymentService, 
			OrderService orderService, AlbumOrderService albumOrderService, ShoppingListItemService shoppingListItemService)
		{
			_userService = userService;
			_addressService = addressService;
			_paymentService = paymentService;
			_orderService = orderService;
			_albumOrderService = albumOrderService;
			_shoppingListItemService = shoppingListItemService;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _userService.GetByToken(User.Identity.Name);
			if(user == null)
			{
				return Unauthorized();
			}

			ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City", user.HomeAddressId);
			return View(new OrderUserDto() { User = user });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(OrderUserDto orderUser)
		{
			var order = orderUser.Order;
			order.OrderDate = DateTime.Now;

			var user = await _userService.GetByToken(User.Identity.Name);
			if (user == null)
			{
				return Unauthorized();
			}

			decimal price = 0;
			foreach(var shoppingListItem in user.ShoppingListItems)
			{
				price += (shoppingListItem.Album.Price * shoppingListItem.Quantity);
			}

			var payment = new Payment() {
				UserId = user.Id,
				Status = PaymentStatus.NotPayed,
				Amount = price
			};

			await _paymentService.Create(payment);

			order.UserId = user.Id;
			order.PaymentId = payment.Id;

			await _orderService.Create(order);

			foreach(var shoppingListItem in user.ShoppingListItems)
			{
				await _albumOrderService.Create(new AlbumOrder()
				{
					AlbumId = shoppingListItem.AlbumId,
					OrderId = order.Id,
					Quantity = shoppingListItem.Quantity
				});
			}

			await _shoppingListItemService.RemoveFromUser(user.Id);

			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
