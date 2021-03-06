/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:tktclient"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;

namespace tktclient.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<TicketSaleViewModel>();
            SimpleIoc.Default.Register<ConfirmPayViewModel>();
            SimpleIoc.Default.Register<TicketExchangeViewModel>();
            SimpleIoc.Default.Register<B2BSaleViewModel>();
            SimpleIoc.Default.Register<SetViewModel>();
            SimpleIoc.Default.Register<TicketTeamViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public TicketSaleViewModel TicketSale
        {
            get
            {
                return new TicketSaleViewModel();
            }
        }

        public ConfirmPayViewModel ConfirmPay
        {
            get
            {
                return new ConfirmPayViewModel();
            }
        }

        public TicketExchangeViewModel TicketExchange
        {
            get
            {
                return new TicketExchangeViewModel();
            }
        }

        public B2BSaleViewModel B2BSale
        {
            get
            {
                return new B2BSaleViewModel();
            }
        }

        public SetViewModel SetView
        {
            get
            {
                return new SetViewModel();
            }
        }

        public TicketTeamViewModel TicketTeam
        {
            get
            {
                return new TicketTeamViewModel();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}