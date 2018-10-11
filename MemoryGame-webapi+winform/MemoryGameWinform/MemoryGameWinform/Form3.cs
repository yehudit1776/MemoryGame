using MemoryGameWinform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryGameWinform
{
    public partial class Form3 : Form
    {
        public Form3(object user1,object user2)
        {
            this.user1 = user1 as User;
            this.user2 = user2 as User;
            InitializeComponent();
        }

        private User user1;
        private User user2;
        private void Form3_Load(object sender, EventArgs e)
        {
            label2.Text = "Partner Name:" + user2.Name;
            label3.Text = "Partner Age:" + user2.Age;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(user1, user2);
            form4.Show();
        }
    }
}
