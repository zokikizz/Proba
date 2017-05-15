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
using Android.Locations;
using Android.Util;
using Android.Gms.Common.Apis;

namespace PaMolimte
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback//, ILocationListener for finding where is device
    {
        private GoogleMap gmap;
        //private MapFragment _mapf;

        #region for finding latlng when you have addres //works
        Geocoder geo;
        async public void Get()
        {
            var add = "Bulevar Nemanjica";
            var approximateLocation = await geo.GetFromLocationNameAsync(add, 1);

            //Toast.MakeText(this, approximateLocation[0].Latitude + "," + approximateLocation[0].Longitude,ToastLength.Long); // nece da stampa ne znam sto
            LatLng latLng = new LatLng(approximateLocation[0].Latitude, approximateLocation[0].Longitude);

            MarkerOptions options = new MarkerOptions().SetPosition(latLng).SetTitle("Nasao sam te").SetSnippet("I");

            gmap.AddMarker(options);
        }

        #endregion

        //my location, dont work
        Location _currentLocation;
        LocationManager _locationManager;

        string _locationProvider;


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

            //InitializeLocationManager(); for finding location

            geo = new Geocoder(this);


            Get();//da nadje ulicu koju trzis tj po imenu ulice da nadje koordinatu

            

        }


        //nadje tacno ulicu koju trazim i stavi marker na tu ulicu, oke je ovo
       

        public void OnMapReady(GoogleMap googleMap)
        {
            gmap = googleMap;


            //set marker
            LatLng latLng = new LatLng(40.776408, -73.970755);
            LatLng latLng2 = new LatLng(40.9231, -73.32145);
            LatLng l2 = new LatLng(41, -73);

            //postasvljanje kamere
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latLng, 10); // apdejtovace se kamera na mesto gde je latLng i uveca ce se puta 10
            gmap.MoveCamera(camera);

            
            MarkerOptions options = new MarkerOptions().SetPosition(latLng).SetTitle("New York").SetSnippet("I"); // snippet podnaslov, duzi opis, mogu da se steve i drugi properitiji pr,
            // set icon = > SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure) menja marker u drugu boju
           
            //ako hocemo da nam se marker pomera Draggable(true)
            //options.Draggable(true);

            gmap.AddMarker(options); //dodavanje markera latLng

            gmap.AddMarker(new MarkerOptions().SetPosition(l2).SetTitle("Marker 2")); // dodavanje makera 2

            gmap.MarkerClick += Marker_Click;

            gmap.MarkerDragEnd += Map_MarkerDragend;

            // for finding where is device dont work
            //gmap.MyLocationEnabled = true;
            //Location l = gmap.MyLocation;
            //Console.WriteLine(l.ToString());

            //44 
            //gmap.SetInfoWindowAdapter(this); ne znam sto nece :D
            //gmap.SetOnInfoWindowClickListener(this);


            gmap.AddMarker(new MarkerOptions().SetPosition(latLng2).SetTitle("New York").SetSnippet("II")); //

            PolylineOptions line = new PolylineOptions().Add(new LatLng(40.776408, -73.970755)).Add(new LatLng(40.9231, -73.32145));



            Polyline poly = gmap.AddPolyline(line);

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


        //to get my location, do not work
        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }

  

      
    }
}