namespace MyNotif
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// タスクトレイ通知アイコン
    /// </summary>
    public partial class NotifyIconWrapper : Component
    {
        /// <summary>
        /// NotifyIconWrapper クラス を生成、初期化します。
        /// </summary>
        public NotifyIconWrapper(NetInfo netInfo)
        {
            // コンポーネントの初期化
            InitializeComponent();

            // ツールチップにIPアドレスとポート番号を追加
            notifyIcon1.Text += $"\nIP: {netInfo.IP}\nPort: {netInfo.Port}";

            // コンテキストメニューのイベントを設定
            toolStripMenuItem_Exit.Click += toolStripMenuItem_Exit_Click;
        }

        /// <summary>
        /// コンテキストメニュー "終了" を選択したとき呼ばれます。
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            // 現在のアプリケーションを終了
            Application.Current.Shutdown();
        }
    }
}
