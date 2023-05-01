using CatalogStructure.Data;
using CatalogStructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace CatalogStructure.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogContext _catalogContext;

        public CatalogController(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public IActionResult Index(int? catalogId)
        {
            if (catalogId == null)
            {
                var catalogs = _catalogContext.Catalogs.Where(c => c.ParentId == null).ToList();
                return View(catalogs);
            }
            else
            {
                var catalog = _catalogContext.Catalogs.Include(c => c.Children).FirstOrDefault(c => c.Id == catalogId);
                if (catalog == null)
                {
                    return NotFound();
                }
                if (catalog.Children != null && catalog.Children.Any())
                {
                    return View(catalog.Children.ToList());
                }
                else
                {
                    return View(new List<Catalog>());
                }
            }
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ParentId = new SelectList(_catalogContext.Catalogs.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Catalog catalog)
        {
            _catalogContext.Catalogs.Add(catalog);
            _catalogContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult DeleteAll()
        {
            _catalogContext.Catalogs.RemoveRange(_catalogContext.Catalogs);
            _catalogContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
