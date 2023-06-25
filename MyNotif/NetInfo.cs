using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyNotif
{
    /// <summary>
    /// IPアドレスとPort番号を持つクラス
    /// </summary>
    public class NetInfo
    {
        public readonly string IP;
        public readonly int Port = 45654;

        public NetInfo()
        {
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/ac91845e-bb04-478e-a554-c2a8d48b9d60/how-to-get-local-machine-ip-address-using-c?forum=csharpgeneral
            IP = Dns.GetHostAddresses(Dns.GetHostName()).
                Where(address => address.AddressFamily == AddressFamily.InterNetwork).
                First().ToString();
        }
    }
}
