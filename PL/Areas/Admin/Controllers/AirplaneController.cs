using BLLProject.Interfaces;
using DAL.Data;
using DAL.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace PL.Areas.Admin.Controllers
{
    [Area(SD.Admin)]
    [Authorize(Roles = SD.Admin)]
    public class AirplaneController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookFilghtsDbContext _dbContext;

        public AirplaneController(IUnitOfWork unitOfWork ,BookFilghtsDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext =  dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var airplanes = await _unitOfWork.Repository<Airplane>().GetAllAsync(q => q.Include(a => a.Airline));
            return View(airplanes);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Airlines = new SelectList(await _unitOfWork.Repository<Airline>().GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Airplane airplane)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Airlines = new SelectList(await _unitOfWork.Repository<Airline>().GetAllAsync(), "Id", "Name");
                return View(airplane);
            }

            await _unitOfWork.Repository<Airplane>().AddAsync(airplane);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var airplane = await _dbContext.Airplanes
                .Include(a => a.Airline)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airplane = await _dbContext.Airplanes.FindAsync(id);
            if (airplane != null)
            {
                _dbContext.Airplanes.Remove(airplane);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _dbContext.Airplanes.FindAsync(id);
            if (airplane == null)
            {
                return NotFound();
            }

            ViewData["AirlineId"] = new SelectList(_dbContext.Airlines, "Id", "Name", airplane.AirlineId);
            return View(airplane);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,SeatCapacity,AirlineId")] Airplane airplane)
        {
            if (id != airplane.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(airplane);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dbContext.Airplanes.Any(e => e.Id == airplane.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AirlineId"] = new SelectList(_dbContext.Airlines, "Id", "Name", airplane.AirlineId);
            return View(airplane);
        }

        private bool AirplaneExists(int id)
        {
            return _dbContext.Airplanes.Any(e => e.Id == id);
        }
    }
}
