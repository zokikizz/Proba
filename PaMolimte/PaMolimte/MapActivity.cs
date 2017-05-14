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
using Android.Gms.Maps.Model;

namespace PaMolimte
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap gmap;
        //private MapFragment _mapf;


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


            //set marker
            LatLng latLng = new LatLng(40.776408, -73.970755);
            LatLng l2 = new LatLng(41, -73);

            //postasvljanje kamere
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latLng, 10); // apdejtovace se kamera na mesto gde je latLng i uveca ce se puta 10
            gmap.MoveCamera(camera);

            
            MarkerOptions options = new MarkerOptions().SetPosition(latLng).SetTitle("New York").SetSnippet("AKA: Big App;e"); // snippet podnaslov, duzi opis, mogu da se steve i drugi properitiji pr,
            // set icon = > SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure) menja marker u drugu boju

            //ako hocemo da nam se marker pomera Draggable(true)
            options.Draggable(true);

            gmap.AddMarker(options); //dodavanje markera latLng

            gmap.AddMarker(new MarkerOptions().SetPosition(l2).SetTitle("Marker 2")); // dodavanje makera 2

            gmap.MarkerClick += Marker_Click;

            gmap.MarkerDragEnd += Map_MarkerDragend;

        }

        private void Marker_Click(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            LatLng pos = e.Marker.Position;
            gmap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(pos, 10));
        }

        private void Map_MarkerDragend(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            LatLng l = e.Marker.Position;
            Console.WriteLine(l.ToString());
        }

        //to change map view
        /*
         * gmap.MapType = GoogleMap.MapTypeNormal; ect
         * 
         * */

        public void SetUpMap()
        {
            if( gmap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapa).GetMapAsync(this); //GetMapAsync zove OnMapReady
            }
        }
      
    }
}