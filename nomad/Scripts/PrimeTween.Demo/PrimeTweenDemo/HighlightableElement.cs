using UnityEngine;

namespace PrimeTweenDemo
{
	public class HighlightableElement : MonoBehaviour
	{
		[SerializeField]
		public Transform highlightAnchor;

		public MeshRenderer[] models { get; private set; }

		private void OnEnable()
		{
			models = GetComponentsInChildren<MeshRenderer>();
			MeshRenderer[] array = models;
			foreach (MeshRenderer obj in array)
			{
				obj.sharedMaterial = new Material(obj.sharedMaterial);
			}
		}
	}
}
