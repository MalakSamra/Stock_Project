using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock_Project.Models;

namespace Stock_Project.Controllers
{
    public class StoreItemController : Controller
    {
        Stock_DbContext db = new Stock_DbContext();
        public IActionResult Index()
        {
            if(db.Stores !=null && db.StoreItems != null )
            {
                var stores = db.Stores.ToList();
                var itemsTable = db.Items.ToList();
                var storeItemsDictionary = new Dictionary <int, List<StoreItem>> ();
                foreach (var store in stores)
                {
                    var items = db.StoreItems
                        .Where(s => s.StoreID == store.Id)
                        .Include(s => s.Item)
                        .ToList();
                    storeItemsDictionary[store.Id] = items;
                }
                ViewBag.Stores = stores;
                ViewBag.Items = itemsTable;
                ViewBag.StoreItemsDictionary = storeItemsDictionary;
                return View();
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult GetItemByStore(int id)
        {
            var items = db.StoreItems
                .Where(s => s.StoreID == id)
                .Select(s => new
                {
                    storeId = s.StoreID,
                    itemId = s.Item.Id,
                    name = s.Item.Name,
                    quantity = s.Quantity
                })
                .ToList();
            return Json(items);
        }
        public IActionResult GetStoreNameByID (int id)
        {
            var items = db.Stores
                .Where(s=>s.Id == id)
                .Select (s => s.Name)
                .ToList();
            return Json(items);
        }
        public IActionResult GetItemNameByID(int id)
        {
            var items = db.Items
                .Where(s => s.Id == id)
                .Select(s => s.Name)
                .ToList();
            return Json(items);
        }
        public IActionResult GetQuantityByStoreIdAndItemId(int storeId, int itemId)
        {
            var quantity = db.StoreItems
                .Where(s=>s.ItemID == itemId && s.StoreID == storeId)
                .ToList();
            return Json(quantity);
        }
        public IActionResult CheckIfExists([Bind("StoreID, ItemID")] StoreItem storeItem)
        {
            bool exists = db.StoreItems.Any(item => item.StoreID == storeItem.StoreID && item.ItemID == storeItem.ItemID);
            return Json(new { exists = exists });
        }
        public IActionResult Create (StoreItem storeItem)
        {
            if(ModelState.IsValid)
            {
                if(db.StoreItems != null)
                {
                    db.StoreItems.Add(storeItem);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false, error = "Invalid Data"});
        }
        public  IActionResult Edit(int store_id, int item_id, StoreItem updatedstoreItem)
        {
            if (store_id != updatedstoreItem.StoreID && item_id != updatedstoreItem.ItemID)
            {
                return Json(new { success = false, error = "Invalid Request" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var storeItem = db.StoreItems
                        .Where(s=>s.StoreID == store_id && s.ItemID == item_id)
                        .FirstOrDefault();
                    if(storeItem == null)
                    {
                        return Json(new { success = false, error = "Not Found" });
                    }
                    storeItem.StoreID = updatedstoreItem.StoreID;
                    storeItem.ItemID = updatedstoreItem.ItemID;
                    storeItem.Quantity = updatedstoreItem.Quantity;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch(Exception ex)
                {
                    return Json(new { success = false, error = "Error updating item", message = ex.Message });
                }
            }
            return Json(new { success = false, error = "Invalid Data" });
        }
        public IActionResult DeleteConfirmation(int store_id, int item_id)
        {
            if (db.StoreItems != null)
            {
                var storeItem = db.StoreItems.FirstOrDefault(s => s.StoreID == store_id && s.ItemID == item_id);
                if (storeItem != null)
                {
                    db.StoreItems.Remove(storeItem);
                    db.SaveChanges();
                    return Json(new { success = true});
                }
                return Json(new { success = false, error = "Not Found" });
            }
            else
            {
                return Json(new { success = false, error = "Not Found" });
            }
        }
    }
    
}
