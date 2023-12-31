using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock_Project.Models;

namespace Stock_Project.Controllers
{
    public class ItemController : Controller
    {
        Stock_DbContext db = new Stock_DbContext();
        public IActionResult Index()
        {
            if (db.Items == null)
            {
                return BadRequest("Bad Request");
            }
            else
            {
                return View(db.Items.ToList());
            }
        }
        public IActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                if (db.Items != null)
                {
                    db.Items.Add(item);
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(item);
        }
        public IActionResult Details(int id)
        {
            if (db.Items != null && db.StoreItems != null)
            {
                var item = db.Items.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    return NotFound();
                }
                var items = db.StoreItems
                    .Where(s => s.ItemID == id)
                    .Include(s => s.Store)
                    .ToList();
                ViewBag.StoreItems = items;
                return View(item);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult Delete(int id)
        {
            if (db.Items != null)
            {
                var store = db.Items.FirstOrDefault(s => s.Id == id);
                return View(store);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult DeleteConfirmation(int id)
        {
            if (db.Items != null)
            {
                var item = db.Items.FirstOrDefault(s => s.Id == id);
                if (item != null)
                {
                    db.Items.Remove(item);
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
        public IActionResult Edit(int id)
        {
            if (db.Items != null)
            {
                var item = db.Items.FirstOrDefault(s => s.Id == id);
                return View(item);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(item);
                    await db.SaveChangesAsync();
                }
                catch
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
    }
}
