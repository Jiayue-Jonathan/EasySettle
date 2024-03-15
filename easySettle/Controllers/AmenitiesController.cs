using easySettle.Models;
using easySettle.Repo;
using easySettle.ViewModel;
using easySettle.ViewModel.paginator;
using Microsoft.AspNetCore.Mvc;

namespace easySettle.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class AmenitiesController : Controller
    {
        private readonly IGenericRepository<Amenities> _amenitiesRepository;

        public AmenitiesController(IGenericRepository<Amenities> amenitiesRepository)
        {
            _amenitiesRepository = amenitiesRepository;
        }

        //[Authorize]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var amenities = await _amenitiesRepository.GetAllAsync();

            var totalItems = amenities.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var amenityViewModel = amenities
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(amenity => new AmenityViewModel
                {
                    Id = amenity.Id,
                    Name = amenity.Name,
                    Enable = amenity.Enable
                }).ToList();

            var paginationInfo = new PaginationInfoViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            var viewModel = new AmenitiesIndexViewModel
            {
                Amenities = amenityViewModel,
                PaginationInfo = paginationInfo
            };

            return View(viewModel);
        }


        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AmenityViewModel vm)
        {
            /* if (ModelState.IsValid)
             {
                 var amenity = new Amenities
                 {
                     Name = vm.Name,
                     CreatedDate = DateTime.UtcNow
                 };

                 _amenitiesRepository.Add(amenity);
                 await _amenitiesRepository.SaveAsync();
                 return RedirectToAction("Index");

             }*/
            return View(vm);
        }

        //[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenity = await _amenitiesRepository.GetByIdAsync(id.Value);

            if (amenity == null)
            {
                return NotFound();
            }

            var viewModel = new AmenityViewModel
            {
                Id = amenity.Id,
                Name = amenity.Name,
                UpdateBy = amenity.UpdateBy,
                UpdatedDate = DateTime.Now,
                Enable = amenity.Enable,
            };

            return View(viewModel);
        }

        /*[HttpPost]
        public async Task<IActionResult> Edit(int id, AmenityViewModel vm)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var amenity = new Amenities
                {
                    Id = vm.Id,
                    Name = vm.Name,
                    UpdateBy = vm.UpdateBy,
                    UpdatedDate = DateTime.Now,
                    Enable = vm.Enable,
                };

                _amenitiesRepository.Update(amenity);
                await _amenitiesRepository.SaveAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(vm);
        }*/

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenity = await _amenitiesRepository.GetByIdAsync(id.Value);

            if (amenity == null)
            {
                return NotFound();
            }

            var viewModel = new AmenityViewModel
            {
                Id = amenity.Id,
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

            var amenity = await _amenitiesRepository.GetByIdAsync(id.Value);

            if (amenity == null)
            {
                return NotFound();
            }

            var viewModel = new AmenityViewModel
            {
                Id = amenity.Id,
                Name = amenity.Name,
                UpdateBy = amenity.UpdateBy,
                UpdatedDate = amenity.UpdatedDate,
                CreatedDate = amenity.CreatedDate,
                Enable = amenity.Enable,
            };

            return View(viewModel);
        }
    }
}