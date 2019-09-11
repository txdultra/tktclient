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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tktclient.Controls
{
    /// <summary>
    /// TicketSelectedButton.xaml 的交互逻辑
    /// </summary>
    public partial class TicketSelectedButton : RadioButton, IComponentConnector
    {
        public static readonly DependencyProperty BarCodeVisibleProperty = DependencyProperty.Register(nameof(BarCodeVisible), typeof(Visibility), typeof(TicketSelectedButton), new PropertyMetadata((object)Visibility.Visible));

        public Visibility BarCodeVisible
        {
            get
            {
                return (Visibility)this.GetValue(TicketSelectedButton.BarCodeVisibleProperty);
            }
            set
            {
                this.SetValue(TicketSelectedButton.BarCodeVisibleProperty, (object)value);
            }
        }

        public event UpdateEnentHander UpdateEnent;

        public TicketSelectedButton()
        {
            this.InitializeComponent();
        }

        private void NumericUpDown_UpdateEnent(bool isfocus)
        {
            UpdateEnentHander updateEnent = this.UpdateEnent;
            if (updateEnent == null)
                return;
            updateEnent(this, isfocus);
        }

        public delegate void UpdateEnentHander(TicketSelectedButton datacontext, bool isfocus);
    }
}
