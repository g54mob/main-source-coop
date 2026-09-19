using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[RequireComponent(typeof(ObiPathSmoother))]
	public class ObiRopePrefabPlugger : MonoBehaviour
	{
		public GameObject prefab;

		public Vector3 instanceScale = Vector3.one;

		public bool plugTears = true;

		public bool plugStart;

		public bool plugEnd;

		private List<GameObject> instances;

		private ObiPathSmoother smoother;

		private void OnEnable()
		{
			instances = new List<GameObject>();
			smoother = GetComponent<ObiPathSmoother>();
			GetComponent<ObiActor>().OnInterpolate += UpdatePlugs;
		}

		private void OnDisable()
		{
			GetComponent<ObiActor>().OnInterpolate -= UpdatePlugs;
			ClearPrefabInstances();
		}

		private GameObject GetOrCreatePrefabInstance(int index)
		{
			if (index < instances.Count)
			{
				return instances[index];
			}
			GameObject gameObject = Object.Instantiate(prefab);
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			instances.Add(gameObject);
			return gameObject;
		}

		public void ClearPrefabInstances()
		{
			for (int i = 0; i < instances.Count; i++)
			{
				Object.DestroyImmediate(instances[i]);
			}
			instances.Clear();
		}

		private void UpdatePlugs(ObiActor actor, float simulatedTime, float substepTime)
		{
			if (!actor.isLoaded)
			{
				return;
			}
			Matrix4x4 localToWorldMatrix = smoother.actor.solver.transform.localToWorldMatrix;
			Quaternion rotation = localToWorldMatrix.rotation;
			int num = 0;
			ObiPathSmootherRenderSystem obiPathSmootherRenderSystem = actor.solver.GetRenderSystem<ObiPathSmoother>() as ObiPathSmootherRenderSystem;
			int chunkCount = obiPathSmootherRenderSystem.GetChunkCount(smoother.indexInSystem);
			for (int i = 0; i < chunkCount; i++)
			{
				if ((plugTears && i > 0) || (plugStart && i == 0))
				{
					GameObject orCreatePrefabInstance = GetOrCreatePrefabInstance(num++);
					orCreatePrefabInstance.SetActive(value: true);
					ObiPathFrame frameAt = obiPathSmootherRenderSystem.GetFrameAt(smoother.indexInSystem, i, 0);
					orCreatePrefabInstance.transform.position = localToWorldMatrix.MultiplyPoint3x4(frameAt.position);
					orCreatePrefabInstance.transform.rotation = rotation * Quaternion.LookRotation(-frameAt.tangent, frameAt.binormal);
					orCreatePrefabInstance.transform.localScale = instanceScale;
				}
				if ((plugTears && i < chunkCount - 1) || (plugEnd && i == chunkCount - 1))
				{
					GameObject orCreatePrefabInstance2 = GetOrCreatePrefabInstance(num++);
					orCreatePrefabInstance2.SetActive(value: true);
					ObiPathFrame obiPathFrame = obiPathSmootherRenderSystem.GetFrameAt(frameIndex: obiPathSmootherRenderSystem.GetSmoothFrameCount(smoother.indexInSystem, i) - 1, rendererIndex: smoother.indexInSystem, chunkIndex: i);
					orCreatePrefabInstance2.transform.position = localToWorldMatrix.MultiplyPoint3x4(obiPathFrame.position);
					orCreatePrefabInstance2.transform.rotation = rotation * Quaternion.LookRotation(obiPathFrame.tangent, obiPathFrame.binormal);
					orCreatePrefabInstance2.transform.localScale = instanceScale;
				}
			}
			for (int j = num; j < instances.Count; j++)
			{
				instances[j].SetActive(value: false);
			}
		}
	}
}
