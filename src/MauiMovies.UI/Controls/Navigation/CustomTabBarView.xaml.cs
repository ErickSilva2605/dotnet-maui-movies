namespace MauiMovies.UI.Controls.Navigation;

public partial class CustomTabBarView : ContentView
{
	public const double TabsHeight = 80;
	public const double IconHeight = 24;

	readonly Color barColorLight;
	readonly Color barColorDark;
	readonly Paint circlePaintLight;
	readonly Paint circlePaintDark;
	Color iconColor;

	int currentTab;

	readonly (Grid tab, Label icon)[] tabs = [];
	TabBarViewDrawable? drawable;

	double SelectedIconTranslation =>
		((CalculateInnerRadius((float)backGraphicsView.Height, TabsPadding) * 2) - IconHeight) / 2;

	double DefaultIconTranslation =>
		((CalculateTabsHeight((float)backGraphicsView.Height, TabsPadding) -
		CalculateInnerRadius((float)backGraphicsView.Height, TabsPadding) - IconHeight) / 2) +
		CalculateInnerRadius((float)backGraphicsView.Height, TabsPadding);

	public static readonly BindableProperty TabsPaddingProperty =
		BindableProperty.Create(nameof(TabsPadding), typeof(Thickness), typeof(CustomTabBarView),
			defaultValue: Thickness.Zero, propertyChanged: OnTabsPaddingChanged);

	public Thickness TabsPadding
	{
		get => (Thickness)GetValue(TabsPaddingProperty);
		set => SetValue(TabsPaddingProperty, value);
	}

	public event EventHandler<AppTab>? TabSelected;

