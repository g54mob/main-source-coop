using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Cameras
{
	public static class PlaceholderCameras
	{
		private static readonly List<Camera> retired = new List<Camera>();

		public static void Retire()
		{
			Restore();
			Camera[] allCameras = Camera.allCameras;
			foreach (Camera camera in allCameras)
			{
				if (!(camera == null) && camera.enabled && !(camera.targetTexture != null) && !(camera.GetComponentInParent<NetworkObject>() != null))
				{
					camera.enabled = false;
					retired.Add(camera);
					Debug.Log("[PlaceholderCameras] '" + camera.name + "' (" + camera.gameObject.scene.name + ") kapatildi - oyuncunun kendi kamerasi devrede, ikinci bir tam sahne cizimi gereksiz.");
				}
			}
		}

		public static void Restore()
		{
			foreach (Camera item in retired)
			{
				if (item != null)
				{
					item.enabled = true;
				}
			}
			retired.Clear();
		}
	}
}
