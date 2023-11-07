using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Hosting.Internal;
using Imbracaminte.Models.VMs;
using Imbracaminte.Models.Entities;

namespace Imbracaminte.Controllers
{
    [Route("[Controller]")]
    public class ArticoleController : Controller
    {
        private const string imgFolderName = "img";
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ImbracaminteContext context;

        public ArticoleController(IWebHostEnvironment hostEnvironment, ImbracaminteContext context)
        {      
            this.hostEnvironment= hostEnvironment;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = context.Articole.Select(p => new ArticoleVM().ProdToProdVM(p)).ToList();
            return View(list);
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            var Articole = new ArticoleVM();
            return View(Articole);
        }

        [HttpPost]
        [Route("New")]
        public IActionResult Create(ArticoleVM dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");           
                return View("New", dto);
            }

            SaveImage(dto);
            context.Add(ArticoleVM.VMProdToProd(dto));
            context.SaveChanges();

            return View("Index", context.Articole.Select(p => new ArticoleVM().ProdToProdVM(p)).ToList());
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var prod = context.Articole.FirstOrDefault(p => p.Id == id);

            if (prod == null)
                return View("Index", context.Articole.Select(p => new ArticoleVM().ProdToProdVM(p)).ToList());
            else
                return View(new ArticoleVM().ProdToProdVM(prod));
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id, ArticoleVM dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");               
                return View($"Edit/{id}", dto);
            }

            var prod = context.Articole.FirstOrDefault(p => p.Id == id);
            if (prod == null)
                return View("Index", context.Articole.Select(p => new ArticoleVM().ProdToProdVM(p)).ToList());

            var oldFileRelativePath = prod.ImagePath;
            if (dto.ProducImage == null)
                dto.ImagePath = oldFileRelativePath;
            else
            {
                if (!string.IsNullOrWhiteSpace(oldFileRelativePath))
                {
                    var olfFileFullPath = Path.Combine(hostEnvironment.WebRootPath, oldFileRelativePath);
                    if (System.IO.File.Exists(olfFileFullPath))
                        System.IO.File.Delete(olfFileFullPath);
                }

                SaveImage(dto);
            }

            prod.Name = dto.Name;
            prod.Description = dto.Description;
            prod.Price = dto.Price;
            prod.Size = dto.Size;
            prod.Color = dto.Color;
            prod.IsAvailable = dto.IsAvailable;
            prod.ImagePath = dto.ImagePath;

            context.Articole.Update(prod);
            context.SaveChanges();


            return View("Index", context.Articole.Select(p => new ArticoleVM().ProdToProdVM(p)).ToList());
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public JsonResult Delete(int id)
        {
            var prod = context.Articole.FirstOrDefault(p => p.Id == id);
            if (prod == null)
                return Json(new { success = true, message = "Already Deleted" });

            if (!string.IsNullOrWhiteSpace(prod.ImagePath))
            {
                var filePath = Path.Combine(hostEnvironment.WebRootPath, prod.ImagePath);

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            context.Articole.Remove(prod);
            context.SaveChanges();

            return Json(new { success = true, message = "Delete success" });
        }

        [HttpGet]
        [Route("Add/{id}")]
        public IActionResult AddToBasket(int id)
        {
            var shopList = HttpContext.Session.Get<List<int>>(SessionHelper.ShoppingCart);

            if (shopList == null)
                shopList = new List<int>();

            if (!shopList.Contains(id))
                shopList.Add(id);

            HttpContext.Session.Set(SessionHelper.ShoppingCart, shopList);

            return RedirectToAction("Index", "Articole");
        }

        [HttpGet]
        [Route("Remove/{id}")]
        public IActionResult RemoveFromBasket(int id)
        {
            var shopList = HttpContext.Session.Get<List<int>>(SessionHelper.ShoppingCart);

            if (shopList == null)
                shopList = new List<int>();

            if (shopList.Contains(id))
                shopList.Remove(id);

            HttpContext.Session.Set(SessionHelper.ShoppingCart, shopList);

            return RedirectToAction("Index", "Articole");
        }

        private void SaveImage(ArticoleVM dto)
        {
            if (dto.ProducImage == null)
                return;

            var imgFolderPath = Path.Combine(hostEnvironment.WebRootPath, imgFolderName);

            if (!Directory.Exists(imgFolderPath))
                Directory.CreateDirectory(imgFolderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.ProducImage.FileName);
            var imgFullPath = Path.Combine(imgFolderPath, fileName);

            using (var fileStream = new FileStream(imgFullPath, FileMode.Create))
                dto.ProducImage.CopyTo(fileStream);

            dto.ImagePath = Path.Combine(imgFolderName, fileName);
        }         
    }
}
