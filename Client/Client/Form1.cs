using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        public string UserName = "";
        NetworkStream networkStream;
        OpenFileDialog fileDialog = new OpenFileDialog();
        string ImgStc = "";
        public Form1()
        {
            InitializeComponent();
        }
        private async void button1_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {
                panLog.Visible = false;
                labName.Text = $"Welcame {txtName.Text}";
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect("127.0.0.1", 2003);
                networkStream = tcpClient.GetStream();

                string s = $"Server;{txtName.Text}";
                SendMsg(s);
                StreamReader reader = new StreamReader(networkStream);
                while (true)
                {
                    string recMsg = await reader.ReadLineAsync();
                    var msgs = recMsg.Split(';');
                    if (msgs[0] == "Clients")
                    {
                        for (int i = 1; i < msgs.Length; i++)
                        {
                            if (!ConnectClientBox.Items.Contains(msgs[i])&& msgs[i]!= txtName.Text)
                            {
                                ConnectClientBox.Items.Add(msgs[i]);
                            }
                        }
                    }
                    else
                    {
                        if (msgs[4] == "txt")
                        {
                            richTextBox1.Text += $" {msgs[3]}  >> : {msgs[2]}\n";
                        }
                        else
                        {
                            richTextBox1.Text += $" {msgs[3]}  >> : Send img Double Click To Show\n";
                            ImgStc = msgs[2];
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Plase Enter Your Name First");
            }
        }
        public async void SendMsg(string mess)
        {
            
            StreamWriter writer = new StreamWriter(networkStream);
            await writer.WriteLineAsync(mess);
            writer.AutoFlush = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ConnectClientBox.Text!= "Choose Your Connected Friends")
            {
                string mess1;
                if (pictureBox2.Visible)
                {
                    mess1 = $"Message;{ConnectClientBox.SelectedItem.ToString()};{ofd.FileName};{txtName.Text};img";
                    pictureBox2.Visible = false;
                    richTextBox1.Text += $"Me >>: Sending img \n";
                }
                else
                {
                    mess1 = $"Message;{ConnectClientBox.SelectedItem.ToString()};{txtSend.Text};{txtName.Text};txt";
                    richTextBox1.Text += $"Me >>: {txtSend.Text}\n";
                }

                SendMsg(mess1);
                txtSend.Clear();
            }
            else
            {
                MessageBox.Show("Choose Connect Friend First");
            }
        }
        


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image originalImage = Image.FromFile(ofd.FileName);
                    pictureBox2.Image = originalImage;
                    pictureBox2.Visible = true;
                }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
        }
        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ImgStc != "")
            {
                labName.Visible = false;
                picShow.Visible = true;
                btnClose.Visible = true;
                picShow.Image = Image.FromFile(ImgStc);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            picShow.Visible = false;
            btnClose.Visible = false;
            labName.Visible = true;
        }

        private void panLog_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
