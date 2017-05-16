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
using Android.Gms.Maps;
using Android.Locations;
using Android.Gms.Maps.Model;
using System.Threading;
using static Android.Manifest;
using Android.Support.V4.App;

namespace PaMolimte
{
    [Activity(Label = "FindDeviceActivity")]
    public class FindDeviceActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        #region map_set_up
        //map setup
        GoogleMap gmap;


        public void OnMapReady(GoogleMap googleMap)
        {
            gmap = googleMap;
            gmap.MyLocationEnabled = true;
        }

        public void SetUpMap()
        {
            if (gmap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapa).GetMapAsync(this); //GetMapAsync zove OnMapReady
            }
        }
        #endregion

        #region ILocationLisentr_Implementation
        //for finding location


        LocationManager locMan;
        string provider;
        public void OnLocationChanged(Location location)
        {
            LatLng pos = new LatLng(location.Latitude, location.Longitude);
            //CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(pos, 15); // apdejtovace se kamera na mesto gde je latLng i uveca ce se puta 10
            //gmap.MoveCamera(camera);


            //da setuje marker, ne dodajem ga jer sam enablovao da se vidi tvoja lokacija na mapi prego GoogleMap-e kada je dobijem u OnMapReady
            //MarkerOptions options = new MarkerOptions().SetPosition(pos).SetTitle("Nasao sam te.");

            //gmap.AddMarker(options);
          
        }




        public void OnProviderDisabled(string provider)
        {
            
        }

        public void OnProviderEnabled(string provider)
        {
           
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }

        #endregion


        //OnCreateActivity

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MapLayout);

            SetUpMap();

            locMan = (LocationManager)GetSystemService(Context.LocationService);

            provider = locMan.GetBestProvider(new Criteria(), false);

        }
  


        #region mobile_options
        //for mobile when is pause or resume
        protected override void OnResume()
        { // ovde je ime locationProvider null ne znam sto ne moze da ga nadje
            base.OnResume();

            locMan.RequestLocationUpdates(provider, 400, 1, this);
            if (locMan != null)
            {
                var locationCriteria = new Criteria();
                locationCriteria.Accuracy = Accuracy.NoRequirement;
                locationCriteria.PowerRequirement = Power.NoRequirement;
                string locationProvider = locMan.GetBestProvider(locationCriteria, true);
                locMan.RequestLocationUpdates(locationProvider, 2000, 1, this);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            locMan.RemoveUpdates(this);
        }
        #endregion
    }
}