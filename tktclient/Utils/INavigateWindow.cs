using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Utils
{
    interface INavigateWindow
    {
        void LoadFunctionPage(string functionCode);

        void ShowGrid(bool isShow, string mess = null);

        void AddMessage(NoticeMess mess);

        void NavigateTo(string functionCode);
    }
}
