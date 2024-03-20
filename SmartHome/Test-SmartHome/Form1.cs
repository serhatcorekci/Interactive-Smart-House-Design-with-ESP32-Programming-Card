using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_SmartHome
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void txtb1_username_Click(object sender, EventArgs e)
        {
            txtb1_username.Text = "";

        }

        private void txtb2_password_Click(object sender, EventArgs e)
        {
            txtb2_password.Text = "";

        }
        private void btn1_login_Click(object sender, EventArgs e)
        {
            string username = "smart";
            string password = "123";

            try
            {
                if (txtb1_username.Text == username && txtb2_password.Text == password)
                {
                    Form2 form2 = new Form2();
                    form2.Show();
                    this.Hide();
                }
                else if ((txtb1_username.Text == "" || txtb1_username.Text == "Username") || (txtb2_password.Text == "" || txtb2_password.Text == "password"))
                {
                    MessageBox.Show("Username and/or password can not be blank.");
                    txtb1_username.Clear();
                    txtb2_password.Clear();
                }
                else
                {
                    MessageBox.Show("Username and/or password is incorrect.");
                    txtb1_username.Clear();
                    txtb2_password.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + "Info" + ex.ToString());
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtb1_username.Text = "Username";
            txtb2_password.Text = "Password";
        }
    }
}
