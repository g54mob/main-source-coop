using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace solidocean
{
	public class InspectManager : MonoBehaviour
	{
		public enum Types
		{
			Text = 0,
			TextAndImage = 1
		}

		[Header("Inspect Type")]
		public Types types;

		[Header("Texts")]
		public string Title;

		[TextArea]
		public string Description;

		[Header("Images")]
		public Sprite image;

		[Header("References")]
		public GameObject Inspector;

		[HideInInspector]
		public TextMeshProUGUI _Title;

		[HideInInspector]
		public TextMeshProUGUI _Description;

		[HideInInspector]
		public Image _image;

		public void ActiveInspector()
		{
			Inspector.SetActive(value: true);
			_Title.text = Title;
			_Description.text = Description;
			_image.sprite = image;
			if (types == Types.Text)
			{
				_Title.gameObject.SetActive(value: true);
				_Description.gameObject.SetActive(value: true);
				_image.gameObject.SetActive(value: false);
			}
			else if (types == Types.TextAndImage)
			{
				_Title.gameObject.SetActive(value: true);
				_Description.gameObject.SetActive(value: true);
				_image.gameObject.SetActive(value: true);
			}
		}

		public void DeactiveInspector()
		{
			Inspector.SetActive(value: false);
		}
	}
}
