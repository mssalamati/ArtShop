//using ArtShop.Util;
//using DataLayer;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace ArtShop.Controllers
//{
//    public class CardController : Controller
//    {
//        ApplicationDbContext storeDB = new ApplicationDbContext();
//        // GET: Card
//        public ActionResult Index()
//        {
//            var cart = CartManager.GetCart(this.HttpContext);

//            // Set up our ViewModel
//            var viewModel = new ShoppingCartViewModel
//            {
//                CartItems = cart.GetCartItems(),
//                CartTotal = cart.GetTotal()
//            };
//            // Return the view
//            return View(viewModel);
//        }

//        //
//        // GET: /Store/AddToCart/5
//        public ActionResult AddToCart(int id)
//        {
//            // Retrieve the album from the database
//            var addedAlbum = storeDB.Products.Find(id);

//            // Add it to the shopping cart
//            var cart = CartManager.GetCart(this.HttpContext);

//            cart.AddToCart(addedAlbum);

//            // Go back to the main store page for more shopping
//            return RedirectToAction("Index");
//        }
//        //
//        // AJAX: /ShoppingCart/RemoveFromCart/5
//        [HttpPost]
//        public ActionResult RemoveFromCart(int id)
//        {
//            // Remove the item from the cart
//            var cart = CartManager.GetCart(this.HttpContext);

//            // Get the name of the album to display confirmation
//            string Pname = storeDB.ShoppingCarts
//                .Single(item => item.Id == id).Product.Name;

//            // Remove from cart
//            decimal itemCount = cart.RemoveAllFromCart(id);

//            // Display the confirmation message
//            var results = new ShoppingCartRemoveViewModel
//            {
//                Message = Server.HtmlEncode(Pname) +
//                    " با موفقیت از سبد خرید حذف شد ",
//                CartTotal = cart.GetTotal(),
//                CartCount = cart.GetCount(),
//                ItemCount = itemCount,
//                DeleteId = id
//            };
//            return Json(results);
//        }

//        [HttpPost]
//        public ActionResult changecnt(int id, int cnt)
//        {
//            // Remove the item from the cart
//            var cart = CartManager.GetCart(this.HttpContext);

//            // Get the name of the album to display confirmation
//            string Pname = storeDB.ShoppingCarts
//                .Single(item => item.Id == id).Product.Name;

//            // Remove from cart
//            decimal itemCount = cart.newcnt(id, cnt);

//            // Display the confirmation message
//            var results = new ShoppingCartRemoveViewModel
//            {
//                Message = Server.HtmlEncode(Pname) +
//                    " تغییر کرد ",
//                CartTotal = cart.GetTotal(),
//                CartCount = cart.GetCount(),
//                ItemCount = itemCount,
//                DeleteId = id
//            };
//            return Json(results);
//        }


//        //
//        // GET: /ShoppingCart/CartSummary
//        [ChildActionOnly]
//        public ActionResult CartSummary()
//        {
//            var cart = CartManager.GetCart(this.HttpContext);

//            ViewData["CartCount"] = cart.GetCount();
//            return PartialView("CartSummary");
//        }
//    }
//}