using BlogMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace BlogMVC.Controllers.Admin
{
    public enum viewType
    {
        Create = 0,
        Update = 1,
    }
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly BlogDbContext context;
        public CategoryController(BlogDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewBag.active = "Category";
            var categories = context.Categories.AsNoTracking().ToList();
            return View(categories);
        }


        [HttpGet]
        public IActionResult CreateOrUpdate(int id, viewType type)
        {
            ViewBag.active = "Category";
            ViewBag.Type = type;
            if (type == viewType.Update)
            {
                var updatedCategory = context.Categories.AsNoTracking().SingleOrDefault(x => x.Id == id);
                return View(updatedCategory);
            }
            else
            {
                return View(new Category());
            }

        }

        [HttpPost]
        public IActionResult CreateOrUpdate(Category category)
        {
            ViewBag.active = "Category";
            if (category.Id == 0)
            {
                category.SeoUrl = ConvertSeoUrl(category.Definition);
                context.Categories.Add(category);
                context.SaveChanges();
            }
            else
            {
                var updatedEntity = context.Categories.SingleOrDefault(x => x.Id == category.Id);
                category.SeoUrl = category.Definition;

                if (updatedEntity != null)
                {
                    if (updatedEntity.Definition != category.Definition)
                    {
                        updatedEntity.Definition = category.Definition;
                        updatedEntity.SeoUrl = ConvertSeoUrl(category.Definition);
                    }

                }
                context.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            ViewBag.active = "Category";
            //var blogCategories = this.context.BlogCategories.Where(x => x.CategoryId == id).ToList();
            //this.context.BlogCategories.RemoveRange(blogCategories);
            //this.context.SaveChanges();
            var deletedCategory = context.Categories.SingleOrDefault(x => x.Id == id);
            context.Categories.Remove(deletedCategory);

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string ConvertSeoUrl(string definition)
        {
            definition = definition.ToLower().Replace(' ', '-');
            definition = definition.Replace('ş', 's');
            definition = definition.Replace('ğ', 'g');
            definition = definition.Replace('ı', 'i');
            definition = definition.Replace('ü', 'u');
            definition = definition.Replace('ö', 'o');
            definition = definition.Replace('ç', 'c');

            return definition;
        }

    }
}
