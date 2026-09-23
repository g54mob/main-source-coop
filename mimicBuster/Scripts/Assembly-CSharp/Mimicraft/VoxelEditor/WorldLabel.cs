using TMPro;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class WorldLabel
	{
		public static TextMeshPro Create(Transform parent, string name, float fontSize, Color color, TMP_FontAsset font = null)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
			if (font != null)
			{
				textMeshPro.font = font;
			}
			textMeshPro.fontSize = fontSize;
			textMeshPro.alignment = TextAlignmentOptions.Center;
			textMeshPro.color = color;
			textMeshPro.textWrappingMode = TextWrappingModes.NoWrap;
			return textMeshPro;
		}

		public static void Place(TextMeshPro label, Vector3 worldPosition, Vector3 cameraPosition, float scaleCompensation)
		{
			Transform transform = label.transform;
			transform.position = worldPosition;
			transform.rotation = Quaternion.LookRotation(worldPosition - cameraPosition);
			transform.localScale = Vector3.one * scaleCompensation;
		}

		public static float ScaleFor(VoxelModel model)
		{
			return 1f / Mathf.Max((model != null) ? model.VoxelSize : 1f, 0.0001f);
		}

		public static Vector3 CameraPosition(Camera cam, Vector3 fallbackSubject)
		{
			if (!(cam != null))
			{
				return fallbackSubject + Vector3.forward;
			}
			return cam.transform.position;
		}
	}
}
