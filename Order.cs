using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeProject
{
    class Order
    {


        public string OrderID { get; set; }
        public string TableName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsPrepared { get; set; }
        public string OrderNote { get; set; }
        public string ProductValue { get { return this.Quantity + " x " + this.ProductName; } }
        public string ProductValueWithPrice { get { return this.Quantity + " x " + this.ProductName +" - "+ (this.Price * this.Quantity) +" TL"; } }
        public string KitchenProductValue { get { return this.Quantity + " x " + this.ProductName + " -> " + this.TableName; } }

    }
}
