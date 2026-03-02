using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using KomorebiLyrs.Services;

namespace KomorebiLyrs.Services;

public class TrayIconService : IDisposable
{
    private readonly IWindowTraitService _windowTraitService;
    private TrayIcon? _trayIcon;
    private NativeMenu? _trayMenu;
    private NativeMenuItem? _clickThroughItem;

    public TrayIconService(IWindowTraitService windowTraitService)
    {
        _windowTraitService = windowTraitService;
        InitializeTrayIcon();
    }

    private void InitializeTrayIcon()
    {
        _trayIcon = new TrayIcon
        {
            Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://KomorebiLyrs/Assets/avalonia-logo.ico"))),
            ToolTipText = "KomorebiLyrs"
        };

        _trayMenu = new NativeMenu();

        _clickThroughItem = new NativeMenuItem
        {
            Header = "Enable Click-Through",
            ToggleType = NativeMenuItemToggleType.CheckBox,
            IsChecked = _windowTraitService.IsClickThroughEnabled
        };
        _clickThroughItem.Click += OnClickThroughToggle;

        var exitItem = new NativeMenuItem
        {
            Header = "Exit"
        };
        exitItem.Click += OnExitClick;

        _trayMenu.Add(_clickThroughItem);
        _trayMenu.Add(exitItem);

        _trayIcon.Menu = _trayMenu;

        // Listen to external changes to state
        _windowTraitService.ClickThroughChanged += OnTraitServiceStateChanged;
    }

    private void OnTraitServiceStateChanged(object? sender, bool isEnabled)
    {
        if (_clickThroughItem != null)
        {
            _clickThroughItem.IsChecked = isEnabled;
        }
    }

    private void OnClickThroughToggle(object? sender, EventArgs e)
    {
        if (_clickThroughItem != null)
        {
            _windowTraitService.SetClickThrough(_clickThroughItem.IsChecked);
        }
    }

    private void OnExitClick(object? sender, EventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }

    public void Dispose()
    {
        _windowTraitService.ClickThroughChanged -= OnTraitServiceStateChanged;
        _trayIcon?.Dispose();
    }
}
