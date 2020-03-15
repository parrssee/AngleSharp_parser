using System.Data.Entity;

namespace WebApplication1.Models
{
    public class GoodsContext : DbContext
    {
        public DbSet<Smartphone> Smartphones { get; set; }
    }
}