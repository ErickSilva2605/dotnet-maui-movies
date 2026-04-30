using Android.App;
using Android.Content.PM;
using Android.Runtime;

namespace MauiMovies.UI;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[Register("com.mauimovies.MainActivity")]
public class MainActivity : MauiAppCompatActivity
{
}