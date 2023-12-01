using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Qibla
{
    public class QiblaViewModel : BaseViewModel
    {
        const double QIBLA_LATITUDE = 21.422487;
        const double QIBLA_LONGITUDE = 39.826206;

        private string headingDisplay;
        private Placemark placemark;
        public string HeadingDisplay
        {
            get => headingDisplay;
            set => SetProperty(ref headingDisplay, value);
        }

        double heading = 0;
        public double Heading
        {
            get => heading;
            set => SetProperty(ref heading, value);
        }

        private double qibla = 0;
        public double Qibla
        {
            get => qibla;
            set => SetProperty(ref qibla, value);
        }

        private double distance = 0;
        public double Distance
        {
            get => distance;
            set => SetProperty(ref distance, value);
        }
        public Placemark Placemark
        {
            get => placemark;
            set => SetProperty(ref placemark, value);
        }

        private Location location;
        public Location Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        public Location QiblaLocation
        { 
            get
            {
                return new Location(QIBLA_LATITUDE, QIBLA_LONGITUDE);
            }
        }

        private string info;
        public String Info
        {
            get => info;
            set => SetProperty(ref info, value);
        }

        public Command StopCommand { get; }
        public Command StartCommand { get; }
        public Command LoadCommand { get; }

        public QiblaViewModel()
        {
            Title = "Arah Kiblat";
            StopCommand = new Command(Stop);
            StartCommand = new Command(Start);
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
        }

        private async Task<Location> GetLocation()
        {
            Location location = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                var cts = new CancellationTokenSource();
                location = await Geolocation.GetLocationAsync(request, cts.Token);
                //Location = location;

                //if (location != null)
                //{
                //    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                //    Placemark = placemarks?.FirstOrDefault();
                //}
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                throw new FeatureNotSupportedException(Message.MSG_FAIL_GET_PRAYER_SCHEDULE + Environment.NewLine + fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                throw new FeatureNotEnabledException(Message.MSG_DEVICE_NOT_ENABLED + Environment.NewLine + fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                throw new PermissionException(Message.MSG_DEVICE_PERMISSION_DENIED + Environment.NewLine + pEx.Message);
            }
            catch (Exception ex)
            {
                // Unable to get location
                throw new Exception(Message.MSG_FAIL_GET_LOCATION + Environment.NewLine + ex.Message);
            }

            return location;
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                IsBusy = true;
                Location = await GetLocation();
                if (Location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    Placemark = placemarks?.FirstOrDefault();

                    //Location sourceCoordinates = new Location(Location.Latitude, Location.Longitude);
                    //Location destinationCoordinates = new Location(QIBLA_LATITUDE, QIBLA_LONGITUDE);
                    Distance = Location.CalculateDistance(Location, QiblaLocation, DistanceUnits.Kilometers);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR,
                    Message.MSG_FAIL_GET_DISTANCE + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        void Stop()
        {
            if (!Compass.IsMonitoring)
                return;

            Compass.ReadingChanged -= Compass_ReadingChanged;
            Compass.Stop();
        }

        void Start()
        {
            if (Compass.IsMonitoring)
                return;

            //Compass.ApplyLowPassFilter = true;
            Compass.ReadingChanged += Compass_ReadingChanged;
            Compass.Start(SensorSpeed.UI);

        }

        //void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        //{
        //    Heading = e.Reading.HeadingMagneticNorth;
        //    var value1 = 360 - Heading;
        //    var value2 = -Heading;

        //    if (Math.Abs(value1) > Math.Abs(value2))
        //    {
        //        Qibla = value2 + 295.25;
        //    }
        //    else
        //    {
        //        Qibla = value1 + 295.25;
        //    }
        //    HeadingDisplay = $"Arah Ke Utara: {Heading.ToString("N2")}";
        //}

        async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            try
            {
                //Qibla lat and long
                //var qiblaLocation = new Location(QIBLA_LATITUDE, QIBLA_LONGITUDE);

                //current position
                //var position = await GetLocation();
                var res = DistanceCalculator.Bearing(Location, QiblaLocation);
                var TargetHeading = (360 - res) % 360;

                var currentHeading = 360 - e.Reading.HeadingMagneticNorth;
                Heading = currentHeading - TargetHeading;

                //Info = string.Format("Garis Lintang: {0:N2}, Bujur: {1:N2}", Location.Latitude, Location.Longitude);
                Info = string.Format($"{Placemark.SubLocality}, {Placemark.Locality}");

            }
            catch (Exception ex)
            {
                //log exception}
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
