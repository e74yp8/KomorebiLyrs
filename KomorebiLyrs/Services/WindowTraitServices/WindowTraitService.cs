
using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;

namespace KomorebiLyrs.Services;

public class WindowTraitService : IWindowTraitService
{
    private Window? _targetWindow;

    // --- Windows API ---
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TRANSPARENT = 0x00000020;
    private const int WS_EX_LAYERED = 0x00080000;

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

    public void SetTargetWindow(Window targetWindow)
    {
        _targetWindow = targetWindow;
    }

    public bool IsClickThroughEnabled { get; private set; }
    public event EventHandler<bool>? ClickThroughChanged;

    public void SetClickThrough(bool enable)
    {
        if (IsClickThroughEnabled == enable) return;

        IsClickThroughEnabled = enable;
        ClickThroughChanged?.Invoke(this, enable);

        if (_targetWindow == null) return;

        var handle = _targetWindow.TryGetPlatformHandle();
        if (handle == null) return;

        var hwnd = handle.Handle;
        var exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

        if (enable)
        {
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle | WS_EX_TRANSPARENT | WS_EX_LAYERED);
        }
        else
        {
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle & ~WS_EX_TRANSPARENT);
        }
    }
}