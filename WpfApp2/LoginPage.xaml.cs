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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        private InstagramApi api;
        private MainWindow mw = (MainWindow)Application.Current.MainWindow;
        private System.Windows.Threading.DispatcherTimer loginTimer = new System.Windows.Threading.DispatcherTimer();
        private MaterialDesignThemes.Wpf.SnackbarMessageQueue smq;
        private int fails = 0;

        public LoginPage()
        {
            InitializeComponent();
            
        }

        private void InitLoginTimer()
        {
            loginTimer.Tick += new EventHandler(loginTimer_tick);
            loginTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            loginTimer.Start();
        }

        private void loginTimer_tick(object sender, EventArgs e)
        {
            if (api.loggedIn)
            {
                fails = 0;
                loginTimer.Stop();
                mw.transitionerIndex = 2;
                MainPage mp = (MainPage)mw.getMainPage;
                mp.API = api;
                mp.InitializeApiVars();
                mw.Enqueue("Login succesfully!");
            }
            else if(api.fail)
            {
                fails++;
                mw.transitionerIndex = 0;
                if(fails < 2)
                    mw.Enqueue("Can't login!");
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            api = new InstagramApi(usernameBox.Text, passwordBox.Password);
            InitLoginTimer();
            api.login();
            fails = 0;
        }
    }
}
