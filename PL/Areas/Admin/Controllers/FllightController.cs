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
    public class FllightController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookFilghtsDbContext _dbContext;

        public FllightController(IUnitOfWork unitOfWork,BookFilghtsDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            //var flights = _unitOfWork.Repository<Flight>.GetAllAsync(
            //    includeProperties: "Airline,Airplane,DepartureAirport,ArrivalAirport"
            //);
            var flights = await _unitOfWork.Repository<Flight>().GetAllAsync( q => q
            .Include(f => f.Airline)
            .Include(f => f.Airplane)
            .Include(f => f.DepartureAirport)
            .Include(f => f.ArrivalAirport));

            return View(flights);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Airlines =await _unitOfWork.Repository<Airline>().GetAllAsync();
            ViewBag.Airplanes = await _unitOfWork.Repository<Airplane>().GetAllAsync();
            ViewBag.Airports = await _unitOfWork.Repository<Airport>().GetAllAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Flight flight)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Repository<Flight>().AddAsync(flight);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }

            // في حالة في Error بيرجع يعيد تحميل البيانات
            ViewBag.Airlines = await _unitOfWork.Repository<Airline>().GetAllAsync();
            ViewBag.Airplanes = await _unitOfWork.Repository<Airplane>().GetAllAsync();
            ViewBag.Airports = await _unitOfWork.Repository<Airport>().GetAllAsync();

            return View(flight);
        }




        [HttpGet]
        // GET: Admin/Flight/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var flight = await _dbContext.Flights.FindAsync(id);
            if (flight == null)
                return NotFound();

            ViewData["AirlineId"] = new SelectList(_dbContext.Airlines, "Id", "Name", flight.AirlineId);
            ViewData["DepartureAirportId"] = new SelectList(_dbContext.Airports, "Id", "Name", flight.DepartureAirportID);
            ViewData["ArrivalAirportId"] = new SelectList(_dbContext.Airports, "Id", "Name", flight.ArrivalAirportID);

            ViewBag.Airlines = await _unitOfWork.Repository<Airline>().GetAllAsync();
            ViewBag.Airplanes = await _unitOfWork.Repository<Airplane>().GetAllAsync();
            ViewBag.Airports = await _unitOfWork.Repository<Airport>().GetAllAsync();


            return View(flight);
        }


        // POST: Admin/Flight/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Flight flight)
        {
            if (id != flight.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.Airlines = await _unitOfWork.Repository<Airline>().GetAllAsync();
                    ViewBag.Airplanes = await _unitOfWork.Repository<Airplane>().GetAllAsync();
                    ViewBag.Airports = await _unitOfWork.Repository<Airport>().GetAllAsync();
                    _dbContext.Update(flight);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dbContext.Flights.Any(e => e.Id == flight.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewData["AirlineId"] = new SelectList(_dbContext.Airlines, "Id", "Name", flight.AirlineId);
            ViewData["DepartureAirportId"] = new SelectList(_dbContext.Airports, "Id", "Name", flight.DepartureAirportID);
            ViewData["ArrivalAirportId"] = new SelectList(_dbContext.Airports, "Id", "Name", flight.ArrivalAirportID);

            return View(flight);
        }




        [HttpGet]
        // GET: Flight/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _unitOfWork.Repository<Flight>()
                .GetByIdAsync(id);

            if (flight == null)
                return NotFound();

            return View(flight);
        }

        // POST: Flight/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _unitOfWork.Repository<Flight>().GetByIdAsync(id);

            if (flight == null)
                return NotFound();

            _unitOfWork.Repository<Flight>().Delete(flight);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var flight = await _dbContext.Flights
                .Include(f => f.Airline)
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null)
                return NotFound();
            ViewBag.Airlines = await _unitOfWork.Repository<Airline>().GetAllAsync();
            ViewBag.Airplanes = await _unitOfWork.Repository<Airplane>().GetAllAsync();
            ViewBag.Airports = await _unitOfWork.Repository<Airport>().GetAllAsync();

            return View(flight);
        }
        // GET: Admin/Flight/Search
        public async Task<IActionResult> Search(string? keyword, DateTime? date)
        {
            var flights = _dbContext.Flights
                .Include(f => f.Airline)
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                flights = flights.Where(f =>
                    f.Airline.Name.Contains(keyword) ||
                    f.DepartureAirport.Name.Contains(keyword) ||
                    f.ArrivalAirport.Name.Contains(keyword));
            }

            if (date.HasValue)
            {
                flights = flights.Where(f => f.DepartureTime.Date == date.Value.Date);
            }

            var result = await flights.ToListAsync();
            return View(result);
        }
    }
}
