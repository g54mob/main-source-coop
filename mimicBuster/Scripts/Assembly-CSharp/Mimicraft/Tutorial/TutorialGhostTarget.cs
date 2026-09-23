using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Tutorial
{
	public class TutorialGhostTarget : MonoBehaviour
	{
		private static readonly Color MissingColor = new Color(0.35f, 0.95f, 0.55f, 0.35f);

		private static readonly Color ExtraColor = new Color(1f, 0.3f, 0.3f, 0.4f);

		private VoxelModel model;

		private VoxelGrid target;

		private readonly VoxelGrid missing = new VoxelGrid();

		private readonly VoxelGrid extra = new VoxelGrid();

		private MeshFilter missingFilter;

		private MeshFilter extraFilter;

		private MeshRenderer missingRenderer;

		private MeshRenderer extraRenderer;

		private Material missingMaterial;

		private Material extraMaterial;

		private VoxelGrid seenGrid;

		private int seenVersion = -1;

		private bool dirty;

		public int MissingCount { get; private set; }

		public int ExtraCount { get; private set; }

		public int TargetCount { get; private set; }

		public bool HasTarget => target != null;

		public bool Satisfied
		{
			get
			{
				if (target != null && MissingCount == 0)
				{
					return ExtraCount == 0;
				}
				return false;
			}
		}

		public static TutorialGhostTarget Create(VoxelModel model)
		{
			GameObject obj = new GameObject("TutorialGhostTarget");
			obj.transform.SetParent(model.transform, worldPositionStays: false);
			TutorialGhostTarget tutorialGhostTarget = obj.AddComponent<TutorialGhostTarget>();
			tutorialGhostTarget.model = model;
			tutorialGhostTarget.Build(out tutorialGhostTarget.missingFilter, out tutorialGhostTarget.missingRenderer, out tutorialGhostTarget.missingMaterial, "Missing", MissingColor);
			tutorialGhostTarget.Build(out tutorialGhostTarget.extraFilter, out tutorialGhostTarget.extraRenderer, out tutorialGhostTarget.extraMaterial, "Extra", ExtraColor);
			return tutorialGhostTarget;
		}

		private void Build(out MeshFilter filter, out MeshRenderer renderer, out Material material, string name, Color color)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			filter = gameObject.AddComponent<MeshFilter>();
			renderer = gameObject.AddComponent<MeshRenderer>();
			renderer.shadowCastingMode = ShadowCastingMode.Off;
			renderer.receiveShadows = false;
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			material = ((shader != null) ? new Material(shader) : null);
			if (material != null)
			{
				material.SetColor("_Color", color);
				renderer.sharedMaterial = material;
			}
			renderer.enabled = false;
		}

		public void SetTarget(VoxelGrid wanted)
		{
			target = wanted;
			TargetCount = wanted?.Count ?? 0;
			dirty = true;
			if (wanted == null)
			{
				int missingCount = (ExtraCount = 0);
				MissingCount = missingCount;
				missingRenderer.enabled = false;
				extraRenderer.enabled = false;
			}
		}

		public void Clear()
		{
			SetTarget(null);
		}

		private void LateUpdate()
		{
			if (target != null && !(model == null))
			{
				if (dirty || model.Grid != seenGrid || model.Grid.Version != seenVersion)
				{
					Recompute();
				}
				float pulse = 0.65f + 0.35f * Mathf.Sin(Time.unscaledTime * 2.4f);
				Tint(missingMaterial, MissingColor, pulse);
				Tint(extraMaterial, ExtraColor, pulse);
			}
		}

		private static void Tint(Material material, Color color, float pulse)
		{
			if (!(material == null))
			{
				color.a *= pulse;
				material.SetColor("_Color", color);
			}
		}

		private void Recompute()
		{
			dirty = false;
			seenGrid = model.Grid;
			seenVersion = model.Grid.Version;
			missing.Clear();
			extra.Clear();
			VoxelGrid grid = model.Grid;
			VoxelData data = new VoxelData(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			foreach (Vector3Int position in target.Positions)
			{
				if (!grid.Contains(position))
				{
					missing.Set(position, data);
				}
			}
			foreach (Vector3Int position2 in grid.Positions)
			{
				if (!target.Contains(position2))
				{
					extra.Set(position2, data);
				}
			}
			MissingCount = missing.Count;
			ExtraCount = extra.Count;
			Rebuild(missingFilter, missingRenderer, missing);
			Rebuild(extraFilter, extraRenderer, extra);
		}

		private static void Rebuild(MeshFilter filter, MeshRenderer renderer, VoxelGrid grid)
		{
			if (filter.sharedMesh != null)
			{
				Object.Destroy(filter.sharedMesh);
			}
			if (grid.Count == 0)
			{
				filter.sharedMesh = null;
				renderer.enabled = false;
			}
			else
			{
				filter.sharedMesh = VoxelMeshBuilder.BuildMesh(grid);
				renderer.enabled = true;
			}
		}

		private void OnDestroy()
		{
			if (missingFilter != null && missingFilter.sharedMesh != null)
			{
				Object.Destroy(missingFilter.sharedMesh);
			}
			if (extraFilter != null && extraFilter.sharedMesh != null)
			{
				Object.Destroy(extraFilter.sharedMesh);
			}
			if (missingMaterial != null)
			{
				Object.Destroy(missingMaterial);
			}
			if (extraMaterial != null)
			{
				Object.Destroy(extraMaterial);
			}
		}
	}
}
