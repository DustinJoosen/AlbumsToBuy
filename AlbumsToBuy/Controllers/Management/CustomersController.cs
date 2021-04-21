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

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.UniqueEmail(user.Email))
                {

                    await _userService.Create(user);

                    _notyf.Information($"Customer {user.FullName} succsessfully created");
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(nameof(user.Email), "This email is already in use");
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

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userService.GetById((int)id);
            if(user == null)
			{
                return NotFound();
			}

            await _userService.Remove(user);
            
            _notyf.Information($"Customer {user.FullName} succsessfully deleted");
            return RedirectToAction(nameof(Index));
        }

    }
}
