using easySettle.Data;
using easySettle.Models;
using easySettle.Repo;
using easySettle.ViewModel;
using easySettle.ViewModel.paginator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace easySettle.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenericRepository<Properties> _propertyRepository;
        private readonly ApplicationDbContext _context;


        public HomeController(IGenericRepository<Properties> propertyRepository,
            ApplicationDbContext context)
        {
            _context = context;
            _propertyRepository = propertyRepository;
        }

        //public IActionResult Index()
        //{
        //    var books = _propertyRepository.GetAllAsync();
        //    return View(books);
        //}

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Properties;
            // .Include(p => p.Amenities)
            // .Include(p => p.BuildingAmenities);
            //   .Include(p => p.PropertyType);

            var lastSixItems = await applicationDbContext
                .OrderByDescending(p => p.Id)
                .Take(6)
                .ToListAsync();

            return View(lastSixItems);
        }

        public async Task<IActionResult> Search(string searchProperty, int page = 1, int pageSize = 5)
        {
            var properties = await _propertyRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(searchProperty))
            {
                properties = properties
                    .Where(p =>
                 p.Name != null && p.Name.Trim().IndexOf(searchProperty, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 p.StreetName != null && p.StreetName.Trim().IndexOf(searchProperty, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 p.ZipCode != null && p.ZipCode.Trim().IndexOf(searchProperty, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 p.Description != null && p.Description.Trim().IndexOf(searchProperty, StringComparison.OrdinalIgnoreCase) >= 0)
             .ToList();
            }

            var totalItems = properties.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var propertyViewModels = properties
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(property => new PropertiesViewModelResults
                {
                    Id = property.Id,
                    Name = property.Name,
                    StreetName = property.StreetName,
                    ZipCode = property.ZipCode,
                    RentPrice = property.RentPrice,
                    Rooms = property.Rooms,
                    BathRooms = property.BathRooms,
                    Description = property.Description,
                    Lat = property.Lat,
                    Lon = property.Lon,
                    Sqft = property.Sqft,
                    PetFriendly = property.PetFriendly,
                    AmenitiesId = property.AmenitiesId,
                    BuildingAmenitiesId = property.BuildingAmenitiesId,
                    PropertyTypeId = property.PropertyTypeId,
                    PicFile1 = property.PicFile1,
                    // PicFile2 = property.PicFile2,
                    //PicFile3 = property.PicFile3,
                    // PicFile4 = property.PicFile4,
                    // PicFile5 = property.PicFile5,
                    /* PropertyType = property.PropertyType,
                     BuildingAmenities = property.BuildingAmenities,
                     Amenities = property.Amenities,
                     PropertyAmenity = property.PropertyAmenity*/
                }).ToList();

            var paginationInfo = new PaginationInfoViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            var viewModel = new PropertyIndexResultViewModel
            {
                Properties = propertyViewModels,
                PaginationInfo = paginationInfo
            };

            return View("SearchResults", viewModel);
        }


        public IActionResult HomeResults()
        {
            var properties = _context.Properties.ToList();

            return View(properties);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}