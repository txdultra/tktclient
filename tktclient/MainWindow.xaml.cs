using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using tktclient.Services;
using tktclient.Utils;
using tktclient.ViewModel;

namespace tktclient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INavigateWindow
    {

        private readonly IDictionary<string, Page> _userControlCache;
        private DispatcherTimer _Timer;
        private int TimeCount;

        private MainViewModel _viewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            this._viewModel = this.DataContext as MainViewModel;
            this.tbSeller.Text = ClientContext.CurrentUser?.UserName;
            _userControlCache = new Dictionary<string, Page>();
            this._Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1.0)
            };
            this._Timer.Tick += new EventHandler(this._Timer_Tick);
            this.Loaded += this.MainWindow_Loaded;

            //var keee = NoBuilder.AESEncrypt("server=172.28.3.45;database=tktdb;uid=root;pwd=123456;charset='utf8';SslMode = none;port=20010;", "m9HAHSczXYfo68mu");
            //this.tbTitle.Text = keee;
        }

        private void RBtnFunction_Click(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb == null || rb.CommandParameter == null)
                return;
            rb.IsChecked = true;
            foreach (object child in this.spGeneralFuncs.Children)
            {
                var rbChid = child as RadioButton;
                if (rbChid != null && rbChid.CommandParameter != rb.CommandParameter)
                    rbChid.IsChecked = false;
            }
            this.LoadFunctionPage(rb.CommandParameter.ToString());
        }

        public void LoadFunctionPage(string functionCode)
        {
            switch (functionCode)
            {
                case "TicketSale":
                    this.tbTitle.Text = "散客售票";
                    if (!this._userControlCache.ContainsKey(functionCode))
                    {
                        this._userControlCache.Add(functionCode, new TktSale());
                    }
                    break;
                case "TravelB2BOrdersPage":
                    this.tbTitle.Text = "电商换票";
                    if (!this._userControlCache.ContainsKey(functionCode))
                    {
                        this._userControlCache.Add(functionCode, new B2BSale());
                    }
                    break;
                case "TicketExchange":
                    this.tbTitle.Text = "换票";
                    if (!this._userControlCache.ContainsKey(functionCode))
                    {
                        this._userControlCache.Add(functionCode, new TktExchange());
                    }
                    break;
            }

            if (!this._userControlCache.ContainsKey(functionCode))
                return;
            this.mainFrame.Navigate(this._userControlCache[functionCode]);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("确定关闭？", "关闭", ConfirmType.Warn, ConfirmButton.OkCancel, false);
            if (!NavigationServiceHelper.ShowOwnerWin((Window)confirmWindow, true) || confirmWindow._ConfirmResult != ConfirmResult.Ok)
                return;
            Application.Current.Shutdown();
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("确定注销？", "注销", ConfirmType.Warn, ConfirmButton.OkCancel, false);
            if (!NavigationServiceHelper.ShowOwnerWin((Window)confirmWindow, true) || confirmWindow._ConfirmResult != ConfirmResult.Ok)
                return;
            this.Logout();
        }

        private void Logout()
        {
            new Login().Show();
            this.Close();
        }

        private void _Timer_Tick(object sender, EventArgs e)
        {
            this.tbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string dayOfWeek = GetDayOfWeek();
            if (!string.IsNullOrEmpty(dayOfWeek) && dayOfWeek != this.tbWeek.Text)
                this.tbWeek.Text = dayOfWeek;
            ++this.TimeCount;
            if (this.TimeCount < 10)
                return;
            this.TimeCount = 0;
            this.CheckNet();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.tbWeek.Text = GetDayOfWeek();
            this._Timer.Start();
            this.btnMax.ToolTip = this.WindowState == WindowState.Maximized ? (object)"还原" : (object)"最大化";
        }

        private async void CheckNet()
        {
            var result = TktService.TicketHealthCheck();
            if (result != "ok")
            {
                this.btnNet.Foreground = (Brush)new SolidColorBrush(Colors.Red);
                this.btnNet.ToolTip = (object)"网络异常";
            }
            else
            {
                this.btnNet.Foreground = (Brush)new SolidColorBrush(Colors.Green);
                this.btnNet.ToolTip = (object)"网络正常";
            }
        }

        private void Dispose()
        {
            if (this._Timer == null)
                return;
            this._Timer.Stop();
        }


        public void ShowGrid(bool isShow, string mess = null)
        {
            if (isShow)
                this.process.Visibility = Visibility.Visible;
            else
                this.process.Visibility = Visibility.Collapsed;
            this.process.BusyText = mess;
        }

        public void AddMessage(NoticeMess mess)
        {
            this._viewModel.AddMessage(mess);
        }

        public void NavigateTo(string functionCode)
        {
        }
        private void NoticeMessControl_RemoveEnent(NoticeMess mess)
        {
            this._viewModel.Remove(mess);
        }

        public string GetDayOfWeek()
        {
            string str = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    str = "星期天";
                    break;
                case DayOfWeek.Monday:
                    str = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    str = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    str = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    str = "星期四";
                    break;
                case DayOfWeek.Friday:
                    str = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    str = "星期六";
                    break;
            }
            return str;
        }

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            var settingWin = new SettingWin();
            settingWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settingWin.Owner = (Window)this;
            NavigationServiceHelper.ShowOwnerWin((Window)settingWin, true);
        }
    }
}
