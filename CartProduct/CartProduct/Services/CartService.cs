using CartProduct.Models;
using System.Text.Json;

public class CartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string SessionKey = "Cart";

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public List<CartItem> GetCart()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var cartJson = session?.GetString(SessionKey);
        return string.IsNullOrEmpty(cartJson)
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
    }

    public void SaveCart(List<CartItem> cart)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.SetString(SessionKey, JsonSerializer.Serialize(cart));
    }

    public void AddToCart(Product product)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == product.Id);
        if (item == null)
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1
            });
        }
        else
        {
            item.Quantity++;
        }
        SaveCart(cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            item.Quantity = quantity;
            SaveCart(cart);
        }
    }

    public void Remove(int productId)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }
    }
}
