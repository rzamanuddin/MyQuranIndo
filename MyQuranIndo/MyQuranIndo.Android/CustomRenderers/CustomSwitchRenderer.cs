using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyQuranIndo.Droid.CustomRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Switch), typeof(CustomSwitchRenderer))]
namespace MyQuranIndo.Droid.CustomRenderers
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                //Control.TextOn = "Si";
                //Control.TextOff = "No";
                Android.Graphics.Color colorOn = Android.Graphics.Color.Green;
                Android.Graphics.Color colorOff = Android.Graphics.Color.LightGray;
                Android.Graphics.Color colorDisabled = Android.Graphics.Color.Gray;

                StateListDrawable drawable = new StateListDrawable();
                drawable.AddState(new int[] { Android.Resource.Attribute.StateChecked }, new ColorDrawable(colorOn));
                drawable.AddState(new int[] { -Android.Resource.Attribute.StateEnabled }, new ColorDrawable(colorDisabled));
                drawable.AddState(new int[] { }, new ColorDrawable(colorOff));

                Control.ThumbDrawable = drawable;
            }
        }
    }
}