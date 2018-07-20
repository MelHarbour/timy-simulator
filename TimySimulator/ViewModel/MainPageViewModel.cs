using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace TimySimulator.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private Lazy<RelayCommand<string>> numberButtonCommand;
        private Lazy<RelayCommand> modeButtonCommand;
        private Lazy<RelayCommand> startButtonCommand;
        private Lazy<RelayCommand> stopButtonCommand;
        private string bibNumber = "1";
        private string elapsedTime;
        private Stopwatch stopWatch = new Stopwatch();
        private DispatcherTimer timer = new DispatcherTimer();
        private Lazy<ObservableCollection<Result>> results = new Lazy<ObservableCollection<Result>>();
        private TimyMode mode;

        public MainPageViewModel()
        {
            numberButtonCommand = new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(param => NumberButton(param)));
            modeButtonCommand = new Lazy<RelayCommand>(() => new RelayCommand(param => ModeButton(), param => true));
            startButtonCommand = new Lazy<RelayCommand>(() => new RelayCommand(param => Impulse(0), param => true));
            stopButtonCommand = new Lazy<RelayCommand>(() => new RelayCommand(param => Impulse(1), param => true));
            timer.Tick += DispatcherTimerTick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
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

        public ObservableCollection<Result> Results {
            get { return results.Value; }
        }

        public ObservableCollection<Result> DisplayResults
        {
            get
            {
                return new ObservableCollection<Result>(Results.OrderBy(x => x.ResultId));
            }
        }

        public TimyMode Mode
        {
            get { return mode; }
            set
            {
                SetField(ref mode, value);
                OnPropertyChanged("ModeText");
            }
        }

        public string ModeText
        {
            get
            {
                if (Mode == TimyMode.Memory)
                    return "NORM";
                else
                    return "MEMO";
            }
        }

        public RelayCommand<string> NumberButtonCommand
        {
            get { return numberButtonCommand.Value; }
        }

        public RelayCommand ModeButtonCommand
        {
            get { return modeButtonCommand.Value; }
        }

        public RelayCommand StartButtonCommand
        {
            get { return startButtonCommand.Value; }
        }

        public RelayCommand StopButtonCommand
        {
            get { return stopButtonCommand.Value; }
        }

        private void Impulse(int channel)
        {
            Results.Add(new Result
            {
                BibNumber = Int32.Parse(BibNumber),
                Channel = channel,
                Time = stopWatch.Elapsed,
                IsManualTime = true
            });
            OnPropertyChanged("DisplayResults");
        }

        private void NumberButton(string number)
        {
            BibNumber += number;
        }

        private void ModeButton()
        {
            if (Mode == TimyMode.Memory)
                Mode = TimyMode.Normal;
            else
                Mode = TimyMode.Memory;
        }

        private void DispatcherTimerTick(object sender, object e)
        {
            ElapsedTime = stopWatch.Elapsed.ToString(@"hh\:mm\:ss\.f");
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

    public enum TimyMode
    {
        Normal,
        Memory
    }
}
