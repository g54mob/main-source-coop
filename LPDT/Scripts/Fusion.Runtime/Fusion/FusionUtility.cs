using UnityEngine;

namespace Fusion
{
	internal static class FusionUtility
	{
		public static void PruneMixedAuthorityHierarchy(Transform currentParent, Transform targetParent, PlayerRef rootAuth)
		{
			for (int num = currentParent.childCount - 1; num >= 0; num--)
			{
				Transform child = currentParent.GetChild(num);
				PruneMixedAuthorityHierarchy(child, targetParent, rootAuth);
				if (child.TryGetComponent<NetworkObject>(out var component) && !component.IsNested && !(component.StateAuthority == rootAuth))
				{
					for (int num2 = child.childCount - 1; num2 >= 0; num2--)
					{
						child.GetChild(num2).SetParent(currentParent);
					}
					child.SetParent(targetParent);
				}
			}
		}
	}
}
