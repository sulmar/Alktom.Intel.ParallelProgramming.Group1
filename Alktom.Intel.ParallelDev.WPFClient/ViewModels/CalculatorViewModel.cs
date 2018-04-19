using Alktom.Intel.ParallelDev.WPFClient.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alktom.Intel.ParallelDev.WPFClient.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }


    }

    public class CalculatorViewModel : BaseViewModel
    {
        public CalculatorViewModel()
        {
            CalculateCommand = new RelayCommand(p => CalculateAsync(), p => CanCalculate());
            CancelCalculateCommand = new RelayCommand(p => cts.Cancel());

            progress = new Progress<int>(value => this.Step = value);
        }

        public ICommand CalculateCommand { get; set; }
        public ICommand CancelCalculateCommand { get; set; }

        public bool CanCancelCalculate => cts.Token.CanBeCanceled;

        private CancellationTokenSource cts = new CancellationTokenSource();
        private IProgress<int> progress { get; set; }

        private int step;
        public int Step
        {
            get
            {
                return step;
            }

            set
            {
                this.step = value;
                OnPropertyChanged(nameof(Step));
            }
        }

        private void CancelCalculate()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
        }

        //private ICommand calculateCommand;
        //public ICommand CalculateCommand
        //{
        //    get
        //    {
        //        if (calculateCommand == null)
        //        {
        //            calculateCommand = new RelayCommand(p => CalculateAsync(), p=>CanCalculate());
        //        }

        //        return calculateCommand;
        //    }
        //}


        public void Calculate(CancellationToken token)
        {
            for (int i = 0; i < 100; i++)
            {
                if (token.IsCancellationRequested)
                {
                    progress.Report(0);

                    break;
                }

                progress.Report(i);
                Trace.Write(i);



                Thread.Sleep(TimeSpan.FromMilliseconds(200));
            }
        }

        public Task CalculateAsync()
        {
            cts = new CancellationTokenSource();

            return Task.Run(() => Calculate(cts.Token));
        }

        public bool CanCalculate()
        {
            return true;
        }
    }
}
