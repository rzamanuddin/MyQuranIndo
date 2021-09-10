using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyQuranIndo.Controls;
using MyQuranIndo.Droid.CustomRenderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomTableView), typeof(CustomTableViewRenderer))]
namespace MyQuranIndo.Droid.CustomRenderers
{
    public class CustomTableViewRenderer : TableViewRenderer
    {
        public CustomTableViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;
            Control.Divider = new ColorDrawable(Android.Graphics.Color.Transparent);
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "SeparatorColor")
            {
                modelRenderer?.NotifyDataSetChanged();
            }
        }

        CustomTableViewModelRenderer modelRenderer;
        protected override TableViewModelRenderer GetModelRenderer(global::Android.Widget.ListView listView, TableView view)
        {
            return modelRenderer = new CustomTableViewModelRenderer(this.Context, listView, view);
        }
    }

    public class CustomTableViewModelRenderer : TableViewModelRenderer
    {
        CustomTableView tableView;
        public CustomTableViewModelRenderer(Context Context, global::Android.Widget.ListView ListView, TableView View)
            : base(Context, ListView, View)
        {
            tableView = (CustomTableView)View;
        }
        public override global::Android.Views.View GetView(int position, global::Android.Views.View convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);

            var element = this.GetCellForPosition(position);

            if (element.GetType() == typeof(TextCell))
            {
                try
                {
                    var divider = (view as LinearLayout).GetChildAt(1);

                    divider.SetBackgroundColor(tableView.SeparatorColor.ToAndroid());
                }
                catch (Exception) { }
            }

            return view;
        }
    }
}