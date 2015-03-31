
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Provider;
using Java.IO;

using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;


namespace LinkOM
{
	[Activity (Label = "AddTaskActivity")]			
	public class AddTaskActivity : Activity
	{
		const int Start_DATE_DIALOG_ID = 0;
		const int End_DATE_DIALOG_ID = 1;

		private TextView tv_StartDate;
		private Button bt_StartDate;
		private TextView tv_EndDate;
		private Button bt_EndDate;

		private DateTime StartDate;
		private DateTime EndDate;


		private File _dir;
		private File _file;
		private ImageView _imageView;	

		public string TokenNumber;
		public ProjectSpinnerAdapter projectList; 

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.AddTask);

			ImageButton bt_Back = FindViewById<ImageButton>(Resource.Id.bt_Back);
			bt_Back.Click += btBackClick;

			Spinner st_Status = FindViewById<Spinner> (Resource.Id.sp_Status);
			var StatusAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskStatus, Android.Resource.Layout.SimpleSpinnerItem);
			StatusAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			st_Status.Adapter = StatusAdapter;

			Spinner st_Priority = FindViewById<Spinner> (Resource.Id.sp_Priority);
			var PriorityAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskPriority, Android.Resource.Layout.SimpleSpinnerItem);
			PriorityAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			st_Priority.Adapter = PriorityAdapter;

			// get the current date
			StartDate = DateTime.Today;

			// get the current date
			EndDate = DateTime.Today;

			tv_StartDate = FindViewById<TextView> (Resource.Id.tv_StartDate);
			tv_StartDate.Text = StartDate.ToString ("d");
			tv_EndDate = FindViewById<TextView> (Resource.Id.tv_EndDate);
			tv_EndDate.Text = EndDate.ToString ("d");

			bt_StartDate = FindViewById<Button> (Resource.Id.bt_StartDate);
			bt_EndDate = FindViewById<Button> (Resource.Id.bt_EndDate);

			// add a click event handler to the button
			bt_StartDate.Click += delegate { ShowDialog (Start_DATE_DIALOG_ID); };
			// add a click event handler to the button
			bt_EndDate.Click += delegate { ShowDialog (End_DATE_DIALOG_ID); };

			if (IsThereAnAppToTakePictures())
			{
				CreateDirectoryForPictures();

				Button bt_camera = FindViewById<Button>(Resource.Id.bt_camera);
				_imageView = FindViewById<ImageView>(Resource.Id.iv_Photo);

				bt_camera.Click += TakeAPicture;
			}

			//Handle Project Spinner

			TokenNumber = Settings.Token;

			int ProjectId = Intent.GetIntExtra ("ProjectId",0);

			string url = Settings.InstanceURL;

			url=url+"/api/ProjectList";


			List<objSort> objSort = new List<objSort>{
				new objSort{ColumnName = "P.Name", Direction = "1"},
				new objSort{ColumnName = "C.Name", Direction = "2"}
			};

			var objProject = new
			{
				Name = string.Empty,
				ClientName = string.Empty,
				DepartmentId = string.Empty,
				ProjectStatusId = string.Empty,
			};

			var objsearch = (new
			{
				objApiSearch = new
				{
					TokenNumber = TokenNumber,
					PageSize = 20,
					PageNumber = 1,
					Sort = objSort,
					Item = objProject
				}
			});


			string results= ConnectWebAPI.Request(url,objsearch);

			ProjectListJson ProjectList = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectListJson> (results);

			projectList = new ProjectSpinnerAdapter (this,ProjectList.Items);

			Spinner sp_Project = FindViewById<Spinner> (Resource.Id.sp_Project);
			//var ProjectAdapter = ArrayAdapter.CreateFromResource (this, Resource.Array.TaskPriority, Android.Resource.Layout.SimpleSpinnerItem);
			sp_Project.Adapter = projectList;

			if(ProjectId!=0)
			sp_Project.SetSelection(projectList.getPositionById(ProjectId)); 
		}

		public void btBackClick(object sender, EventArgs e)
		{
			OnBackPressed ();
		}

		// the event received when the user "sets" the date in the dialog
		void OnStartDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			tv_StartDate.Text = e.Date.ToString ("d");
		}

		// the event received when the user "sets" the date in the dialog
		void OnEndDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			tv_EndDate.Text = e.Date.ToString ("d");
		}

		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) {
			case Start_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnStartDateSet, StartDate.Year, StartDate.Month - 1, StartDate.Day); 
			case End_DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnEndDateSet, EndDate.Year, EndDate.Month - 1, EndDate.Day); 
			}
			return null;
		}

		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}
		private void CreateDirectoryForPictures()
		{
			_dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "Link-OM");
			if (!_dir.Exists())
			{
				_dir.Mkdirs();
			}
		}

		private void TakeAPicture(object sender, EventArgs eventArgs)
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);

			_file = new File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

			intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(_file));

			StartActivityForResult(intent, 0);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			// make it available in the gallery
			Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
			Uri contentUri = Uri.FromFile(_file);
			mediaScanIntent.SetData(contentUri);
			SendBroadcast(mediaScanIntent);

			// display in ImageView. We will resize the bitmap to fit the display
			// Loading the full sized image will consume to much memory 
			// and cause the application to crash.
			int height = _imageView.Height;
			int width = Resources.DisplayMetrics.WidthPixels;
			using (Bitmap bitmap = _file.Path.LoadAndResizeBitmap(width, height))
			{
				_imageView.RecycleBitmap ();
				_imageView.SetImageBitmap(bitmap);
			}
		}

	}
}

