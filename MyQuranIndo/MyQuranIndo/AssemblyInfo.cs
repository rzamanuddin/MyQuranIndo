using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("LPMQ_IsepMisbah.ttf", Alias = "LPMQ")]
[assembly: ExportFont("Montserrat-Light.ttf", Alias = "Mntserrat-Light")]
[assembly: ExportFont("Montserrat-LightItalic.ttf", Alias = "Montserrat-Light-Italic")]
[assembly: ExportFont("Montserrat-ExtraLight.ttf", Alias = "Montserrat-ExtraLight")]
[assembly: ExportFont("Montserrat-ExtraLightItalic.ttf", Alias = "Montserrat-ExtraLight-Italic")]
[assembly: ExportFont("Montserrat-Bold.ttf", Alias = "Montserrat-Bold")]
[assembly: ExportFont("Montserrat-BoldItalic.ttf", Alias = "Montserrat-Bold-Italic")]
[assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
[assembly: ExportFont("Montserrat-MediumItalic.ttf", Alias = "Montserrat-Medium-Italic")]
#if DEBUG
[assembly: Application(Debuggable = true)]
#else
[assembly: Application(Debuggable=false)]
#endif