using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace BackGround
{
    public partial class BackGround : Form
    {
        Player RunMusic = new Player(); 
        private IconButton currentBtn;
        private Panel leftBoderBtn;
        private Form currentChildForm;
        //sql
        SqlConnection connect;
        SqlDataAdapter datasql = new SqlDataAdapter();
        DataTable tb = new DataTable();
        string sourceStr = @"Data Source=NGOCLOIPHAM7\SQLEXPRESS;Initial Catalog=ClientMusic;Integrated Security=True";
        SqlCommand comma;
        //setup SQL
        void DesignColumns()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        void ConnectionListSong() // connect database
        {
            comma = connect.CreateCommand();
            comma.CommandText = "select * from ListSong";
            datasql.SelectCommand = comma;
            tb.Clear();
            datasql.Fill(tb);
            dataGridView1.DataSource = tb;
            DesignColumns();
        }
        void loadTableListSong()
        {
            try
            {
                connect = new SqlConnection(sourceStr);
                connect.Open();
                ConnectionListSong();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi Ket Noi Den Database : " + ex.Message);
            }
        }
        private void BackGround_Load(object sender, EventArgs e)
        {
            loadTableListSong();
        }
        public BackGround()
        {
            InitializeComponent();
            leftBoderBtn = new Panel();
            leftBoderBtn.Size = new Size(7, 90);
            panel1.Controls.Add(leftBoderBtn);
            //
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            //setup sql
            Control.CheckForIllegalCrossThreadCalls = false;
            //kết nối tới server
            Connect();
        }
        //CONNECT IP
        //connect
        IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //kết nối server 
        void Connect()
        {
            try
            {
                client.Connect(IP);
            }
            catch
            {
                MessageBox.Show("không thể kết nối server!", "lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Thread listen = new Thread(receive);
            listen.IsBackground = true;
            listen.Start();
        }
        //
        void Send()
        {

            if (textBox5.Text != string.Empty)
                client.Send(Serialize(textBox4.Text));

        }
        //phân mảnh
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);

        }

        string filename = "";
        //int count = 0;
        // nhận file
        void receive()
        {
            try
            {
                while(true)
                {
                   
                    byte[] clientData = new byte[1024 * 500000];
                    string receivePath = "D:/TRASH/DoAN/BackGround/RECEIVEDLIST/"; // đường dẫn nhận file mp3
                    int receivedBytelen = client.Receive(clientData);
                    //
                    int filenamelen = BitConverter.ToInt32(clientData, 0);
                    filename = Encoding.ASCII.GetString(clientData, 4, filenamelen);
                    //  
                    BinaryWriter bwrite = new BinaryWriter(File.Open(receivePath + filename, FileMode.Append, FileAccess.Write));
                    bwrite.Write(clientData, 4 + filenamelen, receivedBytelen - 4 - filenamelen);
                    // 
                    bwrite.Close();
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
        }
        // 
        private void eventDGV()
        {
            int i = 0;
            i = dataGridView1.CurrentRow.Index;
            textBox1.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            textBox5.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
        }
        //search
        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            //sử dụng thuộc tính RowFilter để tìm kiếm theo tên "NameSong"
            string rowFilter = string.Format("{0} like '{1}'", "NameSong", "*" + textBox2.Text + "*");
            // (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = rowFilter;   
            string authorfilter = string.Format("{0} like '{1}'", "AuthorInfor", "*" + textBox2.Text + "*");
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = authorfilter + " OR " + rowFilter;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Thread ev1 = new Thread(new ThreadStart(eventDGV));
            ev1.IsBackground = true;
            ev1.Start();
        }
        private void iconButton6_Click(object sender, EventArgs e)
        {
            Send();
            filename = "";
        }
        private void iconButton5_Click(object sender, EventArgs e)
        {
            string messagePath = "D:/TRASH/DoAN/BackGround/RECEIVEDLIST/" + filename;
            RunMusic.stringpath = messagePath;
           // RunMusic.idMusic = filename;    
            for( int i  =0;i < dataGridView1.Rows.Count-1; i++ )
            {
                if (filename == (dataGridView1.Rows[i].Cells[0].Value.ToString()+".mp3"))
                {
                    RunMusic.nameMusic=dataGridView1.Rows[i].Cells[1].Value.ToString();
                    break;
                }
            }
            // RunMusic.Show();
            ActivePlaying(sender, RGBcolors.color2);
            OpenChildForm(RunMusic);
            
        }

        //EN IP
        // structs 
        private struct RGBcolors
        {
            public static Color color1 = Color.FromArgb(172, 126, 241);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
        }
        // phuong phap
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton(); 
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(37, 36, 81);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                // left border button
                leftBoderBtn.BackColor = color;
                leftBoderBtn.Location = new Point(0,currentBtn.Location.Y);
                leftBoderBtn.Visible = true;
                leftBoderBtn.BringToFront();
                // iconHome
                iconHome.IconChar = currentBtn.IconChar;
                iconHome.IconColor = color;
            }    
        }

        private void ActivePlaying(object sender, Color color)
        {
            if (sender != null)
            {
                DisableButton();
                currentBtn = iconButton2;
                currentBtn.BackColor = Color.FromArgb(37, 36, 81);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                // left border button
                leftBoderBtn.BackColor = color;
                leftBoderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBoderBtn.Visible = true;
                leftBoderBtn.BringToFront();
                // iconHome
                iconHome.IconChar = currentBtn.IconChar;
                iconHome.IconColor = color;
            }
        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(31, 30, 68);
                currentBtn.ForeColor = Color.NavajoWhite;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.NavajoWhite;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            } 
                
        }

        public void OpenChildForm(Form childForm)
        {
            if (currentChildForm !=null)
            {
                currentChildForm.Hide(); 
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            labelHome.Text = childForm.Text; 
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBcolors.color1);
            if (currentChildForm != null)
            {
                currentChildForm.Hide();
            }
            
        }

        public void iconButton2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBcolors.color2);
            OpenChildForm(RunMusic);
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBcolors.color3);
            OpenChildForm(new Play());

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Play());
            //currentChildForm.Close(); 
            //Reset(); 
        }

        private void Reset()
        {
            DisableButton();
            leftBoderBtn.Visible = false;
            iconHome.IconChar = IconChar.Home;
            iconHome.IconColor = Color.MediumPurple;
            labelHome.Text = "Home";  
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void iconHome_Click(object sender, EventArgs e)
        {

        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0); 
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }
        private void btnWinDow_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal; 
        }

        private void btnWinDowdown_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelDesktop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        // hiển thị thông tin ca sĩ
        private void button2_Click_1(object sender, EventArgs e)
        {
            int i = 0;
            i = dataGridView1.CurrentRow.Index;
            InforSinger inf = new InforSinger(dataGridView1.Rows[i].Cells[2].Value.ToString());
            inf.Show();

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
