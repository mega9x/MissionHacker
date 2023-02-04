using MissionHacker.ConfigHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DataLibs;
using Events;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MissionHacker.MissionHacker _hacker = new();
        private List<string> Error { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            var config = Config.Instance;
            ApiLinkInput.Text = config.MissionHackerConfig?.General!.BitApi;
            WindowIDInput.Text = config.MissionHackerConfig?.General!.BitBrowserId;
            Id.Text = config.MissionHackerConfig?.General!.Id;
            ProxyApiInput.Text = config.MissionHackerConfig?.General!.ProxyApi;
            MissionEvents.ThrowExceptionEvent += (sender, args) =>
            {
                var sb = new StringBuilder();
                Error.Add($"{args.SimpleMessage}{Environment.NewLine}{args.FullMessage}{Environment.NewLine}");
                var revered = Error;
                revered.Reverse();
                foreach (var s in revered)
                {
                    sb.Append(s);
                }
                Debug.Text = sb.ToString();
            };
            MissionEvents.MissionDoneEvent += (i) => ProgressBar.Value = i;
            MissionEvents.MissionLoadedEvent += (i, e) => ProgressBar.Maximum = e.Max;
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Config.Instance.SaveBitBrowserConfig(ApiLinkInput.Text, WindowIDInput.Text);
            Config.Instance.SaveId(Id.Text);
            Config.Instance.SaveProxyApi(ProxyApiInput.Text);
        }
        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            Run.IsEnabled = false;
            _hacker.CanChangeIp = ChangeIpChecked.IsChecked ?? false;
            await RunAsync();
            Run.IsEnabled = true;
        }
        private async Task RunAsync()
        {
            // var hacker = new MissionHacker.MissionHacker();
            await _hacker.LoadMission();
            try
            {
                await _hacker.Run();
            }
            catch (Exception exception)
            {
                MissionEvents.ThrowException(this, exception, "此任务运行失败");
            }
        }

        private void ClearFinished_Click(object sender, RoutedEventArgs e)
        {
            Libs.Instance.ClearTodayFinished();
        }
    }
}