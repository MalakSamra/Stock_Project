using System.ComponentModel.DataAnnotations.Schema;

namespace Stock_Project.Models
{
    public class StoreItem
    {
        [ForeignKey("Store")]
        public int StoreID { get; set; }
        [ForeignKey("Item")]
        public int ItemID { get; set; }
        public int? Quantity { get; set; }
        public virtual Store? Store { get; set; }
        public virtual Item? Item { get; set; }
    }
}
