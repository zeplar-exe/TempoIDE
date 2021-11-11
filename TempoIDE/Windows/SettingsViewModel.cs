using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;
using TempoIDE.Properties;

namespace TempoIDE.Windows
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<SkinDefinition> skinDefinitions;
        
        public IEnumerable<SkinDefinition> SkinDefinitions
        {
            get => skinDefinitions;
            private set
            {
                skinDefinitions = value;
                
                OnPropertyChanged();
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel()
        {
            UpdateSkinDefinitions();
            
            SettingsHelper.SettingsUpdated += OnSettingsUpdated;
        }

        private void OnSettingsUpdated(object _, EventArgs __)
        {
            UpdateSkinDefinitions();
        }

        private void UpdateSkinDefinitions()
        {
            SkinDefinitions = SettingsHelper.Settings.AppSettings.SkinSettings.SkinDefinitions;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}