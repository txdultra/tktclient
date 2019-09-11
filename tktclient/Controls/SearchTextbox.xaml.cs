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
    /// SearchTextbox.xaml 的交互逻辑
    /// </summary>
    public partial class SearchTextbox : Border, IComponentConnector
    {
        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(nameof(SearchCommand), typeof(ICommand), typeof(SearchTextbox));
        public static readonly DependencyProperty InputEnterCommandProperty = DependencyProperty.Register(nameof(InputEnterCommand), typeof(ICommand), typeof(SearchTextbox));
        public static readonly DependencyProperty InputMethodEnableProperty = DependencyProperty.Register(nameof(InputMethodEnable), typeof(bool), typeof(SearchTextbox), new PropertyMetadata((object)true));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(SearchTextbox));
        public static readonly DependencyProperty SearchFontSizeProperty = DependencyProperty.Register(nameof(SearchFontSize), typeof(int), typeof(SearchTextbox));
        public static readonly DependencyProperty SearchHintProperty = DependencyProperty.Register(nameof(SearchHint), typeof(string), typeof(SearchTextbox));
        public static readonly DependencyProperty SearchDelVisibleProperty = DependencyProperty.Register(nameof(SearchDelVisible), typeof(bool), typeof(SearchTextbox), new PropertyMetadata((object)false));
        public static readonly DependencyProperty SearchVisibleProperty = DependencyProperty.Register(nameof(SearchVisible), typeof(bool), typeof(SearchTextbox), new PropertyMetadata((object)true));

        public ICommand SearchCommand
        {
            get
            {
                return (ICommand)this.GetValue(SearchCommandProperty);
            }
            set
            {
                this.SetValue(SearchCommandProperty, (object)value);
            }
        }

        public ICommand InputEnterCommand
        {
            get
            {
                return (ICommand)this.GetValue(InputEnterCommandProperty);
            }
            set
            {
                this.SetValue(InputEnterCommandProperty, (object)value);
            }
        }

        public bool InputMethodEnable
        {
            get
            {
                return (bool)this.GetValue(InputMethodEnableProperty);
            }
            set
            {
                this.SetValue(InputMethodEnableProperty, (object)value);
            }
        }

        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, (object)value);
            }
        }

        public int SearchFontSize
        {
            get
            {
                return (int)this.GetValue(SearchFontSizeProperty);
            }
            set
            {
                this.SetValue(SearchFontSizeProperty, value);
            }
        }

        public string SearchHint
        {
            get
            {
                return (string)this.GetValue(SearchHintProperty);
            }
            set
            {
                this.SetValue(SearchHintProperty, value);
            }
        }

        public bool SearchDelVisible
        {
            get
            {
                return (bool)this.GetValue(SearchDelVisibleProperty);
            }
            set
            {
                this.SetValue(SearchDelVisibleProperty, value);
            }
        }

        public bool SearchVisible
        {
            get
            {
                return (bool)this.GetValue(SearchTextbox.SearchVisibleProperty);
            }
            set
            {
                this.SetValue(SearchVisibleProperty, value);
            }
        }

        public SearchTextbox()
        {
            this.InitializeComponent();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            this.Text = null;
        }

        public void SetFocus()
        {
            this.tbTxt.Focus();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
