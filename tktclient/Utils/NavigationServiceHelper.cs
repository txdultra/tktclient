using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace tktclient.Utils
{
    class NavigationServiceHelper
    {
        public static INavigateWindow NavigateWindow { get; set; }

        public static void LoadMainWindow(INavigateWindow window)
        {
            NavigationServiceHelper.NavigateWindow = window;
            (window as Window).Show();
        }

        public static void ShowChildWindow(INavigateWindow window)
        {
            (window as Window).Show();
        }

        public static void NoticeSuccess(string mess, string title = "提示", bool isAutoRemove = true, int timeout = 3)
        {
            INavigateWindow navigateWindow = NavigationServiceHelper.NavigateWindow;
            if (navigateWindow == null)
                return;
            navigateWindow.AddMessage(new NoticeMess()
            {
                Mess = mess,
                Title = title,
                NoticeType = 1,
                IsAutoRemove = isAutoRemove,
                TimeOut = timeout
            });
        }

        public static void NoticeError(string mess, string title = "提示", bool isAutoRemove = true, int timeout = 3)
        {
            INavigateWindow navigateWindow = NavigationServiceHelper.NavigateWindow;
            if (navigateWindow == null)
                return;
            navigateWindow.AddMessage(new NoticeMess()
            {
                Mess = mess,
                Title = title,
                NoticeType = 2,
                IsAutoRemove = isAutoRemove,
                TimeOut = timeout
            });
        }

        public static void NoticeWarn(string mess, string title = "提示", bool isAutoRemove = true, int timeout = 3)
        {
            INavigateWindow navigateWindow = NavigationServiceHelper.NavigateWindow;
            if (navigateWindow == null)
                return;
            navigateWindow.AddMessage(new NoticeMess()
            {
                Mess = mess,
                Title = title,
                NoticeType = 3,
                IsAutoRemove = isAutoRemove,
                TimeOut = timeout
            });
        }

        public static void NoticeInfo(string mess, string title = "提示", bool isAutoRemove = true, int timeout = 3)
        {
            INavigateWindow navigateWindow = NavigationServiceHelper.NavigateWindow;
            if (navigateWindow == null)
                return;
            navigateWindow.AddMessage(new NoticeMess()
            {
                Mess = mess,
                Title = title,
                NoticeType = 4,
                IsAutoRemove = isAutoRemove,
                TimeOut = timeout
            });
        }

        public static void NoticeQues(string mess, string title = "提示", bool isAutoRemove = true, int timeout = 3)
        {
            INavigateWindow navigateWindow = NavigationServiceHelper.NavigateWindow;
            if (navigateWindow == null)
                return;
            navigateWindow.AddMessage(new NoticeMess()
            {
                Mess = mess,
                Title = title,
                NoticeType = 5,
                IsAutoRemove = isAutoRemove,
                TimeOut = timeout
            });
        }

        public static ConfirmResult ShowConfirm(
          string mess,
          string title,
          ConfirmType confirmType,
          ConfirmButton btn,
          bool isAutoClose = false,
          int timeOut = 5)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(mess, title, confirmType, btn, isAutoClose, timeOut, false);
            NavigationServiceHelper.ShowOwnerWin((Window)confirmWindow, false);
            return confirmWindow._ConfirmResult;
        }

        public static bool ShowOwnerWin(Window window, bool showGrid = true)
        {
            try
            {
                if (NavigationServiceHelper.NavigateWindow != null)
                {
                    if (showGrid)
                        NavigationServiceHelper.NavigateWindow.ShowGrid(true, "正在处理...");
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = NavigationServiceHelper.NavigateWindow as Window;
                    bool? nullable = window.ShowDialog();
                    return nullable.HasValue && nullable.GetValueOrDefault();
                }
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                bool? nullable1 = window.ShowDialog();
                return nullable1.HasValue && nullable1.GetValueOrDefault();
            }
            finally
            {
                if (showGrid)
                    NavigationServiceHelper.NavigateWindow?.ShowGrid(false, (string)null);
            }
        }
    }
}
