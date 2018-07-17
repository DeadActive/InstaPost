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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        private InstagramApi api;
        private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        public MainPage()
        {
            InitializeComponent();
        }

        public object API
        {
            get
            {
                return api;
            }
            set
            {
                api = (InstagramApi)value;
            }
        }

        public void InitializeApiVars()
        {
            username_label.Text = "Username: " + api.getUsername;
            usernameid_label.Text = "ID: " + api.getUsernameID;
        }

        private void logout_button_Click(object sender, RoutedEventArgs e)
        {
            api.logout();
            mw.transitionerIndex = 0;
        }
    }
}
