using MissionHacker.ConfigHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Events;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread _browserThread;
        public MainWindow()
        {
            InitializeComponent();
            var config = Config.Instance;
            ApiLinkInput.Text = config.General.BitApi;
            WindowIDInput.Text = config.General.BitBrowserId;
            MissionEvents.ThrowExceptionEvent += (sender, args) => {
                Debug.Text += $"{args.SimpleMessage}{Environment.NewLine}{args.FullMessage}{Environment.NewLine}";
            };
            MissionEvents.MissionDoneEvent += (i) => ProgressBar.Value = i;
            MissionEvents.MissionLoadedEvent += (i, e) => ProgressBar.Maximum = e.Max;
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Config.Instance.SaveBitBrowserConfig(ApiLinkInput.Text, WindowIDInput.Text);
        }
        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            Run.IsEnabled = false;
            await RunAsync();
            Run.IsEnabled = true;
        }
        private async Task RunAsync()
        {
            var hacker = new MissionHacker.MissionHacker();
            await hacker.LoadMission();
            try
            {
                await hacker.Run();
            }
            catch (Exception exception)
            {
                MissionEvents.ThrowException(this, new()
                {
                    Exception = exception,
                    FullMessage = exception.ToString(),
                    SimpleMessage = exception.Message,
                });
            }
        }
    }
}