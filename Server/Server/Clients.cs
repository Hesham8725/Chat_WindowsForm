using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public delegate void GetMsg(Clients clients);
    public delegate void SendMess(string messRec);
    public class Clients
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public string MRead { get; set; }

        public TcpClient _client;

        NetworkStream _ns;
        public event GetMsg onGetmsg;
        public event SendMess onSendMess;
        public Clients(TcpClient tcpClient)
        {
            _client = tcpClient;
            _ns = tcpClient.GetStream();
            AddClientProp();
            Recive_message();
        }

        public async void AddClientProp()
        {
            StreamReader _sr = new StreamReader(_ns);
            string msg = await _sr.ReadLineAsync();
            var msgs = msg.Split(';');
            Name = msgs[1];
            //ID = int.Parse(msgs[2]);

            if (onGetmsg != null)
            {
                onGetmsg(this);
            }
        }

        public async void SendMsg(string msg)
        {
            StreamWriter _sw = new StreamWriter(_ns);
            await _sw.WriteLineAsync(msg);
            _sw.AutoFlush = true;
        }

        public async void Recive_message()
        {
            StreamReader _sr = new StreamReader(_ns);
            while (true) {
                string s = await _sr.ReadLineAsync();
                if (onSendMess != null)
                {
                    onSendMess(s);
                }
            }
        }
    }
}
