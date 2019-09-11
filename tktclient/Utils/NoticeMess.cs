using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Utils
{
    public class NoticeMess : ViewModelBase
    {
        private int _timeOut = 5;

        public string Title { get; set; }

        public string Mess { get; set; }

        public int NoticeType { get; set; }

        public bool IsAutoRemove { get; set; } = true;

        public int TimeOut
        {
            get
            {
                return this._timeOut;
            }
            set
            {
                this._timeOut = value;
                this.RaisePropertyChanged(nameof(TimeOut));
            }
        }
    }
}
