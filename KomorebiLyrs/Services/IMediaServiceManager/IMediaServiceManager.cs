using System;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public interface IMediaServiceManager
{
    AppSettings.MediaProviderType CurrentProvider { get; }
    event EventHandler<MediaInfoEventArgs>? MediaChanged;
    void Start();
}