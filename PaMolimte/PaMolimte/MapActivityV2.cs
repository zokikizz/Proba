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
using Org.Json;
using System.Net.Sockets;

namespace PaMolimte
{
    [Activity(Label = "MapActivityV2")]
    public class MapActivityV2 : Activity, IOnMapReadyCallback
    {
        #region map_set_up
        GoogleMap gmap;

        public void OnMapReady(GoogleMap googleMap)
        {
            gmap = googleMap;
        }

        public void SetUpMap()
        {
            if (gmap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.mapa).GetMapAsync(this); //GetMapAsync zove OnMapReady
            }
        }
        #endregion

        #region to_find_out_lat_and_lng_by_address

        Geocoder geo;

        async public void GetMarkOnMapForNameOfStreet(string addr)
        {
            geo = new Geocoder(this);

            var add = addr;
            
            var approximateLocation = await geo.GetFromLocationNameAsync(add, 1);

            //Toast.MakeText(this, approximateLocation[0].Latitude + "," + approximateLocation[0].Longitude,ToastLength.Long); // nece da stampa ne znam sto
            LatLng latLng = new LatLng(approximateLocation[0].Latitude, approximateLocation[0].Longitude);
            if (approximateLocation[0].CountryName == "Србија")
            { //da zumira kameru gde mi je marker
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latLng, 10); // apdejtovace se kamera na mesto gde je latLng i uveca ce se puta 10
                gmap.MoveCamera(camera);
                //da setuje marker
                MarkerOptions options = new MarkerOptions().SetPosition(latLng).SetTitle("Nasao sam te.").SetSnippet(addr);

                gmap.AddMarker(options);


                //proveri kao trebalo bi da vrati neki float pitanje sta
                //Location start = new Location("starting point");
                //Location end = new Location("ending point");

                //float bearing = start.BearingTo(end);



              

            }
        }

        #endregion


        #region find_road
     

        public void FindRoute()
        {
            
        }

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.MapLayout);

            SetUpMap();


            //for finding 

            GetMarkOnMapForNameOfStreet("Bulevar Nemanjica, Serbia, Nis, 6"); // to je format za trazenje iscrtavanja konkretne ulice i broja znaci  "ulica, drzvama, grad, broj u ulici"
        }
      
    }
}