using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using tktclient.Utils;

namespace tktclient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BusyStatusViewModel
    {
        private ObservableCollection<NoticeMess> _noticeMessages;

        public ObservableCollection<NoticeMess> NoticeMessages
        {
            get
            {
                if (this._noticeMessages == null)
                    this._noticeMessages = new ObservableCollection<NoticeMess>();
                return this._noticeMessages;
            }
            set
            {
                this._noticeMessages = value;
                this.RaisePropertyChanged(nameof(NoticeMessages));
            }
        }

        public void Remove(NoticeMess mess)
        {
            if (!this.NoticeMessages.Contains(mess))
                return;
            this.NoticeMessages.Remove(mess);
        }

        public void AddMessage(NoticeMess mess)
        {
            this.NoticeMessages.Add(mess);
        }
    }
}