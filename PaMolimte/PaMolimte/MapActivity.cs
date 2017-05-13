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

namespace PaMolimte
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap gmap;
        private MapFragment _mapf;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MapLayout);

            SetUpMap();

            //_mapf = FragmentManager.FindFragmentById(Resource.Id.mapa) as MapFragment;

            //if( _mapf == null)
            //{
            //    GoogleMapOptions mapOptions = new GoogleMapOptions().InvokeMapType(GoogleMap.MapTypeNormal).InvokeZoomControlsEnabled(true).InvokeCompassEnabled(true);

            //    FragmentTransaction frt = FragmentManager.BeginTransaction();
            //    _mapf = MapFragment.NewInstance(mapOptions);
            //    frt.Add(Resource.Id.mapa, _mapf, "mapa");

            //    frt.Commit();
            //}

            //_mapf.GetMapAsync(this);

        }

        public void OnMapReady(GoogleMap googleMap)
        {
            gmap = googleMap;
        }

        public void SetUpMap()
        {
            if( gmap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapa).GetMapAsync(this); //GetMapAsync zove OnMapReady
            }
        }
      
    }
}