using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;
using AlbumsToBuy.ViewModels;

namespace AlbumsToBuy.Controllers.Management
{
    [Route("Management/[controller]/{action=Index}/{id?}")]
    public class OrdersController : Controller
    {
        private OrderService _orderService;
        private AddressService _addressService;
        private PaymentService _paymentService;
        private UserService _userService;
        private AlbumService _albumService;
        private AlbumOrderService _albumOrderService;

        public OrdersController(OrderService orderService, AddressService addressService, 
            PaymentService paymentService, UserService userService, AlbumService albumService, AlbumOrderService albumOrderService)
        {
            _orderService = orderService;
            _addressService = addressService;
            _paymentService = paymentService;
            _userService = userService;
            _albumService = albumService;
            _albumOrderService = albumOrderService;
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

            var albumOrders = await _albumService.GetByOrder(order);
            var vm = new OrderAlbums()
            {
                Order = order,
                AlbumOrders = albumOrders
            };

            return View(vm);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City");
            ViewData["PaymentId"] = new SelectList(await _paymentService.GetAll(), "Id", "Id");
            ViewData["UserId"] = new SelectList(await _userService.GetAll(), "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            order.OrderDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                await _orderService.Create(order);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City", order.AddressId);
            ViewData["PaymentId"] = new SelectList(await _paymentService.GetAll(), "Id", "Id", order.PaymentId);
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
            ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City");
            ViewData["PaymentId"] = new SelectList(await _paymentService.GetAll(), "Id", "Id");
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City", order.AddressId);
            ViewData["PaymentId"] = new SelectList(await _paymentService.GetAll(), "Id", "Id", order.PaymentId);
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
            var order = await _orderService.GetById((int)id);
            if(order == null)
			{
                return NotFound();
			}
            
            await _orderService.Remove(order);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Album(int id)
		{
            ViewData["AlbumId"] = new SelectList(await _albumService.GetAll(), "Id", "Name");
            return View();
		}

        [HttpPost]
        public async Task<IActionResult> Album(int id, AlbumOrder albumOrder)
		{ 
            albumOrder.OrderId = id;
            albumOrder.Id = 0;

			if (ModelState.IsValid)
			{
                await _albumOrderService.Create(albumOrder);
                return RedirectToAction(nameof(Index));
			}
            ViewData["AlbumId"] = new SelectList(await _albumService.GetAll(), "Id", "Name");
            return View(albumOrder);
        }
    }
}