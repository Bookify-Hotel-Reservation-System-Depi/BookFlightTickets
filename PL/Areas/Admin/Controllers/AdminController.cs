using DAL.models;
using BAL.model;
using DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly BookFilghtsDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(BookFilghtsDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalFlights = await _context.Flights.CountAsync(),
                TotalAirlines = await _context.Airlines.CountAsync(),
                TotalAirplanes = await _context.Airplanes.CountAsync(),
                TotalBookings = await _context.Bookings.CountAsync(),

                FlightsByAirline = await _context.Flights
                    .Where(f => f.Airline != null)
                    .GroupBy(f => f.Airline.Name)
                    .Select(g => new { Name = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Name, x => x.Count),

                RecentFlights = await _context.Flights
                    .Include(f => f.Airline)
                    .Include(f => f.Airplane)
                    .Include(f => f.DepartureAirport)
                    .Include(f => f.ArrivalAirport)
                    .OrderByDescending(f => f.DepartureTime)
                    .Take(5)
                    .ToListAsync(),

                RecentBookings = await _context.Bookings
                    .Include(b => b.Flight)
                    .OrderByDescending(b => b.BookingDate)
                    .Take(5)
                    .ToListAsync()
            };

            // generate simple fake data for monthly chart
            var months = Enumerable.Range(0, 6)
                .Select(i => DateTime.Now.AddMonths(-i))
                .OrderBy(m => m)
                .ToList();

            model.MonthlyLabels = months.Select(m => m.ToString("MMM yyyy")).ToList();
            model.MonthlyFlights = months.Select(m =>
                _context.Flights.Count(f => f.DepartureTime.Month == m.Month && f.DepartureTime.Year == m.Year)
            ).ToList();

            return View(model);
        }

        public IActionResult Reports()
        {
            return View();
        }
    }
}
