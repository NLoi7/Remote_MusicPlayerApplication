using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.IO;
namespace Server
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        IPEndPoint IP = new IPEndPoint(IPAddress.Any, 9999);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        List<Socket> clientlist = new List<Socket>();
        Socket client;
        //listen

        void Connect()
        {
            server.Bind(IP);

            Thread listen = new Thread(() =>
            {
                try
                {

                    server.Listen(100);
                    client = server.Accept();
                    clientlist.Add(client);
                    Thread receive = new Thread(Receive);
                    receive.IsBackground = true;
                    receive.Start(client);
                    Console.ReadLine();

                }
                catch
                {
                    IP = new IPEndPoint(IPAddress.Any, 9999);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });
            listen.IsBackground = true;
            listen.Start();
        }
        // tên bài hát được nhận từ server
        string message = "";
        void Receive(Object obj)
        {

            Socket client = obj as Socket;
            try
            {
                while (true)
                {

                    byte[] data = new byte[1024 * 500000];
                    client.Receive(data);
                    message = (string)Deserialize(data);
                    AddMessage(message);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Khong the nhan duoc message , Error : " + ex.Message);
            }
        }
        /// <summary>
        /// add message textbox
        /// </summary>
        /// <param name="s"></param>
        void AddMessage(string s)
        {
            textBox1.Text = s;
            //textBox1.Clear();
        }
        /// <param name="obj"></param>
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
        // gom mảnh  
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        // Important !!!!!!!!!!!
        //int ChooseMusic(string mess)
        //{
        //    int choose = 0;
        //    if (mess == "Muộn Rồi Mà Sao Còn")
        //        choose = 1;
        //    if (mess == "Hơn cả yêu")
        //        choose = 2;
        //    if (mess == "Nến và Hoa")
        //        choose = 3;
        //    if (mess == "Thức Giấc")
        //        choose = 4;
        //    if (mess == "Chìm Sâu")
        //        choose = 5;
        //    return choose;
        //}
        // END
        //string SwitchCase(int Choose)
        //{
        //    string NameMusic = "";
        //    switch (Choose)
        //    {
        //        case 1:
        //            {
        //                NameMusic = "son_tung_m_tp_muon_roi_ma_sao_con.mp3";
        //                break;
        //            }
        //        case 2:
        //            {
        //                NameMusic = "hon_ca_yeu_duc_phuc.mp3";
        //                break;
        //            }
        //        case 3:
        //            {
        //                NameMusic = "rhymastic_nen_va_hoa.mp3";
        //                break;
        //            }
        //        case 4:
        //            {
        //                NameMusic = "thuc_giac_da_lab.mp3";
        //                break;
        //            }
        //        case 5:
        //            {
        //                NameMusic = "chim_sau_rpt_mck.mp3";
        //                break;
        //            }     
        //        default:
        //            {
        //                MessageBox.Show("Không Có Bài Hát Này !!");
        //                break;
        //            }
        //    }
        //    return NameMusic;
        //}
        string[] filenames = new string[100];
        // lấy tên id bài hát trong thư mục
        void GetNameMusic()
        {
            string[] files = Directory.GetFiles("D:/TRASH/DoAN/BackGround/ListSong/");
            for (int i = 0; i < files.Length; i++)
            {
                filenames[i] = Path.GetFileName(files[i]);
            }
        }
        // gởi tự động đã chỉnh sửa
        private void button1_Click(object sender, EventArgs e)
        {
            
            Thread sendMP3 = new Thread(() =>
            {
                try
                {
                    string filename = "";
                    string nameMP3 = ".mp3";
                    string fullname = (message + nameMP3).Trim();
                    GetNameMusic();
                    for (int i =0;i<filenames.Length;i++)
                    {
                        if (fullname == filenames[i])
                        {
                            filename = filenames[i];// +nameMP3;
                            break;
                        }
                    }
                        //string filename="";// = SwitchCase(ChooseMusic(message));
                        string filepath = @"D:\\TRASH\\DoAN\\BackGround\\ListSong\\"; // đường dẫn 
                        byte[] filenamebye = Encoding.ASCII.GetBytes(filename);
                        //
                        byte[] filedata = File.ReadAllBytes(filepath + filename);

                        byte[] clientData = new byte[4 + filenamebye.Length + filedata.Length];
                        byte[] filenamelen = BitConverter.GetBytes(filenamebye.Length);
                        //
                        filenamelen.CopyTo(clientData, 0);
                        filenamebye.CopyTo(clientData, 4);
                        filedata.CopyTo(clientData, 4 + filenamebye.Length);
                        //
                        client.Send(clientData);
                       
                        Console.ReadLine();
                        message = "";
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Khong the Goi Bai Hat ve Client, error : " + ex.Message);
                }
            });
            sendMP3.IsBackground = true;
            sendMP3.Start();
            textBox1.Clear();
        }

        private void Server_Load(object sender, EventArgs e)
        {

        }
    }
}
