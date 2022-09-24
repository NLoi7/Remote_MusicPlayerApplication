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
    public partial class Player : Form
    {
        public Player()
        {
            InitializeComponent();
        }
        public string stringpath = "";
        public string idMusic = "";
        public string nameMusic = "";
        private void axWindowsMediaPlayer1_Enter_1(object sender, EventArgs e)
        {
             
        }
        
        int i = 0;
        private void button3_Click(object sender, EventArgs e)
        {
           
            axWindowsMediaPlayer1.URL = stringpath;
            label3.Text = nameMusic;
            //PlayList.Items.Add(nameMusic);

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
        string[] filepaths = new string[100];
        string[] filenames = new string[100];
        //public void add()
        //{
            
        //   string[] files = Directory.GetFiles("D:/TRASH/DoAN/BackGround/RECEIVEDLIST/");
        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        filenames[i] = Path.GetFileNameWithoutExtension(files[i]);
        //        //filepaths[i] = Path.GetFullPath(files[i]);

        //        PlayList.Items.Add(filenames[i]);// + ": "+ nameMusic);
                
        //    }
        //}
        private void Player_Load(object sender, EventArgs e)
        {

        }
        //private void PlayList_DoubleClick(object sender, EventArgs e)
        //{
        //    if(PlayList.SelectedIndex != -1 )
        //    {
        //        int choose = PlayList.SelectedIndex; 
        //        axWindowsMediaPlayer1.URL = filepaths[choose]; 
        //    }    
        //}
        Random random = new Random();   
        int x = 731, y = 174,a= 1;

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                x += a;
                label3.Location= new Point(x, y);
                if (x>=910)
                {
                    a = -1;
                    //label3.ForeColor = Color.FromArgb(random.Next(0,255),random.Next(0,255), random.Next(0, 255));   
                }
                if (x<= 731)
                {
                    a = 1;
                    //label3.ForeColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255),random.Next(0, 255));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
