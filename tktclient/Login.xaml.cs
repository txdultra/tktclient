using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using tktclient.Services;
using tktclient.Utils;

namespace tktclient
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private readonly string _clearText;

        public Login()
        {
            InitializeComponent();
            this.tbCopyRight.Text = "上海长甲集团";
            this.tbVersionInfo.Text = "1.0.0";
            this._clearText = Guid.NewGuid().ToString();
            this.InitSellers();
        }

        private void InitSellers()
        {
            var userList = new Config().GetLoginUserNames().ToDictionary(a=>a);
            if (userList.Count > 0)
                userList.Add("清除", this._clearText);
            this.txtUserName.ItemsSource = userList;
            this.txtUserName.SelectedValuePath = "Value";
            this.txtUserName.DisplayMemberPath = "Key";
            this.txtUserName.SelectedIndex = userList == null || userList.Count <= 0 ? -1 : userList.Count - 2;
            this.txtUserName.SelectionChanged += (changeSender, changedArgs) =>
            {
                if (this.txtUserName.SelectedValue == null || !this.txtUserName.SelectedValue.ToString().Equals(this._clearText))
                    return;
                this.txtUserName.ItemsSource = null;
            };
            if (string.IsNullOrEmpty(this.txtUserName.Text.Trim()))
                this.txtUserName.Focus();
            else
                this.txtUserPwd.Focus();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void txtUserPwd_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            this.btnLogin_Click(null, null);
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btnLogin.IsEnabled = false;
                this.btnLogin.Content = "登录中...";
                this.tbErrorInfo.Text = null;
                string userName = this.txtUserName.Text.Trim();
                string userPwd = this.txtUserPwd.Password.Trim();
                if (this.txtUserName.SelectedIndex > -1)
                    userName = this.txtUserName.SelectedValue.ToString();
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPwd))
                {
                    this.btnLogin.IsEnabled = true;
                    this.tbErrorInfo.Text = "请输入完整用户名和密码";
                    this.btnLogin.Content = "登录";
                }
                else
                {
                    var requestResult = TktService.Login(userName, userPwd);
                    if (requestResult.Code != "success")
                    {
                        this.tbErrorInfo.Text = string.IsNullOrEmpty(requestResult.Msg) ? "登录失败，请检查网络连接和服务器状态" : requestResult.Msg;
                    }
                    else
                    {
                        ClientContext.CurrentUser = requestResult.data;
                        NavigationServiceHelper.LoadMainWindow(new MainWindow());
                        this.Close();
                    }
                }
            }
            finally
            {
                this.btnLogin.IsEnabled = true;
                this.btnLogin.Content = "登录";
            }
        }
    }
}
