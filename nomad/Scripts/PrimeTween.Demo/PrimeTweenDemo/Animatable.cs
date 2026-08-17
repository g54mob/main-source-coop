using PrimeTween;

namespace PrimeTweenDemo
{
	public abstract class Animatable : Clickable
	{
		public abstract Sequence Animate(bool toEndValue);
	}
}
