using System;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal struct SetAndRestoreVar<T> : IDisposable
	{
		private readonly Action<T> oTYaMbEJOHgWFriwyKVuQJmGgRLMA;

		private readonly T aeBVIwKfLInMbZLMxxXPuWvxrCrQ;

		public SetAndRestoreVar(T P_0, T P_1, Action<T> P_2)
		{
			if (P_2 == null)
			{
				throw new ArgumentNullException("setValueDelegate");
			}
			oTYaMbEJOHgWFriwyKVuQJmGgRLMA = P_2;
			aeBVIwKfLInMbZLMxxXPuWvxrCrQ = P_0;
			P_2(P_1);
		}

		public void Dispose()
		{
			oTYaMbEJOHgWFriwyKVuQJmGgRLMA(aeBVIwKfLInMbZLMxxXPuWvxrCrQ);
		}
	}
}
