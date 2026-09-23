using System;

namespace Coffee.UIEffects
{
	public enum GradationMode
	{
		None = 0,
		Horizontal = 1,
		HorizontalGradient = 2,
		Vertical = 3,
		VerticalGradient = 4,
		Radial = 5,
		[Obsolete]
		RadialFast = 5,
		[Obsolete]
		RadialDetail = 6,
		RadialGradient = 12,
		Diagonal = 11,
		DiagonalToRightBottom = 7,
		DiagonalToLeftBottom = 8,
		Angle = 9,
		AngleGradient = 10
	}
}
