using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.Scripts
{
	public class CrosshairPanel : MonoBehaviour, IInitialize
	{
		private bool _isActive = true;

		[field: SerializeField]
		public Image CrosshairImage { get; set; }

		[field: SerializeField]
		public Sprite DefaultCrosshairSprite { get; set; }

		[field: SerializeField]
		public Sprite StartGrabbingSprite { get; set; }

		[field: SerializeField]
		public Sprite OnGrabbingSprite { get; set; }

		[field: SerializeField]
		public Sprite OnInteractSprite { get; set; }

		[field: SerializeField]
		public Sprite OnInspectSprite { get; set; }

		[field: SerializeField]
		public Sprite InvalidSprite { get; set; }

		public bool IsInitialized { get; set; }

		private void Awake()
		{
			Init();
		}

		public void Init()
		{
			Set(CrosshairType.Default);
			IsInitialized = true;
		}

		public void Set(CrosshairType type)
		{
			if (_isActive)
			{
				CrosshairImage.enabled = true;
				switch (type)
				{
				case CrosshairType.Default:
					CrosshairImage.sprite = DefaultCrosshairSprite;
					break;
				case CrosshairType.StartGrab:
					CrosshairImage.sprite = StartGrabbingSprite;
					break;
				case CrosshairType.OnGrab:
					CrosshairImage.sprite = OnGrabbingSprite;
					break;
				case CrosshairType.Interact:
					CrosshairImage.sprite = OnInteractSprite;
					break;
				case CrosshairType.Inspect:
					CrosshairImage.sprite = OnInspectSprite;
					break;
				case CrosshairType.Invalid:
					CrosshairImage.sprite = InvalidSprite;
					break;
				}
			}
		}

		public void Activate()
		{
			_isActive = true;
			CrosshairImage.enabled = true;
		}

		public void Deactivate()
		{
			_isActive = false;
			CrosshairImage.enabled = false;
		}

		public void SetPosition(Vector3 targetPos)
		{
			CrosshairImage.rectTransform.anchoredPosition = targetPos;
		}

		public void Reset()
		{
			Set(CrosshairType.Default);
			CrosshairImage.rectTransform.anchoredPosition = Vector3.zero;
		}
	}
}
