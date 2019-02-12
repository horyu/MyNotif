using System.Threading.Tasks;
using System.Windows;

//参考 https://garafu.blogspot.com/2015/06/dev-tasktray-residentapplication.html
namespace MyNotif
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// タスクトレイに表示するアイコン
        /// </summary>
        private NotifyIconWrapper notifyIcon;

        /// <summary>
        /// 通信用クラス
        /// </summary>
        private NotifySever notifyServer;

        /// <summary>
        /// 通知作成クラス
        /// </summary>
        private NotifyToaster notifyToaster;


        /// <summary>
        /// System.Windows.Application.Startup イベント を発生させます。
        /// </summary>
        /// <param name="e">イベントデータ を格納している StartupEventArgs</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            NetInfo netInfo = new NetInfo();
            notifyIcon = new NotifyIconWrapper(netInfo);
            notifyServer = new NotifySever(netInfo);
            notifyToaster = new NotifyToaster();

            Task.Run(() => notifyServer.StartAsync(notifyToaster));
        }
        
        /// <summary>
        /// System.Windows.Application.Exit イベント を発生させます。
        /// </summary>
        /// <param name="e">イベントデータ を格納している ExitEventArgs</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            notifyIcon.Dispose();
        }
    }
}
