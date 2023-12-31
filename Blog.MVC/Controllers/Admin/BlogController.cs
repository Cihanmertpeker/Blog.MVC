﻿using BlogMVC.Models;
using BlogMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BlogMVC.Controllers.Admin
{
    [Authorize]
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly BlogDbContext context;
        public BlogController(BlogDbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public IActionResult Index(BlogSearchModel model)
        {
            ViewBag.SearchModel = model;
            ViewBag.active = "Blog";

            var query = context.Blogs.AsQueryable();

            if (model.CreatedDate != DateTime.MinValue)
            {
                query = query.Where(x => x.CreatedDate.Date == model.CreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(model.Title))
            {
                query = query.Where(x => x.Title.Contains(model.Title));
            }



            ViewBag.PageModel = new PageModel
            {
                ActivePage = model.ActivePage,
                PageCount = (int)Math.Ceiling((decimal)query.Count() / model.PageSize),
                PageSize = model.PageSize,
            };

            var blogs = query.Skip((model.ActivePage - 1) * model.PageSize).Take(model.PageSize).ToList();

            return View(blogs);
        }
        [HttpGet]
        public IActionResult AssignCategory(int id)
        {
            ViewBag.SelectedBlog = this.context.Blogs.AsNoTracking().SingleOrDefault(x => x.Id == id);



            var list = new List<AssignCategoryListModel>();

            var allCategories = this.context.Categories.AsNoTracking().ToList();


            var filtredCategories = this.context.Categories.Join(this.context.BlogCategories, category => category.Id, blog => blog.CategoryId, (category, categoryBlog) => new
            {
                category,
                categoryBlog,
            }).Where(x => x.categoryBlog.BlogId == id).Select(x => new Category
            {
                Definition = x.category.Definition,
                Id = x.category.Id,
                SeoUrl = x.category.SeoUrl,
            }).AsNoTracking().ToList();


            foreach (var category in allCategories)
            {
                bool exist = false;
                if (filtredCategories.Contains(category))
                {
                    exist = true;
                }
                list.Add(new AssignCategoryListModel
                {
                    BlogId = id,
                    Definition = category.Definition,
                    Id = category.Id,
                    Exist = exist
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult AssignCategory(List<AssignCategoryListModel> model)
        {
          

            foreach (var assignCategory in model)
            {
                var control = this.context.BlogCategories.SingleOrDefault(x => x.BlogId == assignCategory.BlogId && x.CategoryId == assignCategory.Id);
                if (assignCategory.Exist)
                {

                    if (control == null)
                    {
                        this.context.BlogCategories.Add(new BlogCategory { CategoryId = assignCategory.Id, BlogId = assignCategory.BlogId });
                        this.context.SaveChanges();
                    }
                }

                else
                {
                    if (control != null)
                    {
                        this.context.BlogCategories.Remove(control);
                        this.context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
        }

    }
}
 