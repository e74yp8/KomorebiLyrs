using System;
using System.Text.Json.Nodes;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public class SettingService
{
        private AppSettings _settings;

        public event EventHandler<AppSettings>? SettingsChanged;
    
        public SettingService()
        {
            // Load settings from file or initialize with defaults
            _settings = LoadSettings() ?? new AppSettings();
        }
    
        public AppSettings GetSettings()
        {
            return _settings;
        }
    
        public void UpdateSettings(AppSettings newSettings)
        {
            _settings = newSettings;
            SaveSettings(_settings);
            SettingsChanged?.Invoke(this, newSettings);
        }
    
        private AppSettings? LoadSettings()
        {
            // Implement loading logic (e.g., from JSON file)
            // Return null if no settings file exists
            return null;
        }
    
        private void SaveSettings(AppSettings settings)
        {
            // Implement saving logic (e.g., to JSON file)
        }
}