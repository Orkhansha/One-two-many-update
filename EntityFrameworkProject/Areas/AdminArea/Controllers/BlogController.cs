using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using EntityFrameworkProject.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index() => View(await _context.Blogs.Where(m => !m.IsDeleted).ToListAsync());

        [HttpGet]
        public IActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blog)
        {
            if (!ModelState.IsValid) return View();

            foreach (var photo in blog.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View();
                }


                if (!photo.CheckFileSize(700))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image size");
                    return View();
                }
            }


            foreach (var photo in blog.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);


                await Helper.SaveFile(path, photo);



                Blog newBlog = new Blog
                {
                    Image = fileName,
                    Title = blog.Title,
                    Desc = blog.Desc,
                    Date = DateTime.Now,
                };

                await _context.Blogs.AddAsync(newBlog);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await GetByIdAsync(id);

            if (blog == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", blog.Image);

            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.Delete(path);
            //}

            Helper.DeleteFile(path);

            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Blog blog = await GetByIdAsync((int)id);

            if (blog == null) return NotFound();

            return View(blog);

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int? id, BlogCreateVM blog)
        //{
        //    if (id is null) return BadRequest();

        //    if (blog.Photos == null) return RedirectToAction(nameof(Index));

        //    var dbBlog = await GetByIdAsync((int)id);

        //    if (dbBlog == null) return NotFound();

        //    if (blog.Photos.FirstOrDefault().CheckFileType("image/"))
        //    {
        //        ModelState.AddModelError("Photo", "Please choose correct image type");
        //        return View(dbBlog);
        //    }

        //    if (!blog.Photos.FirstOrDefault().CheckFileSize(200))
        //    {
        //        ModelState.AddModelError("Photo", "Please choose correct image size");
        //        return View(dbBlog);
        //    }

        //    string oldPath = Helper.GetFilePath(_env.WebRootPath, "img", dbBlog.Image);

        //    Helper.DeleteFile(oldPath);

        //    string fileName = Guid.NewGuid().ToString() + "_" + blog.Photos.FirstOrDefault().FileName;

        //    string newPath = Helper.GetFilePath(_env.WebRootPath, "img", fileName);


        //    using (FileStream stream = new FileStream(newPath, FileMode.Create))
        //    {
        //        await blog.Photos.FirstOrDefault().CopyToAsync(stream);
        //    }

        //    dbBlog.Image = fileName;


        //    BlogCreateVM newBlog = new BlogCreateVM
        //    {
        //        Title = blog.Title,
        //        Desc = blog.Desc,
        //        CreateTime = DateTime.Now,
        //    };

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogCreateVM blog)
        {
            if (id is null) return BadRequest();

            if (blog.Photos == null) return RedirectToAction(nameof(Index));

            var dbBlog = await GetByIdAsync((int)id);

            if (dbBlog == null) return NotFound();

            if (!blog.Photos.FirstOrDefault().CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View(dbBlog);
            }

            if (!blog.Photos.FirstOrDefault().CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View(dbBlog);
            }

            string oldPath = Helper.GetFilePath(_env.WebRootPath, "img", dbBlog.Image);

            Helper.DeleteFile(oldPath);

            string fileName = Guid.NewGuid().ToString() + "_" + blog.Photos.FirstOrDefault().FileName;

            string newPath = Helper.GetFilePath(_env.WebRootPath, "img", fileName);


            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await blog.Photos.FirstOrDefault().CopyToAsync(stream);
            }

            dbBlog.Image = fileName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }








        private async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }

    }
}
