using UnityEngine;
using UnityEngine.Rendering;

public class ObjectToggler : MonoBehaviour
{
	public enum HideWhen
	{
		Local = 0,
		OtherClient = 1
	}

	public enum HideType
	{
		Disable = 0,
		DisableRenderers = 1,
		SwitchToShadowOnly = 2,
		SetLocalDontSeeLayer = 3
	}

	public HideWhen hideWhen;

	public HideType hideType;

	private void Start()
	{
		Player componentInParent = GetComponentInParent<Player>();
		if ((bool)componentInParent)
		{
			if (hideWhen == HideWhen.Local)
			{
				if (!componentInParent.data.isLocal)
				{
					return;
				}
			}
			else if (hideWhen == HideWhen.OtherClient && componentInParent.data.isLocal)
			{
				return;
			}
		}
		if (hideType == HideType.Disable)
		{
			base.gameObject.SetActive(value: false);
		}
		else if (hideType == HideType.DisableRenderers)
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
		else if (hideType == HideType.SwitchToShadowOnly)
		{
			SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
		}
		else if (hideType == HideType.SetLocalDontSeeLayer)
		{
			base.gameObject.layer = 29;
		}
	}
}
