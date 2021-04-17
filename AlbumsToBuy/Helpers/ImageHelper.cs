using AlbumsToBuy.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Helpers
{
    public static class ImageHelper
    {
        public static void UploadImage(ref Album album, IWebHostEnvironment _env)
        {
            if (album.FormFile == null)
            {
                album.CoverImage = "NotFound.png";
                return;
            }

            ImageHelper.RemoveImage(ref album, _env);

            string uploadFolder = Path.Combine(_env.WebRootPath, "upload/album");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + album.FormFile.FileName;
            string fp = Path.Combine(uploadFolder, uniqueFileName);

            using (var stream = new FileStream(fp, FileMode.Create))
            {
                album.FormFile.CopyTo(stream);
            }

            album.CoverImage = uniqueFileName;
        }

        public static void RemoveImage(ref Album album, IWebHostEnvironment _env)
        {
            if (album.CoverImage == null || album.CoverImage == "NotFound.png")
			{
                return;
			}

            string folder = Path.Combine(_env.WebRootPath, "upload/album");
            string fp = Path.Combine(folder, album.CoverImage);
            if (File.Exists(fp))
			{
                File.Delete(fp);
            }

            album.CoverImage = "NotFound.png";
		}
    }
}
