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
using AlbumsToBuy.Dtos;

namespace AlbumsToBuy.Controllers.Management
{
    [Authorize(Roles = "Admin")]
    [Route("Management/[controller]/{action=Index}/{id?}")]
    public class PaymentsController : Controller
    {
        private const int PAGESIZE = 10;

        private PaymentService _paymentService;
        private UserService _userService;
        private INotyfService _notyf;

        public PaymentsController(PaymentService paymentService, UserService userService, INotyfService notyf)
        {
            _paymentService = paymentService;
            _userService = userService;
            _notyf = notyf;
        }

        // GET: Payments
        public async Task<IActionResult> Index(bool showPaid = false, int page=1)
        {
            var pagination = new PaginationDto()
            {
                PageSize = PAGESIZE,
                PageNumber = page
            };

            var count = await _paymentService.Count(showPaid);
            pagination.TotalPages = count / PAGESIZE;
            
            if (count % PAGESIZE != 0)
            {
                pagination.TotalPages++;
            }

            var payments = await _paymentService.GetByPage(pagination, showPaid);
            ViewData["showPaid"] = showPaid;
            return View(new ItemListPagination()
            {
                Payments = payments,
                Pagination = pagination
            });
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
            ViewData["UserId"] = new SelectList(users, "Id", "FullName");
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

                _notyf.Information($"Payment succsessfully created");
                return RedirectToAction(nameof(Index));
            }

            var users = await _userService.GetAll();
            ViewData["UserId"] = new SelectList(users, "Id", "FullName");
            
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
            ViewData["UserId"] = new SelectList(users, "Id", "FullName");
            
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
                _notyf.Information($"Payment succsessfully updated");
                return RedirectToAction(nameof(Index));
            }
            var users = await _userService.GetAll();
            ViewData["UserId"] = new SelectList(users, "Id", "FullName");
            
            return View(payment);
        }
    }
}
