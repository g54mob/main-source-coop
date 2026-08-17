using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Extensions;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Scripts
{
	public class UISlidePanelTransition : MonoBehaviour
	{
		[Header("Panels")]
		[Tooltip("The panel that is visible first and slides away (e.g. the main menu side panel).")]
		[SerializeField]
		private RectTransform sidePanel;

		[Tooltip("The panel that slides in (e.g. the Settings panel).")]
		[SerializeField]
		private RectTransform contentPanel;

		[Header("Edges")]
		[SerializeField]
		private SlideEdge contentEntersFrom = SlideEdge.Right;

		[SerializeField]
		private SlideEdge sideExitsTo;

		[Header("Open (forward)")]
		[SerializeField]
		private float openDuration = 0.35f;

		[SerializeField]
		private Ease openEase = Ease.OutCubic;

		[Tooltip("Delay before the side panel starts exiting. 0 = together, openDuration = strictly after content arrives.")]
		[SerializeField]
		[Min(0f)]
		private float sideExitDelay = 0.1f;

		[Header("Close (back)")]
		[SerializeField]
		private float closeDuration = 0.2f;

		[SerializeField]
		private Ease closeEase = Ease.InCubic;

		[SerializeField]
		[Min(0f)]
		private float sideReturnDelay;

		[Header("Off-screen distance")]
		[Tooltip("Slide a full canvas width/height off-screen. Disable to use a fixed customOffset.")]
		[SerializeField]
		private bool useCanvasSizeForOffset = true;

		[SerializeField]
		private float customOffset = 1200f;

		[Header("Optional auto-wiring")]
		[Tooltip("Leave empty when a manager drives ShowContent/ShowSide directly.")]
		[SerializeField]
		private Button openButton;

		[SerializeField]
		private Button closeButton;

		[Header("Startup")]
		[Tooltip("Place the content panel off-screen and hidden on Awake.")]
		[SerializeField]
		private bool startWithContentHidden = true;

		[Header("Sound (swoosh, optional)")]
		[SerializeField]
		private SoundID openSound;

		[SerializeField]
		private SoundID closeSound;

		[Inject]
		private IAudioManager _audioManager;

		private CanvasGroup _sideGroup;

		private CanvasGroup _contentGroup;

		private Vector2 _sideHome;

		private Vector2 _contentHome;

		private Vector2 _canvasSize;

		private Sequence _sequence;

		private bool _cached;

		private bool _contentShown;

		public bool IsContentShown => _contentShown;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			Cache();
			if (openButton != null)
			{
				openButton.onClick.AddListener(ShowContent);
			}
			if (closeButton != null)
			{
				closeButton.onClick.AddListener(ShowSide);
			}
			if (startWithContentHidden)
			{
				ApplyHiddenContentState();
			}
		}

		private void OnDestroy()
		{
			if (openButton != null)
			{
				openButton.onClick.RemoveListener(ShowContent);
			}
			if (closeButton != null)
			{
				closeButton.onClick.RemoveListener(ShowSide);
			}
		}

		private void Cache()
		{
			if (!_cached)
			{
				if (sidePanel != null)
				{
					_sideGroup = sidePanel.GetComponent<CanvasGroup>();
					_sideHome = sidePanel.anchoredPosition;
				}
				if (contentPanel != null)
				{
					_contentGroup = contentPanel.GetComponent<CanvasGroup>();
					_contentHome = contentPanel.anchoredPosition;
				}
				_canvasSize = ResolveCanvasSize();
				_cached = true;
			}
		}

		public void ShowContent()
		{
			if (sidePanel == null || contentPanel == null)
			{
				return;
			}
			Cache();
			if (!_contentShown)
			{
				_contentShown = true;
				PlaySound(openSound);
				_sequence.Stop();
				SetAlpha(_contentGroup, 1f);
				SetInteractable(_contentGroup, on: true);
				SetInteractable(_sideGroup, on: false);
				Vector2 endValue = _sideHome + EdgeOffset(sideExitsTo);
				_sequence = Sequence.Create(1, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true).Chain(Tween.UIAnchoredPosition(contentPanel, _contentHome, openDuration, openEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true)).Group(Tween.UIAnchoredPosition(sidePanel, endValue, openDuration, openEase, 1, CycleMode.Restart, sideExitDelay, 0f, useUnscaledTime: true))
					.ChainCallback(this, delegate(UISlidePanelTransition t)
					{
						SetAlpha(t._sideGroup, 0f);
					});
			}
		}

		public void ShowSide()
		{
			ShowSide(force: false);
		}

		public bool ShowSide(bool force)
		{
			if (sidePanel == null || contentPanel == null)
			{
				return false;
			}
			Cache();
			if (!force && !_contentShown)
			{
				return false;
			}
			_contentShown = false;
			PlaySound(closeSound);
			_sequence.Stop();
			SetAlpha(_sideGroup, 1f);
			SetInteractable(_sideGroup, on: true);
			SetInteractable(_contentGroup, on: false);
			Vector2 endValue = _contentHome + EdgeOffset(contentEntersFrom);
			_sequence = Sequence.Create(1, Sequence.SequenceCycleMode.Restart, Ease.Linear, useUnscaledTime: true).Chain(Tween.UIAnchoredPosition(contentPanel, endValue, closeDuration, closeEase, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true)).Group(Tween.UIAnchoredPosition(sidePanel, _sideHome, closeDuration, closeEase, 1, CycleMode.Restart, sideReturnDelay, 0f, useUnscaledTime: true))
				.ChainCallback(this, delegate(UISlidePanelTransition t)
				{
					SetAlpha(t._contentGroup, 0f);
				});
			return true;
		}

		public void SnapToSide()
		{
			Cache();
			_sequence.Stop();
			if (sidePanel != null)
			{
				sidePanel.anchoredPosition = _sideHome;
			}
			ApplyHiddenContentState();
		}

		private void ApplyHiddenContentState()
		{
			if (contentPanel != null)
			{
				contentPanel.anchoredPosition = _contentHome + EdgeOffset(contentEntersFrom);
			}
			SetAlpha(_contentGroup, 0f);
			SetInteractable(_contentGroup, on: false);
			_contentShown = false;
		}

		private Vector2 ResolveCanvasSize()
		{
			if (!useCanvasSizeForOffset)
			{
				return new Vector2(customOffset, customOffset);
			}
			RectTransform rectTransform = ((contentPanel != null) ? contentPanel : sidePanel);
			Canvas canvas = ((rectTransform != null) ? rectTransform.GetComponentInParent<Canvas>() : null);
			Canvas canvas2 = ((canvas != null) ? canvas.rootCanvas : null);
			if (canvas2 != null && canvas2.transform is RectTransform { rect: var rect })
			{
				return rect.size;
			}
			return new Vector2(Screen.width, Screen.height);
		}

		private Vector2 EdgeOffset(SlideEdge edge)
		{
			return edge switch
			{
				SlideEdge.Left => new Vector2(0f - _canvasSize.x, 0f), 
				SlideEdge.Right => new Vector2(_canvasSize.x, 0f), 
				SlideEdge.Top => new Vector2(0f, _canvasSize.y), 
				SlideEdge.Bottom => new Vector2(0f, 0f - _canvasSize.y), 
				_ => Vector2.zero, 
			};
		}

		private static void SetAlpha(CanvasGroup group, float alpha)
		{
			if (group != null)
			{
				group.alpha = alpha;
			}
		}

		private static void SetInteractable(CanvasGroup group, bool on)
		{
			if (!(group == null))
			{
				group.interactable = on;
				group.blocksRaycasts = on;
			}
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				_audioManager?.PlayOneShotUI(sound);
			}
		}
	}
}
