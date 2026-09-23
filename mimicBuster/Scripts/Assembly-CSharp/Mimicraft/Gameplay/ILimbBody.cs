using System.Collections.Generic;
using Mimicraft.VoxelEditor;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public interface ILimbBody
	{
		bool HasVoxelBody { get; }

		int BodyVersion { get; }

		float BodySizeRatio { get; }

		float CurrentRunLift { get; }

		Quaternion CurrentRunLean { get; }

		Vector3 CurrentRunLeanPivot { get; }

		bool TryGetBodyBoundsLocal(out Bounds bounds);

		IEnumerable<VoxelModel> BodyPieces();

		void SetRunLift(float lift);

		void SetRunLean(Quaternion lean, Vector3 pivotLocal);
	}
}
