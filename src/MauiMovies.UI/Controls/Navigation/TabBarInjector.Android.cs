using Android.Views;
using Android.Widget;
using Microsoft.Maui.Platform;

namespace MauiMovies.UI.Controls.Navigation;

static partial class TabBarInjector
{
	internal static partial void Inject(CustomTabBarView tabBar, IMauiContext mauiContext)
	{
		var platformView = tabBar.ToPlatform(mauiContext);
		var activity = Platform.CurrentActivity!;
		var content = activity.FindViewById<FrameLayout>(Android.Resource.Id.Content)!;
		var density = activity.Resources!.DisplayMetrics!.Density;
		var heightPx = (int)(CustomTabBarView.TabsHeight * density);

		var layoutParams = new FrameLayout.LayoutParams(
			ViewGroup.LayoutParams.MatchParent,
			heightPx,
			GravityFlags.Bottom);

		content.AddView(platformView, layoutParams);
	}
}
