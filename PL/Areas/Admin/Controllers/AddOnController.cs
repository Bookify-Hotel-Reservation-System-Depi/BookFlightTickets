using DAL.Data;
using DAL.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AddOnController : Controller
    {
        private readonly BookFilghtsDbContext _context;

        public AddOnController(BookFilghtsDbContext context)
        {
            _context = context;
        }
        // GET: Admin/AddOns
        public async Task<IActionResult> Index()
        {
            var addons = await _context.AddOns.ToListAsync();
            return View(addons);
        }
        // GET: Admin/AddOns/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddOn addon)
        {
            if (ModelState.IsValid)
            {
                _context.AddOns.Add(addon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(addon);
        }
        // GET: Admin/AddOns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var addon = await _context.AddOns.FindAsync(id);
            if (addon == null)
                return NotFound();

            return View(addon);
        }  // POST: Admin/AddOns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddOn addon)
        {
            if (id != addon.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addon);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AddOns.Any(a => a.Id == addon.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(addon);
        }
        // GET: Admin/AddOns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var addon = await _context.AddOns.FirstOrDefaultAsync(a => a.Id == id);
            if (addon == null)
                return NotFound();

            return View(addon);
        }

        // POST: Admin/AddOns/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var addon = await _context.AddOns.FindAsync(id);
            if (addon != null)
            {
                _context.AddOns.Remove(addon);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    
    }
}
