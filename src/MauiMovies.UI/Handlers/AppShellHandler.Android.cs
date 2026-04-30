using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace MauiMovies.UI.Handlers;

class AppShellRenderer : ShellRenderer
{
	protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
		=> new ZeroInsetToolbarTracker();

	sealed class ZeroInsetToolbarTracker : IShellToolbarAppearanceTracker
	{
		void IShellToolbarAppearanceTracker.SetAppearance(
			AndroidX.AppCompat.Widget.Toolbar toolbar,
			IShellToolbarTracker toolbarTracker,
			ShellAppearance appearance)
		{
			toolbar.SetContentInsetsAbsolute(0, 0);
			toolbar.ContentInsetStartWithNavigation = 0;
		}

		void IShellToolbarAppearanceTracker.ResetAppearance(
			AndroidX.AppCompat.Widget.Toolbar toolbar,
			IShellToolbarTracker toolbarTracker)
		{ }

		void IDisposable.Dispose() { }
	}
}
