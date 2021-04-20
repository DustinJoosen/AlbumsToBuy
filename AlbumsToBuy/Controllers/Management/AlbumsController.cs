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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace AlbumsToBuy.Controllers.Management
{
    [Route("Management/[controller]/{action=Index}/{id?}")]
    [Authorize(Roles = "Admin")]
    public class AlbumsController : Controller
    {
        private AlbumService _albumService;
        private TrackService _trackService;
        private INotyfService _notyf;
        private IWebHostEnvironment _env;

        public AlbumsController(AlbumService albumService, TrackService trackService, INotyfService notyf, IWebHostEnvironment env)
        {
            _albumService = albumService;
            _trackService = trackService;
            _notyf = notyf;
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

                _notyf.Information($"Album {album.Name} succsessfully created");
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
                    _notyf.Information($"Album {album.Name} succsessfully updated");
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

            _notyf.Information($"Album {album.Name} succsessfully removed");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateTrack(int id)
		{
            var album = await _albumService.GetById(id);
            return View(album);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrack(int albumId, int duration, string name)
		{
            var track = new Track()
            {
                AlbumId = albumId,
                Duration = duration,
                Name = name
            };

            await _trackService.Create(track);
            return RedirectToAction("UpdateTrack", new { id = albumId });

		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrack(int trackId, int albumId, Track track)
		{
            track.Id = trackId;
            track.AlbumId = albumId;

            if (ModelState.IsValid)
			{
                await _trackService.Update(track);
                return RedirectToAction("UpdateTrack", new { id = albumId });
            }
            return View(track);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrack(int trackId)
		{
            var track = await _trackService.GetById(trackId);
            if(track == null)
			{
                return NotFound();
			}

            await _trackService.Remove(track);
            return RedirectToAction("UpdateTrack", new { id = track.AlbumId });
        }

    }
}
