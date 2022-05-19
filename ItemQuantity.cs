using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeProject
{
    public partial class ItemQuantity : Form
    {
        public string ItemName { get; set; }
        public string TableName { get; set; }
        public ItemQuantity(string item, string tableName)
        {
            InitializeComponent();
            ItemName = item;
            TableName = tableName;
            this.Text = ItemName;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AddToBasket();
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                AddToBasket();
                e.Handled = true;
            }


            if (textBox1.Text.Length >= 3 && e.KeyChar != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        void AddToBasket()
        {
            if (String.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = "0";

            if (int.Parse(textBox1.Text) > 0)
            {
                var listBox = (ListBox)OrderForm.theForm.Controls.Find("listBox2", true).First();
                listBox.DisplayMember = "ProductValue";

                Product product = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(System.IO.File.ReadAllText(OrderForm.ProductsDataPath)).OrderBy(xx => xx.Name).ToList().FirstOrDefault(xx => xx.Name == ItemName);

                var order = new Order();
                order.OrderID = null;
                order.ProductID = product.ProductID;
                order.ProductName = product.Name;
                order.Price = product.Price;
                order.Quantity = int.Parse(textBox1.Text);
                order.IsPrepared = false;
                order.OrderNote = txtOrderNote.Text;
                order.TableName = TableName;
                listBox.Items.Add(order);

                this.Close();
            }
            else
            {
                MessageBox.Show("Lütfen adet belirtin.");
            }
        }

        private void ItemQuantity_Load(object sender, EventArgs e)
        {
            label1.Text = ItemName;
        }
    }
}
