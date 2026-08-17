using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Scripts
{
	public class UISoundEmitter : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
	{
		[Header("Sounds (leave empty to skip)")]
		[SerializeField]
		private SoundID hoverSound;

		[SerializeField]
		private SoundID clickSound;

		[SerializeField]
		private SoundID pressSound;

		[SerializeField]
		private SoundID exitSound;

		[Tooltip("Skip sounds when a Selectable on this object is non-interactable (e.g. a disabled Button).")]
		[SerializeField]
		private bool respectInteractable = true;

		[Inject]
		private IAudioManager _audioManager;

		private Selectable _selectable;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			TryGetComponent<Selectable>(out _selectable);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Play(hoverSound);
		}

		public void OnSelect(BaseEventData eventData)
		{
			Play(hoverSound);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Play(exitSound);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Play(pressSound);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Play(clickSound);
		}

		public void OnSubmit(BaseEventData eventData)
		{
			Play(clickSound);
		}

		private void Play(SoundID sound)
		{
			if (sound.IsValid() && (!respectInteractable || !(_selectable != null) || _selectable.interactable))
			{
				_audioManager?.PlayOneShotUI(sound);
			}
		}
	}
}
