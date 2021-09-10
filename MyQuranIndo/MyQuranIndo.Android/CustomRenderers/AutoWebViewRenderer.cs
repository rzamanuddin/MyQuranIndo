using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MyQuranIndo.Controls;
using MyQuranIndo.Droid.CustomRenderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Android.Webkit.WebView;

//[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(MyWebViewRenderer))]
[assembly: ExportRenderer(typeof(AutoWebView), typeof(AutoWebViewRenderer))]
namespace MyQuranIndo.Droid.CustomRenderers
{
    public class AutoWebViewRenderer : WebViewRenderer
    {

        //public MyWebViewRenderer(Context context) : base(context)
        //{
        //}

        class ExtendedWebViewClient : Android.Webkit.WebViewClient
        {
            AutoWebView xwebView;
            public ExtendedWebViewClient(AutoWebView webView)
            {
                xwebView = webView;
            }

            async public override void OnPageFinished(Android.Webkit.WebView view, string url)
            {
                if (xwebView != null)
                {
                    int i = 10;
                    while (view.ContentHeight == 0 && i-- > 0) // wait here till content is rendered
                        await System.Threading.Tasks.Task.Delay(100);
                    xwebView.HeightRequest = view.ContentHeight;
                    // Here use parent to find the ViewCell, you can adjust the number of parents depending on your XAML
                    //(xwebView.Parent.Parent as StackLayout).ForceLayout();//.ForceUpdateSize();
                    var viewCell = (xwebView.Parent.Parent as ViewCell);
                    if (viewCell != null)
                    {
                        viewCell.ForceUpdateSize();
                    }

                    //var lv = new ListView { HasUnevenRows = true };
                    //var cell = new ViewCell { Parent = lv };
                }

                base.OnPageFinished(view, url);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.SetWebViewClient(new ExtendedWebViewClient(Element as AutoWebView));

        }

        //public AutoWebViewRenderer(Context context) : base(context)
        //{
        //}

        //class ExtendedWebViewClient : Android.Webkit.WebViewClient
        //{
        //    private readonly AutoWebView _control;

        //    public ExtendedWebViewClient(AutoWebView control)
        //    {
        //        _control = control;
        //    }

        //    public override async void OnPageFinished(WebView view, string url)
        //    {
        //        if (_control != null)
        //        {
        //            int i = 10;
        //            while (view.ContentHeight == 0 && i-- > 0) // wait here till content is rendered
        //            {
        //                await System.Threading.Tasks.Task.Delay(100);
        //            }

        //            //view.ScrollView.ContentSize.Height;                    
        //            _control.HeightRequest =  view.ContentHeight;
        //        }

        //        base.OnPageFinished(view, url);
        //    }
        //}

        //protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.NewElement is AutoWebView webViewControl)
        //    {
        //        if (e.OldElement == null)
        //        {
        //            Control.SetWebViewClient(new ExtendedWebViewClient(webViewControl));
        //        }
        //    }
        //}



        //static AutoWebView _xwebView = null;

        //public MyWebViewRenderer(Context context) : base(context)
        //{
        //}

        //class DynamicSizeWebViewClient : WebViewClient
        //{
        //    public async override void OnPageFinished(Android.Webkit.WebView view, string url)
        //    {
        //        try
        //        {
        //            if (_xwebView != null)
        //            {
        //                view.Settings.JavaScriptEnabled = true;
        //                view.Settings.DomStorageEnabled = true;
        //                _xwebView.HeightRequest = 0d;

        //                await Task.Delay(100);

        //                string result = await _xwebView.EvaluateJavaScriptAsync("(function(){return document.body.scrollHeight;})()");
        //                _xwebView.HeightRequest = Convert.ToDouble(result);
        //            }
        //            base.OnPageFinished(view, url);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"{ex.Message}");
        //        }
        //    }
        //}

        //protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        //{
        //    base.OnElementChanged(e);
        //    _xwebView = e.NewElement as AutoWebView;

        //    if (e.OldElement == null)
        //    {
        //        Control.SetWebViewClient(new DynamicSizeWebViewClient());
        //    }
        //}
    }
}