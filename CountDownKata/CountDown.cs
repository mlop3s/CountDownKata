using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CountDownKata
{
    public sealed class CountDown : ViewModelBase, IDisposable
    {
        
        private CancellationTokenSource _cancelationSource;

        public CountDown()
        {
            StartCommand = new RelayCommand(Start, CanStart);
            StopCommand = new RelayCommand(Stop, CanStop);
        }

        private bool CanStop()
        {
            return _cancelationSource != null;
        }

        private void Stop()
        {
            Elapsed = TimeSpan.FromSeconds(0);
            _cancelationSource?.Cancel();
            _cancelationSource = null;
            StartCommand.RaiseCanExecuteChanged();
        }

        private void Start()
        {
            StartCounter();
        }

        private bool CanStart()
        {
            return _cancelationSource == null && (Minutes + Seconds) > 0;

        }

        private Brush _background = Brushes.White;

        public Brush Background
        {
            get
            {
                return _background;
            }

            set
            {
                Set(ref _background, value);
            }
        }

        public RelayCommand StartCommand { get; private set; }
        public RelayCommand StopCommand { get; }

        public TimeSpan StartTime
        {
            get
            {
                return TimeSpan.FromMinutes(Minutes).Add(TimeSpan.FromSeconds(Seconds + 1)).Subtract(TimeSpan.FromMilliseconds(1));;
            }

         }

        private TimeSpan _current;
        private bool _disposed;

        public TimeSpan Elapsed { get; private set; }

        public TimeSpan Current
        {
            get
            {
                return _current;
            }

            set
            {
                Set(ref _current, value);
                RaisePropertyChanged(nameof(CounterString));
            }
        }

        public string CounterString
        {
            get
            {
                return Current.ToString(@"mm\:ss");
            }
        }

        private int _seconds;

        public int Seconds
        {
            get
            {
                return _seconds;
            }

            set
            {
                Set(ref _seconds, value);
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        private int _minutes;

        public int Minutes
        {
            get
            {
                return _minutes;
            }

            set
            {
                Set(ref _minutes, value);
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        private string _elapsedString;

        public string ElapsedString
        {
            get
            {
                return _elapsedString;
            }

            set
            {
                Set(ref _elapsedString, value);
            }
        }


        public void StartCounter()
        {           
            Background = Brushes.White;
            Current = StartTime;
            if (_cancelationSource == null)
            {
                _cancelationSource = new CancellationTokenSource();
                RunPeriodic(100, _cancelationSource.Token, UpdateCounter).ConfigureAwait(true);
                StopCommand.RaiseCanExecuteChanged();
                StartCommand.RaiseCanExecuteChanged();

            }
        }

        private void UpdateCounter()
        {
            //if (Elapsed.TotalMilliseconds >= StartTime.TotalMilliseconds)
            //{
            //    return;
            //}

            Elapsed = Elapsed.Add(TimeSpan.FromMilliseconds(+100));
            ElapsedString = Elapsed.ToString(@"mm\:ss");
            Current = StartTime.Subtract(Elapsed);
            if (Current.TotalMilliseconds <= StartTime.TotalMilliseconds * 0.4)
            {
                Background = Brushes.Red;
            }

        }

        public async Task RunPeriodic(int milliseconds, CancellationToken token, Action action)
        {
            try {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(milliseconds, token);
                    action();
                }
            }
            catch (TimeoutException)
            {

            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                _cancelationSource?.Dispose();
            }

            _disposed = true;
        }
    }
}
