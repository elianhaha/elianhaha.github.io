using Ecommerce.Database;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly ShoppingData context;
        public PagesController(ShoppingData context)
        {
            this.context = context;
        }
        //GET/admin/pages
        public async Task<IActionResult> Index()
        {
            IQueryable<Page>pages = from p in context.Pages orderby p.Sorting select p;
            List<Page> pageList = await pages.ToListAsync();
            return View(pageList);
        }
        //GET/admin/pages/details/id
        public async Task<IActionResult> Details (int id)
        {
            Page page = await context.Pages.FirstOrDefaultAsync(p=>p.Id==id);
            if(page==null)
            {
                return NotFound();
            }
            return View(page);
        }
        //GET/admin/pages/create
        public IActionResult Create() => View();

        //POST/admin/pages/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if(slug != null)
                {
                    ModelState.AddModelError("","The page already exists");
                    return View(page);
                }
                context.Add(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been added";

                return RedirectToAction("Index");
            }
            return View(page);
        }
        //POST/admin/oages/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        //POST/admin/pages/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;

                var slug = await context.Pages.Where(x=>x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The title already exists");
                    return View(page);
                }
                context.Update(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been edited";

                return RedirectToAction("Edit", new{ id = page.Id});
            }
            return View(page);
        }

        public async Task<IActionResult> Delete (int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if(page == null)
            {
                TempData["Error"] = "The page doesn't exist";
            }
            else
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The page has been deleted";
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach(var paegId in id)
            {
                Page page = await context.Pages.FindAsync(paegId);
                page.Sorting = count;
                context.Update(page);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }
    }
}
