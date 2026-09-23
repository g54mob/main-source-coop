using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class CenterMarkerGizmo : MonoBehaviour
	{
		private const float WorldDiameter = 0.12f;

		private static readonly Color MarkerColor = new Color(1f, 0.9f, 0.15f, 0.95f);

		private MeshRenderer meshRenderer;

		public static CenterMarkerGizmo Create(Transform parent)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.name = "CenterMarkerGizmo";
			Collider component = gameObject.GetComponent<Collider>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			CenterMarkerGizmo centerMarkerGizmo = gameObject.AddComponent<CenterMarkerGizmo>();
			centerMarkerGizmo.meshRenderer = gameObject.GetComponent<MeshRenderer>();
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				centerMarkerGizmo.meshRenderer.sharedMaterial = new Material(shader)
				{
					color = MarkerColor
				};
			}
			gameObject.SetActive(value: false);
			return centerMarkerGizmo;
		}

		public void Show(VoxelModel model, float sizeMultiplier = 1f)
		{
			if (!GridBounds.TryCompute(model.Grid, out var _, out var _))
			{
				Hide();
				return;
			}
			float num = Mathf.Max(model.VoxelSize, 0.0001f);
			base.transform.localPosition = model.GetCurrentBoundsCenterLocal();
			base.transform.localScale = Vector3.one * (0.12f * sizeMultiplier / num);
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
