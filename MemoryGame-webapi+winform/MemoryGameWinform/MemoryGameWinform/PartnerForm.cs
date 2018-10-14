using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;
using MemoryGameWinform.Models;
using System.Web.Script.Serialization;

namespace MemoryGameWinform
{
    public partial class PartnerForm : Form
    {
       
        public PartnerForm(object user)
        {
            this.user = user as User;
            InitializeComponent();
        }
        private User user;
        private List<User> users;
        Timer MyTimer;
        private void btn_Click(object sender, EventArgs e)
        {
            string choosenUserName = (sender as Button).Name.ToString();
            MessageBox.Show(choosenUserName);
            User choosenUser = users.FirstOrDefault(us => us.Name.Equals(choosenUserName));
            try
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:59956/api/user/"+user.Name);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(choosenUser);
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
                            MessageBox.Show("ok");
                   

                        }
                        else
                        { MessageBox.Show("error"); }
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("error");
            }
        }



        private void Form2_Load(object sender, EventArgs e)
        {

            GetList();
             MyTimer = new Timer();
            MyTimer.Interval = (2000);
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();


            


        

        }
        private void GetDetayls()
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:59956/api/user/"+user.Name);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";


            //Read Response Bytes From Stream
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            string statusCode = webResponse.StatusCode.ToString();

            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.ASCII);
            string str123 = sr.ReadToEnd().ToString();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            User userWithDet  = javaScriptSerializer.Deserialize<User>(str123);

            if(userWithDet.PartnerUserName!=null)
            {

                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create("http://localhost:59956/api/user/" + userWithDet.PartnerUserName);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";


                //Read Response Bytes From Stream
                HttpWebResponse webResponse2 = (HttpWebResponse)request2.GetResponse();
                string statusCode2 = webResponse2.StatusCode.ToString();

                StreamReader sr2 = new StreamReader(webResponse2.GetResponseStream(), System.Text.Encoding.ASCII);
                string str1232 = sr2.ReadToEnd().ToString();
                JavaScriptSerializer javaScriptSerializer2 = new JavaScriptSerializer();
                User partner = javaScriptSerializer2.Deserialize<User>(str1232);

                StartGame f3 = new StartGame(user,partner);
                MyTimer.Stop();
                f3.Show();
            }
        }
        
        private void GetList()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:59956/api/user");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";


            //Read Response Bytes From Stream
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            string statusCode = webResponse.StatusCode.ToString();

            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.ASCII);
            string str123 = sr.ReadToEnd().ToString();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            users = javaScriptSerializer.Deserialize<List<User>>(str123);

            //Clear out the existing controls, we are generating a new table layout
            tableLayoutPanel1.Controls.Clear();

            //Clear out the existing row and column styles
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            //Now we will generate the table, setting up the row and column counts first
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.RowCount = users.Count();

            for (int x = 0; x < 3; x++)
            {
                //First add a column
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                for (int y = 0; y < users.Count(); y++)
                {
                    if (users[y].Name.Equals(user.Name))
                        continue;
                    //Next, add a row.  Only do this when once, when creating the first column
                    else if (x == 0)
                    {
                        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        TextBox tb1 = new TextBox();
                        tb1.Text = users[y].Name;
                        tableLayoutPanel1.Controls.Add(tb1, x, y);
                    }
                    else if (x == 1)
                    {

                        TextBox tb2 = new TextBox();
                        tb2.Text = users[y].Age.ToString();
                        tableLayoutPanel1.Controls.Add(tb2, x, y);
                    }
                    else
                    {
                        Button btn = new Button();
                        btn.Text = "Choose";
                        btn.Name = users[y].Name;
                        btn.Click += new System.EventHandler(this.btn_Click);

                        tableLayoutPanel1.Controls.Add(btn, x, y);
                    }


                }

            }

        }
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            GetDetayls();
            GetList();
           
        }
    }

    
}
