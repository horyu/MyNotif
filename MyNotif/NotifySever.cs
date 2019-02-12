using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyNotif
{
    class NotifySever
    {
        private readonly TcpListener server;

        public NotifySever(NetInfo netInfo)
        {
            int port = netInfo.Port;
            IPAddress addr = IPAddress.Parse(netInfo.IP);
            server = new TcpListener(addr, port);
        }

        public async Task StartAsync(NotifyToaster toaster)
        {
            //サーバーを開始 
            server.Start();
            try
            {
                while (true)
                {
                    using (TcpClient client = await server.AcceptTcpClientAsync())
                    {
                        await HandleClient(client, toaster);
                    }
                }
            }
            finally
            {
                server.Stop();
            }

        }

        private async Task HandleClient(TcpClient client, NotifyToaster toaster)
        {
            using (NetworkStream stream = client.GetStream())
            {
                stream.ReadTimeout = 5000;

                byte[] resBytes = new byte[1024];
                int resSize = 0;
                using (MemoryStream ms = new MemoryStream())
                {
                    do
                    {
                        //一部を受信 → msに蓄積
                        resSize = await stream.ReadAsync(resBytes, 0, resBytes.Length).ConfigureAwait(false);
                        ms.Write(resBytes, 0, resSize);
                        //ストリーム死or受信長0で終わり
                    } while (stream.DataAvailable || (resSize >= 1));

                    string resMsg = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                    toaster.Fire(resMsg);
                }
            }
        }
    }
}
