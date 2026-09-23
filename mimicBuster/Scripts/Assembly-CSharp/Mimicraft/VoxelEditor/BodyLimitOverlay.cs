using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.UI;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class BodyLimitOverlay : MonoBehaviour
	{
		private class BoxGizmo : MonoBehaviour
		{
			private MeshRenderer meshRenderer;

			private Material material;

			public static BoxGizmo Create(Transform parent)
			{
				GameObject obj = new GameObject("BodyLimitBox");
				obj.transform.SetParent(parent, worldPositionStays: false);
				BoxGizmo boxGizmo = obj.AddComponent<BoxGizmo>();
				boxGizmo.Init();
				return boxGizmo;
			}

			private void Init()
			{
				MeshFilter meshFilter = base.gameObject.AddComponent<MeshFilter>();
				meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
				meshFilter.sharedMesh = BuildCubeMesh();
				Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
				if (shader != null)
				{
					material = new Material(shader);
					meshRenderer.sharedMaterial = material;
				}
				base.gameObject.SetActive(value: false);
			}

			public void Show(Transform reference, Vector3 centerVoxels, Vector3 sizeVoxels, Color color)
			{
				base.transform.SetParent(reference, worldPositionStays: false);
				base.transform.localPosition = centerVoxels;
				base.transform.localRotation = Quaternion.identity;
				base.transform.localScale = sizeVoxels;
				if (material != null)
				{
					material.color = color;
				}
				base.gameObject.SetActive(value: true);
			}

			public void Hide()
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(value: false);
				}
			}

			private static Mesh BuildCubeMesh()
			{
				Mesh mesh = new Mesh
				{
					name = "BodyLimitBoxMesh"
				};
				Vector3[] array = new Vector3[8]
				{
					new Vector3(-0.5f, -0.5f, -0.5f),
					new Vector3(0.5f, -0.5f, -0.5f),
					new Vector3(0.5f, -0.5f, 0.5f),
					new Vector3(-0.5f, -0.5f, 0.5f),
					new Vector3(-0.5f, 0.5f, -0.5f),
					new Vector3(0.5f, 0.5f, -0.5f),
					new Vector3(0.5f, 0.5f, 0.5f),
					new Vector3(-0.5f, 0.5f, 0.5f)
				};
				List<Vector3> vertices = new List<Vector3>(24);
				List<int> triangles = new List<int>(36);
				AddFace(vertices, triangles, array[0], array[1], array[2], array[3]);
				AddFace(vertices, triangles, array[7], array[6], array[5], array[4]);
				AddFace(vertices, triangles, array[4], array[5], array[1], array[0]);
				AddFace(vertices, triangles, array[6], array[7], array[3], array[2]);
				AddFace(vertices, triangles, array[7], array[4], array[0], array[3]);
				AddFace(vertices, triangles, array[5], array[6], array[2], array[1]);
				mesh.SetVertices(vertices);
				mesh.SetTriangles(triangles, 0);
				mesh.RecalculateNormals();
				return mesh;
			}

			private static void AddFace(List<Vector3> vertices, List<int> triangles, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
			{
				int count = vertices.Count;
				vertices.Add(a);
				vertices.Add(b);
				vertices.Add(c);
				vertices.Add(d);
				triangles.Add(count);
				triangles.Add(count + 1);
				triangles.Add(count + 2);
				triangles.Add(count);
				triangles.Add(count + 2);
				triangles.Add(count + 3);
			}
		}

		private class PieceWarningGizmo : MonoBehaviour
		{
			private const float TintAlpha = 0.4f;

			private static readonly Color TintColor = new Color(0.85f, 0.15f, 0.1f, 1f);

			private const float IconFontSize = 3f;

			private const float TextFontSize = 1.1f;

			private const float IconWorldOffset = 0.6f;

			private const float TextWorldGapBelowIcon = 0.35f;

			private const string WarningTextKey = "InvalidPlacement";

			private MeshFilter tintMeshFilter;

			private Material tintMaterial;

			private TextMeshPro iconLabel;

			private TextMeshPro textLabel;

			public static PieceWarningGizmo Create(Transform parent)
			{
				GameObject obj = new GameObject("PieceWarningGizmo");
				obj.transform.SetParent(parent, worldPositionStays: false);
				PieceWarningGizmo pieceWarningGizmo = obj.AddComponent<PieceWarningGizmo>();
				pieceWarningGizmo.Init();
				return pieceWarningGizmo;
			}

			private void Init()
			{
				GameObject gameObject = new GameObject("Tint");
				gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				tintMeshFilter = gameObject.AddComponent<MeshFilter>();
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				Shader shader = Shader.Find("Mimicraft/SurfaceOverlayUnlit");
				if (shader != null)
				{
					tintMaterial = new Material(shader)
					{
						color = new Color(TintColor.r, TintColor.g, TintColor.b, 0.4f)
					};
					meshRenderer.sharedMaterial = tintMaterial;
				}
				iconLabel = CreateLabel("Icon", 3f);
				iconLabel.text = "!";
				iconLabel.color = TintColor;
				textLabel = CreateLabel("Text", 1.1f);
				textLabel.text = Loc.Get("InvalidPlacement");
				textLabel.color = Color.white;
				base.gameObject.SetActive(value: false);
			}

			private TextMeshPro CreateLabel(string name, float fontSize)
			{
				GameObject obj = new GameObject(name);
				obj.transform.SetParent(base.transform, worldPositionStays: false);
				TextMeshPro textMeshPro = obj.AddComponent<TextMeshPro>();
				textMeshPro.fontSize = fontSize;
				textMeshPro.alignment = TextAlignmentOptions.Center;
				textMeshPro.textWrappingMode = TextWrappingModes.NoWrap;
				return textMeshPro;
			}

			public void Show(VoxelEditorController controller, Camera cam)
			{
				VoxelModel model = controller.Model;
				MeshFilter component = controller.GetComponent<MeshFilter>();
				if (model == null || component == null || !GridBounds.TryCompute(model.Grid, out var min, out var max))
				{
					Hide();
					return;
				}
				base.gameObject.SetActive(value: true);
				base.transform.SetParent(controller.transform, worldPositionStays: false);
				base.transform.localPosition = Vector3.zero;
				base.transform.localRotation = Quaternion.identity;
				base.transform.localScale = Vector3.one;
				if (tintMeshFilter.sharedMesh != component.sharedMesh)
				{
					tintMeshFilter.sharedMesh = component.sharedMesh;
				}
				Vector3 vector = max + Vector3.one;
				Vector3 vector2 = min;
				Vector3 position = new Vector3((vector2.x + vector.x) * 0.5f, vector.y, (vector2.z + vector.z) * 0.5f);
				Vector3 vector3 = controller.transform.TransformPoint(position);
				Vector3 camPos = ((cam != null) ? cam.transform.position : (vector3 + Vector3.forward));
				float scaleCompensation = 1f / Mathf.Max(model.VoxelSize, 0.0001f);
				Vector3 vector4 = vector3 + Vector3.up * 0.6f;
				PlaceLabel(iconLabel, vector4, camPos, scaleCompensation);
				Vector3 worldPos = vector4 - Vector3.up * 0.35f;
				PlaceLabel(textLabel, worldPos, camPos, scaleCompensation);
			}

			private static void PlaceLabel(TextMeshPro label, Vector3 worldPos, Vector3 camPos, float scaleCompensation)
			{
				Transform obj = label.transform;
				obj.position = worldPos;
				obj.rotation = Quaternion.LookRotation(worldPos - camPos);
				obj.localScale = Vector3.one * scaleCompensation;
			}

			public void Hide()
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(value: false);
				}
			}
		}

		private static readonly Color TooSmall = new Color(1f, 0.35f, 0.25f, 0.16f);

		private static readonly Color TooLarge = new Color(1f, 0.2f, 0.2f, 0.12f);

		private readonly List<BoxGizmo> boxes = new List<BoxGizmo>();

		private readonly Dictionary<VoxelEditorController, PieceWarningGizmo> pieceWarnings = new Dictionary<VoxelEditorController, PieceWarningGizmo>();

		private readonly List<VoxelBodyPiece> gathered = new List<VoxelBodyPiece>();

		private readonly List<VoxelEditorController> gatheredControllers = new List<VoxelEditorController>();

		private Transform reference;

		private string lastWarning;

		private readonly List<VoxelEditorController> deadWarnings = new List<VoxelEditorController>();

		private const string WithheldWarningKey = "Warning.PieceInsideGeometry";

		private void Awake()
		{
			DimensionLabels.EnsureExists(base.gameObject);
		}

		private void PruneDestroyed()
		{
			boxes.RemoveAll((BoxGizmo box) => box == null);
			deadWarnings.Clear();
			foreach (KeyValuePair<VoxelEditorController, PieceWarningGizmo> pieceWarning in pieceWarnings)
			{
				if (pieceWarning.Key == null || pieceWarning.Value == null)
				{
					deadWarnings.Add(pieceWarning.Key);
				}
			}
			foreach (VoxelEditorController deadWarning in deadWarnings)
			{
				if (pieceWarnings.TryGetValue(deadWarning, out var value) && value != null)
				{
					Object.Destroy(value.gameObject);
				}
				pieceWarnings.Remove(deadWarning);
			}
		}

		private void Update()
		{
			PruneDestroyed();
			if (VoxelEditorSettings.IsMovementMode || !VoxelEditorSettings.BodyRulesApply)
			{
				HideAll();
				lastWarning = null;
				return;
			}
			VoxelBodyReport report = VoxelBodyRules.Evaluate(VoxelFocusManager.GatherBody(gathered, gatheredControllers));
			reference = FirstUsableTransform();
			HashSet<VoxelModel> hashSet = PlayerVoxelBody.LocallyWithheld();
			bool flag = hashSet != null && hashSet.Count > 0;
			bool flag2 = report.HasVoxels && !report.IsValid;
			if (reference == null || (!flag2 && !flag))
			{
				HideAll();
				lastWarning = null;
				return;
			}
			int num = 0;
			if (flag2 && report.BelowMin)
			{
				ShowBox(num++, VoxelEditorSettings.MinBoundExtent, report.Bounds.center, TooSmall);
			}
			if (flag2 && report.AboveMax)
			{
				ShowBox(num++, VoxelEditorSettings.MaxBoundExtent, report.Bounds.center, TooLarge);
			}
			for (int i = num; i < boxes.Count; i++)
			{
				boxes[i].Hide();
			}
			UpdateStrandedPieceWarnings(report, gatheredControllers, flag ? hashSet : null);
			Announce(flag2 ? BuildWarning(report) : Loc.Get("Warning.PieceInsideGeometry"));
		}

		private void UpdateStrandedPieceWarnings(VoxelBodyReport report, List<VoxelEditorController> controllers, HashSet<VoxelModel> withheld)
		{
			foreach (PieceWarningGizmo value in pieceWarnings.Values)
			{
				value.Hide();
			}
			Camera activeCamera = VoxelEditorSettings.ActiveCamera;
			if (report.HasInvalidPieces)
			{
				foreach (int item in InvalidPieceIndices(report))
				{
					if (item >= 0 && item < controllers.Count)
					{
						ShowPieceWarning(controllers[item], activeCamera);
					}
				}
			}
			if (withheld == null)
			{
				return;
			}
			foreach (VoxelEditorController controller in controllers)
			{
				if (controller != null && controller.Model != null && withheld.Contains(controller.Model))
				{
					ShowPieceWarning(controller, activeCamera);
				}
			}
		}

		private void ShowPieceWarning(VoxelEditorController controller, Camera cam)
		{
			if (!(controller == null))
			{
				if (!pieceWarnings.TryGetValue(controller, out var value) || value == null)
				{
					value = PieceWarningGizmo.Create(base.transform);
					pieceWarnings[controller] = value;
				}
				value.Show(controller, cam);
			}
		}

		private static IEnumerable<int> InvalidPieceIndices(VoxelBodyReport report)
		{
			if (report.StrandedPieces != null)
			{
				foreach (int strandedPiece in report.StrandedPieces)
				{
					yield return strandedPiece;
				}
			}
			if (report.OverlappingPieces != null)
			{
				foreach (int overlappingPiece in report.OverlappingPieces)
				{
					if (report.StrandedPieces == null || !report.StrandedPieces.Contains(overlappingPiece))
					{
						yield return overlappingPiece;
					}
				}
			}
			if (report.SmallPieces != null)
			{
				foreach (int smallPiece in report.SmallPieces)
				{
					if ((report.StrandedPieces == null || !report.StrandedPieces.Contains(smallPiece)) && (report.OverlappingPieces == null || !report.OverlappingPieces.Contains(smallPiece)))
					{
						yield return smallPiece;
					}
				}
			}
			if (report.FragmentedPieces == null)
			{
				yield break;
			}
			foreach (int fragmentedPiece in report.FragmentedPieces)
			{
				if ((report.StrandedPieces == null || !report.StrandedPieces.Contains(fragmentedPiece)) && (report.OverlappingPieces == null || !report.OverlappingPieces.Contains(fragmentedPiece)) && (report.SmallPieces == null || !report.SmallPieces.Contains(fragmentedPiece)))
				{
					yield return fragmentedPiece;
				}
			}
		}

		private static string BuildWarning(VoxelBodyReport report)
		{
			if (report.BelowMin)
			{
				return $"Minimum limit aşıldı. Model her eksende en az {VoxelEditorSettings.MinBoundExtent} " + $"birim olmalı; yalnızca bir eksen {VoxelEditorSettings.SlimBoundExtent} birime kadar inebilir.";
			}
			if (report.AboveMax)
			{
				return $"Maksimum limit aşıldı - model her eksende en fazla {VoxelEditorSettings.MaxBoundExtent} birim olabilir.";
			}
			if (report.OverlappingPieces != null && report.OverlappingPieces.Count > 0)
			{
				return "Geçersiz yerleşim - parçalar iç içe; bir parça başka bir parçanın içinde duramaz.";
			}
			if (report.SmallPieces != null && report.SmallPieces.Count > 0)
			{
				return $"Parça çok küçük. Her parça her eksende en az {VoxelEditorSettings.MinBoundExtent} " + $"birim olmalı; yalnızca bir eksen {VoxelEditorSettings.SlimBoundExtent} birime kadar inebilir.";
			}
			if (report.FragmentedPieces != null && report.FragmentedPieces.Count > 0)
			{
				return Loc.Get("Reject.ModelFragments");
			}
			return "Geçersiz yerleşim - bir parça modelin geri kalanından çok uzakta.";
		}

		private void Announce(string warning)
		{
			if (!(warning == lastWarning))
			{
				lastWarning = warning;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(warning);
				}
				AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.editDeniedClip);
			}
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

		private void ShowBox(int index, float extentVoxels, Vector3 centerVoxels, Color color)
		{
			while (boxes.Count <= index)
			{
				boxes.Add(BoxGizmo.Create(base.transform));
			}
			boxes[index].Show(reference, centerVoxels, Vector3.one * extentVoxels, color);
		}

		private void HideAll()
		{
			foreach (BoxGizmo box in boxes)
			{
				box.Hide();
			}
			foreach (PieceWarningGizmo value in pieceWarnings.Values)
			{
				value.Hide();
			}
		}
	}
}
