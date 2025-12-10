using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class ProductController : Controller
{
  // this controller depends on the NorthwindRepository
  private DataContext _dataContext;
  public ProductController(DataContext db) => _dataContext = db;
  public IActionResult Category() => View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  public IActionResult Index(int id)
  {
    ViewBag.id = id;
    return View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  }
  public IActionResult Discount() => View(_dataContext.Discounts.Include(d => d.Product).Where(d => d.StartTime <= DateTime.Now && d.EndTime > DateTime.Now).OrderBy(d => d.EndTime).ToList());

  [Authorize(Roles = "northwind-employee")]
  public IActionResult AddDiscount(int id)
  {
    ViewBag.DiscountId = id;
    ViewBag.Product = new SelectList(_dataContext.Products.OrderBy(p => p.ProductName), "ProductId", "ProductName");
    return View(new Discount());
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  [Authorize(Roles = "northwind-employee")]
  public IActionResult AddDiscount(int id, Discount discount)
  {
    discount.DiscountId = id;
    if (ModelState.IsValid)
    {
      // Generate random 4-digit code
      Random random = new Random();
      discount.Code = random.Next(1000, 10000);
      
      _dataContext.AddDiscount(discount);
      return RedirectToAction("Discount", new { id = id });
    }
    @ViewBag.DiscountId = id;
    @ViewBag.Product = new SelectList(_dataContext.Products.OrderBy(p => p.ProductName), "ProductId", "ProductName");
    return View();
  }

  [Authorize(Roles = "northwind-employee")]
  public IActionResult DeleteDiscount(int id)
  {
    _dataContext.DeleteDiscount(_dataContext.Discounts.FirstOrDefault(b => b.DiscountId == id));
    return RedirectToAction("Discount");
  }

  [Authorize(Roles = "northwind-employee")]
  public IActionResult EditDiscount(int id)
  {
    var discount = _dataContext.Discounts.FirstOrDefault(d => d.DiscountId == id);
    if (discount == null)
    {
      return NotFound();
    }
    ViewBag.Product = new SelectList(_dataContext.Products.OrderBy(p => p.ProductName), "ProductId", "ProductName", discount.ProductId);
    return View(discount);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  [Authorize(Roles = "northwind-employee")]
  public IActionResult EditDiscount(Discount discount)
  {
    if (ModelState.IsValid)
    {
      _dataContext.EditDiscount(discount);
      return RedirectToAction("Discount");
    }
    ViewBag.Product = new SelectList(_dataContext.Products.OrderBy(p => p.ProductName), "ProductId", "ProductName", discount.ProductId);
    return View(discount);
  }
}
