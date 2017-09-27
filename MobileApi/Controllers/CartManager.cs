using DataLayer;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApi.Controllers
{
    public class CartManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static CartManager GetCart(HttpContext context)
        {
            var cart = new CartManager();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public void AddToCart(Product p, Ordertype type)
        {
            // Get the matching cart and album instances
            var cartItem = db.ShoppingCarts.SingleOrDefault(
                c => c.CartNumber == ShoppingCartId
                && c.ProductId == p.Id);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new ShoppingCart
                {
                    ProductId = p.Id,
                    CartNumber = ShoppingCartId,
                    Quantity = 1,
                    CreateDate = DateTime.Now,
                    type = type,
                };
                db.ShoppingCarts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Quantity++;
            }
            // Save changes
            db.SaveChanges();
        }

        public decimal RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = db.ShoppingCarts.Single(
                cart => cart.CartNumber == ShoppingCartId
                && cart.Id == id);

            decimal itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    db.ShoppingCarts.Remove(cartItem);
                }
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }

        public decimal newcnt(int id, int newval)
        {
            // Get the cart
            var cartItem = db.ShoppingCarts.Single(
                cart => cart.CartNumber == ShoppingCartId
                && cart.Id == id);

            decimal itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity != 0)
                {
                    cartItem.Quantity = newval;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    db.ShoppingCarts.Remove(cartItem);
                }
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }
        public decimal RemoveAllFromCart(int id)
        {
            // Get the cart
            var cartItem = db.ShoppingCarts.Single(
                cart => cart.CartNumber == ShoppingCartId
                && cart.Id == id);

            decimal itemCount = 0;

            if (cartItem != null)
            {

                db.ShoppingCarts.Remove(cartItem);

                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.ShoppingCarts.Where(
                cart => cart.CartNumber == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.ShoppingCarts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }
        public List<ShoppingCart> GetCartItems()
        {
            return db.ShoppingCarts.Include("Product").Where(
                cart => cart.CartNumber == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.ShoppingCarts
                          where cartItems.CartNumber == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in db.ShoppingCarts
                              where cartItems.CartNumber == ShoppingCartId
                              select (int?)cartItems.Quantity *
                              cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Product = item.Product,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Quantity,
                    type = item.type
                };
                orderTotal += (item.Quantity * item.Product.Price);
                order.OrderDetails.Add(orderDetail);
            }
            order.TotalPrice = (double)orderTotal;
            order.TransactionDetail = new TransactionDetail() { amount = orderTotal, Method = PaymentMethod.zarinpall, currencyRate = 1 };
            db.Orders.Add(order);
            db.SaveChanges();
            return order.Id;
        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContext context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.ShoppingCarts.Where(
                c => c.CartNumber == ShoppingCartId);

            foreach (ShoppingCart item in shoppingCart)
            {
                item.CartNumber = userName;
            }
            db.SaveChanges();
        }
    }
}