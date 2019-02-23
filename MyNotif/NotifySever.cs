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

                const int maxLen = 512 * 4; // 適当：通知幅的に表示できる文字数 * 1文字当たりのバイト数
                byte[] resBytes = new byte[maxLen];
                int resSize = 0;
                using (MemoryStream ms = new MemoryStream())
                {
                    do
                    {
                        // 一部を受信 → msに蓄積
                        resSize = await stream.ReadAsync(resBytes, 0, resBytes.Length).ConfigureAwait(false);
                        ms.Write(resBytes, 0, resSize);
                        // maxLenバイト以上受信 で読み込み終わり
                        if (ms.Length >= maxLen)
                        {
                            break;
                        }
                        // ストリーム死 or 受信長0（ストリームの末尾） で読み込み終わり
                    } while (stream.DataAvailable || (resSize >= 1));

                    int readLength = System.Math.Min((int)ms.Length, maxLen);
                    // バッファーの末尾がマルチバイト文字の途中の場合、切り捨て
                    string resMsg = Encoding.UTF8.GetString(ms.GetBuffer(), 0, readLength);
                    toaster.Fire(resMsg);
                }
            }
        }
    }
}
