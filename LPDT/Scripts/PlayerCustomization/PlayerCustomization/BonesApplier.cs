using System.Collections.Generic;
using UnityEngine;

namespace PlayerCustomization
{
	public class BonesApplier : MonoBehaviour
	{
		[SerializeField]
		private Transform _characterRootBone;

		[SerializeField]
		private SkinnedMeshRenderer _characterSkinnedMesh;

		[SerializeField]
		private List<GameObject> _skinObjects;

		[ContextMenu("ApplyBones")]
		public void ApplyBones()
		{
			foreach (GameObject skinObject in _skinObjects)
			{
				SkinnedMeshRenderer[] componentsInChildren = skinObject.GetComponentsInChildren<SkinnedMeshRenderer>();
				foreach (SkinnedMeshRenderer obj in componentsInChildren)
				{
					obj.rootBone = _characterRootBone;
					obj.bones = _characterSkinnedMesh.bones;
				}
			}
		}
	}
}
