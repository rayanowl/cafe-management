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
    public partial class KitchenForm : Form
    {
        public static string OrdersDataPath = @"..\\..\\orders.dat";

        public KitchenForm()
        {
            InitializeComponent();
        }

        private void Kitchen_Load(object sender, EventArgs e)
        {
            var Orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(System.IO.File.ReadAllText(OrdersDataPath)) ?? new List<Order>();
            listBox1.DisplayMember = "KitchenProductValue";
            listBox1.Items.AddRange(Orders.Where(xx => xx.IsPrepared == false).ToArray());
        }

        private void Kitchen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            var PreparedOrder = ((Order)listBox1.SelectedItem);
            var result = MessageBox.Show(PreparedOrder.KitchenProductValue + "\n mark as prepared?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //listBox1.Items.Remove(listBox1.SelectedItem);
                listBox1.Items.Clear();

                var Orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(System.IO.File.ReadAllText(OrdersDataPath)) ?? new List<Order>();
                Orders.Remove(Orders.FirstOrDefault(xx => xx.OrderID == PreparedOrder.OrderID));

                PreparedOrder.IsPrepared = true;
                Orders.Add(PreparedOrder);

                System.IO.File.WriteAllText(OrdersDataPath, Newtonsoft.Json.JsonConvert.SerializeObject(Orders, Formatting.Indented));

                listBox1.DisplayMember = "KitchenProductValue";
                listBox1.Items.AddRange(Orders.Where(xx => xx.IsPrepared == false).ToArray());
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolTip1.AutoPopDelay = 15000;
            toolTip1.InitialDelay = 1;
            toolTip1.ReshowDelay = 1;
            toolTip1.ShowAlways = true;
            toolTip1.IsBalloon = true;
            toolTip1.ToolTipTitle = "Order Note";
            toolTip1.ToolTipIcon = ToolTipIcon.Warning;
            toolTip1.UseAnimation = true;
            toolTip1.UseFading = true;
            if (listBox1.SelectedItem != null)
                toolTip1.SetToolTip(listBox1, ((Order)listBox1.SelectedItem).OrderNote);
        }
    }
}
