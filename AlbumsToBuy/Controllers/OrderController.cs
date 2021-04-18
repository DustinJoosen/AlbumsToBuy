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
		private PaymentService _paymentService;
		private OrderService _orderService;
		private AlbumOrderService _albumOrderService;
		private ShoppingListItemService _shoppingListItemService;

		public OrderController(UserService userService, PaymentService paymentService, 
			OrderService orderService, AlbumOrderService albumOrderService, ShoppingListItemService shoppingListItemService)
		{
			_userService = userService;
			_paymentService = paymentService;
			_orderService = orderService;
			_albumOrderService = albumOrderService;
			_shoppingListItemService = shoppingListItemService;
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

			return View(new OrderUserDto() { User = user , Order = order});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(OrderUserDto orderUser)
		{
			var order = orderUser.Order;
			order.OrderDate = DateTime.Now;

			var user = await _userService.GetById(Convert.ToInt32(User.Identity.Name));
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
