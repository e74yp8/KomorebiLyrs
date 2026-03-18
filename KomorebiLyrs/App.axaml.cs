using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KomorebiLyrs.Services;
using KomorebiLyrs.ViewModels;
using KomorebiLyrs.Views;

namespace KomorebiLyrs;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
           
            IWindowTraitService windowTraitService;
            var settingService = new SettingService();

            var mediaServices = new List<IMediaService>();
            
#if WINDOWS
            mediaServices.Add(new WindowsMediaService());
            mediaServices.Add(new DummyMediaService());
            windowTraitService = new WindowTraitService();
#else
                    mediaServices.Add(new DummyMediaService());
                    windowTraitService = new DummyWindowTraitService();
#endif  

            var mediaServiceManager = new MediaServiceManager(settingService, mediaServices);
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(mediaServiceManager, windowTraitService)
            };

            desktop.Startup += (sender, args) =>
            {
#if WINDOWS
                if (windowTraitService is WindowTraitService winService && desktop.MainWindow != null)
                {
                    winService.SetTargetWindow(desktop.MainWindow);
                }
                // Initialize TrayIconService
                _trayIconService = new TrayIconService(windowTraitService);
#endif  
            };

            desktop.MainWindow.Opened += (sender, args) =>
            {
                windowTraitService.SetClickThrough(true);
            };
            
            desktop.Exit += (sender, args) =>
            {
                _trayIconService?.Dispose();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private TrayIconService? _trayIconService;


    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}