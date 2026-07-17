using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MyQuranIndo.Controls;
using MyQuranIndo.Droid.CustomRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(BindableTabbedPage), typeof(BindableTabbedPageRenderer))]
namespace MyQuranIndo.Droid.CustomRenderers
{
    public class BindableTabbedPageRenderer : TabbedPageRenderer
    {
        public BindableTabbedPageRenderer(Context context) : base(context) { }
        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            // Cari TabLayout di dalam view
            for (int i = 0; i < ChildCount; i++)
            {
                var view = GetChildAt(i);
                if (view is Android.Support.Design.Widget.TabLayout tabLayout)
                {
                    tabLayout.LayoutDirection = Android.Views.LayoutDirection.Rtl;
                }
            }
        }
    }
}