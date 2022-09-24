using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackGround
{
    public partial class InforSinger : Form
    {
        public InforSinger()
        {
            InitializeComponent();
        }
        string DataSource = "";
        public InforSinger(string strvalue) : this()
        {
            DataSource = strvalue;
        }
        int ChooseSinger(string AuthorName)
        {
            int value = 0;
            if (AuthorName == "Sơn Tùng - MTP")
                value = 1;
            if (AuthorName == "Đức Phúc")
                value = 2;
            if (AuthorName == "Rhymastic")
                value = 3;
            if (AuthorName == "Da Lab")
                value = 4;
            if (AuthorName == "RPT MCK")
                value = 5;
           
            return value;
        }
        private void InforSinger_Load(object sender, EventArgs e)
        {
            switch (ChooseSinger(DataSource))
            {
                case 1:
                    {
                        label3.Text = "Nguyễn Thanh Tùng";
                        label7.Text = "5/7/1994";
                        label8.Text = "Thai Binh City";
                        label9.Text = "Sơn Tùng - MTP";
                        break;
                    }

            
                case 2:
                    {
                        label3.Text = "Nguyễn Đức Phúc";
                        label7.Text = "15/10/1996";
                        label8.Text = "Ha Noi Capital";
                        label9.Text = "Đức Phúc";
                        break;
                    }
               
                
                case 3:
                    {
                        label3.Text = "Nguyễn Đức Thiện";
                        label7.Text = "8/4/1991";
                        label8.Text = "Ha Noi Capital";
                        label9.Text = "Rhymastic";
                        break;
                    }
               
                case 4:
                    {
                        label3.Text = "Da Lab";
                        label7.Text = "2007";
                        label8.Text = "Northside";
                        label9.Text = "Da Lab";
                        break;
                    }
                case 5:
                    {
                        label3.Text = "Nghiêm Vũ Hoàng Long";
                        label7.Text = "2/3/1999";
                        label8.Text = "Thai Binh City";
                        label9.Text = "RPT MCK, Long Nger, YoungItachi..";
                        break;
                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();    
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
