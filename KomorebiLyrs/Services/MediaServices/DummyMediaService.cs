using System;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public class DummyMediaService : IMediaService
{
    public AppSettings.MediaProviderType ProviderType => AppSettings.MediaProviderType.Dummy;
    public event EventHandler<MediaInfoEventArgs>? MediaChanged;
    public void Start()
    {
    }
}