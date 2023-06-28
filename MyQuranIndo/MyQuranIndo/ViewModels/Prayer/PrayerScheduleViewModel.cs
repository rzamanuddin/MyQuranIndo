using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Prayers;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Prayer
{
    public class PrayerScheduleViewModel : BaseViewModel
    {
        private string city;
        private Placemark placemark;
        //private PrayerSchedule prayerSchedule;
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
            Title = "Jadwal Sholat (Beta)";
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
                // Handle not supported on device exception
                throw new FeatureNotSupportedException(Message.MSG_DEVICE_NOT_SUPPORTED+ Environment.NewLine + fnsEx.Message);
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
                throw new Exception(Message.MSG_FAIL_GET_LOCATION);
            }
            return location;
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                //Placemark = await GetPlacemarksAsync();
                //City = Placemark.AdminArea; // Placemark.SubAdminArea;

                //if (City != null)
                //{
                //    City = City.Trim();
                //}
                var location = await GetLocationAsync();
                PrayerTimes.Clear();

                //PrayerSchedule = await PrayerDataService.GetAsync(location.Latitude, location.Longitude, location.Altitude, false);
                if (Placemark == null)
                {
                    return;
                }

                var locationResults = await PrayerDataService.GetLocationsAsync(Placemark.SubAdminArea);
                var ps = await PrayerDataService.GetAsync(locationResults);

                if (ps == null || ps.Location == null || ps.Location.Schedule == null)
                {
                    return;
                }

                PrayerSchedule = ps.Location.Schedule;
                PrayerTimes.Add(new PrayerTime()
                { 
                    Name = "Imsak",
                    Time = PrayerSchedule.Imsak,
                    Row = 1
                });

                PrayerTimes.Add(new PrayerTime()
                {
                    Name = "Subuh",
                    Time = PrayerSchedule.Subuh,
                    Row = 2
                });

                PrayerTimes.Add(new PrayerTime()
                {
                    Name = "Terbit",
                    Time = PrayerSchedule.Terbit,
                    Row = 3
                });

                PrayerTimes.Add(new PrayerTime()
                {
                    Name = "Dzuhur",
                    Time = PrayerSchedule.Dzuhur,
                    Row = 4
                });

                PrayerTimes.Add(new PrayerTime()
                {
                    Name = "Ashar",
                    Time = PrayerSchedule.Ashar,
                    Row = 5
                });

                PrayerTimes.Add(new PrayerTime()
                {
                    Name = "Maghrib",
                    Time = PrayerSchedule.Maghrib,
                    Row = 6
                });

                PrayerTimes.Add(new PrayerTime()
                {
                    Name = "Isya",
                    Time = PrayerSchedule.Isya,
                    Row = 7
                });

                string[] times = System.DateTime.Now.ToString("HH:mm").Split(':');

                int hour = 0;
                int minute = 0;

                int.TryParse(times[0], out hour);
                int.TryParse(times[1], out minute);

                List<String> prayerTimes = new List<string>();
                for (int t = PrayerTimes.Count - 1; t >= 0; t--)
                {
                    prayerTimes.Add(PrayerTimes[t].Time);
                }

                int row = prayerTimes.Count - 1;
                bool isFound = false;
                foreach (var prayerTime in prayerTimes)
                {
                    if (hour > Convert.ToInt32(prayerTime.Substring(0, 2))
                        || (hour >= Convert.ToInt32(prayerTime.Substring(0, 2))
                            && minute >= Convert.ToInt32(prayerTime.Substring(3, 2)))
                       )
                    {
                        PrayerTimes[row].FontAttributes = FontAttributes.Bold;
                        isFound = true;
                        break;
                    }
                    row--;
                }
                // If prayer time is not found then set font attribute isya to bold
                if (!isFound)
                {
                    PrayerTimes[PrayerTimes.Count - 1].FontAttributes = FontAttributes.Bold;
                }

                //if (PrayerSchedule != null)
                //{
                //    if (PrayerSchedule.DateTime != null && PrayerSchedule.DateTime.Count > 0)
                //    {
                //        for (int i = 0; i < PrayerSchedule.DateTime.Count; i++)
                //        {
                //            var dt = PrayerSchedule.DateTime[i];

                //            PrayerTimes.Add(new PrayerTime()
                //            {
                //                Name = "Imsak",
                //                Time = GetPrayTime(dt.Times.Imsak, 8 - 2),
                //                Row = 1
                //            });

                //            PrayerTimes.Add(new PrayerTime()
                //            {
                //                Name = "Subuh",
                //                Time = GetPrayTime(dt.Times.Fajr, 8 - 2),
                //                Row = 2
                //            });

                //            PrayerTimes.Add(new PrayerTime()
                //            {
                //                Name = "Terbit",
                //                Time = dt.Times.Sunrise,
                //                Row = 3
                //            });
                //            PrayerTimes.Add(new PrayerTime()
                //            {
                //                Name = "Dzuhur",
                //                Time = GetPrayTime(dt.Times.Dhuhr, 2, false),
                //                Row = 4
                //            });
                //            PrayerTimes.Add(new PrayerTime()
                //            {
                //                Name = "Ashar",
                //                Time = GetPrayTime(dt.Times.Asr, 2, false),
                //                Row = 5
                //            });
                //            PrayerTimes.Add(new PrayerTime()
                //            {
                //                Name = "Maghrib",
                //                Time = GetPrayTime(dt.Times.Maghrib, 2, false),
                //                Row = 6
                //            });
                //            PrayerTimes.Add(new PrayerTime()
                //            {
                //                Name = "Isya",
                //                Time = GetPrayTime(dt.Times.Isha, 4 + 2, false),
                //                Row = 7
                //            });

                //            string[] times = System.DateTime.Now.ToString("HH:mm").Split(':');

                //            int hour = 0;
                //            int minute = 0;

                //            int.TryParse(times[0], out hour);
                //            int.TryParse(times[1], out minute);

                //            List<String> prayerTimes = new List<string>();
                //            for (int t = PrayerTimes.Count - 1 ; t >=0; t--)
                //            {
                //                prayerTimes.Add(PrayerTimes[t].Time);
                //            }

                //            int row = prayerTimes.Count - 1;
                //            bool isFound = false;
                //            foreach (var prayerTime in prayerTimes)
                //            {
                //                if (hour > Convert.ToInt32(prayerTime.Substring(0, 2))
                //                    || (hour >= Convert.ToInt32(prayerTime.Substring(0, 2))
                //                        && minute >= Convert.ToInt32(prayerTime.Substring(3, 2)))
                //                   )
                //                {
                //                    PrayerTimes[row].FontAttributes = FontAttributes.Bold;
                //                    isFound = true;
                //                    break;
                //                }
                //                row--;
                //            }
                //            // If prayer time is not found then set font attribute isya to bold
                //            if (!isFound)
                //            {
                //                PrayerTimes[PrayerTimes.Count - 1].FontAttributes = FontAttributes.Bold;
                //            }
                //        }
                //    }
                //}
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

        private string GetPrayTime(string time, int diff, bool isDecrease = true)
        {
            var times = time.Split(':');

            int hour = 0;
            int minute = 0;
            int.TryParse(times[0], out hour);
            int.TryParse(times[1], out minute);

            if (isDecrease)
            {
                if (minute - diff < 0)
                {
                    minute = 60 + (minute - diff);
                    return $"{(hour - 1).ToString("00")}:{minute.ToString("00")}";
                }
                else
                {
                    minute -= diff;
                    return $"{hour.ToString("00")}:{minute.ToString("00")}";
                }
            }
            else 
            {
                if (minute + diff > 60)
                {
                    minute = (minute + diff) - 60;
                    return $"{(hour + 1).ToString("00")}:{minute.ToString("00")}";
                }
                else if (minute + diff == 60)
                {
                    return $"{(hour + 1).ToString("00")}:{"00"}";
                }
                else
                {
                    minute += diff;
                    return $"{hour.ToString("00")}:{minute.ToString("00")}";
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
