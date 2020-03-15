namespace WebApplication1.Models
{
    public class Smartphone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; } = null;
    }
}