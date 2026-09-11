using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerAnimRefHandler : MonoBehaviour
{
	private Player player;

	public List<SyncedAnimTransforms> syncedTransforms = new List<SyncedAnimTransforms>();

	public void DoInit()
	{
		player = GetComponent<Player>();
		SpawnAnimRef();
	}

	internal GameObject AddItem(ItemInstance currentItem, Vector3 spawnPos, Quaternion spawnRot)
	{
		GameObject gameObject = Object.Instantiate(currentItem.gameObject, spawnPos, spawnRot, base.transform);
		StripAnimRef(gameObject, stripScripts: true);
		return gameObject;
	}

	private void SpawnAnimRef()
	{
		GameObject gameObject = Object.Instantiate(player.refs.rigRoot, base.transform.position + Vector3.up * 3f, base.transform.rotation, base.transform);
		gameObject.name = "AnimationRig";
		StripAnimRef(gameObject);
		StripRig(player.refs.rigRoot);
		FindSyncedTransforms(player.refs.rigRoot, gameObject);
	}

	private void StripRig(GameObject rigRoot)
	{
		AudioSource[] componentsInChildren = rigRoot.GetComponentsInChildren<AudioSource>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren[i]);
		}
		AudioLoop[] componentsInChildren2 = rigRoot.GetComponentsInChildren<AudioLoop>(includeInactive: true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			Object.DestroyImmediate(componentsInChildren2[j]);
		}
		FootstepHandler[] componentsInChildren3 = rigRoot.GetComponentsInChildren<FootstepHandler>(includeInactive: true);
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			Object.Destroy(componentsInChildren3[k]);
		}
		SFX_PlayOneShot[] componentsInChildren4 = rigRoot.GetComponentsInChildren<SFX_PlayOneShot>(includeInactive: true);
		for (int l = 0; l < componentsInChildren4.Length; l++)
		{
			Object.Destroy(componentsInChildren4[l]);
		}
		FollowHeadFromRig[] componentsInChildren5 = rigRoot.GetComponentsInChildren<FollowHeadFromRig>(includeInactive: true);
		for (int m = 0; m < componentsInChildren5.Length; m++)
		{
			Object.Destroy(componentsInChildren5[m]);
		}
	}

	private void StripAnimRef(GameObject spawned, bool stripScripts = false)
	{
		Renderer[] componentsInChildren = spawned.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		FootstepOnOverlap[] componentsInChildren2 = spawned.GetComponentsInChildren<FootstepOnOverlap>(includeInactive: true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			Object.Destroy(componentsInChildren2[j]);
		}
		Bodypart[] componentsInChildren3 = spawned.GetComponentsInChildren<Bodypart>();
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].gameObject.AddComponent<BodypartAnimationTarget>().Config(componentsInChildren3[k].bodypartType);
			Object.DestroyImmediate(componentsInChildren3[k]);
		}
		Joint[] componentsInChildren4 = spawned.GetComponentsInChildren<Joint>();
		for (int l = 0; l < componentsInChildren4.Length; l++)
		{
			Object.DestroyImmediate(componentsInChildren4[l]);
		}
		Rigidbody[] componentsInChildren5 = spawned.GetComponentsInChildren<Rigidbody>();
		for (int m = 0; m < componentsInChildren5.Length; m++)
		{
			Object.DestroyImmediate(componentsInChildren5[m]);
		}
		Collider[] componentsInChildren6 = spawned.GetComponentsInChildren<Collider>();
		for (int n = 0; n < componentsInChildren6.Length; n++)
		{
			componentsInChildren6[n].enabled = false;
		}
		UniversalAdditionalLightData[] componentsInChildren7 = spawned.GetComponentsInChildren<UniversalAdditionalLightData>();
		for (int num = 0; num < componentsInChildren7.Length; num++)
		{
			Object.DestroyImmediate(componentsInChildren7[num]);
		}
		Light[] componentsInChildren8 = spawned.GetComponentsInChildren<Light>();
		for (int num2 = 0; num2 < componentsInChildren8.Length; num2++)
		{
			Object.DestroyImmediate(componentsInChildren8[num2]);
		}
		if (stripScripts)
		{
			MonoBehaviour[] componentsInChildren9 = spawned.GetComponentsInChildren<MonoBehaviour>();
			for (int num3 = 0; num3 < componentsInChildren9.Length; num3++)
			{
				if (!(componentsInChildren9[num3] is BodypartAnimationTarget) && !(componentsInChildren9[num3] is HandGizmo))
				{
					Object.DestroyImmediate(componentsInChildren9[num3]);
				}
			}
		}
		Camera[] componentsInChildren10 = spawned.GetComponentsInChildren<Camera>();
		for (int num4 = 0; num4 < componentsInChildren10.Length; num4++)
		{
			Object.DestroyImmediate(componentsInChildren10[num4].gameObject);
		}
	}

	private void LateUpdate()
	{
		HandleSyncedTransforms();
	}

	private void FindSyncedTransforms(GameObject rigRoot, GameObject spawned)
	{
		List<Transform> list = HelperFunctions.FindAllChildrenWithTag("Sync", rigRoot.transform);
		for (int i = 0; i < list.Count; i++)
		{
			Transform setTarget = HelperFunctions.FindChildRecursive(list[i].name, spawned.transform);
			syncedTransforms.Add(new SyncedAnimTransforms(list[i], setTarget));
		}
	}

	private void HandleSyncedTransforms()
	{
		for (int i = 0; i < syncedTransforms.Count; i++)
		{
			syncedTransforms[i].follower.SetLocalPositionAndRotation(syncedTransforms[i].target.transform.localPosition, syncedTransforms[i].target.transform.localRotation);
			syncedTransforms[i].follower.localScale = syncedTransforms[i].target.localScale;
		}
	}
}
