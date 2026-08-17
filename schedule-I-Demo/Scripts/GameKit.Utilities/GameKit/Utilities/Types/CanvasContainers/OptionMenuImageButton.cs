using UnityEngine;
using UnityEngine.UI;

namespace GameKit.Utilities.Types.CanvasContainers
{
	public class OptionMenuImageButton : OptionMenuButton
	{
		[Tooltip("Image component to show image on.")]
		[SerializeField]
		private Image _image;

		public virtual void Initialize(ImageButtonData buttonData)
		{
			base.Initialize(buttonData);
			_image.sprite = buttonData.DisplayImage;
		}
	}
}
