using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Prayers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Prayer
{
    public class PrayerScheduleViewModel : BaseViewModel
    {
        private string city;
        private Placemark placemark;
        private MyQuranIndo.Models.PrayerSchedules.PrayerSchedule prayerSchedule;

        public ObservableCollection<PrayerTime> PrayerTimes { get; }

        public string City
        {
            get => city;
            set => SetProperty(ref city, value);
        }

        public MyQuranIndo.Models.PrayerSchedules.PrayerSchedule PrayerSchedule
        {
            get => prayerSchedule;
            set => SetProperty(ref prayerSchedule, value);
        }

        public Placemark Placemark
        {
            get => placemark;
            set => SetProperty(ref placemark, value);
        }

        public ICommand LoadCommand { get; private set; }

        public PrayerScheduleViewModel()
        {
            Title = "Jadwal Sholat";
            this.PrayerSchedule = new Models.PrayerSchedules.PrayerSchedule();
            PrayerTimes = new ObservableCollection<PrayerTime>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
        }

        private async Task<Xamarin.Essentials.Location> GetLocationAsync()
        {
            Xamarin.Essentials.Location location = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var cts = new CancellationTokenSource();
                location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    Placemark = placemarks?.FirstOrDefault();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                throw new FeatureNotSupportedException(Message.MSG_DEVICE_NOT_SUPPORTED + Environment.NewLine + fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                throw new FeatureNotEnabledException(Message.MSG_DEVICE_NOT_ENABLED + Environment.NewLine + fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                throw new PermissionException(Message.MSG_DEVICE_PERMISSION_DENIED + Environment.NewLine + pEx.Message);
            }
            catch (Exception)
            {
                throw new Exception(Message.MSG_FAIL_GET_LOCATION);
            }
            return location;
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                var location = await GetLocationAsync();
                if (Placemark == null) return;

                string searchArea = !string.IsNullOrWhiteSpace(Placemark.SubAdminArea) ? Placemark.SubAdminArea : Placemark.Locality;
                if (string.IsNullOrWhiteSpace(searchArea)) return;

                var locationResults = await PrayerDataService.GetLocationsAsync(searchArea);
                var ps = await PrayerDataService.GetAsync(locationResults);

                if (ps?.Location?.Schedule == null) return;

                PrayerSchedule = ps.Location.Schedule;

                var tempTimes = new List<PrayerTime>
                {
                    new PrayerTime { Name = "Imsak", Time = PrayerSchedule.Imsak, Row = 1 },
                    new PrayerTime { Name = "Subuh", Time = PrayerSchedule.Subuh, Row = 2 },
                    new PrayerTime { Name = "Terbit", Time = PrayerSchedule.Terbit, Row = 3 },
                    new PrayerTime { Name = "Dhuha", Time = PrayerSchedule.Dhuha, Row = 4 },
                    new PrayerTime { Name = "Dzuhur", Time = PrayerSchedule.Dzuhur, Row = 5 },
                    new PrayerTime { Name = "Ashar", Time = PrayerSchedule.Ashar, Row = 6 },
                    new PrayerTime { Name = "Maghrib", Time = PrayerSchedule.Maghrib, Row = 7 },
                    new PrayerTime { Name = "Isya", Time = PrayerSchedule.Isya, Row = 8 }
                };

                // Evaluasi waktu sholat saat ini untuk mencetak tebal
                string[] times = System.DateTime.Now.ToString("HH:mm").Split(':');
                int.TryParse(times[0], out int hour);
                int.TryParse(times[1], out int minute);

                bool isFound = false;
                for (int t = tempTimes.Count - 1; t >= 0; t--)
                {
                    var prayerTime = tempTimes[t].Time;
                    if (!string.IsNullOrEmpty(prayerTime) && prayerTime.Length >= 5)
                    {
                        int pHour = Convert.ToInt32(prayerTime.Substring(0, 2));
                        int pMinute = Convert.ToInt32(prayerTime.Substring(3, 2));

                        if (hour > pHour || (hour == pHour && minute >= pMinute))
                        {
                            tempTimes[t].FontAttributes = FontAttributes.Bold;
                            isFound = true;
                            break;
                        }
                    }
                }

                if (!isFound && tempTimes.Count > 0)
                {
                    tempTimes[tempTimes.Count - 1].FontAttributes = FontAttributes.Bold;
                }

                // Batch assignment ke ObservableCollection untuk performa UI
                PrayerTimes.Clear();
                foreach (var item in tempTimes)
                {
                    PrayerTimes.Add(item);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR,
                    Message.MSG_FAIL_GET_PRAYER_SCHEDULE + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}