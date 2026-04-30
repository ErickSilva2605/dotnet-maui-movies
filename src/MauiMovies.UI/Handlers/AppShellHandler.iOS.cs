using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace MauiMovies.UI.Handlers;

class AppShellRenderer : ShellRenderer
{
	protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
		=> new ZeroMarginNavBarTracker();

	sealed class ZeroMarginNavBarTracker : IShellNavBarAppearanceTracker
	{
		void IShellNavBarAppearanceTracker.SetAppearance(UINavigationController controller, ShellAppearance appearance)
		{
			controller.NavigationBar.LayoutMargins = UIEdgeInsets.Zero;
		}

		void IShellNavBarAppearanceTracker.UpdateLayout(UINavigationController controller) { }

		void IShellNavBarAppearanceTracker.ResetAppearance(UINavigationController controller) { }

		void IShellNavBarAppearanceTracker.SetHasShadow(UINavigationController controller, bool hasShadow) { }

		void IDisposable.Dispose() { }
	}
}
