using easySettle.Models;
using easySettle.Repo;
using easySettle.ViewModel;
using easySettle.ViewModel.paginator;
using Microsoft.AspNetCore.Mvc;

namespace easySettle.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly IGenericRepository<City> _cityRepository;

        public CitiesController(IGenericRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
        }

        //[Authorize]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var cities = await _cityRepository.GetAllAsync();

            var totalItems = cities.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var cityViewModel = cities
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(city => new CityViewModel
                {
                    Id = city.Id,
                    CityName = city.CityName,
                    Enable = city.Enable
                }).ToList();

            var paginationInfo = new PaginationInfoViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            var viewModel = new CityIndexViewModel
            {
                City = cityViewModel,
                PaginationInfo = paginationInfo
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CityViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var city = new City
                {
                    CityName = vm.CityName,
                    CreatedDate = DateTime.UtcNow
                };

                _cityRepository.Add(city);
                await _cityRepository.SaveAsync();

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        //[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetByIdAsync(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            var viewModel = new CityViewModel
            {
                Id = city.Id,
                CityName = city.CityName,
                UpdateBy = city.UpdateBy,
                UpdatedDate = DateTime.Now,
                Enable = city.Enable,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CityViewModel vm)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var city = new City
                {
                    Id = vm.Id,
                    CityName = vm.CityName,
                    UpdateBy = vm.UpdateBy,
                    UpdatedDate = DateTime.Now,
                    Enable = vm.Enable,
                };

                _cityRepository.Update(city);
                await _cityRepository.SaveAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetByIdAsync(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            var viewModel = new CityViewModel
            {
                Id = city.Id,
                IsDeleted = true,
                DeletedDate = DateTime.UtcNow,
            };

            return View(viewModel);
        }

        //[Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetByIdAsync(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            var viewModel = new CityViewModel
            {
                Id = city.Id,
                CityName = city.CityName,
                UpdateBy = city.UpdateBy,
                UpdatedDate = city.UpdatedDate,
                CreatedDate = city.CreatedDate,
                Enable = city.Enable,
            };

            return View(viewModel);
        }
    }
}