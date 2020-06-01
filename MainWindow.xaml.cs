using Big_Finish_Player.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Big_Finish_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PlayerVM _vm;
        private static System.Timers.Timer postionUpdateTimer;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new PlayerVM(configuration: new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());

            DataContext = _vm;
            SetTimer();
        }

        private void SetTimer()
        {
            postionUpdateTimer = new System.Timers.Timer(100);
            postionUpdateTimer.Elapsed += OnTimedEvent;
            postionUpdateTimer.AutoReset = true;
            postionUpdateTimer.Enabled = true;
        }

        private  void OnTimedEvent(Object source, object e)
        {
            _vm.UpdateCurrentPosition();
        }

    }
}
