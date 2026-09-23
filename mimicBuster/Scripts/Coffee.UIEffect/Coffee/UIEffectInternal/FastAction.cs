using System;

namespace Coffee.UIEffectInternal
{
	internal class FastAction : FastActionBase<Action>
	{
		public void Invoke()
		{
			Invoke(delegate(Action action)
			{
				action();
			});
		}
	}
}
