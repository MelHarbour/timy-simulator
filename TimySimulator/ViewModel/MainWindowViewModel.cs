using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TimySimulator.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private RelayCommand<string> numberButtonCommand;
        private string bibNumber;

        public string BibNumber
        {
            get { return bibNumber; }
            set { SetField(ref bibNumber, value); }
        }

        public RelayCommand<string> NumberButtonCommand
        {
            get
            {
                if (numberButtonCommand == null)
                    numberButtonCommand = new RelayCommand<string>(param => NumberButton(param), param => true);
                return numberButtonCommand;
            }
        }

        private void NumberButton(string number)
        {
            BibNumber += number;
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
