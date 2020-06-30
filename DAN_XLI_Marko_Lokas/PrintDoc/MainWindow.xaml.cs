using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
        static BackgroundWorker bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
        public string textPanelString;
        public string numberCopiesString;
        public string currentTextPanel;
        public string currentNumberCopies;
        public int sumCopies = 0;
        
        public MainWindow()
        {
            InitializeComponent();

            this.Language = XmlLanguage.GetLanguage("sr-SR");
            txtTextPanel.Focus();
            notice.Visibility = Visibility.Visible;
            Print();
        }

        /// <summary>
        /// Starting BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //If it is started, it prints a message that it has already started
            if (bw.IsBusy)
            {
                MessageBox.Show("Printing is already in progress. Please wait...","Printing...", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                bw = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                currentTextPanel = txtTextPanel.Text.ToString();
                currentNumberCopies = txtNumOfCopies.Text.ToString();

                textPanelString = currentTextPanel;
                numberCopiesString = currentNumberCopies;

                notice.Visibility = Visibility.Collapsed;

                bw.DoWork += bw_DoWork;
                bw.ProgressChanged += bw_ProgressChanged;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.RunWorkerAsync();
                btnStopPrint.IsEnabled = true;
            }
        }

        /// <summary>
        /// An indicator of what it is doing in the background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            sumCopies = 0;
            int percentage = 100 / Convert.ToInt32(numberCopiesString);
            for (int i = 0; i <= 100; i += percentage)
            {
                //Duration of the printing process
                Thread.Sleep(1000);
                sumCopies = sumCopies + 1 ;
                if (i % 3 == 0)
                {
                    bw.ReportProgress(i);
                    if (i == 99)
                    {
                        bw.ReportProgress(100);
                    }
                }
                else
                {
                    bw.ReportProgress(i);
                }
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    bw.ReportProgress(0);
                    return;
                }
                //Naming a storage document
                DateTime date = DateTime.Now;
                string dateString = date.ToString("dd_MM_yyyy_hh_mm");
                string nameDocument = @"..\..\Printed\" + sumCopies + "." + dateString + ".txt";

                if (Convert.ToInt32(numberCopiesString) >= sumCopies)
                {
                    CreateCopies(nameDocument);
                }
            }
            //To always finish 100%
            //Due to the number of copies that leave a remainder when divided by 100.
            bw.ReportProgress(100);
            e.Result = "Finish printing";
        }

        /// <summary>
        /// State BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                lblProgressBar.Content = ("You canceled!");
                sumCopies = 0;
            }
            else if (e.Error != null)
            {
                lblProgressBar.Content = ("Worker exception: " + e.Error.ToString());
            }
            else
            {
                lblProgressBar.Content = ("Successfully completed: " + e.Result);
                //When the printing process is complete, the stop printing button is disabled
                btnStopPrint.IsEnabled = false;
            }
        }

        /// <summary>
        /// Notification to the user of what is happening in the background of the process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (Convert.ToInt32(numberCopiesString) < sumCopies)
            {
                lblProgressBar.Content = "The printing process is soon over...";
            }
            else
            {
                lblProgressBar.Content = "Printing copies " + sumCopies;
            }
            lblPercentage.Content  =  ("Completed " + e.ProgressPercentage + "%");
        }

        /// <summary>
        /// Button that stops the process of creating files (background worker)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStopPrint_Click(object sender, RoutedEventArgs e)
        {
            if (bw.IsBusy)
            {
                bw.CancelAsync();
                btnStopPrint.IsEnabled = false;
                lblPercentage.Content = "Stopped the printing process...";
            }
        }

        /// <summary>
        /// Method for creating copies
        /// </summary>
        /// <param name="location"></param>
        private void CreateCopies(string location)
        {
            //If there is no folder, create it
            if (!Directory.Exists(@"..\..\Printed"))
            {
                Directory.CreateDirectory(@"..\..\Printed");
            }
            //Creates a new file
            using (StreamWriter outputFile = new StreamWriter(location))
            {

                outputFile.WriteLine(textPanelString.ToString());
            }
        }

        /// <summary>
        /// Visual indicators of a correctly filled field
        /// </summary>
        private void InputText(object sender, TextChangedEventArgs e)
        {
            tbCapsLock.Visibility = Visibility.Collapsed;

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

        /// <summary>
        /// Visual indicators of a correctly filled field
        /// </summary>
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

        /// <summary>
        /// Checks that the input fields are filled in correctly and enables the print button accordingly
        /// </summary>
        private void Print()
        {
            //Everything is OK
            if (textPanel && numberCopies && Convert.ToInt16(txtNumOfCopies.Text) != 0)
            {
                btnPrint.IsEnabled = true;
                notice.Foreground = Brushes.Green;
                notice.Text = "Everything is ready to print, press the button.".ToString();
                arrow.Visibility = Visibility.Visible;
            }
            //If a field is not filled in correctly
            else
            {
                btnPrint.IsEnabled = false;
                notice.Foreground = Brushes.Red;
                notice.Visibility = Visibility.Visible;

                arrow.Visibility = Visibility.Collapsed;

                notice.Text = "Notice: \nEnter the desired text in the text input field, " +
                            "then the number of copies. " +
                            "After you have done this, press the \"Print\" " +
                            "button to start the document printing simulation.text input field...".ToString();
            }
        }

        /// <summary>
        /// Prohibits space input
        /// </summary>
        private void TxtBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
            if (e.Key == Key.Space)
            {
                tbCapsLock.Visibility = Visibility.Visible;
                error.Text = "Try without spaces.";
                SystemSounds.Beep.Play();
            }
            else
            {
                tbCapsLock.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Field number of copies, receives numbers only
        /// </summary>
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

        /// <summary>
        /// It forbids entering anything that is not a number
        /// </summary>
        private void PreviewTextInputHandlerDigit(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !DigitAllowed(e.Text);
            MaxNumberCopies();
        }

        /// <summary>
        /// The maximum number of characters is 4 
        /// in the number of copies field
        /// </summary>
        public async void MaxNumberCopies()
        {
            int duration = 5000;
            if (txtNumOfCopies.Text.Length == 4 && txtNumOfCopies.IsFocused == true)
            {
                SystemSounds.Hand.Play();
                error.Text = "Maximum number of characters".ToString();
                tbCapsLock.Visibility = Visibility.Visible;

                await Task.Delay(duration);
                tbCapsLock.Visibility = Visibility.Collapsed;
            }
        }
    }
}
