using System.Collections.Generic;
using Boxophobic.StyledGUI;
using UnityEngine;

namespace TheVisualEngine
{
	[HelpURL("https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.q4fstlrr3cw4")]
	[ExecuteInEditMode]
	[AddComponentMenu("BOXOPHOBIC/The Visual Engine/TVE Prefab")]
	[DefaultExecutionOrder(1)]
	public class TVEPrefab : StyledMonoBehaviour
	{
		public TVEPrefabBlending prefabBlending = new TVEPrefabBlending();

		private List<Renderer> prefabRenderers;

		private void OnEnable()
		{
			if (!(TVEManager.Instance == null) && prefabBlending.blendMode == 1)
			{
				GetPrefabRenderers();
				UpdatePrefabBlending();
			}
		}

		private void GetPrefabRenderers()
		{
			prefabRenderers = new List<Renderer>();
			List<GameObject> list = new List<GameObject>();
			list.Add(base.gameObject);
			TVEUtils.GetChildRecursive(base.gameObject, list);
			for (int i = 0; i < list.Count; i++)
			{
				GameObject gameObject = list[i];
				if (gameObject != null)
				{
					Renderer component = gameObject.GetComponent<Renderer>();
					prefabRenderers.Add(component);
				}
			}
		}

		private void UpdatePrefabBlending()
		{
			List<TVETerrain> sceneTerrains = TVEManager.Instance.sceneTerrains;
			for (int i = 0; i < sceneTerrains.Count; i++)
			{
				TVETerrain tVETerrain = sceneTerrains[i];
				if (tVETerrain == null)
				{
					continue;
				}
				Vector3 terrainPosition = tVETerrain.terrainPosition;
				Vector3 terrainSize = tVETerrain.terrainSize;
				Vector3 vector = terrainPosition + terrainSize;
				if (base.transform.position.x > terrainPosition.x && base.transform.position.z > terrainPosition.z && base.transform.position.x < vector.x && base.transform.position.z < vector.z)
				{
					for (int j = 0; j < prefabRenderers.Count; j++)
					{
						Renderer renderer = prefabRenderers[j];
						TVEUtils.CopyTerrainDataToRenderer(tVETerrain, renderer);
					}
				}
			}
		}
	}
}
