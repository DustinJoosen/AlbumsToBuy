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

namespace AlbumsToBuy.Controllers.Management
{
    [Authorize(Roles = "Admin")]
    [Route("Management/[controller]/{action=Index}/{id?}")]
    public class CustomersController : Controller
    {
        private readonly UserService _userService;
        private INotyfService _notyf;

        public CustomersController(UserService userService, INotyfService notyf)
        {
            _userService = userService;
            _notyf = notyf;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _userService.GetCustomers();
            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.Update(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userService.Exists(user))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyf.Information($"Customer {user.FullName} succsessfully updated");
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
