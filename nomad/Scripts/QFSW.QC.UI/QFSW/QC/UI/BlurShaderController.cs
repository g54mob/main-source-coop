using UnityEngine;

namespace QFSW.QC.UI
{
	[ExecuteInEditMode]
	public class BlurShaderController : MonoBehaviour
	{
		[SerializeField]
		private Material _blurMaterial;

		[SerializeField]
		private float _blurRadius = 1f;

		[SerializeField]
		private Vector2 _referenceResolution = new Vector2(1920f, 1080f);

		private void LateUpdate()
		{
			if ((bool)_blurMaterial)
			{
				float value = new Vector2(Screen.width, Screen.height).y / _referenceResolution.y;
				_blurMaterial.SetFloat("_Radius", _blurRadius);
				_blurMaterial.SetFloat("_BlurMultiplier", value);
			}
		}
	}
}