	public CustomTabBarView()
	{
		var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;

		barColorLight = ResolveColor(ResourceKeys.SurfaceLight, Colors.White);
		barColorDark = ResolveColor(ResourceKeys.SurfaceDark, Colors.DarkBlue);
		circlePaintLight = new SolidPaint(ResolveColor(ResourceKeys.TopBarButtonBackgroundLight, Colors.Blue));
		circlePaintDark = new SolidPaint(ResolveColor(ResourceKeys.TopBarButtonBackgroundDark, Colors.Blue));
		iconColor = ResolveColor(isDark ? ResourceKeys.TopBarIconColorDark : ResourceKeys.TopBarIconColorLight, Colors.Gray);

		InitializeComponent();

		tabs = [(tab0, icon0), (tab1, icon1), (tab2, icon2), (tab3, icon3), (tab4, icon4)];
		rootGrid.HeightRequest = TabsHeight + TabsPadding.VerticalThickness;
		backGraphicsView.SizeChanged += OnBackGraphicsViewSizeChanged;
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Application.Current is not { } app)
			return;
		if (Handler is not null)
			app.RequestedThemeChanged += OnAppThemeChanged;
		else
			app.RequestedThemeChanged -= OnAppThemeChanged;
	}

	void OnAppThemeChanged(object? sender, AppThemeChangedEventArgs e)
	{
		iconColor = ResolveColor(
			e.RequestedTheme == AppTheme.Dark ? ResourceKeys.TopBarIconColorDark : ResourceKeys.TopBarIconColorLight,
			Colors.Gray);

		foreach (var (_, icon) in tabs)
			icon.TextColor = iconColor;

		backGraphicsView?.Invalidate();
	}

	static Color ResolveColor(string key, Color fallback) =>
		Application.Current?.Resources.TryGetValue(key, out var obj) == true
			? obj as Color ?? fallback
			: fallback;

	void OnBackGraphicsViewSizeChanged(object? sender, EventArgs e)
	{
		if (backGraphicsView.Height <= 0)
			return;

		if (drawable is null)
			InitializeDrawable();

		SetCircleCenterX(CalculateCircleCenterX(currentTab));
		tabs[currentTab].icon.TranslationY = SelectedIconTranslation;
		backGraphicsView.Invalidate();
	}

	void InitializeDrawable()
	{
		drawable = new TabBarViewDrawable(barColorLight, barColorDark, circlePaintLight, circlePaintDark) { TabsPadding = TabsPadding };
		backGraphicsView.Drawable = drawable;

		foreach (var (_, icon) in tabs)
		{
			icon.TranslationY = DefaultIconTranslation;
			icon.TextColor = iconColor;
		}
	}

	void OnTab0Tapped(object sender, TappedEventArgs e) => OnTabTapped(0);
	void OnTab1Tapped(object sender, TappedEventArgs e) => OnTabTapped(1);
	void OnTab2Tapped(object sender, TappedEventArgs e) => OnTabTapped(2);
	void OnTab3Tapped(object sender, TappedEventArgs e) => OnTabTapped(3);
	void OnTab4Tapped(object sender, TappedEventArgs e) => OnTabTapped(4);

	void OnTabTapped(int column)
	{
		if (column == currentTab)
			return;

		AnimateToTab(column);

		currentTab = column;
		TabSelected?.Invoke(this, (AppTab)column);
	}

	public void SelectTab(AppTab tab, bool animate = true)
	{
		int column = (int)tab;
		if (column == currentTab)
			return;

		if (animate)
			AnimateToTab(column);
		else
			SnapToTab(column);

		currentTab = column;
	}

	void AnimateToTab(int column)
	{
		if (drawable is null)
			return;

		int fromTab = currentTab;
		int difference = Math.Abs(fromTab - column);
		const uint duration = 400;
		uint scaledDuration = (uint)(Math.Pow(difference, 1.0 / 3.0) * duration);
		double iconRatio = (double)duration / scaledDuration;

		var oldIcon = tabs[fromTab].icon;
		var newIcon = tabs[column].icon;

		var oldIconAnimation = new Animation(
			v => oldIcon.TranslationY = v,
			oldIcon.TranslationY, DefaultIconTranslation,
			easing: Easing.SpringOut);

		var newIconAnimation = new Animation(
			v => newIcon.TranslationY = v,
			newIcon.TranslationY, SelectedIconTranslation,
			easing: Easing.SpringOut);

		var baseAnimation = new Animation
		{
			{ 0, 0.8d, CreateCircleAnimation(CalculateCircleCenterX(column)) },
			{ 0, iconRatio, oldIconAnimation },
			{ 1 - iconRatio, 1, newIconAnimation }
		};

		baseAnimation.Commit(this, "TabAnimation", length: duration);
	}

	void SnapToTab(int column)
	{
		if (drawable is null)
			return;

		tabs[currentTab].icon.TranslationY = DefaultIconTranslation;
		tabs[column].icon.TranslationY = SelectedIconTranslation;

		SetCircleCenterX(CalculateCircleCenterX(column));
	}

	Animation CreateCircleAnimation(float targetX) =>
		new(
			v => SetCircleCenterX((float)v),
			drawable!.CircleCenterX, targetX,
			easing: Easing.SpringOut,
			finished: () => SetCircleCenterX(targetX));

	void SetCircleCenterX(float x)
	{
		if (drawable is null)
			return;
		drawable.CircleCenterX = x;
		backGraphicsView.Invalidate();
	}

	float CalculateCircleCenterX(int column)
	{
		var segmentWidth = (backGraphicsView.Width - TabsPadding.HorizontalThickness) / tabs.Length;
		return (float)((column * segmentWidth) + (segmentWidth / 2) + TabsPadding.Left);
	}

	static void OnTabsPaddingChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is not CustomTabBarView view || newValue is not Thickness padding)
			return;

		view.rootGrid.HeightRequest = TabsHeight + padding.VerticalThickness;
		view.buttonsGrid.Padding = padding;

		if (view.drawable is null)
			return;

		view.drawable.TabsPadding = padding;
		view.backGraphicsView.Invalidate();
	}

	public static float CalculateTabsHeight(float viewHeight, Thickness padding) =>
		(float)(viewHeight - padding.VerticalThickness);

	public static float CalculateInnerRadius(float viewHeight, Thickness padding) =>
		CalculateTabsHeight(viewHeight, padding) / (11f / 4f);

	public static float CalculateOuterRadius(float viewHeight, Thickness padding) =>
		CalculateInnerRadius(viewHeight, padding) + (CalculateTabsHeight(viewHeight, padding) / 12f);
}
