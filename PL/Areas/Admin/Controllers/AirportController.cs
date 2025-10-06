using BLLProject.Interfaces;
using DAL.models;
using Microsoft.AspNetCore.Mvc;
using PL.ViewModels;

namespace PL.Areas.Admin.Controllers
{
    public class AirportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region  API Calls (Data Tables)

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var airports = await _unitOfWork.Repository<Airport>().GetAllAsync();
            var airportsVM = airports.Select(s => (AirportViewModel)s).ToList();

            return Json(new { data = airportsVM });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return BadRequest("Id parameter is required.");

            var airportToBeDeleted = await _unitOfWork.Repository<Airport>().GetByIdAsync(id.Value);
            if (airportToBeDeleted is null)
            {
                return Json(new { success = false, message = "Airport not found" });
            }

            _unitOfWork.Repository<Airport>().Delete(airportToBeDeleted);
            await _unitOfWork.CompleteAsync();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AirportViewModel obj)
        {
            if(ModelState.IsValid)
            {
                await _unitOfWork.Repository<Airport>().AddAsync((Airport)obj);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["success"] = "Airport has been Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(obj);
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest("Id parameter is required.");
            var airport = await _unitOfWork.Repository<Airport>().GetByIdAsync(id.Value);

            if (airport is null)
                return NotFound();

            return View((AirportViewModel)airport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, AirportViewModel obj)
        {
            if (id != obj.Id)  
                return BadRequest();

            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<Airport>().Update((Airport)obj);
                int count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["success"] = "Airport Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(obj);
        }

        #endregion

    }
}
