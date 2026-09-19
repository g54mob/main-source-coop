using UnityEngine;

public static class MouthPointFinder
{
	public static Transform Find(Transform root)
	{
		if (root == null)
		{
			return null;
		}
		Transform jawEnd = null;
		Transform jaw = null;
		Transform head = null;
		Search(root, ref jawEnd, ref jaw, ref head);
		if (jawEnd != null)
		{
			return jawEnd;
		}
		if (jaw != null)
		{
			return jaw;
		}
		if (head != null)
		{
			return head;
		}
		return root;
	}

	private static void Search(Transform t, ref Transform jawEnd, ref Transform jaw, ref Transform head)
	{
		string text = t.name.ToLowerInvariant();
		if (jawEnd == null && text.Contains("jaw") && text.Contains("end"))
		{
			jawEnd = t;
		}
		else if (jaw == null && (text.Contains("jaw") || text.Contains("mouth")))
		{
			jaw = t;
		}
		else if (head == null && text.Contains("head"))
		{
			head = t;
		}
		for (int i = 0; i < t.childCount; i++)
		{
			Search(t.GetChild(i), ref jawEnd, ref jaw, ref head);
			if (jawEnd != null)
			{
				break;
			}
		}
	}
}
