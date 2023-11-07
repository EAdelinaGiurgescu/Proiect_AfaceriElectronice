using ProiectAfaceriElectronice.Models.Attributes;
using Imbracaminte.Models;
using System.ComponentModel.DataAnnotations;
using Imbracaminte.Models.Entities;

namespace Imbracaminte.Models.VMs
{
    public class ArticoleVM
    {
        public ArticoleVM()
        {
            Name = string.Empty;
            Description= string.Empty;
            Color = string.Empty;
        }

        public int Id { get; set; }
        [Required (AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(2000)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImagePath { get; set; }
  

        [AllowedExtensions(".jpg", ".png", ".jpeg")]
        [MaxFileSize(3 * 1024 * 1024)]
        public IFormFile? ProducImage { get; set; }

        public static Articole VMProdToProd(ArticoleVM vm)
        {
            var prod = new Articole();

            prod.Name = vm.Name;
            prod.Description = vm.Description;
            prod.Price = vm.Price;
            prod.Size = vm.Size;
            prod.Color = vm.Color;
            prod.IsAvailable = vm.IsAvailable;
            prod.ImagePath = vm.ImagePath;

            return prod;
        }

        public ArticoleVM ProdToProdVM(Articole? prod)
        {
            if (prod == null)
                return new ArticoleVM();

            var vm = new ArticoleVM();

            vm.Id = prod.Id;
            vm.Name = prod.Name;
            vm.Description = prod.Description;
            vm.Price = prod.Price;
            vm.Size = prod.Size;
                vm.Color = prod.Color;
;            vm.IsAvailable = prod.IsAvailable;
            vm.ImagePath = prod.ImagePath; 
           
            return vm;
        }

    }
}
