using UnityEngine;

namespace Obi
{
	public class SkeletonDataBatch
	{
		public struct SkeletonData
		{
			public int firstBone;

			public int boneCount;
		}

		public ObiNativeList<SkeletonData> skeletonData;

		public ObiNativeList<Matrix4x4> world2Solver;

		public ObiNativeList<Vector3> bonePositions;

		public ObiNativeList<Quaternion> boneRotations;

		public ObiNativeList<Vector3> boneScales;

		public int Count => skeletonData.count;

		public SkeletonDataBatch()
		{
			skeletonData = new ObiNativeList<SkeletonData>();
			world2Solver = new ObiNativeList<Matrix4x4>();
			bonePositions = new ObiNativeList<Vector3>();
			boneRotations = new ObiNativeList<Quaternion>();
			boneScales = new ObiNativeList<Vector3>();
		}

		public void Dispose()
		{
			skeletonData.Dispose();
			world2Solver.Dispose();
			bonePositions.Dispose();
			boneRotations.Dispose();
			boneScales.Dispose();
		}

		public void Clear()
		{
			skeletonData.Clear();
			world2Solver.Clear();
			bonePositions.Clear();
			boneRotations.Clear();
			boneScales.Clear();
		}

		public int AddSkeleton(Transform[] bones, Matrix4x4 worldToSolver)
		{
			SkeletonData item = new SkeletonData
			{
				firstBone = bonePositions.count
			};
			foreach (Transform transform in bones)
			{
				if (transform != null)
				{
					bonePositions.Add(transform.position);
					boneRotations.Add(transform.rotation);
					boneScales.Add(transform.localScale);
				}
			}
			item.boneCount = bonePositions.count;
			skeletonData.Add(item);
			world2Solver.Add(worldToSolver);
			return skeletonData.count - 1;
		}

		public void PrepareForCompute()
		{
			skeletonData.SafeAsComputeBuffer<SkeletonData>();
			world2Solver.SafeAsComputeBuffer<Matrix4x4>();
			bonePositions.SafeAsComputeBuffer<Vector3>();
			boneRotations.SafeAsComputeBuffer<Quaternion>();
			boneScales.SafeAsComputeBuffer<Vector3>();
		}

		public void SetBoneTransform(int index, int boneIndex, Transform transform)
		{
			if (transform != null)
			{
				int index2 = skeletonData[index].firstBone + boneIndex;
				bonePositions[index2] = transform.position;
				boneScales[index2] = transform.lossyScale;
				boneRotations[index2] = transform.rotation;
			}
		}

		public void UpdateBoneTransformsCompute()
		{
			bonePositions.Upload();
			boneScales.Upload();
			boneRotations.Upload();
		}

		public Matrix4x4 GetWorldToSolverTransform(int index)
		{
			return world2Solver[index];
		}

		public void SetWorldToSolverTransform(int index, Matrix4x4 trfm)
		{
			world2Solver[index] = trfm;
		}
	}
}
