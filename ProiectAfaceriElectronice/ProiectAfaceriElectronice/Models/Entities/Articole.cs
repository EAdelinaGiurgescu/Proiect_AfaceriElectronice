namespace Imbracaminte.Models.Entities
{
    public class Articole
    {
        public Articole()
        {
            Name = string.Empty;
            Description = string.Empty;
            Color = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImagePath { get; set; }
    }
}
