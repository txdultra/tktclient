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
using System.Windows.Shapes;
using tktclient.Db;
using tktclient.Utils;
using tktclient.ViewModel;

namespace tktclient
{
    /// <summary>
    /// SettingWin.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWin : Window
    {
        private SetViewModel _ViewModel;
        public SettingWin()
        {
            InitializeComponent();
            this._ViewModel = this.DataContext as SetViewModel;
            this.tbVersionInfo.Text = Assemblies.AssemblyVersion();
        }

        private async void btnResetPrinter_Click(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("确定重置？", "提示", ConfirmType.Warn, ConfirmButton.OkCancel, false);
            if (!NavigationServiceHelper.ShowOwnerWin(confirmWindow, true) || confirmWindow._ConfirmResult != ConfirmResult.Ok)
                return;
            await this._ViewModel.ResetPrinter();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            await this._ViewModel.Save();
            this.Close();
        }
    }
}
