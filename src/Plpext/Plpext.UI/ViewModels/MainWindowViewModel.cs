using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plpext.UI.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Plpext.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IPlatformStorageService _platformStorageService = null!;
    private readonly IFileLoaderService _fileLoaderService = null!;
    private readonly IConvertService _convertService = null!;

    public MainWindowViewModel()
    {
    }

    public MainWindowViewModel(IPlatformStorageService platformStorageService, IFileLoaderService fileLoaderService,
        IConvertService convertService)
    {
        _fileLoaderService = fileLoaderService;
        _platformStorageService = platformStorageService;
        _convertService = convertService;
    }

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(LoadFileCommand))]
    private string _originPath = null!;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConvertAllFilesCommand),nameof(ConvertSelectedFilesCommand))]
    private string _targetPath = null!;

    [ObservableProperty] private int _totalFilesToExtract;

    [ObservableProperty] private int _filesReady;

    [ObservableProperty] private string _progressBarText = null!;

    [ObservableProperty] private double _progressBarValue;

    [ObservableProperty] private bool _isProgressBarIndeterminate;

    [ObservableProperty] private string _progressBarDetails = null!;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConvertAllFilesCommand), nameof(ConvertSelectedFilesCommand), nameof(LoadFileCommand))]
    private bool _showProgressBar;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ConvertAllFilesCommand),nameof(ConvertSelectedFilesCommand))]
    private ObservableCollection<AudioPlayerViewModel> _audioFiles = new();


    [RelayCommand(CanExecute = nameof(CanLoadFile))]
    private async Task LoadFile()
    {
        TotalFilesToExtract = await _fileLoaderService.GetFileCountAsync(OriginPath);
        //AudioFiles.Clear();
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ProgressBarText = "Loading files...";
            ProgressBarValue = 0 / (double)TotalFilesToExtract * 100;
            ShowProgressBar = true;
            IsProgressBarIndeterminate = false;
            FilesReady = 0;
        });

        await foreach (var item in _fileLoaderService.LoadFilesAsync())
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                AudioFiles.Add(item);
                FilesReady += 1;
                ProgressBarValue = FilesReady / (double)TotalFilesToExtract * 100;
                ProgressBarDetails = $"{FilesReady} of {TotalFilesToExtract} ({ProgressBarValue:00.0}%)";
            });
        }

        await Dispatcher.UIThread.InvokeAsync(() => ShowProgressBar = false);
    }

    private bool CanLoadFile() => !string.IsNullOrEmpty(OriginPath) && !ShowProgressBar;


    [RelayCommand]
    private async Task SelectOriginPath()
    {
        OriginPath = await _platformStorageService.GetOriginFilePath();
    }

    [RelayCommand]
    private async Task SelectTargetPath()
    {
        TargetPath = await _platformStorageService.GetTargetFolderPath();
    }

    [RelayCommand(CanExecute = nameof(CanConvertAllFiles))]
    private async Task ConvertAllFiles()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ProgressBarText = $"Converting {TotalFilesToExtract} files...";
            IsProgressBarIndeterminate = true;
            ShowProgressBar = true;
            ProgressBarDetails = string.Empty;
        });
        await _convertService.ConvertFilesAsync(AudioFiles.Select(x => x.AudioFile), TargetPath);
        await Dispatcher.UIThread.InvokeAsync(() => ShowProgressBar = false);
    }

    private bool CanConvertAllFiles() => !string.IsNullOrEmpty(TargetPath) && AudioFiles.Any() && !ShowProgressBar;
    private bool CanConvertSelectFiles() => CanConvertAllFiles();


    [RelayCommand(CanExecute = nameof(CanConvertSelectFiles))]
    private async Task ConvertSelectedFiles()
    {
        var filesToConvert = AudioFiles.Where(a => a.IsSelected).Select(x => x.AudioFile).ToArray();
        if (filesToConvert.Length == 0) return;
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ProgressBarText = $"Converting {filesToConvert.Length} files...";
            IsProgressBarIndeterminate = true;
            ShowProgressBar = true;
            ProgressBarDetails = string.Empty;
        });
        await _convertService.ConvertFilesAsync(filesToConvert, TargetPath);
        await Dispatcher.UIThread.InvokeAsync(() => ShowProgressBar = false);
    }
}