using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Alktom.Intel.ParallelDev.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void bCalculate_Click(object sender, RoutedEventArgs e)
        {
            bCalculate.IsEnabled = false;

            IProgress<int> progress = new Progress<int>(value => progressBar.Value = value);

            await CalculateAsync(progress);

            bCalculate.IsEnabled = true;
        }

        private void Calculate()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(200));

                progressBar.Dispatcher.Invoke(() => progressBar.Value = i);

                // OnDispatcher(() => progressBar.Value = i);

                // this.Dispatcher.Invoke(() => progressBar.Value = i);

                // progressBar.Value = i;

                Trace.Write(i);
            }
        }

        private void Calculate(IProgress<int> progress)
        {
            for (int i = 0; i < 100; i++)
            {
                progress.Report(i);


                Thread.Sleep(TimeSpan.FromMilliseconds(200));


                // progressBar.Dispatcher.Invoke(() => progressBar.Value = i);

                // OnDispatcher(() => progressBar.Value = i);

                // this.Dispatcher.Invoke(() => progressBar.Value = i);

                // progressBar.Value = i;

                Trace.Write(i);
            }
        }

        private void OnDispatcher(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {

            }

            this.Dispatcher.Invoke(action);
        }

        private Task CalculateAsync()
        {
            return Task.Run(() => Calculate());
        }

        private Task CalculateAsync(IProgress<int> progress)
        {           
            return Task.Run(() => Calculate(progress));
        }
    }
}
