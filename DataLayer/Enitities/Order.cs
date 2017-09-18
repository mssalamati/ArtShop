using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class Order
    {
        [Key]
        public virtual int Id { get; set; }
        [ForeignKey("user")]
        public virtual String user_id { get; set; }
        [Display(Name = "کاربر")]
        public virtual UserProfile user { get; set; }
        public virtual DateTime BuyDate { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual double TotalPrice { get; set; }
        [ForeignKey("TransactionDetail")]
        public virtual int? TransactionDetailId { get; set; }
        public virtual TransactionDetail TransactionDetail { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string BuildingNumber { get; set; }
        public string ReceiverName { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }

        public Order()
        {
            BuyDate = DateTime.Now;
            Status = OrderStatus.NoSeen;
        }
    }

    public class TransactionDetail
    {
        [Key]
        public virtual int Id { get; set; }
        public string Description { get; set; }
        public DateTime date { get; set; }
        public string Number { get; set; }
        public string TransactionNumber { get; set; }
        public bool Payed { get; set; }
        public decimal amount { get; set; }
        public TransactionDetail()
        {
            date = DateTime.Now;
        }
    }

    public class OrderDetail
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual Ordertype type { get; set; }
        [ForeignKey("order")]
        public virtual int orderId { get; set; }
        public virtual Order order { get; set; }
        [ForeignKey("Product")]
        public virtual int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
    }

    public enum Ordertype
    {
        Orginal = 0,
        Print = 1,
    }
    public enum OrderStatus
    {
        NoSeen = 0,
        Seen = 1,
        Delivered = 2,
        Posted = 3
    }

    public static class ErrorLevelExtensions
    {
        public static string ToOrderStatus(this OrderStatus me)
        {
            switch (me)
            {
                case OrderStatus.NoSeen:
                    return ShareRes.OrderStatus_NoSeen;
                case OrderStatus.Seen:
                    return ShareRes.OrderStatus_Seen;
                case OrderStatus.Delivered:
                    return ShareRes.OrderStatus_Delivered;
                case OrderStatus.Posted:
                    return ShareRes.OrderStatus_Posted;
                default:
                    return "";
            }
        }
    }
}
