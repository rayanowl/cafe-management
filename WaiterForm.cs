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
    public partial class WaiterForm : Form
    {
        //public String EmplyeeName { get; set; }
        public WaiterForm(string EmployeeName)
        {
            InitializeComponent();
            this.Text = EmployeeName;
        }

        private void ShowOrderForm_Click(object sender, EventArgs e)
        {
            new OrderForm(((Button)sender).Text).ShowDialog();
        }

        private void WaiterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }
    }
}
