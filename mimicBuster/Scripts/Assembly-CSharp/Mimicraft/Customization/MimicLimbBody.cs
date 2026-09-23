using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Customization
{
	public class MimicLimbBody : MonoBehaviour, ILimbBody
	{
		[Tooltip("Tezgahtaki ana model - MimicCustomizationView'ün Mimic alanındaki objenin VoxelModel'i. Boş bırakılırsa bu objenin altındaki ilk VoxelModel alınır. Parçalar bunun kardeşleridir.")]
		[SerializeField]
		private VoxelModel primary;

		private float appliedRunLift;

		private Quaternion appliedRunLean = Quaternion.identity;

		private Vector3 appliedRunLeanPivot;

		public int BodyVersion { get; private set; }

		public float CurrentRunLift => appliedRunLift;

		public Quaternion CurrentRunLean => appliedRunLean;

		public Vector3 CurrentRunLeanPivot => appliedRunLeanPivot;

		private VoxelModel Primary
		{
			get
			{
				if (primary == null)
				{
					primary = GetComponentInChildren<VoxelModel>(includeInactive: true);
				}
				return primary;
			}
		}

		public bool HasVoxelBody
		{
			get
			{
				if (Primary != null && Primary.gameObject.activeInHierarchy && Primary.Grid != null)
				{
					return Primary.Grid.Count > 0;
				}
				return false;
			}
		}

		public float BodySizeRatio
		{
			get
			{
				if (!HasVoxelBody || !TryGetBodyBoundsLocal(out var bounds))
				{
					return 1f;
				}
				float num = Mathf.Max(Primary.InitialWorldSize, 0.0001f);
				Vector3 size = bounds.size;
				return Mathf.Clamp(Mathf.Pow(Mathf.Max(size.x, 0.0001f) * Mathf.Max(size.y, 0.0001f) * Mathf.Max(size.z, 0.0001f), 1f / 3f) / num, 0.25f, 8f);
			}
		}

		private void OnEnable()
		{
			UndoManager.EditStepApplied += OnEditStep;
			MarkChanged();
		}

		private void OnDisable()
		{
			UndoManager.EditStepApplied -= OnEditStep;
		}

		private void OnEditStep(IUndoableCommand command, UndoManager.EditStepKind kind)
		{
			MarkChanged();
		}

		public void MarkChanged()
		{
			BodyVersion++;
		}

		public IEnumerable<VoxelModel> BodyPieces()
		{
			VoxelModel voxelModel = Primary;
			Transform bodyParent = ((voxelModel != null) ? voxelModel.transform.parent : null);
			VoxelModel[] componentsInChildren = GetComponentsInChildren<VoxelModel>(includeInactive: false);
			foreach (VoxelModel voxelModel2 in componentsInChildren)
			{
				if (bodyParent == null || voxelModel2.transform.parent == bodyParent)
				{
					yield return voxelModel2;
				}
			}
		}

		public bool TryGetBodyBoundsLocal(out Bounds bounds)
		{
			bounds = default(Bounds);
			if (!HasVoxelBody)
			{
				return false;
			}
			bool flag = false;
			Quaternion quaternion = Quaternion.Inverse(appliedRunLean);
			Vector3 vector = appliedRunLeanPivot;
			foreach (VoxelModel item in BodyPieces())
			{
				if (item.Grid == null || !GridBounds.TryCompute(item.Grid, out var min, out var max))
				{
					continue;
				}
				for (int i = 0; i < 8; i++)
				{
					Vector3 position = new Vector3(((i & 1) == 0) ? min.x : (max.x + 1), ((i & 2) == 0) ? min.y : (max.y + 1), ((i & 4) == 0) ? min.z : (max.z + 1));
					Vector3 vector2 = base.transform.InverseTransformPoint(item.transform.TransformPoint(position));
					vector2 = vector + quaternion * (vector2 - vector);
					if (!flag)
					{
						bounds = new Bounds(vector2, Vector3.zero);
						flag = true;
					}
					else
					{
						bounds.Encapsulate(vector2);
					}
				}
			}
			return flag;
		}

		public void SetRunLift(float lift)
		{
			if (Mathf.Approximately(lift, appliedRunLift))
			{
				return;
			}
			float num = lift - appliedRunLift;
			appliedRunLift = lift;
			Vector3 vector = appliedRunLean * (Vector3.up * num);
			foreach (VoxelModel item in BodyPieces())
			{
				item.transform.localPosition += vector;
			}
		}

		public void SetRunLean(Quaternion lean, Vector3 pivotLocal)
		{
			bool num = Quaternion.Angle(lean, appliedRunLean) < 0.01f;
			bool flag = (pivotLocal - appliedRunLeanPivot).sqrMagnitude < 1E-08f;
			if (num && (flag || Quaternion.Angle(lean, Quaternion.identity) < 0.01f))
			{
				return;
			}
			Quaternion quaternion = Quaternion.Inverse(appliedRunLean);
			foreach (VoxelModel item in BodyPieces())
			{
				Transform transform = item.transform;
				Vector3 vector = appliedRunLeanPivot + quaternion * (transform.localPosition - appliedRunLeanPivot);
				Quaternion quaternion2 = quaternion * transform.localRotation;
				transform.localPosition = pivotLocal + lean * (vector - pivotLocal);
				transform.localRotation = lean * quaternion2;
			}
			appliedRunLean = lean;
			appliedRunLeanPivot = pivotLocal;
		}
	}
}
