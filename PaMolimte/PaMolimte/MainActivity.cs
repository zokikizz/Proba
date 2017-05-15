using Android.App;
using Android.Widget;
using Android.OS;

namespace PaMolimte
{
    [Activity(Label = "Welcome to Core", MainLauncher = true, Icon = "@drawable/icon", Theme ="@style/CoreTheme")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
             SetContentView (Resource.Layout.Main);

            Button b = FindViewById<Button>(Resource.Id.btnOpenMap);
            b.Click += delegate { OpenMap(); };
        }

        public void OpenMap()
        {
            StartActivity(typeof(MapActivityV2));
        }
    }
}

