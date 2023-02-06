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
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

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
            var config = Config.Config.Instance;
            ApiLinkInput.Text = config.MissionHackerConfig?.General!.BitApi;
            WindowIDInput.Text = config.MissionHackerConfig?.General!.BitBrowserId;
            Id.Text = config.MissionHackerConfig?.General!.Id;
            ProxyApiInput.Text = config.MissionHackerConfig?.General!.ProxyApi;
            MissionEvents.ThrowExceptionEvent += (sender, args) =>
            {
                // Error.Add($"{args.SimpleMessage}{Environment.NewLine}{args.FullMessage}{Environment.NewLine}");
                AddErrorMsg($"{args.SimpleMessage}{Environment.NewLine}");
            };
            MissionEvents.ThrowUnrecoverableExceptionEvent += (sender, args) =>
            {
                // Error.Add($"{args.SimpleMessage}{Environment.NewLine}{args.FullMessage}{Environment.NewLine}");
                AddErrorMsg($"{args.SimpleMessage}{Environment.NewLine}{args.FullMessage}{Environment.NewLine}");
            };
            MissionEvents.MissionDoneEvent += (i) => ProgressBar.Value = i;
            MissionEvents.MissionLoadedEvent += (i, e) => ProgressBar.Maximum = e.Max;
            ProgramEvents.ProgramEndEvent += (o, e) => 
            {
                Run.IsEnabled = true;
            };
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Config.Config.Instance.SaveBitBrowserConfig(ApiLinkInput.Text, WindowIDInput.Text);
            Config.Config.Instance.SaveId(Id.Text);
            Config.Config.Instance.SaveProxyApi(ProxyApiInput.Text);
        }
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Run.IsEnabled = false;
            // var backgroundWorker = new BackgroundWorker();
            // backgroundWorker.DoWork += Start;
            var t = new Thread(() =>
            {
                Run.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Func<Task>(Start));
            });
            t.Start();
            // backgroundWorker.RunWorkerCompleted += (s,e) => { ProgramEvents.ProgramEndHandler(this); };
            // backgroundWorker.RunWorkerAsync();
        }
       
        private async Task Start()
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
                // Run.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Func<Task>(_hacker.Run));
                await _hacker.Run();
            }
            catch (Exception exception)
            {
                MissionEvents.ThrowUnrecoverableException(this, exception, "此任务运行失败");
            }
        }

        private void ClearFinished_Click(object sender, RoutedEventArgs e)
        {
            Libs.Instance.ClearTodayFinished();
        }

        private void AddErrorMsg(string msg)
        {
            var sb = new StringBuilder();
            Error.Add(msg);
            var revered = Error;
            revered.Reverse();
            foreach (var s in revered)
            {
                sb.Append(s);
            }
            Debug.Text = sb.ToString();
        }
    }
}