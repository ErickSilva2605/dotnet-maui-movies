namespace MauiMovies.UI.Controls.Navigation;

class TabBarViewDrawable(Color barColorLight, Color barColorDark, Paint circlePaintLight, Paint circlePaintDark) : IDrawable
{
	public float CircleCenterX { get; set; } = 0f;
	public Thickness TabsPadding { get; set; }

	bool IsDark => Application.Current?.RequestedTheme == AppTheme.Dark;
	Color BarFillColor  => IsDark ? barColorDark  : barColorLight;
	Paint CircleFillPaint => IsDark ? circlePaintDark : circlePaintLight;

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		var innerRadius = CustomTabBarView.CalculateInnerRadius(dirtyRect.Height, TabsPadding);
		var outerRadius = CustomTabBarView.CalculateOuterRadius(dirtyRect.Height, TabsPadding);
		var circleY = innerRadius + (float)TabsPadding.Top;
		var path = CreatePath(dirtyRect, innerRadius, outerRadius, CircleCenterX, circleY);

		canvas.FillColor = BarFillColor;
		canvas.FillPath(path);

		var circleRect = new RectF(CircleCenterX - innerRadius, circleY - innerRadius, innerRadius * 2, innerRadius * 2);
		canvas.SetFillPaint(CircleFillPaint, circleRect);
		canvas.FillCircle(CircleCenterX, circleY, innerRadius);
	}

	static PathF CreatePath(RectF bounds, float innerRadius, float outerRadius, float circleX, float circleY)
	{
		var pts = ComputeNotchPoints(innerRadius, outerRadius, circleX, circleY);
		return BuildPath(bounds, innerRadius, pts);
	}

	static NotchPoints ComputeNotchPoints(float innerRadius, float outerRadius, float circleX, float circleY)
	{
		var notchBottom = new PointF(circleX, circleY + outerRadius);

		float tangentY     = notchBottom.Y * (4f / 5f);
		float circleConst  = (float)(Math.Pow(circleX, 2) + Math.Pow(circleY, 2) - Math.Pow(outerRadius, 2));
		float discriminant = (float)(Math.Pow(2 * circleX, 2) - (4 * (Math.Pow(tangentY, 2) - (2 * circleY * tangentY) + circleConst)));
		float leftContactX  = circleX - (float)(Math.Sqrt(discriminant) / 2);
		float rightContactX = circleX + (float)(Math.Sqrt(discriminant) / 2);

		var leftTangentEnd  = new PointF(Math.Min(leftContactX, rightContactX), tangentY);
		var rightTangentEnd = new PointF(Math.Max(leftContactX, rightContactX), tangentY);

		float tangentAngle   = (float)((Math.PI / 2) - Math.Atan((circleX - leftTangentEnd.X) / (leftTangentEnd.Y - circleY)));
		float horizontalSpan = (float)Math.Tan(tangentAngle) * (leftTangentEnd.Y - circleY);

		var leftTransitionControl  = new PointF(leftTangentEnd.X  - horizontalSpan, innerRadius);
		var rightTransitionControl = new PointF(rightTangentEnd.X + horizontalSpan, innerRadius);

		var leftFlatEnd    = new PointF(leftTransitionControl.X  - (outerRadius - innerRadius), innerRadius);
		var rightFlatStart = new PointF(rightTransitionControl.X + (outerRadius - innerRadius), innerRadius);

		float tangentLength = (float)Math.Sqrt(Math.Pow(leftTangentEnd.X - leftTransitionControl.X, 2) + Math.Pow(leftTangentEnd.Y - leftTransitionControl.Y, 2));
		float bezierScale   = (outerRadius - innerRadius) / tangentLength;
		float bezierOffsetX = (leftTangentEnd.X - leftTransitionControl.X) * bezierScale;
		float bezierOffsetY = (leftTangentEnd.Y - leftTransitionControl.Y) * bezierScale;

		var leftTangentStart  = new PointF(leftTransitionControl.X  + bezierOffsetX, leftTransitionControl.Y  + bezierOffsetY);
		var rightTangentStart = new PointF(rightTransitionControl.X - bezierOffsetX, rightTransitionControl.Y + bezierOffsetY);

		float arcTopT = (notchBottom.Y - leftTangentStart.Y) / (leftTangentEnd.Y - leftTangentStart.Y);
		float arcTopX = leftTangentStart.X + arcTopT * (leftTangentEnd.X - leftTangentStart.X);

		var leftArcControl  = new PointF(arcTopX, notchBottom.Y);
		var rightArcControl = new PointF(2 * circleX - arcTopX, notchBottom.Y);

		return new NotchPoints(
			leftFlatEnd,
			leftTransitionControl, leftTangentStart,
			leftTangentEnd,
			leftArcControl, notchBottom,
			rightArcControl, rightTangentEnd,
			rightTangentStart, rightTransitionControl,
			rightFlatStart);
	}

	static PathF BuildPath(RectF bounds, float innerRadius, NotchPoints pts)
	{
		var path = new PathF();
		path.MoveTo(0, innerRadius);
		path.LineTo(pts.LeftFlatEnd.X, pts.LeftFlatEnd.Y);
		path.QuadTo(pts.LeftTransitionControl.X, pts.LeftTransitionControl.Y, pts.LeftTangentStart.X, pts.LeftTangentStart.Y);
		path.LineTo(pts.LeftTangentEnd.X, pts.LeftTangentEnd.Y);
		path.QuadTo(pts.LeftArcControl.X, pts.LeftArcControl.Y, pts.NotchBottom.X, pts.NotchBottom.Y);
		path.QuadTo(pts.RightArcControl.X, pts.RightArcControl.Y, pts.RightTangentEnd.X, pts.RightTangentEnd.Y);
		path.LineTo(pts.RightTangentStart.X, pts.RightTangentStart.Y);
		path.QuadTo(pts.RightTransitionControl.X, pts.RightTransitionControl.Y, pts.RightFlatStart.X, pts.RightFlatStart.Y);
		path.LineTo(bounds.Width, innerRadius);
		path.LineTo(bounds.Width, bounds.Height);
		path.LineTo(0, bounds.Height);
		path.Close();
		return path;
	}

	readonly record struct NotchPoints(
		PointF LeftFlatEnd,
		PointF LeftTransitionControl, PointF LeftTangentStart,
		PointF LeftTangentEnd,
		PointF LeftArcControl, PointF NotchBottom,
		PointF RightArcControl, PointF RightTangentEnd,
		PointF RightTangentStart, PointF RightTransitionControl,
		PointF RightFlatStart);
}
