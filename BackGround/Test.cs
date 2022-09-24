using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace ClientMusic
{
    public partial class Form1 : Form
    {
        // kết nối database listSong
        //
        //
        //
        //
        SqlConnection connect;
        SqlDataAdapter datasql = new SqlDataAdapter();
        DataTable tb= new DataTable();
        string sourceStr = @"Data Source=NGOCLOIPHAM7\SQLEXPRESS;Initial Catalog=ClientMusic;Integrated Security=True";
        SqlCommand comma;
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
        private void Form1_Load(object sender, EventArgs e)
        {
            loadTableListSong();   
        }
        public Form1() 
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
              //sử dụng thuộc tính RowFilter để tìm kiếm theo tên "NameSong"
             string rowFilter = string.Format("{0} like '{1}'", "NameSong", "*" + textBox1.Text + "*");
            // (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = rowFilter;   
             string authorfilter  = string.Format("{0} like '{1}'", "AuthorInfor", "*" + textBox1.Text + "*");
             (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = authorfilter + " OR " + rowFilter;
        }
        //
        //
        //
        //
        //hiển thị ra các thông tin của bài hát đã chọn

        private void eventDGV()
        {
            int i = 0;
            i = dataGridView1.CurrentRow.Index;
            textBox3.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox5.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            textBox7.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            textBox2.Text = "-> NameSong : " + dataGridView1.Rows[i].Cells[1].Value.ToString();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Thread ev1 = new Thread(new ThreadStart(eventDGV));
            ev1.IsBackground = true;
            ev1.Start();
        }
        // form 2
        // MoreDetail
        private void button3_Click(object sender, EventArgs e)
        {
            int i = 0;
            i = dataGridView1.CurrentRow.Index;
            InforSinger inf = new InforSinger(dataGridView1.Rows[i].Cells[2].Value.ToString());
            inf.Show();
        }
       
        //
        //
        // truyền dữ liệu cho form InforSinger
        
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
