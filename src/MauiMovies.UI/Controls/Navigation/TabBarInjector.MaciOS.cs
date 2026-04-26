using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace MauiMovies.UI.Controls.Navigation;

static partial class TabBarInjector
{
	internal static partial void Inject(CustomTabBarView tabBar, IMauiContext mauiContext)
	{
		var platformView = (UIView)tabBar.ToPlatform(mauiContext);
		var windowScene = UIApplication.SharedApplication.ConnectedScenes
			.OfType<UIWindowScene>()
			.FirstOrDefault();

		var window = windowScene?.Windows.FirstOrDefault(w => w.IsKeyWindow);
		if (window is null)
			return;

		var height = (nfloat)CustomTabBarView.TabsHeight;
		var bounds = window.Bounds;

		platformView.Frame = new CGRect(0, bounds.Height - height, bounds.Width, height);
		platformView.AutoresizingMask =
			UIViewAutoresizing.FlexibleWidth |
			UIViewAutoresizing.FlexibleTopMargin;

		window.AddSubview(platformView);
		window.BringSubviewToFront(platformView);
	}
}
