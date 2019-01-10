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


namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public object transitionerIndex
        {
            get
            {
                return Transitioner.SelectedIndex;
            }
            set
            {
                Transitioner.SelectedIndex = (int)value;
            }
        }

        public object getMainPage
        {
            get
            {
                return mPage;
            }
        }

        public void Enqueue(string msg)
        {
            snackbar.MessageQueue.Enqueue(msg);
        }

        public async void ShowLoginWarning()
        {
            
            //await MaterialDesignThemes.Wpf.DialogHost.Show(login_error_dialog.Content);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.Out.WriteLine("closing");
        }
    }
}
