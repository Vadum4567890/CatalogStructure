using CatalogStructure.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CatalogStructure.Controllers
{
    public class ExportController : Controller
    {
        private readonly CatalogContext _catalogContext;

        public ExportController(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public IActionResult Export()
        {
            var catalogs = _catalogContext.Catalogs.ToList();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var jsonString = JsonConvert.SerializeObject(catalogs, settings);

            return File(System.Text.Encoding.UTF8.GetBytes(jsonString), "application/json", "catalogs.json");
        }
    }
}