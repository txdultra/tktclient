using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// ProcessGrid.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessGrid : UserControl , IComponentConnector
    {
        public static readonly DependencyProperty BusyTextProperty = DependencyProperty.Register(nameof(BusyText), typeof(string), typeof(ProcessGrid), (PropertyMetadata)new UIPropertyMetadata((object)"处理中...", new PropertyChangedCallback(ProcessGrid.TextChangedCallback)));
        public static readonly DependencyProperty GridBackgroundProperty = DependencyProperty.Register(nameof(GridBackground), typeof(SolidColorBrush), typeof(ProcessGrid), new PropertyMetadata((object)new SolidColorBrush(Colors.Black)));
        public static readonly DependencyProperty IsCancelAbleProperty = DependencyProperty.Register(nameof(IsCancelAble), typeof(bool), typeof(ProcessGrid), new PropertyMetadata((object)false));
        public static readonly DependencyProperty ProcessCancelCommandProperty = DependencyProperty.Register(nameof(ProcessCancelCommand), typeof(ICommand), typeof(ProcessGrid));
        public static readonly DependencyProperty GridVisibleProperty = DependencyProperty.Register(nameof(GridVisible), typeof(bool), typeof(ProcessGrid), new PropertyMetadata((object)true));
        public static readonly DependencyProperty ProCancelForegroundProperty = DependencyProperty.Register(nameof(ProCancelForeground), typeof(SolidColorBrush), typeof(ProcessGrid), new PropertyMetadata((object)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)48, (byte)167, (byte)250))));

        [Description("处理时，显示出的文本")]
        [Category("遮罩层")]
        [DefaultValue("处理中...")]
        public string BusyText
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this.GetValue(ProcessGrid.BusyTextProperty);
            }
            [DebuggerStepThrough]
            set
            {
                this.SetValue(ProcessGrid.BusyTextProperty, (object)value);
                this.tbText.Text = value;
            }
        }

        public SolidColorBrush GridBackground
        {
            get
            {
                return (SolidColorBrush)this.GetValue(ProcessGrid.GridBackgroundProperty);
            }
            set
            {
                this.SetValue(ProcessGrid.GridBackgroundProperty, (object)value);
            }
        }

        public bool IsCancelAble
        {
            get
            {
                return (bool)this.GetValue(ProcessGrid.IsCancelAbleProperty);
            }
            set
            {
                this.SetValue(ProcessGrid.IsCancelAbleProperty, (object)value);
            }
        }

        public ICommand ProcessCancelCommand
        {
            get
            {
                return (ICommand)this.GetValue(ProcessGrid.ProcessCancelCommandProperty);
            }
            set
            {
                this.SetValue(ProcessGrid.ProcessCancelCommandProperty, (object)value);
            }
        }

        public bool GridVisible
        {
            get
            {
                return (bool)this.GetValue(ProcessGrid.GridVisibleProperty);
            }
            set
            {
                this.SetValue(ProcessGrid.GridVisibleProperty, (object)value);
            }
        }

        public SolidColorBrush ProCancelForeground
        {
            get
            {
                return (SolidColorBrush)this.GetValue(ProcessGrid.ProCancelForegroundProperty);
            }
            set
            {
                this.SetValue(ProcessGrid.ProCancelForegroundProperty, (object)value);
            }
        }

        public ProcessGrid()
        {
            this.InitializeComponent();
        }

        private static void TextChangedCallback(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            (d as ProcessGrid).tbText.Text = e.NewValue as string;
        }
    }
}
