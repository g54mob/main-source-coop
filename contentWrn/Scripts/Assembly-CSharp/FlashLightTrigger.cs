using UnityEngine;

public class FlashLightTrigger : MonoBehaviour
{
	public int lightLevel = 1;

	private bool isPlayerLight;

	private void Start()
	{
		if ((bool)GetComponentInParent<Player>())
		{
			isPlayerLight = true;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		Player component = other.transform.root.GetComponent<Player>();
		if (!component || other.isTrigger)
		{
			return;
		}
		Bot componentInChildren = component.GetComponentInChildren<Bot>();
		if ((bool)componentInChildren && componentInChildren.lightLevelRequired <= lightLevel && (!componentInChildren.flashLightRequireLineOfSight || !isPlayerLight || !HelperFunctions.LineCheck(base.transform.position, componentInChildren.Center(), HelperFunctions.LayerType.TerrainProp).transform))
		{
			componentInChildren.sinceFlashLit = 0f;
			if (!isPlayerLight)
			{
				componentInChildren.sinceFlashLit_LevelLight = 0f;
			}
			else
			{
				componentInChildren.sinceFlashLit_PlayerLight = 0f;
			}
		}
	}
}
