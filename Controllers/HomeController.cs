using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Controllers
{
    public class HomeController(DataContext db) : Controller
    {
        // this controller depends on the DataContext
        private readonly DataContext _dataContext = db;

        [Authorize(Roles = "northwind-employee")]
        public IActionResult Index()
        {
            var discounts = _dataContext.Discounts
                .Include(d => d.Product)
                .Where(d => d.StartTime <= DateTime.Now && d.EndTime > DateTime.Now)
                .Take(3)
                .ToList();

            ViewBag.Products = _dataContext.Products.ToList(); //--Uncomment this line if using bootstrap modal UI on homepage
            return View(discounts);
        }
    
    //Create Discount UI
        [Authorize(Roles = "northwind-employee")]
        public IActionResult AddDiscount()
        {
            return View();
        }

    //Actually add newly created discount DB
    
        [HttpPost]
        [Authorize(Roles = "northwind-employee")]
        public IActionResult AddDiscount(Discount model)
        {
            model.Code = Random.Shared.Next(10000000, 99999999);
            _dataContext.Discounts.Add(model);
            _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }

    //Edit Disount UI
        [Authorize(Roles = "northwind-employee")]
        public IActionResult EditDiscount(int id)
        {
            var d = _dataContext.Discounts.Find(id);
            ViewBag.Products = _dataContext.Products.ToList();
            return View(d);
        }

    //Update Discount DB
        [HttpPost]
        [Authorize(Roles = "northwind-employee")]
        public IActionResult EditDiscount(Discount model)
        {
            _dataContext.Discounts.Update(model);
            _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }

    //Delete Discount UI and DB
        [Authorize(Roles = "northwind-employee")]
        public IActionResult DeleteDiscount(int id)
        {
            var d = _dataContext.Discounts.Find(id);
            _dataContext.Discounts.Remove(d);
            _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
