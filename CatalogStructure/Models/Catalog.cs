using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogStructure.Models
{
    public class Catalog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public Catalog Parent { get; set; }
        public ICollection<Catalog> Children { get; set; }
    }
}
