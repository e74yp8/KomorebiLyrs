using System;

namespace KomorebiLyrs.Services;

public interface IWindowTraitService
{
    bool IsClickThroughEnabled { get; }
    event EventHandler<bool>? ClickThroughChanged;
    void SetClickThrough(bool enable);
}