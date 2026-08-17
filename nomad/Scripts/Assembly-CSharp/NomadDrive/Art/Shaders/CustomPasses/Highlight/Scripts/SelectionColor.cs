using UnityEngine;

namespace NomadDrive.Art.Shaders.CustomPasses.Highlight.Scripts
{
	[RequireComponent(typeof(Renderer))]
	public class SelectionColor : MonoBehaviour
	{
		public Color selectionColor = new Color(1f, 0.5f, 0f, 1f);

		private void Start()
		{
			SetColor();
		}

		private void OnValidate()
		{
			SetColor();
		}

		private void SetColor()
		{
			Renderer component = GetComponent<Renderer>();
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			component.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetColor("_SelectionColor", selectionColor);
			component.SetPropertyBlock(materialPropertyBlock);
		}
	}
}
