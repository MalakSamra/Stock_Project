using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock_Project.Models;

namespace Stock_Project.Controllers
{
    public class StoreController : Controller
    {
        Stock_DbContext db = new Stock_DbContext();
        public IActionResult Index()
        {
            if (db.Stores == null)
            {
                return BadRequest("Bad Request");
            }
            else
            {
                return View(db.Stores.ToList());
            }
        }
        public IActionResult Create(Store store)
        {
            if(ModelState.IsValid)
            {
                if(db.Stores != null)
                {
                    db.Stores.Add(store);
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(store);
        }
        public IActionResult Details(int id)
        {
            if(db.Stores != null && db.StoreItems != null)
            {
                var store = db.Stores.FirstOrDefault(x => x.Id == id);
                if (store == null)
                {
                    return NotFound();
                }
                var items = db.StoreItems
                    .Where(s => s.StoreID == id)
                    .Include(s => s.Item)
                    .ToList();
                ViewBag.StoreItems = items;
                return View(store);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult Delete (int id)
        {
            if (db.Stores != null)
            {
                var store = db.Stores.FirstOrDefault(s => s.Id == id);
                return View(store);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult DeleteConfirmation(int id)
        {
            if(db.Stores != null)
            {
                var store = db.Stores.FirstOrDefault(s => s.Id == id);
                if(store != null)
                {
                    db.Stores.Remove(store);
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult Edit (int id)
        {
            if (db.Stores != null)
            {
                var store = db.Stores.FirstOrDefault(s=>s.Id== id);
                return View(store);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Store store)
        {
            if (id != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(store);
                    await db.SaveChangesAsync();
                }
                catch
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }
    }
}
