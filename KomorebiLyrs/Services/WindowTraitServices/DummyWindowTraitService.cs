using System;

namespace KomorebiLyrs.Services;

public class DummyWindowTraitService : IWindowTraitService
{
    public bool IsClickThroughEnabled { get; private set; }
    public event EventHandler<bool>? ClickThroughChanged;

    public void SetClickThrough(bool enable)
    {
        if (IsClickThroughEnabled == enable) return;
        IsClickThroughEnabled = enable;
        ClickThroughChanged?.Invoke(this, enable);
    }
}