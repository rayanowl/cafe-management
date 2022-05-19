using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Linq;

namespace CafeProject
{
    public partial class OrderForm : Form
    {
        public static Form theForm; 

        public static string ProductsDataPath = @"..\\..\\products.dat";
        public static string OrdersDataPath = @"..\\..\\orders.dat";
        public string tableName { get; set; }

        List<Product> Products { get; set; }
        List<Order> Orders = new List<Order>();

        public OrderForm(string TableName = "Bahçe 001")
        {
            InitializeComponent();
            theForm = this;
            tableName = TableName;
            this.Text = TableName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(System.IO.File.ReadAllText(ProductsDataPath)).OrderBy(xx => xx.Name).ToList();
            
            var Orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(System.IO.File.ReadAllText(OrdersDataPath)) ?? new List<Order>();

            listBox2.DisplayMember = "ProductValue";

            foreach (var item in Orders.Where(xx => xx.TableName == tableName))
            {
                //listBox2.Items.Add(item.Quantity + " x " + item.ProductName);
                listBox2.Items.Add(item);
            }

            var Cats = Products.Select(xx => xx.Category).Distinct().ToList();
            comboBox1.Items.Add("All");
            comboBox1.Items.AddRange(Products.Select(xx => xx.Category).Distinct().ToArray());
            comboBox1.SelectedIndex = 0;
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            new ItemQuantity(listBox1.SelectedItem.ToString().Split('>')[0].Trim(), tableName).ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(System.IO.File.ReadAllText(OrdersDataPath)) ?? new List<Order>();

            var OrdersNew = Orders.Where(xx => xx.TableName != tableName).ToList();

            foreach (Order item in listBox2.Items)
            {
                item.IsPrepared = Orders.FirstOrDefault(xx => xx.OrderID == item.OrderID)?.IsPrepared ?? false;
                item.OrderID = Orders.FirstOrDefault(xx => xx.OrderID == item.OrderID)?.OrderID ?? Guid.NewGuid().ToString();
                OrdersNew.Add(item);
            }

            System.IO.File.WriteAllText(OrdersDataPath, Newtonsoft.Json.JsonConvert.SerializeObject(OrdersNew, Formatting.Indented));
            this.Close();
        }

        private void ListBox2_DoubleClick(object sender, EventArgs e)
        {
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var Product = Products.FirstOrDefault(xx => xx.Name == listBox1.SelectedItem.ToString().Split('>')[0].Trim());
                pictureBox1.Image = new Bitmap("..\\..\\images\\" + Product.Image) ?? new Bitmap(0, 0);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch { }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            pictureBox1.Image = null;
            if (((ComboBox)sender).Text == "All")
            {
                listBox1.Items.AddRange(Products.Select(xx => new { Name = xx.Name + " > " + xx.Price + " TL" }).Select(xx => xx.Name).ToArray());
            }
            else
            {
                listBox1.Items.AddRange(Products.Where(xx => xx.Category == ((ComboBox)sender).Text).Select(xx => new { Name = xx.Name + " > " + xx.Price + " TL" }).Select(xx => xx.Name).ToArray());
            }
        }

    }
}
