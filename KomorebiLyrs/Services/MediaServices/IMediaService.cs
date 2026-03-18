using System;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public interface IMediaService
{
    AppSettings.MediaProviderType ProviderType { get; }
    event EventHandler<MediaInfoEventArgs>? MediaChanged;
    void Start();
}

public class MediaInfoEventArgs : EventArgs
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
}