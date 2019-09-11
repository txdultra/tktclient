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
    /// NumericUpDown.xaml 的交互逻辑
    /// </summary>
    public partial class NumericUpDown : UserControl, IComponentConnector
    {
        public static readonly DependencyProperty NumReadonlyProperty = DependencyProperty.Register(nameof(NumReadonly), typeof(bool), typeof(NumericUpDown), new PropertyMetadata((object)false));
        public static readonly DependencyProperty MinNumProperty = DependencyProperty.Register(nameof(MinNum), typeof(int), typeof(NumericUpDown), new PropertyMetadata((object)1));
        public static readonly DependencyProperty MaxNumProperty = DependencyProperty.Register(nameof(MaxNum), typeof(int), typeof(NumericUpDown), new PropertyMetadata((object)100000));
        public static readonly DependencyProperty ObjectValueProperty = DependencyProperty.Register(nameof(ObjectValue), typeof(object), typeof(NumericUpDown));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int), typeof(NumericUpDown), new PropertyMetadata((object)1));

        public string Num { get; set; }

        public event NumericUpDown.UpdateEnentHander UpdateEnent;

        public bool NumReadonly
        {
            get
            {
                return (bool)this.GetValue(NumericUpDown.NumReadonlyProperty);
            }
            set
            {
                this.SetValue(NumericUpDown.NumReadonlyProperty, (object)value);
            }
        }

        public int MinNum
        {
            get
            {
                return (int)this.GetValue(NumericUpDown.MinNumProperty);
            }
            set
            {
                this.SetValue(NumericUpDown.MinNumProperty, (object)value);
            }
        }

        public int MaxNum
        {
            get
            {
                return (int)this.GetValue(NumericUpDown.MaxNumProperty);
            }
            set
            {
                this.SetValue(NumericUpDown.MaxNumProperty, (object)value);
            }
        }

        public object ObjectValue
        {
            get
            {
                return this.GetValue(NumericUpDown.ObjectValueProperty);
            }
            set
            {
                this.SetValue(NumericUpDown.ObjectValueProperty, value);
            }
        }

        public int Value
        {
            get
            {
                return (int)this.GetValue(NumericUpDown.ValueProperty);
            }
            set
            {
                this.SetValue(NumericUpDown.ValueProperty, (object)value);
            }
        }

        public NumericUpDown()
        {
            this.InitializeComponent();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbNum?.Text))
                return;
            int result = 0;
            if (!int.TryParse(this.tbNum.Text, out result))
                return;
            if (result > this.MinNum)
            {
                this.tbNum.Text = (--result).ToString();
                NumericUpDown.UpdateEnentHander updateEnent = this.UpdateEnent;
                if (updateEnent == null)
                    return;
                updateEnent(false);
            }
            else
            {
                NumericUpDown.UpdateEnentHander updateEnent = this.UpdateEnent;
                if (updateEnent == null)
                    return;
                updateEnent(true);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbNum?.Text))
                return;
            int result = 0;
            if (!int.TryParse(this.tbNum.Text, out result))
                return;
            if (result < this.MaxNum)
            {
                this.tbNum.Text = (++result).ToString();
                NumericUpDown.UpdateEnentHander updateEnent = this.UpdateEnent;
                if (updateEnent == null)
                    return;
                updateEnent(false);
            }
            else
            {
                NumericUpDown.UpdateEnentHander updateEnent = this.UpdateEnent;
                if (updateEnent == null)
                    return;
                updateEnent(true);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Update();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Update();
        }

        private void tbNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            this.tbFocus.Focus();
            this.Update();
        }

        private void Update()
        {
            int num;
            if (string.IsNullOrEmpty(this.tbNum.Text))
            {
                TextBox tbNum = this.tbNum;
                num = this.MinNum;
                string str = num.ToString();
                tbNum.Text = str;
            }
            int result = 0;
            if (int.TryParse(this.tbNum.Text, out result))
            {
                if (result < this.MinNum)
                {
                    TextBox tbNum = this.tbNum;
                    num = this.MinNum;
                    string str = num.ToString();
                    tbNum.Text = str;
                }
                if (result > this.MaxNum)
                {
                    TextBox tbNum = this.tbNum;
                    num = this.MaxNum;
                    string str = num.ToString();
                    tbNum.Text = str;
                }
                NumericUpDown.UpdateEnentHander updateEnent = this.UpdateEnent;
                if (updateEnent == null)
                    return;
                updateEnent(false);
            }
            else
            {
                TextBox tbNum = this.tbNum;
                num = this.MinNum;
                string str = num.ToString();
                tbNum.Text = str;
                NumericUpDown.UpdateEnentHander updateEnent = this.UpdateEnent;
                if (updateEnent == null)
                    return;
                updateEnent(false);
            }
        }

        private void tbNum_GotFocus(object sender, RoutedEventArgs e)
        {
            NumericUpDown.UpdateEnentHander updateEnent = this.UpdateEnent;
            if (updateEnent == null)
                return;
            updateEnent(true);
        }

        public delegate void UpdateEnentHander(bool isfocus);
    }
}
