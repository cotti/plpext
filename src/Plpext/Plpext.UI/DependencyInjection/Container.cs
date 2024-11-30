using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plpext.Core.FileStorage;
using Plpext.Core.Interfaces;
using Plpext.Core.MP3Parser;
using Plpext.Core.PackExtractor;
using Plpext.UI.ViewModels;
using Serilog;
using System;
using Plpext.Core.AudioConverter;
using Plpext.Core.AudioPlayer;
using Plpext.UI.Services;
using Plpext.UI.Services.Converter;
using Plpext.UI.Services.FileLoader;
using Plpext.UI.Services.PlatformStorage;
using Plpext.UI.Views;

namespace Plpext.UI.DependencyInjection;

public static class Container
{
    private static IServiceProvider? _container;
    public static IServiceProvider Services
    {
        get => _container ?? Register();
    }

    private static IServiceProvider Register()
    {
        var hostBuilder = Host
            .CreateDefaultBuilder()
            .UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.WriteTo.Debug();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<AudioContext>();
                services.AddScoped<IAudioPlayer, AudioPlayer>();
                services.AddSingleton<IAudioConverter, MP3AudioConverter>();
                services.AddSingleton<IMP3Parser, MP3Parser>();
                services.AddSingleton<IPackExtractor, CorePackExtractor>();
                services.AddSingleton<IFileStorage, DiskFileStorage>();
                services.AddSingleton<IPlatformStorageService, PlatformStorageService>();
                services.AddSingleton<IFileLoaderService, FileLoaderService>();
                services.AddSingleton<IConvertService, FileConvertService>();
            })
            .Build();
        hostBuilder.Start();
        _container = hostBuilder.Services;
        return _container;
    }
}
