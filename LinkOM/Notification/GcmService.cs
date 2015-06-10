//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Gcm.Client;
//using Android.Support.V4.App;
//using Java.Lang;
//using String = System.String;
//
//namespace LinkOM
//{
//	[Service]
//	public class GcmService : GcmServiceBase
//	{
//		private static readonly int ButtonClickNotificationId = 1000;
//
//		public GcmService() : base(GcmBroadcastReceiver.SENDER_IDS) { }
//
//		protected override void OnRegistered (Context context, string registrationId)
//		{
//			Console.WriteLine ("Device Id:" + registrationId);
//			var preferences = GetSharedPreferences("AppData", FileCreationMode.Private);
//			var deviceId = preferences.GetString("DeviceId","");
//			if (string.IsNullOrEmpty (deviceId)) {
//				var editor = preferences.Edit ();
//				editor.PutString ("DeviceId", registrationId);
//				editor.Commit ();
//			}
//		}
//
//		protected override void OnMessage (Context context, Intent intent)
//		{
//			if (intent != null && intent.Extras != null) {
//				string message = intent.Extras.GetString ("message");
//
//				if (!message.Equals ("")) {
//					PushObject data = new PushObject ();
//					string[] stringSeparators = new string[] {"@"};
//
//					string[] temp = message.Split (stringSeparators,StringSplitOptions.None);
//
//					data.activity = temp [0];
//					data.id = Int32.Parse(temp [1]);
//					data.desc = temp [2];
//
//					createNotification ("Link-OM", data);
//				}
//			}
//		}
//
//		protected override void OnUnRegistered (Context context, string registrationId)
//		{
//			//Receive notice that the app no longer wants notifications
//		}
//
//		protected override void OnError (Context context, string errorId)
//		{
//			//Some more serious error happened
//		}
//
//		private void createNotification(string title, PushObject data)
//		{
//			// These are the values that we want to pass to the next activity
//			Bundle valuesForActivity = new Bundle();
//			if(data.activity.Equals("TASK")){
//				valuesForActivity.PutInt("TaskId", data.id);
//
//				var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
//				var uiIntent = new Intent(this, typeof(TaskDetailActivity));
//				uiIntent.PutExtras(valuesForActivity); // Pass some values to SecondActivity.
//
//				var notification = new Notification(Resource.Drawable.Icon, title);
//				notification.Flags = NotificationFlags.AutoCancel;
//				notification.Defaults = NotificationDefaults.Sound;
//				notification.SetLatestEventInfo(this, title, data.desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));
//				notificationManager.Notify(1, notification);
//			}
//		}
//
//	}
//
//		public class PushObject {
//			public string activity { get; set; }
//			public int id { get; set; }
//			public string desc { get; set; }
//		}
//}
//
