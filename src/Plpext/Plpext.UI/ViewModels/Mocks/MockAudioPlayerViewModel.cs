using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plpext.Core.AudioPlayer;
using Plpext.Core.Models;

namespace Plpext.UI.ViewModels.Mocks;

    public static class MockAudioPlayerViewModel
    {
        private static readonly AudioPlayer _audioPlayer = new AudioPlayer();
        private static readonly AudioFile _audioFile = new AudioFile()
        { Name = "Dummy", Duration = TimeSpan.FromSeconds(14), Format = AudioFormat.Mono16, Frequency = 44100 };

        static MockAudioPlayerViewModel()
        {
            Instance = new MockAudioPlayerViewModelInstance();
        }

        public static MockAudioPlayerViewModelInstance Instance { get; }

        public class MockAudioPlayerViewModelInstance : INotifyPropertyChanged
        {
            public MockAudioPlayerViewModelInstance()
            {
                PlayCommand = new RelayCommand(async () => await Play());
            }

            private bool _isPlaying;
            public bool IsPlaying
            {
                get => _isPlaying;
                set => SetProperty(ref _isPlaying, value);
            }

            private string _totalDuration;
            public string TotalDuration
            {
                get => _totalDuration;
                set => SetProperty(ref _totalDuration, value);
            }

            private string _currentDuration;
            public string CurrentDuration
            {
                get => _currentDuration;
                set => SetProperty(ref _currentDuration, value);
            }

            private string _name;
            public string Name
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }

            public ICommand PlayCommand { get; }

            private async Task Play()
            {
                IsPlaying = !IsPlaying;
                // Implement play logic here
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public async void Execute(object parameter) => await _execute();

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }