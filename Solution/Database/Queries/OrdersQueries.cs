using AQuestTest.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace Database
{
    public class OrdersQueries : BaseQueries
    {
        public static List<Orders> Orders()
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                return db.Orders.Include(x => x.OrderStates).ToList();
            }
        }

        public static Orders OrderById(int id)
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                return db.Orders.Where(x => x.Id == id).First();
            }
        }

        public static Orders OrderDetailById(int id)
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                return db.Orders
                    .Where(x => x.Id == id)
                    .Include(x => x.Orders_Products)
                    .Include(x => x.Coupons)
                    .Include(x => x.Orders_Products.Select(op => op.Products))
                    .First();
            }
        }

        public static Orders OrderWithUserInfoById(int id)
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                return db.Orders
                    .Where(x => x.Id == id)
                    .Include(x => x.UserInfos)
                    .First(x => x.Id == id);
            }
        }

        public static bool isQuantityInStock(int idProduct, int newQuantity)
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                Products product = db.Products.First(x => x.Id == idProduct);
                return newQuantity <= product.StockQty;
            }
        }

        public static List<string> CheckQuantitiesOrder(int id)
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                Orders order = db.Orders
                    .Include(x => x.Orders_Products)
                    .Include(x => x.Orders_Products.Select(op => op.Products))
                    .First(x => x.Id == id);

                List<string> list = new List<string>();

                foreach (Orders_Products op in order.Orders_Products)
                {
                    if (op.Quantity > op.Products.StockQty)
                    {
                        list.Add($"-Diminuire la quantità il prodotto <strong>{op.Products.Name}</strong>; pezzi disponibili: <strong>{op.Products.StockQty}</strong>");
                    }
                }

                return list;
            }
        }

        public static Orders UpdateInfoProductOrder(int idProduct, int idOrder, int newQuantity)
        {
            Orders order = new Orders();

            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                order = db.Orders
                    .Include(x => x.Coupons)
                    .Include(x => x.Orders_Products)
                    .First(x => x.Id == idOrder);

                Orders_Products orderProduct = order.Orders_Products
                    .First(x => x.ProductsId == idProduct);

                orderProduct.Quantity = Math.Abs(newQuantity);

                order.TotalPrice = order.TotalPriceWithoutDiscount = 0;

                foreach (Orders_Products op in order.Orders_Products)
                {
                    order.TotalPriceWithoutDiscount += op.Quantity * op.Products.Price;
                }

                if (order.Coupons == null)
                {
                    order.TotalPrice = order.TotalPriceWithoutDiscount;
                }
                else
                {
                    order.TotalPrice = Math.Round(order.TotalPriceWithoutDiscount - ((order.TotalPriceWithoutDiscount / 100) * order.Coupons.PercentageDiscount), 2);
                }

                if (orderProduct.Quantity == 0)
                {
                    order.Orders_Products.Remove(orderProduct);
                }

                // update order entity
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                Products product = db.Products.First(x => x.Id == idProduct);
                product.StockQty = (newQuantity > 0) ? product.StockQty - 1 : product.StockQty + 1;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                return order;
            }
        }

        public static Orders RemoveProductOrder(int idProduct, int idOrder)
        {
            Orders order = new Orders();

            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                order = db.Orders
                    .Include(x => x.Coupons)
                    .Include(x => x.Orders_Products)
                    .Include(x => x.Orders_Products.Select(op => op.Products))
                    .First(x => x.Id == idOrder);

                Orders_Products orderProduct = order.Orders_Products
                    .First(x => x.ProductsId == idProduct);

                Products product = order.Orders_Products.First(x => x.ProductsId == idProduct).Products;
                product.StockQty = product.StockQty + orderProduct.Quantity;

                order.Orders_Products.Remove(orderProduct);

                order.TotalPrice = order.TotalPriceWithoutDiscount = 0;
                foreach (Orders_Products op in order.Orders_Products)
                {
                    order.TotalPriceWithoutDiscount += op.Quantity * op.Products.Price;
                }
                if (order.Coupons == null)
                {
                    order.TotalPrice = order.TotalPriceWithoutDiscount;
                }
                else
                {
                    order.TotalPrice = Math.Round(order.TotalPriceWithoutDiscount - ((order.TotalPriceWithoutDiscount / 100) * order.Coupons.PercentageDiscount), 2);
                }

                // remove the item product in OrdersProducts
                db.Entry(order).State = EntityState.Modified;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                return order;
            }
        }

        public static void CloseOrder(int id)
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                Orders order = db.Orders
                    .Include(x => x.Coupons)
                    .First(x => x.Id == id);
                order.DateOrder = System.DateTime.Now;

                order.OrderStatesId = 2;

                // Set coupons used
                if (order.Coupons != null)
                {
                    order.Coupons.Active = false;
                }

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                // Updates product quantity 
                foreach (Orders_Products op in order.Orders_Products)
                {
                    Products product = db.Products.First(x => x.Id == op.ProductsId);
                    product.StockQty = product.StockQty - op.Quantity;

                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteOrder(int id)
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                Orders order = db.Orders.First(x => x.Id == id);
                order.OrderStatesId = 3; // Cancellato

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
