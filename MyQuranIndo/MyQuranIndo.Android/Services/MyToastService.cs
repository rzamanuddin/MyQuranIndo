using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyQuranIndo.Droid.Services;
using MyQuranIndo.Services;
using Android.Text;

[assembly: Xamarin.Forms.Dependency(typeof(MyToastService))]
namespace MyQuranIndo.Droid.Services
{
    public class MyToastService : IToastService
    {
        public void Show(string message)
        {
            //var toast = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long);//.Show();
            ////toast.SetGravity(GravityFlags.CenterHorizontal, 0, 0);3            
            //toast.Show();
           Show(message, true, false);
        }

        public void Show(string message, bool isLongDuration)
        {
            Show(message, isLongDuration, false);
        }

        public void Show(string message, bool isLongDuration, bool isCenter = false)
        {
            var toast = Toast.MakeText(Android.App.Application.Context, message, (isLongDuration ? ToastLength.Long : ToastLength.Short));
            if (isCenter)
                toast.SetGravity(GravityFlags.CenterHorizontal, 0, 0);            
            toast.Show();
        }
    }
}