using CatalogStructure.Data;
using CatalogStructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CatalogStructure.Controllers
{
    public class ImportController : Controller
    {
        private readonly CatalogContext _catalogContext;

        public ImportController(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View("~/Views/File/Import.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Import(int a)
        {
            a = Convert.ToInt32(a);
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                using var streamReader = new StreamReader(file.OpenReadStream());
                var jsonString = await streamReader.ReadToEndAsync();
                var catalogList = JsonConvert.DeserializeObject<List<Catalog>>(jsonString);

                // Add new or update existing catalog structure
                foreach (var catalog in catalogList)
                {
                    var existingCatalog = await _catalogContext.Catalogs.FindAsync(catalog.Id);
                    if (existingCatalog != null)
                    {
                        _catalogContext.Entry(existingCatalog).CurrentValues.SetValues(catalog);
                        _catalogContext.Update(existingCatalog);
                    }
                    else
                    {
                        await _catalogContext.AddAsync(catalog);
                    }
                }

                await _catalogContext.SaveChangesAsync();

                return RedirectToAction("Index", "Catalog");
            }

            return View("Import");
        }
    }
}