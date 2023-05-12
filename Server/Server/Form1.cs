using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        private List<Clients> _clients = new List<Clients>();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            IPAddress iP = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(iP, 2003);
            server.Start();

            richTextBox1.Text += "Server is starting .....\n";
            while (true)
            {
                TcpClient Client = await server.AcceptTcpClientAsync();
                Clients clients = new Clients(Client);
                _clients.Add(clients);
                clients.onGetmsg += Clients_onGetmsg;
                clients.onSendMess += Clients_onSendMess; ;

            }
            
        }

        private void Clients_onSendMess(string messRec)
        {
            var Data = messRec.Split(';');
            Clients send = GetClient(Data[1]);
            richTextBox1.Text += $"Send Message From {Data[1]} to {Data[3]}\n";
            send.SendMsg(messRec);
        }
        private void Clients_onGetmsg(Clients clients)
        {
            string x = _clients[_clients.Count - 1].Name;
            listOfClient.Items.Add(x);
            richTextBox1.Text += $"Client : {x} is Connected\n";
            string message = $"Clients";
            for(int i = 0; i < _clients.Count; i++)
            {
                message += $";{ _clients[i].Name}";
            }
            for (int i = 0; i < _clients.Count; i++)
            {
                _clients[i].SendMsg(message);
            }
        }

        public Clients GetClient (string Name)
        {
            for(int i = 0; i < _clients.Count; i++)
            {
                if(_clients[i].Name == Name)
                {
                    return _clients[i];
                }
            }
            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
