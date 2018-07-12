using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace TimySimulator.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private RelayCommand<string> numberButtonCommand;
        private string bibNumber;
        private string elapsedTime;
        private Stopwatch stopWatch;
        private DispatcherTimer timer;

        public MainWindowViewModel()
        {
            timer = new DispatcherTimer();
            timer.Tick += DispatcherTimerTick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            stopWatch = new Stopwatch();
            stopWatch.Start();
            timer.Start();
        }

        public string BibNumber
        {
            get { return bibNumber; }
            set { SetField(ref bibNumber, value); }
        }

        public string ElapsedTime
        {
            get { return elapsedTime; }
            set { SetField(ref elapsedTime, value); }
        }

        public RelayCommand<string> NumberButtonCommand
        {
            get
            {
                if (numberButtonCommand == null)
                    numberButtonCommand = new RelayCommand<string>(param => NumberButton(param));
                return numberButtonCommand;
            }
        }

        private void NumberButton(string number)
        {
            BibNumber += number;
        }

        private void DispatcherTimerTick(object sender, object e)
        {
            ElapsedTime = stopWatch.Elapsed.ToString(@"hh\:mm\:ss\.ff");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
