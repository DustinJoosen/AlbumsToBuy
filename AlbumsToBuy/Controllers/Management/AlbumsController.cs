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
using Microsoft.AspNetCore.Hosting;
using AlbumsToBuy.Helpers;

namespace AlbumsToBuy.Controllers.Management
{
    [Route("Management/[controller]/{action=Index}/{id?}")]
    [Authorize(Roles = "Admin")]
    public class AlbumsController : Controller
    {
        private AlbumService _albumService;
        private IWebHostEnvironment _env;

        public AlbumsController(AlbumService albumService, IWebHostEnvironment env)
        {
            _albumService = albumService;
            _env = env;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var albums = await _albumService.GetAll();
            return View(albums);
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumService.GetById((int)id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Album album)
        {
            if (ModelState.IsValid)
            {
                ImageHelper.UploadImage(ref album, _env);
                
                await _albumService.Create(album);
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumService.GetById((int)id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        // PUT : Albums/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Album album)
        {
            if (id != album.Id)
			{
                return BadRequest();
			}

            if (ModelState.IsValid)
            {
                try
                {
                    if (album.FormFile != null)
					{
                        ImageHelper.UploadImage(ref album, _env);
                    }

                    await _albumService.Update(album);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_albumService.Exists(album))
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
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumService.GetById((int)id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // DELETE : Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _albumService.GetById(id);
            if(album == null)
			{
                return NotFound();
			}

            ImageHelper.RemoveImage(ref album, _env);
            await _albumService.Remove(album);
            return RedirectToAction(nameof(Index));
        }

    }
}
