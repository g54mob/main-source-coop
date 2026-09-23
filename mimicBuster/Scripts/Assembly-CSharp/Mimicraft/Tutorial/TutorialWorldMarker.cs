using System;
using Mimicraft.VoxelEditor;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Tutorial
{
	public class TutorialWorldMarker : MonoBehaviour
	{
		private const int RingSegments = 40;

		private static readonly Color RingColor = new Color(1f, 0.75f, 0.25f, 0.6f);

		private static readonly Color StarColor = new Color(1f, 0.85f, 0.3f, 1f);

		private MeshFilter ringFilter;

		private MeshRenderer ringRenderer;

		private Material ringMaterial;

		private Mesh ringMesh;

		private TextMeshPro star;

		private GameObject ringPrefab;

		private GameObject ringInstance;

		private Vector3 starPosition;

		public Vector3 RingPosition { get; private set; }

		public bool RingShown
		{
			get
			{
				if (!(ringRenderer != null) || !ringRenderer.enabled)
				{
					if (ringInstance != null)
					{
						return ringInstance.activeSelf;
					}
					return false;
				}
				return true;
			}
		}

		public static TutorialWorldMarker Create()
		{
			GameObject gameObject = new GameObject("TutorialWorldMarker");
			TutorialWorldMarker tutorialWorldMarker = gameObject.AddComponent<TutorialWorldMarker>();
			GameObject gameObject2 = new GameObject("Ring");
			gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
			tutorialWorldMarker.ringFilter = gameObject2.AddComponent<MeshFilter>();
			tutorialWorldMarker.ringRenderer = gameObject2.AddComponent<MeshRenderer>();
			tutorialWorldMarker.ringRenderer.shadowCastingMode = ShadowCastingMode.Off;
			tutorialWorldMarker.ringRenderer.receiveShadows = false;
			tutorialWorldMarker.ringRenderer.enabled = false;
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				tutorialWorldMarker.ringMaterial = new Material(shader);
				tutorialWorldMarker.ringMaterial.SetColor("_Color", RingColor);
				tutorialWorldMarker.ringRenderer.sharedMaterial = tutorialWorldMarker.ringMaterial;
			}
			tutorialWorldMarker.ringMesh = new Mesh
			{
				name = "TutorialRing"
			};
			tutorialWorldMarker.ringFilter.sharedMesh = tutorialWorldMarker.ringMesh;
			tutorialWorldMarker.star = WorldLabel.Create(gameObject.transform, "Star", 5f, StarColor);
			tutorialWorldMarker.star.text = "★";
			tutorialWorldMarker.star.gameObject.SetActive(value: false);
			return tutorialWorldMarker;
		}

		public void ShowRing(Vector3 worldPosition, float radius, GameObject prefab = null, float yaw = 0f)
		{
			RingPosition = worldPosition;
			if (prefab != null)
			{
				if (ringInstance == null || ringPrefab != prefab)
				{
					if (ringInstance != null)
					{
						UnityEngine.Object.Destroy(ringInstance);
					}
					ringInstance = UnityEngine.Object.Instantiate(prefab, base.transform);
					ringInstance.name = "Ring (prefab)";
					ringPrefab = prefab;
				}
				ringInstance.transform.SetPositionAndRotation(worldPosition, Quaternion.Euler(0f, yaw, 0f));
				ringInstance.SetActive(value: true);
				ringRenderer.enabled = false;
			}
			else
			{
				if (ringInstance != null)
				{
					ringInstance.SetActive(value: false);
				}
				BuildRing(radius);
				ringFilter.transform.position = worldPosition + Vector3.up * 0.01f;
				ringRenderer.enabled = true;
			}
		}

		public void ShowStar(Vector3 worldPosition)
		{
			starPosition = worldPosition;
			star.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			ringRenderer.enabled = false;
			if (ringInstance != null)
			{
				ringInstance.SetActive(value: false);
			}
			star.gameObject.SetActive(value: false);
		}

		private void BuildRing(float radius)
		{
			float num = radius * 0.78f;
			Vector3[] array = new Vector3[80];
			Vector3[] array2 = new Vector3[80];
			int[] array3 = new int[240];
			for (int i = 0; i < 40; i++)
			{
				float f = (float)i / 40f * MathF.PI * 2f;
				Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
				array[i * 2] = vector * num;
				array[i * 2 + 1] = vector * radius;
				array2[i * 2] = Vector3.up;
				array2[i * 2 + 1] = Vector3.up;
				int num2 = (i + 1) % 40;
				int num3 = i * 6;
				array3[num3] = i * 2;
				array3[num3 + 1] = num2 * 2;
				array3[num3 + 2] = i * 2 + 1;
				array3[num3 + 3] = i * 2 + 1;
				array3[num3 + 4] = num2 * 2;
				array3[num3 + 5] = num2 * 2 + 1;
			}
			ringMesh.Clear();
			ringMesh.vertices = array;
			ringMesh.normals = array2;
			ringMesh.triangles = array3;
			ringMesh.RecalculateBounds();
		}

		private void LateUpdate()
		{
			float num = 0.6f + 0.4f * Mathf.Sin(Time.unscaledTime * 2.4f);
			if (ringMaterial != null && ringRenderer.enabled)
			{
				Color ringColor = RingColor;
				ringColor.a *= num;
				ringMaterial.SetColor("_Color", ringColor);
			}
			if (star.gameObject.activeSelf)
			{
				Camera activeCamera = VoxelEditorSettings.ActiveCamera;
				Vector3 worldPosition = starPosition + Vector3.up * (0.05f * Mathf.Sin(Time.unscaledTime * 1.7f));
				WorldLabel.Place(star, worldPosition, WorldLabel.CameraPosition(activeCamera, starPosition), 1f);
			}
		}

		private void OnDestroy()
		{
			if (ringMesh != null)
			{
				UnityEngine.Object.Destroy(ringMesh);
			}
			if (ringMaterial != null)
			{
				UnityEngine.Object.Destroy(ringMaterial);
			}
		}
	}
}
