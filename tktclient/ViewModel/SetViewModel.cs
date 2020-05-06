using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastReport.Utils;
using tktclient.Db;
using tktclient.Utils;

namespace tktclient.ViewModel
{
    public class SetViewModel : BusyStatusViewModel
    {
        private long _ticketsNumInPrinter;
        private long _initTicketsNumInPrinter;
        private long _ribbonsNumInPrinter;
        private long initRibbonsNumInPrinter;
        private Services.Config cfg = new tktclient.Services.Config();

        public long TicketsNumInPrinter
        {
            get
            {
                return this._ticketsNumInPrinter;
            }
            set
            {
                this.Set<long>(ref this._ticketsNumInPrinter, value, false, nameof(TicketsNumInPrinter));
            }
        }

        public long InitTicketsNumInPrinter
        {
            get
            {
                return this._initTicketsNumInPrinter;
            }
            set
            {
                this.Set<long>(ref this._initTicketsNumInPrinter, value, false, nameof(InitTicketsNumInPrinter));
            }
        }

        public long RibbonsNumInPrinter
        {
            get
            {
                return this._ribbonsNumInPrinter;
            }
            set
            {
                this.Set<long>(ref this._ribbonsNumInPrinter, value, false, nameof(RibbonsNumInPrinter));
            }
        }

        public long InitRibbonsNumInPrinter
        {
            get
            {
                return this.initRibbonsNumInPrinter;
            }
            set
            {
                this.Set<long>(ref this.initRibbonsNumInPrinter, value, false, nameof(InitRibbonsNumInPrinter));
            }
        }

        public SetViewModel()
        {
            LoadPrinterSetting();
        }

        private void LoadPrinterSetting()
        {
            this.InitTicketsNumInPrinter = cfg.GetInitPrinterTickets();
            this.InitRibbonsNumInPrinter = cfg.GetInitPrinterRibbons();
            this.TicketsNumInPrinter = ClientContext.RemainTickets;
            this.RibbonsNumInPrinter = ClientContext.RemainRibbons;
        }

        public async Task Save()
        {
            await StorageProvider.SetRemainTickets(cfg.GetPrinterNo(), (int) this.TicketsNumInPrinter);
            await StorageProvider.SetRemainRibbons(cfg.GetPrinterNo(), (int) this.RibbonsNumInPrinter);
        }

        public async Task ResetPrinter()
        {
            await StorageProvider.SetRemainTickets(cfg.GetPrinterNo(), cfg.GetInitPrinterTickets());
            await StorageProvider.SetRemainRibbons(cfg.GetPrinterNo(), cfg.GetInitPrinterRibbons());
            this.LoadPrinterSetting();
        }

    }
}
