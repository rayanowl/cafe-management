using Newtonsoft.Json;
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
    public partial class RecieptForm : Form
    {
        public string TableName { get; set; }
        public static string OrdersDataPath = @"..\\..\\orders.dat";
        List<Order> Orders = new List<Order>();
        public RecieptForm(string tableName)
        {
            InitializeComponent();
            TableName = tableName;
        }

        private void RecieptForm_Load(object sender, EventArgs e)
        {
            Orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(System.IO.File.ReadAllText(OrdersDataPath)).Where(xx => xx.TableName == TableName).ToList() ?? new List<Order>();
            listBox1.DisplayMember = "ProductValueWithPrice";
            listBox1.Items.AddRange(Orders.ToArray());

            GetTotal();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            ListBox.ObjectCollection lst = listBox1.Items;
            
            string txt = String.Format("Total: {0} TL", (listBox1.SelectedItems.Cast<Order>().ToList()).ToList().Sum(xx=> (xx.Price * xx.Quantity)));

            var result = MessageBox.Show(txt, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(System.IO.File.ReadAllText(OrdersDataPath)) ?? new List<Order>();

                foreach (Order item in listBox1.SelectedItems)
                {
                    Orders.Remove(Orders.FirstOrDefault(xx => xx.OrderID == item.OrderID));
                }

                System.IO.File.WriteAllText(OrdersDataPath, Newtonsoft.Json.JsonConvert.SerializeObject(Orders, Formatting.Indented));
                RefreshListBox();
            }
            GetTotal();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string txt = String.Format("Total: {0} TL", (listBox1.Items.Cast<Order>().ToList()).ToList().Sum(xx=> (xx.Price * xx.Quantity)));
            var result = MessageBox.Show(txt, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(System.IO.File.ReadAllText(OrdersDataPath)) ?? new List<Order>();

                foreach (Order item in listBox1.Items)
                {
                    Orders.Remove(Orders.FirstOrDefault(xx => xx.OrderID == item.OrderID));
                }

                System.IO.File.WriteAllText(OrdersDataPath, Newtonsoft.Json.JsonConvert.SerializeObject(Orders, Formatting.Indented));

                RefreshListBox();
            }
            GetTotal();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTotal();
        }

        void RefreshListBox()
        {
            listBox1.Items.Clear();
            Orders = Orders.Where(xx => xx.TableName == TableName).ToList() ?? new List<Order>();
            listBox1.DisplayMember = "ProductValueWithPrice";
            listBox1.Items.AddRange(Orders.ToArray());
        }

        void GetTotal()
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                label1.Text = String.Format("Total: {0} TL", (listBox1.Items.Cast<Order>().ToList()).ToList().Sum(xx => (xx.Price * xx.Quantity)));
            }
            else
            {
                label1.Text = String.Format("Selected {1} Item(s): {0} TL", (listBox1.SelectedItems.Cast<Order>().ToList()).ToList().Sum(xx => (xx.Price * xx.Quantity)), listBox1.SelectedItems.Count);
            }
        }

    }
}
