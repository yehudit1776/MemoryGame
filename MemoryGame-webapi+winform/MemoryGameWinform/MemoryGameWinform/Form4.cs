using MemoryGameWinform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace MemoryGameWinform
{
    public partial class Form4 : Form
    {
        private User user1;
        private User user2;
        string[] selectedCards = new string[2];
        List<Button> placesBtns;
        Timer MyTimer;
        HelpCardsAndCurrentTurn game;

        private string currentUser;
        public Form4(object u1, object u2)
        {
            user1 = u1 as User;
            user2 = u2 as User;
            selectedCards[0] = "";
            selectedCards[1] = "";
            InitializeComponent();
        }
        private void card_Click(object sender, EventArgs e)
        {

            (sender as Button).BackColor = Color.White;
            if (selectedCards[0] == "")
                selectedCards[0] = (sender as Button).Text;
            else if (selectedCards[1] == "")

            {
                selectedCards[1] = (sender as Button).Text;
                try
                {

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:59956/api/game/" + user1.Name);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "PUT";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(selectedCards);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        {
                            var result = streamReader.ReadToEnd();
                            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                            HelpCardsAndCurrentTurn helpCardsAndCurrentTurn = javaScriptSerializer.Deserialize<HelpCardsAndCurrentTurn>(result);
                            Dictionary<string, string> reqCard = helpCardsAndCurrentTurn.CardList;


                            if (reqCard[selectedCards[0]] != null)
                            {
                                MessageBox.Show("well!!!");

                                for (int i = 0; i < placesBtns.Count; i++)
                                {
                                    if (placesBtns[i].Text == selectedCards[1])
                                        placesBtns[i].BackColor = Color.Pink;
                                    placesBtns[i].Enabled = false;
                                }

                            }
                            else
                            {
                                MessageBox.Show("mistake!!!");

                                for (int i = 0; i < placesBtns.Count; i++)
                                {
                                    if (placesBtns[i].Text == selectedCards[0] || placesBtns[i].Text == selectedCards[1])
                                        placesBtns[i].BackColor = Color.Black;

                                }

                                //currentUser = helpCardsAndCurrentTurn.CurrentTurn;
                                //label1.Text = "Current player name: " + currentUser;

                            }


                            selectedCards[0] = "";
                            selectedCards[1] = "";
                            currentUser = helpCardsAndCurrentTurn.CurrentTurn;
                            label1.Text = "Current player name: " + currentUser;



                        }

                    }


                }
                catch (Exception ex)
                {
                    throw;

                }


            }

        }



        private void Form4_Load(object sender, EventArgs e)
        {
            MyTimer = new Timer();
            MyTimer.Interval = (2000);
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();



            label2.Text = "Current Game matching sets:";
            label3.Text = "Your Counter:";
            label5.Text = user2.Name + " Counter:";
            label4.Text = 0.ToString();
            label6.Text = 0.ToString();

            GetGame();
            currentUser = game.CurrentTurn.ToString();
            label1.Text += currentUser;
            Dictionary<string, string> cards = game.CardList;
            List<Button> buttons = new List<Button>();
            foreach (KeyValuePair<string, string> item in cards)
            {
                for (int i = 0; i < 2; i++)
                {
                    Button btn1 = new Button();
                    btn1.Text = item.Key;
                    btn1.Click += new System.EventHandler(this.card_Click);
                    btn1.Width = 70;
                    btn1.Height = 70;
                    btn1.Name = i + btn1.Text;

                    buttons.Add(btn1);

                  

                }


            }

            placesBtns = new List<Button>();
            for (int i = 0; i < 18; i++)
            {
                placesBtns.Add(new Button() {  Text="x"});
            }
            int f = 0;
            while (f< 18)
            {
                Random rnd = new Random();
                
                int x = rnd.Next(0, 18);
                Button b = buttons[f];
                if (placesBtns[x].Text=="x")
                {
                    placesBtns[x] = new Button();
                    placesBtns[x].Text = b.Text;
                    // btn1.BackColor = Color.Black;
                    placesBtns[x].Click += new System.EventHandler(this.card_Click);
                    placesBtns[x].Width = 70;
                    placesBtns[x].Height = 70;
                    placesBtns[x].BackColor = Color.Black;
                    placesBtns[x].Name = f + placesBtns[f].Text;


                    f++;
                }
               

            }

            int k = 0, row = 50, col = 50, c = 0;


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    placesBtns[k].Location = new Point(row, col);
                    row += 80;
                    panel2.Controls.Add(placesBtns[k]);
                    c = panel2.Controls.Count;


                    k++;
                }
                row = 50;
                col += 80;




            }





        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            GetGame();
            ChangeCurrentTurn();
            CheckGameOver();


        }

        private void GetGame()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:59956/api/game/" + user1.Name);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";


            //Read Response Bytes From Stream
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            string statusCode = webResponse.StatusCode.ToString();

            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.ASCII);
            string str123 = sr.ReadToEnd().ToString();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            game = javaScriptSerializer.Deserialize<HelpCardsAndCurrentTurn>(str123);
        }


        private void ChangeCurrentTurn()
        {
            label1.Text = "Current player name: " + currentUser;
            if (game.CurrentTurn!=user1.Name)
            {
                for (int i = 0; i < placesBtns.Count; i++)
                {
                    placesBtns[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < placesBtns.Count; i++)
                {
                    if (game.CardList.FirstOrDefault(c => c.Key == placesBtns[i].Text).Value != null)
                    {
                        placesBtns[i].BackColor = Color.Pink;
                    }
                    else
                        placesBtns[i].Enabled = true;
                }
            }

        }

        private void CheckGameOver()
        {
            int pointUser1 = 0;
            int pointUser2 = 0;
            foreach (var item in game.CardList.Values)
            {
                if (item == user1.Name)
                    pointUser1++;
                else if (item == user2.Name)
                    pointUser2++;
             
            }
            label4.Text = pointUser1.ToString();
            label6.Text = pointUser2.ToString();
            if (pointUser1 + pointUser2 == game.CardList.Count)
            {
                string winner = pointUser1 > pointUser2 ? user1.Name : user2.Name;
                MyTimer.Stop();
                MessageBox.Show("GAME OVER!!!!  the winner is " + winner);
               

            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

          





        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
