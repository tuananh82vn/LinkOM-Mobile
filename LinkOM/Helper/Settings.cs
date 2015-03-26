using System;
using Refractored.Xam.Settings.Abstractions;
using Refractored.Xam.Settings;

namespace LinkOM
{
	public static class Settings
	{
		private const string SomeIntKey = "int_key";
		private static readonly int SomeIntDefault = 0;


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

		public static int SmallestWidth
		{
			get { return AppSettings.GetValueOrDefault(SomeIntKey, SomeIntDefault); }
			set { AppSettings.AddOrUpdateValue(SomeIntKey, value); }
		}
	}
}

