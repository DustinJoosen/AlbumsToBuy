using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;
using AlbumsToBuy.Helpers;

namespace AlbumsToBuy.Controllers.Management
{
    [Authorize(Roles = "Admin")]
    [Route("Management/[controller]/{action=Index}/{id?}")]
    public class OrdersController : Controller
    {
        private OrderService _orderService;
        private PaymentService _paymentService;
        private UserService _userService;
        private AlbumService _albumService;
        private AlbumOrderService _albumOrderService;
        private INotyfService _notyf;

        public OrdersController(OrderService orderService,
            PaymentService paymentService, UserService userService, AlbumService albumService, 
            AlbumOrderService albumOrderService, INotyfService notyf)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _userService = userService;
            _albumService = albumService;
            _albumOrderService = albumOrderService;
            _notyf = notyf;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAll();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetById((int)id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = new SelectList(await _userService.GetAll(), "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            order.Payment.Status = PaymentStatus.NotPayed;
            order.Payment.UserId = order.UserId;

            await _paymentService.Create(order.Payment);

            order.PaymentId = order.Payment.Id;
            order.OrderDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                await _orderService.Create(order);

                _notyf.Information($"Order {order.Id} succsessfully created");
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(await _userService.GetAll(), "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetById((int)id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(await _userService.GetAll(), "Id", "Email");
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.Update(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_orderService.Exists(order))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyf.Information($"Order {order.Id} succsessfully updated");
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(await _userService.GetAll(), "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetById((int)id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderService.GetById(id);
            if(order == null)
			{
                return NotFound();
			}

            await _orderService.Remove(order);
            await _paymentService.Remove(order.Payment);

            _notyf.Information($"Order {order.Id} succsessfully deleted");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Albums(int id)
		{
            var order = await _orderService.GetById(id);
            if(order == null)
			{
                return NotFound();
			}

            ViewData["AlbumId"] = new SelectList(await _albumService.GetAll(), "Id", "Name");
            return View(order);
		}

        [HttpPost]
        public async Task<IActionResult> AddAlbumToOrder(int albumId, int orderId)
		{
            var order = await _orderService.GetById(orderId);
            if(order == null)
			{
                return NotFound();
			}

            await _albumOrderService.Create(new AlbumOrder()
            {
                OrderId = orderId,
                AlbumId = albumId,
                Quantity = 1
            });

            return RedirectToAction(nameof(Albums), new { id = orderId });
		}


        [HttpPost]
        public async Task<IActionResult> RemoveFromOrder(int albumOrderId)
        {
            var order = await _albumOrderService.GetById(albumOrderId);
            if (order == null)
            {
                return NotFound();
            }

            await _albumOrderService.Remove(order);

            return RedirectToAction(nameof(Albums), new { id = order.OrderId });
        }

        [HttpPost]
        public async Task<IActionResult> IncrementQuantity(int albumOrderId)
        {
            var order = await _albumOrderService.GetById(albumOrderId);
            if (order == null)
            {
                return NotFound();
            }

            order.Quantity++;

            await _albumOrderService.Update(order);
            return RedirectToAction(nameof(Albums), new { id = order.OrderId });
        }

        [HttpPost]
        public async Task<IActionResult> DecrementQuantity(int albumOrderId)
        {
            var order = await _albumOrderService.GetById(albumOrderId);
            if (order == null)
            {
                return NotFound();
            }

            order.Quantity--;

            if (order.Quantity > 0)
            {
                await _albumOrderService.Update(order);
            }
            else
            {
                await _albumOrderService.Remove(order);
            }

            return RedirectToAction(nameof(Albums), new { id = order.OrderId });
        }
    }
}