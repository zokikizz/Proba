using Android.App;
using Android.Widget;
using Android.OS;

namespace PaMolimte
{
    [Activity(Label = "Welcome to Core", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/CoreTheme")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            //    SetContentView (Resource.Layout.LoginLayout);
            StartActivity(typeof(FindDeviceActivity));

         //   FindViewById<Button>(Resource.Id.Loginbtn).Click += delegate { Login(); };
        }

        public void Login()
        {

            if ((FindViewById<EditText>(Resource.Id.tbEmail).Text == "admin") && ( FindViewById<EditText>(Resource.Id.tbPassword).Text == "admin"))
            {
                StartActivity(typeof(MapActivityV2));
            }
        }
    }
}

