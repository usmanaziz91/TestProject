using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace Assignment
{
    public partial class Login : Form
    {
        //public static string Username
        //{
        //    get { return Username; }
        //    set { Username = value; }
        //}
        //public static int UserID
        //{
        //    get { return UserID; }
        //    set { UserID = value; }
        //}
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string encPassword = "";
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();
                using (MD5 md5Hash = MD5.Create())
                {
                    encPassword = GetMd5Hash(md5Hash, password);
                }

                DAL.DataClasses1DataContext objdal = new DAL.DataClasses1DataContext();
                DAL.User objuser = objdal.Users.FirstOrDefault(x => x.Username.Trim() == username && x.Password.Trim() == encPassword);

                if (objuser!=null)
                {
                    
                    LoggedInUser.UserID = objuser.ID;
                    LoggedInUser.Username = objuser.Username;

                    Main objmain = new Main();
                    objmain.Tag = this;
                    objmain.Show(this);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
                
            }
            catch
            { 
            
            }
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
    }
}
