using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class DimensionLabels : MonoBehaviour
	{
		private const int AxisCount = 3;

		private const float FontSize = 0.05f;

		private const float OutwardOffset = 1.2f;

		private static readonly Color LabelColor = Color.white;

		private readonly TextMeshPro[] labels = new TextMeshPro[3];

		private readonly List<VoxelBodyPiece> gathered = new List<VoxelBodyPiece>();

		private readonly Vector3[] corners = new Vector3[8];

		[SerializeField]
		private Material materialOverride;

		[Tooltip("Ölçü yazılarının fontu. Boş bırakılırsa TMP'nin kendi varsayılanı kullanılır.")]
		[SerializeField]
		private TMP_FontAsset fontOverride;

		public static void EnsureExists(GameObject host)
		{
			if (!(host == null) && !(Object.FindFirstObjectByType<DimensionLabels>(FindObjectsInactive.Include) != null))
			{
				host.AddComponent<DimensionLabels>();
			}
		}

		private void Awake()
		{
			for (int i = 0; i < 3; i++)
			{
				labels[i] = WorldLabel.Create(base.transform, $"Axis{i}", 0.05f, LabelColor, fontOverride);
				labels[i].gameObject.SetActive(value: false);
			}
		}

		private void Update()
		{
			if (!VoxelEditorSettings.ShowDimensions || VoxelEditorSettings.IsMovementMode)
			{
				HideAll();
				return;
			}
			VoxelBodyReport voxelBodyReport = VoxelBodyRules.Evaluate(VoxelFocusManager.GatherBody(gathered));
			Transform transform = FirstUsableTransform();
			if (!voxelBodyReport.HasVoxels || transform == null)
			{
				HideAll();
			}
			else
			{
				Show(voxelBodyReport.Bounds, transform);
			}
		}

		private void Show(Bounds bounds, Transform reference)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			for (int i = 0; i < 8; i++)
			{
				corners[i] = new Vector3(((i & 1) != 0) ? max.x : min.x, ((i & 2) != 0) ? max.y : min.y, ((i & 4) != 0) ? max.z : min.z);
			}
			Camera activeCamera = VoxelEditorSettings.ActiveCamera;
			Vector3 vector = reference.TransformPoint(bounds.center);
			Vector3 vector2 = WorldLabel.CameraPosition(activeCamera, vector);
			VoxelModel component = reference.GetComponent<VoxelModel>();
			float scaleCompensation = WorldLabel.ScaleFor(component);
			Vector3 size = bounds.size;
			for (int j = 0; j < 3; j++)
			{
				Vector3 vector3 = BestEdgeMidpoint(j, reference, vector2);
				Vector3 vector4 = vector3 - vector;
				if (vector4.sqrMagnitude > 1E-06f)
				{
					vector3 += vector4.normalized * (1.2f * ReferenceScale(component));
				}
				TextMeshPro textMeshPro = labels[j];
				textMeshPro.gameObject.SetActive(value: true);
				textMeshPro.text = Mathf.RoundToInt(size[j]).ToString();
				if (materialOverride != null && textMeshPro.fontSharedMaterial != materialOverride)
				{
					textMeshPro.fontSharedMaterial = materialOverride;
				}
				WorldLabel.Place(textMeshPro, vector3, vector2, scaleCompensation);
			}
		}

		private Vector3 BestEdgeMidpoint(int axis, Transform reference, Vector3 camPos)
		{
			int num = 1 << axis;
			Vector3 result = Vector3.zero;
			float num2 = float.MaxValue;
			for (int i = 0; i < 8; i++)
			{
				if ((i & num) == 0)
				{
					Vector3 vector = reference.TransformPoint(corners[i]);
					Vector3 vector2 = reference.TransformPoint(corners[i | num]);
					Vector3 vector3 = (vector + vector2) * 0.5f;
					float sqrMagnitude = (vector3 - camPos).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
						result = vector3;
					}
				}
			}
			return result;
		}

		private static float ReferenceScale(VoxelModel model)
		{
			if (!(model != null))
			{
				return 1f;
			}
			return model.VoxelSize;
		}

		private static Transform FirstUsableTransform()
		{
			foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
			{
				if (item.Model != null)
				{
					return item.transform;
				}
			}
			return null;
		}

		private void HideAll()
		{
			TextMeshPro[] array = labels;
			foreach (TextMeshPro textMeshPro in array)
			{
				if (textMeshPro != null && textMeshPro.gameObject.activeSelf)
				{
					textMeshPro.gameObject.SetActive(value: false);
				}
			}
		}
	}
}
