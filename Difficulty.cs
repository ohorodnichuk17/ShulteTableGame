using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork
{
    public class Difficulty : INotifyPropertyChanged
    {
        double diff = 1;
        public double Diff {
            get => diff;
            set
            {
                diff = value;
                OnPropertyChanged();
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public override string ToString()
        {
            return diff.ToString(); 
        }
    }
}
