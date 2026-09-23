using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class PlayerEarAnchor : MonoBehaviour
	{
		private const float EarHeight = 1.6f;

		private AudioListener listener;

		private Transform anchor;

		private PlayerCameraRig cameraRig;

		private Transform listenFrom;

		private bool listenAtHeadHeight;

		public void ListenFrom(Transform target, bool atHeadHeight)
		{
			listenFrom = ((target == base.transform) ? null : target);
			listenAtHeadHeight = atHeadHeight;
			if (listenFrom == null && anchor != null)
			{
				anchor.localPosition = new Vector3(0f, 1.6f, 0f);
			}
		}

		public void Initialize(PlayerCameraRig rig, bool isOwner)
		{
			cameraRig = rig;
			if (!isOwner)
			{
				base.enabled = false;
				return;
			}
			AudioListener[] componentsInChildren = GetComponentsInChildren<AudioListener>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			anchor = new GameObject("EarAnchor").transform;
			anchor.SetParent(base.transform, worldPositionStays: false);
			anchor.localPosition = new Vector3(0f, 1.6f, 0f);
			listener = anchor.gameObject.AddComponent<AudioListener>();
		}

		private void LateUpdate()
		{
			if (!(listener == null) && !(cameraRig == null))
			{
				if (listenFrom != null)
				{
					anchor.position = listenFrom.position + (listenAtHeadHeight ? (Vector3.up * 1.6f) : Vector3.zero);
				}
				Transform transform = ActiveCamera();
				if (transform != null)
				{
					anchor.rotation = transform.rotation;
				}
			}
		}

		private Transform ActiveCamera()
		{
			Camera main = Camera.main;
			if (!(main != null))
			{
				return null;
			}
			return main.transform;
		}
	}
}
