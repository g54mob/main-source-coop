using System.Collections;
using Portningsbolaget.Platforms;
using UnityEngine;

public class IslandUnlock : MonoBehaviour
{
	public SFX_Instance unlockSFX;

	public bool locked = true;

	public int price = 500;

	private int framesInRoom;

	private void Start()
	{
	}

	internal int GetID()
	{
		return base.transform.GetSiblingIndex();
	}

	internal void Unlock()
	{
		unlockSFX.Play(base.transform.position);
		StartCoroutine(IUnlock());
		IEnumerator IUnlock()
		{
			Transform transform = base.transform.Find("Locked");
			Object.Instantiate(Resources.Load("UnlockExplosion"), transform.position, Quaternion.LookRotation(Vector3.up));
			yield return new WaitForSeconds(0.35f);
			Activate();
			OnIslandUnlock();
		}
	}

	internal void Activate()
	{
		if (locked)
		{
			locked = false;
			base.transform.Find("Unlocked").gameObject.SetActive(value: true);
			base.transform.Find("Locked").gameObject.SetActive(value: false);
		}
	}

	private void OnIslandUnlock()
	{
		int siblingIndex = base.transform.GetSiblingIndex();
		Debug.Log("[Achievement] OnIsland Unlock Achievement: " + base.gameObject.name + " Index: " + siblingIndex);
		switch (siblingIndex)
		{
		case 0:
			Debug.Log("[Achievement] Unlocked Pool");
			PlatformManager.UnlockAchievement(Achievements.ACH_POOL);
			break;
		case 1:
			Debug.Log("[Achievement] Unlocked Cinema!");
			PlatformManager.UnlockAchievement(Achievements.ACH_CINEMA);
			break;
		case 2:
			Debug.Log("[Achievement] Unlocked PodCast");
			PlatformManager.UnlockAchievement(Achievements.ACH_PODCAST);
			break;
		case 3:
			Debug.Log("[Achievement] Unlocked GreenScreen");
			PlatformManager.UnlockAchievement(Achievements.ACH_GREENSCREEN);
			break;
		case 4:
			Debug.Log("[Achievement] Unlocked Trampoline!");
			PlatformManager.UnlockAchievement(Achievements.ACH_TRAMPOLINE);
			break;
		}
	}
}
