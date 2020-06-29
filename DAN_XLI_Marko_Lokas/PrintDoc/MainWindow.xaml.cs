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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrintDoc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            notice.Text = "Notice: \nEnter the desired text in the text input field, " +
                            "then the number of copies. " +
                            "After you have done this, press the \"Print\" " +
                            "button to start the document printing simulation.text input field...".ToString();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnStopPrint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
