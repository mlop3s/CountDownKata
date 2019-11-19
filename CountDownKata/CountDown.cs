using GalaSoft.MvvmLight;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CountDownKata
{
    public class CountDown : ViewModelBase
    {
        private TimeSpan _time;
        private CancellationTokenSource _cancelationSource;

        public TimeSpan Time
        {
            get
            {
                return _time;
            }

            set
            {
                Set(ref _time, value);
            }
        }

        private TimeSpan _current;

        public TimeSpan Current
        {
            get
            {
                return _current;
            }

            set
            {
                Set(ref _current, value);
                RaisePropertyChanged();
            }
        }

        public string CounterString
        {
            get
            {
                return Current.ToString(@"hh\:ss");
            }
        }


        public void StartTime()
        {
           
            Current = new TimeSpan(0,15,0);
            if (_cancelationSource == null)
            {
                _cancelationSource = new CancellationTokenSource();
                RunPeriodic(100, _cancelationSource.Token, () => Current = Current.Add(TimeSpan.FromMilliseconds(100) ));
            }
        }

        public static async Task RunPeriodic(int milliseconds, CancellationToken token, Action action)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(milliseconds, token);
                action();
            }
        }
    }
}
