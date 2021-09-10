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
using MyQuranIndo.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Text;
using Android.Text.Method;
using System.ComponentModel;
using MyQuranIndo.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace MyQuranIndo.Droid.CustomRenderers
{
    public class HtmlLabelRenderer : LabelRenderer
    {        
        public HtmlLabelRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (HtmlLabel)Element;
            if (view == null || view.Text == null) return;

            // Control.MovementMethod = LinkMovementMethod.Instance;

            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.N)
            {
                Control.SetText(Html.FromHtml(view.Text, FromHtmlOptions.ModeLegacy), TextView.BufferType.Spannable);
            }
            else
            {
                // For API < 24 
                Control.SetText(Html.FromHtml(view.Text), TextView.BufferType.Normal);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                var view = (HtmlLabel)sender;
                if (view == null || view.Text == null) return;
                if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.N)
                {
                    Control.SetText(Html.FromHtml(view.Text, FromHtmlOptions.ModeLegacy), TextView.BufferType.Spannable);
                }
                else
                {
                    // For API < 24 
                    Control.SetText(Html.FromHtml(view.Text), TextView.BufferType.Normal);
                }
            }
        }
    }
}