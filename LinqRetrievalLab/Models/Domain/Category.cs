using LinqRetrievalLab.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace LinqRetrievalLab.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<Product> Product { get; set; } = new();
    }
}
