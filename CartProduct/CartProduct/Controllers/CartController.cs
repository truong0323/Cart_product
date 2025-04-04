using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    private readonly CartService _cart;

    public CartController(CartService cart)
    {
        _cart = cart;
    }

    public IActionResult Index()
    {
        var cart = _cart.GetCart();
        var total = cart.Sum(item => item.Total);
        ViewBag.Total = total;
        return View(cart);
    }

    [HttpPost]
    public IActionResult Update(int productId, int quantity)
    {
        _cart.UpdateQuantity(productId, quantity);
        return RedirectToAction("Index");
    }

    public IActionResult Remove(int productId)
    {
        _cart.Remove(productId);
        return RedirectToAction("Index");
    }
}
