using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MyNotif
{
    class NotifyToaster
    {
        // AppUserModelID ゴミ箱を使用
        // 確認方法 https://www.ka-net.org/blog/?p=6250
        private const string APP_ID = "::{645FF040-5081-101B-9F08-00AA002F954E}";
        private readonly XmlDocument OriXML = new XmlDocument();
        private readonly char[] N = { '\n' };

        public NotifyToaster()
        {
            OriXML.LoadXml(@"
                <toast activationType='foreground' scenario='reminder'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text></text>
                            <text></text>
                        </binding>
                    </visual>
                    <actions>
                        <action activationType='background'
                                arguments=''
                                content='OK' />
                    </actions>
                    <audio src='ms-winsoundevent:Notification.Default' />
                </toast>");
        }

        public void Fire(string msg)
        {
            string[] arr = msg.Split(N, 2);
            XmlDocument xml = (XmlDocument)OriXML.CloneNode(true);
            XmlNodeList texts = xml.GetElementsByTagName("text");

            texts[0].InnerText = arr[0];
            if (arr.Length > 1)
            {
                texts[1].InnerText = arr[1];
            }

            ToastNotification toast = new ToastNotification(xml);
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }
    }
}
