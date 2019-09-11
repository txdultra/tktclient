using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tktclient.Utils;

namespace tktclient.Controls
{
    /// <summary>
    /// NoticeMessControl.xaml 的交互逻辑
    /// </summary>
    public partial class NoticeMessControl : UserControl
    {
        private Timer _Timer;
        private NoticeMess _Mess;

        public event RemoveHander RemoveEnent;
        public delegate void RemoveHander(NoticeMess mess);

        public NoticeMessControl()
        {
            this.InitializeComponent();
            this.Loaded += this.NoticeMessControl_Loaded;
            this.Unloaded += this.NoticeMessControl_Unloaded;
        }

        private void NoticeMessControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._Mess = this.DataContext as NoticeMess;
            if (this._Mess.IsAutoRemove)
            {
                this._Timer = new Timer() { Interval = 1000.0 };
                this._Timer.Elapsed += this._Timer_Elapsed;
                this._Timer.Start();
            }
            else
                this.tbTimeout.Text = null;
        }

        private void NoticeMessControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this._Timer == null)
                return;
            this._Timer.Stop();
            this._Timer.Dispose();
            this._Timer = null;
        }

        private void _Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            --this._Mess.TimeOut;
            if (this._Mess.TimeOut > 0)
                return;
            this.Dispatcher.Invoke((Action)(() =>
            {
                RemoveHander removeEnent = this.RemoveEnent;
                if (removeEnent == null)
                    return;
                removeEnent(this._Mess);
            }));
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            RemoveHander removeEnent = this.RemoveEnent;
            if (removeEnent == null)
                return;
            removeEnent(this._Mess);
        }
    }
}
