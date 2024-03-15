using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace easySettle.Controllers
{
    public class PropnuevoController1 : Controller
    {
        // GET: PropnuevoController1
        public ActionResult Index()
        {
            return View();
        }

        // GET: PropnuevoController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PropnuevoController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropnuevoController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropnuevoController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PropnuevoController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropnuevoController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PropnuevoController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
