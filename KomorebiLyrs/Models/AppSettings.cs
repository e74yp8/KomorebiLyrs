namespace KomorebiLyrs.Models;

public class AppSettings
{
    public enum MediaProviderType
    {
        Windows,
        Tuna,
        Dummy
    }

    public MediaProviderType MediaProvider { get; set; } = MediaProviderType.Windows;
    public double WindowOpacity { get; set; } = 1.0;
    public bool IsClickThroughEnabled { get; set; } = true;
}
