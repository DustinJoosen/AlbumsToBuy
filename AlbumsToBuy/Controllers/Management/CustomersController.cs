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
    public class CustomersController : Controller
    {
        private readonly UserService _userService;
        private readonly AddressService _addressService;

        public CustomersController(UserService userService, AddressService addressService)
        {
            _userService = userService;
            _addressService = addressService;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAll();
            return View(users);
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
        public async Task<IActionResult> Create()
        {
            ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City");
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.Create(user);
                return RedirectToAction(nameof(Index));
            }

            ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City", user.HomeAddressId);
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

            ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City");
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(await _addressService.GetAll(), "Id", "City", user.HomeAddressId);
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
            return RedirectToAction(nameof(Index));
        }

    }
}
