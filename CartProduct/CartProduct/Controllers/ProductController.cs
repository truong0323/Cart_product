using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CartService _cart;

    public ProductController(ApplicationDbContext context, CartService cart)
    {
        _context = context;
        _cart = cart;
    }

    public IActionResult Index()
    {
        var products = _context.Products.ToList();
        return View(products);
    }

    public IActionResult AddToCart(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _cart.AddToCart(product);
            TempData["message"] = "Đã thêm sản phẩm vào giỏ hàng.";
        }
        return RedirectToAction("Index");
    }
}
