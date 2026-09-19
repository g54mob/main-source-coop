using UnityEngine;

public static class HipPointFinder
{
	public static Transform Find(Transform root)
	{
		if (root == null)
		{
			return null;
		}
		Transform centeredHip = null;
		Transform sidedHip = null;
		Search(root, ref centeredHip, ref sidedHip);
		if (centeredHip != null)
		{
			return centeredHip;
		}
		if (sidedHip != null)
		{
			return sidedHip;
		}
		return root;
	}

	private static void Search(Transform t, ref Transform centeredHip, ref Transform sidedHip)
	{
		string text = t.name.ToLowerInvariant();
		if (text.Contains("hip") || text.Contains("pelvis"))
		{
			if (IsSidedName(text))
			{
				if (sidedHip == null)
				{
					sidedHip = t;
				}
			}
			else if (centeredHip == null)
			{
				centeredHip = t;
			}
		}
		for (int i = 0; i < t.childCount; i++)
		{
			Search(t.GetChild(i), ref centeredHip, ref sidedHip);
			if (centeredHip != null)
			{
				break;
			}
		}
	}

	private static bool IsSidedName(string n)
	{
		if (!n.Contains("left") && !n.Contains("right") && !n.Contains(".l") && !n.Contains(".r") && !n.Contains("_l") && !n.Contains("_r") && !n.Contains("l_") && !n.Contains("r_") && !n.Contains(" l"))
		{
			return n.Contains(" r");
		}
		return true;
	}
}
