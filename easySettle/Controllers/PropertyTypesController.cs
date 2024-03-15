using easySettle.Models;
using easySettle.Repo;
using easySettle.ViewModel;
using easySettle.ViewModel.paginator;
using Microsoft.AspNetCore.Mvc;

namespace easySettle.Controllers
{
    public class PropertyTypesController : Controller
    {
        private readonly IGenericRepository<PropertyType> _propertyTypeRepository;

        public PropertyTypesController(IGenericRepository<PropertyType> propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        //[Authorize]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var propertyTypes = await _propertyTypeRepository.GetAllAsync();

            var totalItems = propertyTypes.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var propertyTypeViewModel = propertyTypes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(propertyTypes => new PropertyTypeViewModel
                {
                    Id = propertyTypes.Id,
                    Name = propertyTypes.Name,
                    Enable = propertyTypes.Enable
                }).ToList();

            var paginationInfo = new PaginationInfoViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            var viewModel = new PropertyTypeIndexViewModel
            {
                PropertyType = propertyTypeViewModel,
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
        public async Task<IActionResult> Create(PropertyTypeViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var propertyType = new PropertyType
                {
                    Name = vm.Name,
                    CreatedDate = DateTime.UtcNow
                };

                _propertyTypeRepository.Add(propertyType);
                await _propertyTypeRepository.SaveAsync();

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

            var propertyType = await _propertyTypeRepository.GetByIdAsync(id.Value);

            if (propertyType == null)
            {
                return NotFound();
            }

            var viewModel = new PropertyTypeViewModel
            {
                Id = propertyType.Id,
                Name = propertyType.Name,
                UpdateBy = propertyType.UpdateBy,
                UpdatedDate = DateTime.Now,
                Enable = propertyType.Enable,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PropertyTypeViewModel vm)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var propertyType = new PropertyType
                {
                    Id = vm.Id,
                    Name = vm.Name,
                    UpdateBy = vm.UpdateBy,
                    UpdatedDate = DateTime.Now,
                    Enable = vm.Enable,
                };

                _propertyTypeRepository.Update(propertyType);
                await _propertyTypeRepository.SaveAsync();
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

            var propertyType = await _propertyTypeRepository.GetByIdAsync(id.Value);

            if (propertyType == null)
            {
                return NotFound();
            }

            var viewModel = new PropertyTypeViewModel
            {
                Id = propertyType.Id,
                IsDeleted = true,
                DeletedDate = DateTime.UtcNow,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyType = await _propertyTypeRepository.GetByIdAsync(id.Value);

            if (propertyType == null)
            {
                return NotFound();
            }

            var viewModel = new PropertyTypeViewModel
            {
                Id = propertyType.Id,
                Name = propertyType.Name,
                UpdateBy = propertyType.UpdateBy,
                UpdatedDate = propertyType.UpdatedDate,
                CreatedDate = propertyType.CreatedDate,
                Enable = propertyType.Enable,
            };

            return View(viewModel);
        }
    }
}
