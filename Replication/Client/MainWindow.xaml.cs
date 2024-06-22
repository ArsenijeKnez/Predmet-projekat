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
using System.Net;
using System.Net.Sockets;
using static Client.Communication;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int ServiceID;
        private int ServiceType;

        public MainWindow(int serviceID, int serviceType)
        {
            ServiceID = serviceType;
            ServiceType = serviceType;
            InitializeComponent();
            if (serviceType == 1)
                ReplicateButton.IsEnabled = false;
            InitializeWindowsSockets();
            RegisterService(serviceID, serviceType);
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;
            OutputTextBox.AppendText($"You entered: {input}\n");
            ProcessInput(input);
        }

        private void ProcessInput(string input)
        {
            string sendData = input;
            if (isSave.IsChecked == true)
            {
                sendData = input + "Save!@#$%^&&*";
            }
            SendData(ServiceID, sendData);
            InputTextBox.Text = "";
        }

        private bool InitializeWindowsSockets()
        {
            try
            {
                Communication.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                return true;
            }
            catch (Exception ex)
            {
                OutputTextBox.AppendText($"Socket initialization failed: {ex.Message}\n");
                return false;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CloseHandler(ServiceID))
            {
                if(MessageBox.Show("Connection unssucesfuly closed, do you still want to exit?", "Close window", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Register rg = new Register();
                    rg.Show();
                    this.Close();
                }
            }
            else
            {
                Register rg = new Register();
                rg.Show();
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string data = CallbackHandler(ServiceID);
            OutputTextBox.AppendText($"Last replicated message: {data}\n");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (ServiceType == 1)
                TestSync(ServiceID);
            else
                TestAsync(ServiceID);
        }

        private void ReplicateButton_Click(object sender, RoutedEventArgs e)
        {
            if(ServiceType == 2)
            {
                if (ReplicateHandler(ServiceID))
                {
                    OutputTextBox.AppendText($"Succesfuly replicated\n");
                }
                else
                {
                    OutputTextBox.AppendText($"Failed to replicate\n");
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string data = GraphHandler(ServiceID);
            Graph g = new Graph(data);
            g.Show();
            g.Focus();
        }
    }
}
