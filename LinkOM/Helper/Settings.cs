using System;
using Refractored.Xam.Settings.Abstractions;
using Refractored.Xam.Settings;

namespace LinkOM
{
	public static class Settings
	{
		private const string InstanceURLKey = "username_key";
		private static readonly string InstanceURL_Default = string.Empty;

		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		public static string InstanceURL
		{
			get { return AppSettings.GetValueOrDefault(InstanceURLKey, InstanceURL_Default); }
			set { AppSettings.AddOrUpdateValue(InstanceURLKey, value); }
		}
	}
}

