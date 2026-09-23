using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class LineHighlight : MonoBehaviour
	{
		private LineRenderer lineRenderer;

		public static LineHighlight Create(Transform parent, bool loop, float width)
		{
			GameObject obj = new GameObject("LineHighlight");
			obj.transform.SetParent(parent, worldPositionStays: false);
			LineHighlight lineHighlight = obj.AddComponent<LineHighlight>();
			lineHighlight.Init(loop, width);
			return lineHighlight;
		}

		private void Init(bool loop, float width)
		{
			lineRenderer = base.gameObject.AddComponent<LineRenderer>();
			lineRenderer.useWorldSpace = true;
			lineRenderer.loop = loop;
			lineRenderer.widthMultiplier = width;
			lineRenderer.numCapVertices = 4;
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				lineRenderer.material = new Material(shader);
			}
			base.gameObject.SetActive(value: false);
		}

		public void Show(Vector3[] worldPoints, Color color)
		{
			lineRenderer.positionCount = worldPoints.Length;
			lineRenderer.SetPositions(worldPoints);
			if (lineRenderer.material != null)
			{
				lineRenderer.material.color = color;
			}
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
