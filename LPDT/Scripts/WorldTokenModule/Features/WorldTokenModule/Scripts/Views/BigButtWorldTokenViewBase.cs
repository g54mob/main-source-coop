using UnityEngine;

namespace Features.WorldTokenModule.Scripts.Views
{
	public abstract class BigButtWorldTokenViewBase : WorldTokenViewBase
	{
		public abstract void SetColor(Color color);

		public abstract void SetMovementDirection(Vector2 direction);
	}
}
