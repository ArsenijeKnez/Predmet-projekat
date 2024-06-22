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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int serviceID;
            int serviceType;
            if (ComboReplicator.SelectedItem == R1)
            {
                serviceID = 1;
            }
            else
            {
                serviceID = 2;
            }
            if (ComboType.SelectedItem == S)
            {
                serviceType = 1;
            }
            else
            {
                serviceType = 2;
            }
            MainWindow mw = new MainWindow(serviceID, serviceType);
            mw.Show();
            this.Close();
        }
    }
}
