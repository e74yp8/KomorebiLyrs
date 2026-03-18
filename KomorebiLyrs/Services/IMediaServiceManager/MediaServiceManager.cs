using System;
using System.Collections.Generic;
using System.Linq;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public class MediaServiceManager: IMediaServiceManager
{
    private readonly SettingService _settingService;
    private readonly IEnumerable<IMediaService> _services;
    private IMediaService? _currentStrategy;

    public AppSettings.MediaProviderType CurrentProvider => _currentStrategy?.ProviderType ?? AppSettings.MediaProviderType.Dummy;
    public event EventHandler<MediaInfoEventArgs>? MediaChanged;

    // Inject available media services so we can select the appropriate one based on settings
    public MediaServiceManager(SettingService settingService,IEnumerable<IMediaService> services)
    {
        _settingService = settingService;
        _services = services;
        _settingService.SettingsChanged += OnSettingsChanged;
        
        // Initialize the first strategy based on current settings
        UpdateStrategy(_settingService.GetSettings());
    }

    private void OnSettingsChanged(object? sender, AppSettings newSettings)
    {
        UpdateStrategy(newSettings);
    }

    private void UpdateStrategy(AppSettings settings)
    {
        // Resolve the strategy that matches the current settings
        var newStrategy = _services.FirstOrDefault(s => s.ProviderType == settings.MediaProvider)
                          ?? _services.FirstOrDefault(s => s.ProviderType == AppSettings.MediaProviderType.Dummy);

        // If the strategy instance hasn't changed, avoid re-subscribing and re-starting
        if (ReferenceEquals(newStrategy, _currentStrategy))
        {
            return;
        }

        // Unsubscribe from the old strategy to prevent memory leaks
        if (_currentStrategy != null)
        {
            _currentStrategy.MediaChanged -= OnStrategyMediaChanged;
            // TODO: implement _currentStrategy.stop() for all IMediaServices 
            // Optionally stop the old strategy if IMediaService has a Stop() method
        }

        _currentStrategy = newStrategy;

        // Subscribe to and start the new strategy
        if (_currentStrategy != null)
        {
            _currentStrategy.MediaChanged += OnStrategyMediaChanged;
            _currentStrategy.Start();
        }
    }

    // Bubble up the event from the underlying strategy to whoever is listening to the Manager
    private void OnStrategyMediaChanged(object? sender, MediaInfoEventArgs e)
    {
        MediaChanged?.Invoke(this, e);
    }

    public void Start()
    {
        _currentStrategy?.Start();
    }
}