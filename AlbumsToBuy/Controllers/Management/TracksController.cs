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
    public class TracksController : Controller
    {
        private readonly TrackService _trackService;
        private readonly AlbumService _albumService;

        public TracksController(TrackService _trackService, AlbumService albumService)
        {
            this._trackService = _trackService;
            this._albumService = albumService;
        }

        // GET: Tracks
        public async Task<IActionResult> Index()
        {
            var tracks = await _trackService.GetAll();
            return View(tracks);
        }

        // GET: Tracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _trackService.GetById((int)id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Tracks/Create
        public async Task<IActionResult> Create()
        {
            var albums = await _albumService.GetAll();
            ViewData["AlbumId"] = new SelectList(albums, "Id", "Name");

            return View();
        }

        // POST: Tracks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Track track)
        {
            if (ModelState.IsValid)
            {
                await _trackService.Create(track);
                return RedirectToAction(nameof(Index));
            }

            var albums = await _albumService.GetAll();
            ViewData["AlbumId"] = new SelectList(albums, "Id", "Name");

            return View(track);
        }

        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _trackService.GetById((int)id);
            if (track == null)
            {
                return NotFound();
            }

            var albums = await _albumService.GetAll();
            ViewData["AlbumId"] = new SelectList(albums, "Id", "Name", track.AlbumId);

            return View(track);
        }

        // POST: Tracks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Track track)
        {
            if (id != track.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _trackService.Update(track);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_trackService.Exists(track))
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
            var albums = await _albumService.GetAll();
            ViewData["AlbumId"] = new SelectList(albums, "Id", "Name");

            return View(track);
        }

        // GET: Tracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _trackService.GetById((int)id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var track = await _trackService.GetById((int)id);
            await _trackService.Remove(track);
            return RedirectToAction(nameof(Index));
        }
    }
}
