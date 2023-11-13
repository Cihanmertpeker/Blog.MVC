using Blog.MVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Blog.MVC.Controllers.Admin
{
    public enum viewType
    {
        Create = 0,
        Update = 1,
    }

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
            var categories = this.context.Categories.AsNoTracking().ToList();
            return View(categories);
        }


        [HttpGet]
        public IActionResult CreateOrUpdate(int id, viewType type)
        {
            ViewBag.Type = type;
            if (type == viewType.Update)
            {
                var updatedCategory = this.context.Categories.AsNoTracking().SingleOrDefault(x => x.Id == id);
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
            if (category.Id == 0)
            {
                category.SeoUrl = ConvertSeoUrl(category.Definition);
                this.context.Categories.Add(category);
                this.context.SaveChanges();
            }
            else
            {
                var updatedEntity = this.context.Categories.SingleOrDefault(x => x.Id == category.Id);
                category.SeoUrl = category.Definition;

                if (updatedEntity != null)
                {
                    if (updatedEntity.Definition != category.Definition)
                    {
                        updatedEntity.Definition = category.Definition;
                        updatedEntity.SeoUrl = ConvertSeoUrl(category.Definition);
                    }

                }
                this.context.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            //var blogCategories = this.context.BlogCategories.Where(x => x.CategoryId == id).ToList();
            //this.context.BlogCategories.RemoveRange(blogCategories);
            //this.context.SaveChanges();
            var deletedCategory = this.context.Categories.SingleOrDefault(x => x.Id == id);
            this.context.Categories.Remove(deletedCategory);

            this.context.SaveChanges();

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
