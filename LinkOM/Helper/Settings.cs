﻿using System;
using Refractored.Xam.Settings.Abstractions;
using Refractored.Xam.Settings;

namespace LinkOM
{
	public static class Settings
	{
		private const string SmallestWidthKey = "int_key";
		private static readonly int SmallestWidthDefault = 0;

		private const string UserIdKey = "int_key";
		private static readonly int UserIdDefault = 0;

		private const string InstanceURLKey = "InstanceURLKey";
		//private static readonly string InstanceURL_Default = "http://10.0.2.2:60943";
		private static readonly string InstanceURL_Default = "http://linkom-ins1.softwarestaging.com.au";

		private const string TokenKey = "TokenKey";
		private static readonly string TokenKey_Default = string.Empty;

		private const string UserNameKey = "UserNameKey";
		private static readonly string UserNameKey_Default = string.Empty;

		private const string PasswordKey = "PasswordKey";
		private static readonly string PasswordKey_Default = string.Empty;

		private const string RememberMeKey = "RememberMeKey";
		private static readonly bool RememberMeKey_Default = false;

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
			get { return AppSettings.GetValueOrDefault(SmallestWidthKey, SmallestWidthDefault); }
			set { AppSettings.AddOrUpdateValue(SmallestWidthKey, value); }
		}

		public static int UserId
		{
			get { return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault); }
			set { AppSettings.AddOrUpdateValue(UserIdKey, value); }
		}

		public static string Token
		{
			get { return AppSettings.GetValueOrDefault(TokenKey, TokenKey_Default); }
			set { AppSettings.AddOrUpdateValue(TokenKey, value); }
		}

		public static string Username
		{
			get { return AppSettings.GetValueOrDefault(UserNameKey, UserNameKey_Default); }
			set { AppSettings.AddOrUpdateValue(UserNameKey, value); }
		}

		public static string Password
		{
			get { return AppSettings.GetValueOrDefault(PasswordKey, PasswordKey_Default); }
			set { AppSettings.AddOrUpdateValue(PasswordKey, value); }
		}

		public static bool RememberMe
		{
			get { return AppSettings.GetValueOrDefault(RememberMeKey, RememberMeKey_Default); }
			set { AppSettings.AddOrUpdateValue(RememberMeKey, value); }
		}


	}
}

