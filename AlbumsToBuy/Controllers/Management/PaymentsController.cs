using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;

namespace AlbumsToBuy.Controllers.Management
{
    [Route("Management/[controller]/{action=Index}/{id?}")]
    public class PaymentsController : Controller
    {
        private PaymentService _paymentService;
        private UserService _userService;

        public PaymentsController(PaymentService paymentService, UserService userService)
        {
            _paymentService = paymentService;
            _userService = userService;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAll();
            return View(payments);
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _paymentService.GetById((int)id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create()
        {
            var users = await _userService.GetAll();
            ViewData["UserId"] = new SelectList(users, "Id", "FirstName");
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                await _paymentService.Create(payment);
                return RedirectToAction(nameof(Index));
            }

            var users = await _userService.GetAll();
            ViewData["UserId"] = new SelectList(users, "Id", "FirstName");
            
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _paymentService.GetById((int)id);
            if (payment == null)
            {
                return NotFound();
            }

            var users = await _userService.GetAll();
            ViewData["UserId"] = new SelectList(users, "Id", "FirstName");
            
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _paymentService.Update(payment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_paymentService.Exists(payment))
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
            var users = await _userService.GetAll();
            ViewData["UserId"] = new SelectList(users, "Id", "FirstName");
            
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id
            )
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _paymentService.GetById((int)id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _paymentService.GetById(id);
            if(payment == null)
			{
                return NotFound();
			}

            await _paymentService.Remove(payment);
            return RedirectToAction(nameof(Index));
        }
    }
}
