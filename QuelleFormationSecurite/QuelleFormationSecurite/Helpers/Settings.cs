// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using QuelleFormationSecurite.BusinessLayer;
using System.Collections.Generic;

namespace QuelleFormationSecurite.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

        private const string _lsJsonResults = @"lsJsonResults";
        private const string _lsJsonResultsDefault = "";

        #endregion


        public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}

        public static string lsJsonResults
        {
            get
            {
                return AppSettings.GetValueOrDefault(_lsJsonResults, _lsJsonResultsDefault);
            }
            set
            {
                //if value has changed then save it!
                AppSettings.AddOrUpdateValue(_lsJsonResults, value);
            }
        }

    }
}