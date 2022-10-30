using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogTextController : Controller
    {
        private readonly AppDbContext _context;

        public BlogTextController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            BlogText blogText = await _context.BlogTexts.FirstOrDefaultAsync();
            return View(blogText);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogText blogText)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }


               
                await _context.BlogTexts.AddAsync(blogText);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            BlogText blogText = await _context.BlogTexts.FindAsync(id);

            if (blogText == null) return NotFound();

            return View(blogText);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            BlogText blogText = await _context.BlogTexts.FirstOrDefaultAsync(m => m.Id == id);

            _context.BlogTexts.Remove(blogText);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                BlogText blogText = await _context.BlogTexts.FirstOrDefaultAsync(m => m.Id == id);

                if (blogText is null) return NotFound();

                return View(blogText);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogText blogText)
        {
            try
            {
                

                BlogText dbBlogText = await _context.BlogTexts.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbBlogText is null) return NotFound();

                _context.BlogTexts.Update(blogText);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
