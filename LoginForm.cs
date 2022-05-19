using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeProject
{
    public partial class LoginForm : Form
    {
        SplashScreen SplashScreen = new SplashScreen();
        public LoginForm()
        {
            Thread t = new Thread(new ThreadStart(Splash));
            t.Start();
            Thread.Sleep(3000);
            InitializeComponent();
            t.Abort();
        }

        public void Splash()
        {
            Application.Run(SplashScreen);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var Employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(System.IO.File.ReadAllText(@"..\\..\\employees.dat")) ?? new List<Employee>();
            var employee = Employees.FirstOrDefault(xx => xx.Username == textBox1.Text && xx.Password == textBox2.Text);
            if (employee != null)
            {
                if (employee.EmployeeType == "Waiter")
                {
                    new WaiterForm(employee.Name).Show();
                    this.Hide();
                }
                else if (employee.EmployeeType == "Kitchen")
                {
                    new KitchenForm().Show();
                    this.Hide();
                }
                else if (employee.EmployeeType == "Register")
                {
                    new RegisterForm(employee.Name).Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Unknown user type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Username or password is wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }
    }
}
