using easySettle.Models;
using easySettle.Repo;
using easySettle.ViewModel;
using easySettle.ViewModel.paginator;
using Microsoft.AspNetCore.Mvc;

namespace easySettle.Controllers
{

    // [Authorize(Roles = "Admin")]
    public class BuildingAmenitiesController : Controller
    {
        private readonly IGenericRepository<BuildingAmenities> _buildingAmenitiesRepository;
        public BuildingAmenitiesController(IGenericRepository<BuildingAmenities> buildingAmenitiesRepository)
        {
            _buildingAmenitiesRepository = buildingAmenitiesRepository;
        }

        //[Authorize]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var buildingAmenities = await _buildingAmenitiesRepository.GetAllAsync();

            var totalItems = buildingAmenities.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var buildingViewModel = buildingAmenities
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(building => new BuildingAmenitiesViewModel
                {
                    Id = building.Id,
                    Name = building.Name,
                    Enable = building.Enable
                }).ToList();

            var paginationInfo = new PaginationInfoViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            var viewModel = new BuildingAmenitiesIndexViewModel
            {
                BuildingAmenities = buildingViewModel,
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
        public async Task<IActionResult> Create(BuildingAmenitiesViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var building = new BuildingAmenities
                {
                    Name = vm.Name,
                    CreatedDate = DateTime.UtcNow
                };

                _buildingAmenitiesRepository.Add(building);
                await _buildingAmenitiesRepository.SaveAsync();

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

            var building = await _buildingAmenitiesRepository.GetByIdAsync(id.Value);

            if (building == null)
            {
                return NotFound();
            }

            var viewModel = new BuildingAmenitiesViewModel
            {
                Id = building.Id,
                Name = building.Name,
                UpdateBy = building.UpdateBy,
                UpdatedDate = DateTime.Now,
                Enable = building.Enable,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BuildingAmenitiesViewModel vm)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var building = new BuildingAmenities
                {
                    Id = vm.Id,
                    Name = vm.Name,
                    UpdateBy = vm.UpdateBy,
                    UpdatedDate = DateTime.Now,
                    Enable = vm.Enable,
                };

                _buildingAmenitiesRepository.Update(building);
                await _buildingAmenitiesRepository.SaveAsync();
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

            var building = await _buildingAmenitiesRepository.GetByIdAsync(id.Value);

            if (building == null)
            {
                return NotFound();
            }

            var viewModel = new BuildingAmenitiesViewModel
            {
                Id = building.Id,
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

            var building = await _buildingAmenitiesRepository.GetByIdAsync(id.Value);

            if (building == null)
            {
                return NotFound();
            }

            var viewModel = new BuildingAmenities
            {
                Id = building.Id,
                Name = building.Name,
                UpdateBy = building.UpdateBy,
                UpdatedDate = building.UpdatedDate,
                CreatedDate = building.CreatedDate,
                Enable = building.Enable,
            };

            return View(viewModel);
        }
    }
}

