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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace tktclient
{
    /// <summary>
    /// ConfirmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmWindow : Window, IComponentConnector
    {

        private int _TimeOut = 5;
        private Timer _Timer;

        private string _Title { get; set; }

        private string _Message { get; set; }

        private ConfirmType _ConfirmType { get; set; }

        private ConfirmButton _ConfirmBtn { get; set; }

        public ConfirmResult _ConfirmResult { get; set; }

        public ConfirmWindow(
      string mess,
      string title,
      ConfirmType type,
      ConfirmButton btn,
      bool isPos = false)
      : this(mess, title, type, btn, false, 5, isPos)
        {
        }

        public ConfirmWindow(
          string mess,
          string title,
          ConfirmType type,
          ConfirmButton btn,
          bool autoClose,
          int timeOut,
          bool isPos = false)
        {
            this.InitializeComponent();
            this._Title = title;
            this._Message = mess;
            this._ConfirmType = type;
            this._ConfirmBtn = btn;
            this.Loaded += new RoutedEventHandler(this.ConfirmWindow_Loaded);
            this.Unloaded += new RoutedEventHandler(this.ConfirmWindow_Unloaded);
            this._TimeOut = timeOut;
            if (autoClose)
            {
                this.tbTimeout.Text = this._TimeOut.ToString();
                this._Timer = new Timer() { Interval = 1000.0 };
                this._Timer.Elapsed += new ElapsedEventHandler(this._Timer_Elapsed);
                this._Timer.Start();
            }
            else
                this.tbTimeout.Text = (string)null;
            if (!isPos)
                return;
            this.btnYes.Background = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)91, (byte)205, (byte)200));
            this.btnOk.Background = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)91, (byte)205, (byte)200));
        }

        private void _Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                --this._TimeOut;
                this.tbTimeout.Text = this._TimeOut.ToString();
                if (this._TimeOut > 0)
                    return;
                this._ConfirmResult = ConfirmResult.None;
                this.DialogResult = new bool?(true);
            }));
        }

        private void ConfirmWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitMessage();
            this.InitBtn();
        }

        private void ConfirmWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this._Timer == null)
                return;
            this._Timer.Stop();
            this._Timer.Dispose();
            this._Timer = (Timer)null;
        }

        private void InitBtn()
        {
            switch (this._ConfirmBtn)
            {
                case ConfirmButton.Yes:
                    this.btnYes.Visibility = Visibility.Visible;
                    break;
                case ConfirmButton.No:
                    this.btnNo.Visibility = Visibility.Visible;
                    break;
                case ConfirmButton.YesNo:
                    this.btnYes.Visibility = Visibility.Visible;
                    this.btnNo.Visibility = Visibility.Visible;
                    break;
                case ConfirmButton.Ok:
                    this.btnOk.Visibility = Visibility.Visible;
                    break;
                case ConfirmButton.Cancel:
                    this.btnCancel.Visibility = Visibility.Visible;
                    break;
                case ConfirmButton.OkCancel:
                    this.btnOk.Visibility = Visibility.Visible;
                    this.btnCancel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void InitMessage()
        {
            this.tbTitle.Text = this._Title;
            this.tbMess.Text = this._Message;
            switch (this._ConfirmType)
            {
                case ConfirmType.Success:
                    this.tbIcon.Text = "\xE61E";
                    this.tbIcon.Foreground = (Brush)Application.Current.FindResource((object)"NoticeSuccess");
                    break;
                case ConfirmType.Error:
                    this.tbIcon.Text = "\xE616";
                    this.tbIcon.Foreground = (Brush)Application.Current.FindResource((object)"NoticeError");
                    break;
                case ConfirmType.Warn:
                    this.tbIcon.Text = "\xE6AA";
                    this.tbIcon.Foreground = (Brush)Application.Current.FindResource((object)"NoticeWarn");
                    break;
                case ConfirmType.Info:
                    this.tbIcon.Text = "\xE621";
                    this.tbIcon.Foreground = (Brush)Application.Current.FindResource((object)"NoticeInfo");
                    break;
                case ConfirmType.Ques:
                    this.tbIcon.Text = "\xE6B8";
                    this.tbIcon.Foreground = (Brush)Application.Current.FindResource((object)"NoticeQues");
                    break;
            }
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            this._ConfirmResult = ConfirmResult.Yes;
            this.DialogResult = new bool?(true);
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            this._ConfirmResult = ConfirmResult.No;
            this.DialogResult = new bool?(true);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this._ConfirmResult = ConfirmResult.Ok;
            this.DialogResult = new bool?(true);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this._ConfirmResult = ConfirmResult.Cancel;
            this.DialogResult = new bool?(true);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this._ConfirmResult = ConfirmResult.None;
            this.DialogResult = new bool?(false);
        }
    }

    public enum ConfirmButton
    {
        Yes,
        No,
        YesNo,
        Ok,
        Cancel,
        OkCancel,
    }

    public enum ConfirmResult
    {
        None,
        Yes,
        No,
        Ok,
        Cancel,
    }
    public enum ConfirmType
    {
        None,
        Success,
        Error,
        Warn,
        Info,
        Ques,
    }

}
