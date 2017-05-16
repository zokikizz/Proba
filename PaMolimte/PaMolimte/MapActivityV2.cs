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
using System.Net.Http;
using System.Net;

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
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latLng, 15); // apdejtovace se kamera na mesto gde je latLng i uveca ce se puta 10
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
        //za ovo prvo u nuget-u instaliras Microsoft.Bcl.Build, pa onda Http Client trazis isto tu i skines i to


        HttpClient client;
        public async void FindRouteAsync(string source, string destrination) // radi sa primerom https://maps.googleapis.com/maps/api/directions/json?origin=Brooklyn&destination=Queens&mode=transit&key=AIzaSyDY8RF7Oa9h5IeD7DX6l_GtGPnjT6J7k4U
        {
            client = new HttpClient();
            // string default_for_every_query = "https://maps.googleapis.com/maps/api/directions/json?";

            HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/directions/json?origin=" + source + "&destination=" + destrination +"&mode=transit&key=AIzaSyDY8RF7Oa9h5IeD7DX6l_GtGPnjT6J7k4U");

            if ( response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var json = await content.ReadAsStringAsync();
                Console.WriteLine(json.ToString());

                JSONObject jsonresoult = new JSONObject(json);
                JSONArray routesArray = jsonresoult.GetJSONArray("routes");
                JSONObject routes = routesArray.GetJSONObject(0);
                JSONObject overviewPolylines = routes.GetJSONObject("overview_polyline");
                String encodedString = overviewPolylines.GetString("points");
                List<LatLng> list = decodePoly(encodedString);

                PolylineOptions po = new PolylineOptions();
                             //treba i < list.Count zasto puca sa tim?
              
                for (int i = 0; i < list.Count; i++)
                {
                    po.Add(list[i]).InvokeWidth(10).InvokeColor(0x66FF0000);//red color
                }

                gmap.AddPolyline(po);
            }

        }

        public async void FindOptimizaton(string source, string destrination)// ne radi otimizacija mora dugacije
        {
            client = new HttpClient();
            // string default_for_every_query = "https://maps.googleapis.com/maps/api/directions/json?";

            HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/directions/json?origin=" + source + "&destination=" + destrination + "&mode=transit&waypoints=optimize:true|Nikole Pasica, Serbia, Nis, 6&key=AIzaSyDY8RF7Oa9h5IeD7DX6l_GtGPnjT6J7k4U");
            //
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var json = await content.ReadAsStringAsync();
                Console.WriteLine(json.ToString());

                

                JSONObject jsonresoult = new JSONObject(json);
                JSONArray routesArray = jsonresoult.GetJSONArray("routes");
                JSONObject routes = routesArray.GetJSONObject(0);
                JSONObject overviewPolylines = routes.GetJSONObject("overview_polyline");
                String encodedString = overviewPolylines.GetString("points");
                List<LatLng> list = decodePoly(encodedString);

                PolylineOptions po = new PolylineOptions();
                //treba i < list.Count zasto puca sa tim?
                for (int i = 0; i < list.Count - 1; i++)
                {
                    LatLng src = list.GetRange(i, 1)[0];
                    LatLng dest = list.GetRange(i + 1, 1)[0];


                    po.Add(src, dest).InvokeWidth(10);//.InvokeColor(SystemColors ;
                    

                    gmap.AddPolyline(po);

                }
            }

        }



        // dekodiranje vracene putanje od GoogleMapsAPI-a
        private List<LatLng> decodePoly(String encoded)
        {

            List<LatLng> poly = new List<LatLng>();
            int index = 0, len = encoded.Length;
            int lat = 0, lng = 0;

            while (index < len)
            {
                int b, shift = 0, result = 0;
                do
                {
                    b = encoded[index++] - 63;
                    result |= (b & 0x1f) << shift;
                    shift += 5;
                } while (b >= 0x20);
                int dlat = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
                lat += dlat;

                shift = 0;
                result = 0;
                do
                {
                    b = encoded[index++] - 63;
                    result |= (b & 0x1f) << shift;
                    shift += 5;
                } while (b >= 0x20);
                int dlng = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
                lng += dlng;

                LatLng p = new LatLng((((double)lat / 1E5)),
                        (((double)lng / 1E5)));
                poly.Add(p);
            }

            return poly;
        }

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.MapLayout);

            

            SetUpMap();


            //for finding 

            GetMarkOnMapForNameOfStreet("Bulevar Nemanjica, Serbia, Nis, 6"); // to je format za trazenje iscrtavanja konkretne ulice i broja znaci  "ulica, drzvama, grad, broj u ulici"

            //prikaz putanje
             FindRouteAsync("Bulevar Nemanjica, Serbia, Nis, 6", "Kosovska, Srbia, Nis,  6");//Kosovska 6, Nis, "Jug Bogdanova, Srbia, Nis,  6"

            // FindOptimizaton("Bulevar Nemanjica, Serbia, Nis, 6", "Jug Bogdanova, Srbia, Nis,  6");//ne radi


        }
      
    }
}