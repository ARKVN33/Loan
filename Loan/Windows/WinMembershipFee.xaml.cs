using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Loan.Windows
{
    /// <summary>
    /// Interaction logic for WinMembershipFee.xaml
    /// </summary>
    public partial class WinMembershipFee
    {
        public WinMembershipFee()
        {
            InitializeComponent();
        }

        public string MembershipFee { get; set; }

        #region Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnNew_Click(null, null);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(TxtFee.Text.Trim()) || long.Parse(Regex.Replace(TxtFee.Text, "[\\W]", "")) == 0))
            {
                MembershipFee = Regex.Replace(TxtFee.Text, "[\\W]", "");
            }
            Close();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            TxtFee.Text = "0";
        }

        private void TxtFee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtFee.Text.Trim() == string.Empty)
            {
                TxtFee.Text = "0";
            }
            else
            {
                decimal number;
                if (!decimal.TryParse(TxtFee.Text, out number)) return;
                TxtFee.Text = $"{number:N0}";
                TxtFee.SelectionStart = TxtFee.Text.Length;

            }
        }

        #endregion

        #region FixedEvent

        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void NumberInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DisablePaste(object sender, ExecutedRoutedEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = e.Command == ApplicationCommands.Paste && regex.IsMatch(Clipboard.GetText());
        }
        #endregion
    }
}
