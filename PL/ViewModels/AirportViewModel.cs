using DAL.models;

namespace PL.ViewModels
{
    public class AirportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        #region Mapping

        public static explicit operator AirportViewModel(Airport model)
        {
            return new AirportViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                City = model.City,
                Country = model.Country,
            };
        }

        public static explicit operator Airport(AirportViewModel viewModel)
        {
            return new Airport
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Code = viewModel.Code,
                City = viewModel.City,
                Country = viewModel.Country,
            };
        }

        #endregion
    }
}
