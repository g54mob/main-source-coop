using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;

namespace Features.WorldTokenModule.Scripts.Views
{
	public abstract class WorldTokenViewBase : ViewBehaviour
	{
		[SerializeField]
		protected RectTransform _tokenVisual;

		[SerializeField]
		private TMP_Text _tokenText;

		public bool IsActive { get; set; }

		public Camera ActiveCamera { get; set; }

		private void LateUpdate()
		{
			if (!(ActiveCamera == null))
			{
				base.transform.rotation = ActiveCamera.transform.rotation;
			}
		}

		public void SetText(string text)
		{
			_tokenText.SetText(text);
		}

		public abstract void StartMovement();

		public abstract void StopMovement();
	}
}
