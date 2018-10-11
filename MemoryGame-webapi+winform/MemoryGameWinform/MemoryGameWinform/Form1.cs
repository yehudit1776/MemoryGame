using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web;
using MemoryGameWinform.Models;
using System.Runtime.Serialization.Json;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace MemoryGameWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_name.Text.Length < 2 || textBox_name.Text.Length > 10)
                MessageBox.Show("השם חייב להיות בין 2 ל 10 תווים");
            else
            {
                User user = new User();
                user.Name = textBox_name.Text;
                user.Age = (int)numericUpDown_age.Value;


                try
                {

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:59956/api/user");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(user);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        {
                            var result = streamReader.ReadToEnd();
                            if (result == "true")
                            { 
                                Form2 form2 = new Form2(user);
                                form2.Show();


                            }
                            else
                            { MessageBox.Show("כבר קיים משתמש בשם זה"); }
                        }

                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("error");
                }
            }





        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
