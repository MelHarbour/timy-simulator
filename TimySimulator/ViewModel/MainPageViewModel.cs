using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
        private Lazy<RelayCommand> upButtonCommand;
        private Lazy<RelayCommand> downButtonCommand;
        private Lazy<RelayCommand> stnButtonCommand;
        private Lazy<RelayCommand> okButtonCommand;
        private string bibNumber = "1";
        private string elapsedTime;
        private bool isEditingBibNumber;
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
            upButtonCommand = new Lazy<RelayCommand>(() => new RelayCommand(param => UpButton(), param => true));
            downButtonCommand = new Lazy<RelayCommand>(() => new RelayCommand(param => DownButton(), param => true));
            stnButtonCommand = new Lazy<RelayCommand>(() => new RelayCommand(param => StnButton(), param => true));
            okButtonCommand = new Lazy<RelayCommand>(() => new RelayCommand(param => OkButton(), param => true));
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
        
        public int FocussedResultId { get; set; }
        public bool IsEditingBibNumber
        {
            get { return isEditingBibNumber; }
            set { SetField(ref isEditingBibNumber, value); }
        }

        public ObservableCollection<Result> DisplayResults
        {
            get
            {
                if (Mode == TimyMode.Normal)
                {
                    return new ObservableCollection<Result>
                        (
                            Results.OrderBy(x => x.ResultId)
                                .Where(x => x.ResultId >= FocussedResultId - 4 && x.ResultId <= FocussedResultId)
                        );
                }
                else
                {
                    return new ObservableCollection<Result>
                        (
                            Results.Where(x => !x.IsSaved).OrderBy(x => x.ResultId)
                        );
                }
            }
        }

        public TimyMode Mode
        {
            get { return mode; }
            set
            {
                SetField(ref mode, value);
                OnPropertyChanged("ModeText");
                OnPropertyChanged("DisplayResults");
                OnPropertyChanged("ModeVisibility");
                OnPropertyChanged("CopyStnText");
                OnPropertyChanged("AktOkText");
            }
        }

        public Visibility ModeVisibility
        {
            get
            {
                if (Mode == TimyMode.Memory)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
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

        public string CopyStnText
        {
            get
            {
                if (Mode == TimyMode.Memory)
                    return "COPY";
                else
                    return "STN";
            }
        }

        public string AktOkText
        {
            get
            {
                if (Mode == TimyMode.Memory)
                    return "OK";
                else
                    return "AKT";
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

        public RelayCommand UpButtonCommand
        {
            get { return upButtonCommand.Value; }
        }

        public RelayCommand DownButtonCommand
        {
            get { return downButtonCommand.Value; }
        }

        public RelayCommand StnButtonCommand
        {
            get { return stnButtonCommand.Value; }
        }

        public RelayCommand OkButtonCommand
        {
            get { return okButtonCommand.Value; }
        }

        private void Impulse(int channel)
        {
            int maxResultId = 0;
            if (Results.Count > 0)
                maxResultId = Results.Select(x => x.ResultId).Max();

            Results.Add(new Result
            {
                BibNumber = Int32.Parse(BibNumber, CultureInfo.InvariantCulture),
                Channel = channel,
                Time = stopWatch.Elapsed,
                IsManualTime = true,
                ResultId = maxResultId + 1,
                IsSaved = Mode == TimyMode.Normal
            });
            if (FocussedResultId == maxResultId)
                FocussedResultId++;
            OnPropertyChanged("DisplayResults");
        }

        private void NumberButton(string number)
        {
            if (Mode == TimyMode.Normal)
                BibNumber += number;
            else
            {
                var currentResult = Results.Where(x => !x.IsSaved).OrderBy(x => x.ResultId)?.First();
                if (currentResult != null)
                {
                    if (currentResult.BibNumberText.Length == 4)
                        currentResult.BibNumberText = currentResult.BibNumberText.Substring(1);
                    currentResult.BibNumberText += number;
                    OnPropertyChanged("DisplayResults");
                }
            }
        }

        private void OkButton()
        {
            if (Mode == TimyMode.Memory)
            {
                var memoryResults = Results.Where(x => !x.IsSaved).OrderBy(x => x.ResultId);
                if (memoryResults.Count() > 0)
                {
                    memoryResults.First().IsSaved = true;
                    OnPropertyChanged("DisplayResults");
                }
            }
        }

        private void ModeButton()
        {
            if (Mode == TimyMode.Memory)
                Mode = TimyMode.Normal;
            else
                Mode = TimyMode.Memory;
        }

        private void UpButton()
        {
            if (FocussedResultId > 1)
                FocussedResultId--;
            OnPropertyChanged("DisplayResults");
        }

        private void DownButton()
        {
            if (FocussedResultId < Results.Select(x => x.ResultId).Max())
                FocussedResultId++;
            OnPropertyChanged("DisplayResults");
        }

        private void StnButton()
        {
            if (Results.Count > 0)
            {
                IsEditingBibNumber = true;
            }
        }

        private void DispatcherTimerTick(object sender, object e)
        {
            ElapsedTime = stopWatch.Elapsed.ToString(@"hh\:mm\:ss\.f", CultureInfo.CurrentCulture);
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
