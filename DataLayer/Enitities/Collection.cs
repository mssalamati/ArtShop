using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public enum CollectionType { Community, Portfolio }
    public class Collection
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public CollectionType Type { get; set; }
        public ICollection<CollectionProduct> Artworks { get; set; }
    }

    public class CollectionProduct
    {
        public int Id { get; set; }
        public virtual Product product { get; set; }
        [ForeignKey("product")]
        public int productId { get; set; }
        public virtual Collection collection { get; set; }
        [ForeignKey("collection")]
        public int collectionId { get; set; }
    }
}
