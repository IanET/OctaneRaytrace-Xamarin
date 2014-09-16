using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Java.Lang;
using OctaneRaytrace_CSharp;

namespace OctaneRaytraceXamarin
{
	[Activity (Label = "OctaneRaytrace-Xamarin", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		// Pulled from the Octane sources
		static int MIN_ITERATIONS = 32;
		static long REFERENCE_SCORE = 739989;

		bool measuring;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += HandleClick;
		}

		void HandleClick (object sender, EventArgs e)
		{
			TextView tv = FindViewById<TextView> (Resource.Id.textView1);

			if (measuring)
			{
				return;
			}

			measuring = true;
			tv.Append ("Raytrace...\r\n");

			// Warmup
			Measure(null);
			// Benchmark
			Data data = new Data();
			while (data.runs < MIN_ITERATIONS)
			{
				Measure(data);
				tv.Append("Runs: " + data.runs + ", Elapsed: " + data.elapsed + "\r\n");
			}
			long usec = (data.elapsed * 1000) / data.runs;
			long score = (REFERENCE_SCORE / usec) * 100;
			tv.Append("Score: " + score + "\r\n");
			tv.Append("Done.\r\n\r\n");
			measuring = false;
		}
			
		public class Data
		{
			public long runs;
			public long elapsed;
		}

		public void Measure(Data data) {
			// Run for a second
			long start = JavaSystem.CurrentTimeMillis();
			long elapsed = 0;
			int i = 0;

			while (elapsed < 1000) {
				RayTracer.renderScene(null);
				i++;
				elapsed = JavaSystem.CurrentTimeMillis() - start;
			}

			if (data != null) {
				data.runs += i;
				data.elapsed += elapsed;
			}
		}

	}
}


