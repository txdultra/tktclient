using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace tktclient.ViewModel
{
    public class BusyStatusViewModel:ViewModelBase
    {
        private bool _IsBusy;
        private string _busyContent;
        private string _errorMessage;
        private ICommand _cancelCommand;
        public bool _IsUnLoaded;

        public bool IsBusy
        {
            get
            {
                return this._IsBusy;
            }
            set
            {
                if (this._IsBusy == value)
                    return;
                this._IsBusy = value;
                this.RaisePropertyChanged(nameof(IsBusy));
            }
        }

        public string BusyContent
        {
            get
            {
                return this._busyContent;
            }
            set
            {
                this._busyContent = value;
                this.RaisePropertyChanged(nameof(BusyContent));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this._errorMessage;
            }
            set
            {
                this._errorMessage = value;
                this.RaisePropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (this._cancelCommand == null)
                    this._cancelCommand = (ICommand)new RelayCommand(new Action(this.Cancel), false);
                return this._cancelCommand;
            }
            set
            {
                this.Set<ICommand>(ref this._cancelCommand, value, false, nameof(CancelCommand));
            }
        }

        public virtual void Cancel()
        {
            this.IsBusy = false;
            this.BusyContent = (string)null;
        }
    }
}
