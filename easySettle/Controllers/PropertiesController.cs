using easySettle.Data;
using easySettle.Models;
using easySettle.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace easySettle.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PropertiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Properties.Include(p => p.Amenities).Include(p => p.BuildingAmenities).Include(p => p.PropertyType);
            var applicationDbContext = _context.Properties;
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult AllProperties()
        {
            var properties = _context.Properties.ToList();

            return View(properties);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Properties == null)
            {
                return NotFound();
            }

            var properties = await _context.Properties
                //   .Include(p => p.Amenities)
                //.Include(p => p.BuildingAmenities)
                //  .Include(p => p.PropertyType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (properties == null)
            {
                return NotFound();
            }

            return View(properties);
        }

        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.City, "Id", "CityName");
            ViewBag.Amenities = _context.Amenities.ToList();
            ViewBag.BuildingAmenities = _context.BuildingAmenities.ToList();
            ViewData["PropertyTypeId"] = new SelectList(_context.PropertyType, "Id", "Name");
            ViewData["BuildingAmenitiesId"] = new SelectList(_context.BuildingAmenities, "Id", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertiesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var photoPaths = new List<string>();

                foreach (var propertyInfo in model.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType == typeof(IFormFile))
                    {
                        var photo = (IFormFile)propertyInfo.GetValue(model);
                        if (photo != null && photo.Length > 0)
                        {
                            // Generate a unique filename
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                            var filePath = Path.Combine("wwwroot/uploadGallery", fileName);
                            photoPaths.Add(fileName);

                            // Save the file to the server
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }
                            // fileName = model.PicFile1.ToString();
                        }
                    }
                }

                var property = new Properties
                {
                    Name = model.Name,
                    StreetName = model.StreetName,
                    ZipCode = model.ZipCode,
                    RentPrice = model.RentPrice,
                    Rooms = model.Rooms,
                    CityId = model.CityId,
                    BathRooms = model.BathRooms,
                    Description = model.Description,
                    Lat = model.Lat,
                    Lon = model.Lon,
                    Sqft = model.Sqft,
                    PetFriendly = model.PetFriendly,
                    AmenitiesId = model.AmenitiesId,
                    BuildingAmenitiesId = model.BuildingAmenitiesId,
                    PropertyTypeId = model.PropertyTypeId,
                    PicFile1 = photoPaths.ElementAtOrDefault(0),
                    PicFile2 = photoPaths.ElementAtOrDefault(1),
                    PicFile3 = photoPaths.ElementAtOrDefault(2),
                    PicFile4 = photoPaths.ElementAtOrDefault(3),
                    PicFile5 = photoPaths.ElementAtOrDefault(4),
                };

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                foreach (var amenityId in model.SelectedAmenities)
                {
                    var propertyAmenity = new PropertyAmenity
                    {
                        PropertyId = property.Id,
                        AmenityId = amenityId
                    };
                    _context.PropertyAmenity.Add(propertyAmenity);
                }

                foreach (var buildingAmenityId in model.SelectedBuildingAmenities)
                {
                    var buildingAmenity = new PropertyBuildingAmenity
                    {
                        PropertyId = property.Id,
                        BuildingAmenitiesId = buildingAmenityId
                    };
                    _context.PropertyBuildingAmenity.Add(buildingAmenity);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Amenities = _context.Amenities.ToList();
            ViewBag.BuildingAmenities = _context.BuildingAmenities.ToList();
            return View(model);
        }


        public IActionResult Edit(int id)
        {
            //var property = _context.Properties.Include(p => p.PropertyAmenity).Include(p => p.PropertyBuildingAmenity).FirstOrDefault(p => p.Id == id);
            var property = _context.Properties.Include(p => p.PropertyAmenity).FirstOrDefault(p => p.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            ViewData["CityId"] = new SelectList(_context.City, "Id", "CityName");
            ViewBag.Amenities = _context.Amenities.ToList();
            ViewBag.BuildingAmenities = _context.BuildingAmenities.ToList();
            ViewData["PropertyTypeId"] = new SelectList(_context.PropertyType, "Id", "Name");

            // Create an instance of PropertiesViewModel and populate it with existing data
            var model = new PropertiesEditViewModel
            {
                Id = property.Id,
                Name = property.Name,
                StreetName = property.StreetName,
                ZipCode = property.ZipCode,
                RentPrice = property.RentPrice,
                Rooms = property.Rooms,
                CityId = property.CityId,
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
                //SelectedAmenities = property.PropertyAmenity.Select(pa => pa.AmenityId).ToList(),
                //SelectedBuildingAmenities = property.PropertyBuildingAmenities.Select(pba => pba.BuildingAmenitiesId).ToList(),
                // Add other properties as needed
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PropertiesEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var photoPaths = new List<string>();

                foreach (var propertyInfo in model.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType == typeof(IFormFile))
                    {
                        var photo = (IFormFile)propertyInfo.GetValue(model);
                        if (photo != null && photo.Length > 0)
                        {
                            // Generate a unique filename
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                            var filePath = Path.Combine("wwwroot/uploadGallery", fileName);
                            photoPaths.Add(fileName);

                            // Save the file to the server
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }
                        }
                        else
                        {
                            // If no new photo is provided, retain the existing photo path
                            var existingPhotoPath = propertyInfo.GetValue(model);
                            if (existingPhotoPath != null)
                            {
                                photoPaths.Add(existingPhotoPath.ToString());
                            }
                        }
                    }
                }

                var property = new Properties
                {
                    Id = model.Id,
                    Name = model.Name,
                    StreetName = model.StreetName,
                    ZipCode = model.ZipCode,
                    RentPrice = model.RentPrice,
                    Rooms = model.Rooms,
                    CityId = model.CityId,
                    BathRooms = model.BathRooms,
                    Description = model.Description,
                    Lat = model.Lat,
                    Lon = model.Lon,
                    Sqft = model.Sqft,
                    PetFriendly = model.PetFriendly,
                    AmenitiesId = model.AmenitiesId,
                    BuildingAmenitiesId = model.BuildingAmenitiesId,
                    PropertyTypeId = model.PropertyTypeId,
                    PicFile1 = model.PicFile1,
                    PicFile2 = model.PicFile2,
                    PicFile3 = model.PicFile3,
                    PicFile4 = model.PicFile4,
                    PicFile5 = model.PicFile5
                };

                _context.Properties.Update(property);
                await _context.SaveChangesAsync();

                // Update PropertyAmenities and PropertyBuildingAmenities
                var existingAmenities = await _context.PropertyAmenity.Where(pa => pa.PropertyId == id).ToListAsync();
                _context.PropertyAmenity.RemoveRange(existingAmenities);

                var existingBuildingAmenities = await _context.PropertyBuildingAmenity.Where(pba => pba.PropertyId == id).ToListAsync();
                _context.PropertyBuildingAmenity.RemoveRange(existingBuildingAmenities);

                foreach (var amenityId in model.SelectedAmenities)
                {
                    var propertyAmenity = new PropertyAmenity
                    {
                        PropertyId = property.Id,
                        AmenityId = amenityId
                    };
                    _context.PropertyAmenity.Add(propertyAmenity);
                }

                foreach (var buildingAmenityId in model.SelectedBuildingAmenities)
                {
                    var buildingAmenity = new PropertyBuildingAmenity
                    {
                        PropertyId = property.Id,
                        BuildingAmenitiesId = buildingAmenityId
                    };
                    _context.PropertyBuildingAmenity.Add(buildingAmenity);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CityId"] = new SelectList(_context.City, "Id", "CityName");
            ViewBag.Amenities = _context.Amenities.ToList();
            ViewBag.BuildingAmenities = _context.BuildingAmenities.ToList();
            ViewData["PropertyTypeId"] = new SelectList(_context.PropertyType, "Id", "Name");

            return View(model);
        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Properties == null)
            {
                return NotFound();
            }

            var properties = await _context.Properties
                //.Include(p => p.Amenities)
                //  .Include(p => p.BuildingAmenities)
                //.Include(p => p.PropertyType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (properties == null)
            {
                return NotFound();
            }

            return View(properties);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Properties == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Properties'  is null.");
            }
            var properties = await _context.Properties.FindAsync(id);
            if (properties != null)
            {
                _context.Properties.Remove(properties);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertiesExists(int id)
        {
            return (_context.Properties?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
