using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Xaml;

namespace PrintDoc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool textPanel;
        public bool numberCopies;

        public MainWindow()
        {
            InitializeComponent();

            this.Language = XmlLanguage.GetLanguage("sr-SR");
            txtTextPanel.Focus();
            Print();


        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStopPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InputText(object sender, TextChangedEventArgs e)
        {
            if (txtTextPanel.Text.Length < 1)
            {
                txtTextPanel.BorderBrush = new SolidColorBrush(Colors.Red);
                txtTextPanel.Foreground = new SolidColorBrush(Colors.Red);
                textPanel = false;
            }
            else
            {
                txtTextPanel.BorderBrush = new SolidColorBrush(Colors.Green);
                txtTextPanel.Foreground = new SolidColorBrush(Colors.Green);
                textPanel = true;
            }
            Print();

        }

        private void NumberCopies(object sender, TextChangedEventArgs e)
        {
            if (txtNumOfCopies.Text.Length < 1)
            {
                txtNumOfCopies.BorderBrush = new SolidColorBrush(Colors.Red);
                txtNumOfCopies.Foreground = new SolidColorBrush(Colors.Red);
                numberCopies = false;
            }
            else
            {
                txtNumOfCopies.BorderBrush = new SolidColorBrush(Colors.Green);
                txtNumOfCopies.Foreground = new SolidColorBrush(Colors.Green);
                numberCopies = true;
            }
            if (txtNumOfCopies.Text.Length >= 4)
            {
                SystemSounds.Beep.Play();
            }
            Print();
        }

        private void Print()
        {
            if (textPanel && numberCopies)
            {
                btnPrint.IsEnabled = true;
                notice.Foreground = Brushes.Green;
                notice.Text = "Everything is ready to print, press the button.".ToString();
            }
            else
            {
                btnPrint.IsEnabled = false;
                notice.Foreground = Brushes.Red;
                notice.Text = "Notice: \nEnter the desired text in the text input field, " +
                            "then the number of copies. " +
                            "After you have done this, press the \"Print\" " +
                            "button to start the document printing simulation.text input field...".ToString();

            }
        }

        private void TxtBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
            if (e.Key == Key.Space)
            {
                tbCapsLock.Visibility = Visibility.Visible;
                //
                error.Text = "Try without spaces.";
                SystemSounds.Beep.Play();
            }
            else
            {
                tbCapsLock.Visibility = Visibility.Collapsed;
            }
        }

        private Boolean DigitAllowed(String s)
        {
            foreach (Char c in s.ToCharArray())
            {
                if (Char.IsDigit(c))
                {
                    tbCapsLock.Visibility = Visibility.Collapsed;
                    continue;
                }
                else
                {
                    error.Text = "Only numbers are allowed.".ToString();
                    tbCapsLock.Visibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                    return false;
                }
            }
            return true;
        }

        private void PreviewTextInputHandlerDigit(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !DigitAllowed(e.Text);
            MaxNumberCopies();
        }

        public async void MaxNumberCopies()
        {
            int trajanjePoruke = 5000;
            if (txtNumOfCopies.Text.Length == 4 && txtNumOfCopies.IsFocused == true)
            {
                SystemSounds.Hand.Play();
                error.Text = "Maximum number of characters".ToString();
                tbCapsLock.Visibility = Visibility.Visible;

                await Task.Delay(trajanjePoruke);
                tbCapsLock.Visibility = Visibility.Collapsed;
            }
        }
    }
}
