using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zorro.ControllerSupport;

public class IntroScreenAnimator : MonoBehaviour
{
	public InputActionReference m_skipAction;

	public Button m_hiddenSkipButton;

	public Graphic m_image;

	public Animator m_animator;

	public CanvasGroup m_canvasGroup;

	public AudioSource m_audioSource;

	public AudioSource m_ambience;

	public bool skipping;

	private static bool m_hasPlayed;

	private void Start()
	{
		if (m_hasPlayed)
		{
			base.gameObject.SetActive(value: false);
			m_ambience.volume = 0.1f;
		}
		else
		{
			m_hasPlayed = true;
		}
	}

	private void Skip()
	{
		skipping = true;
		m_animator.enabled = false;
	}

	private void Update()
	{
		if (m_image.color.a <= 0f && !m_audioSource.isPlaying)
		{
			Object.Destroy(base.gameObject);
		}
		if (skipping)
		{
			m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, 0f, Time.unscaledDeltaTime * 10f);
			m_audioSource.volume = Mathf.Lerp(m_audioSource.volume, 0f, Time.unscaledDeltaTime * 2f);
		}
		if (m_canvasGroup.alpha < 0.5f)
		{
			m_canvasGroup.blocksRaycasts = false;
			m_canvasGroup.interactable = false;
		}
		m_ambience.volume = (1f - m_canvasGroup.alpha) * 0.1f;
		bool num = InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad;
		bool flag = m_image.color.a > 0.5f && !skipping;
		if (num)
		{
			if (flag)
			{
				m_hiddenSkipButton.gameObject.SetActive(value: true);
				if (EventSystem.current.currentSelectedGameObject != m_hiddenSkipButton.gameObject)
				{
					EventSystem.current.SetSelectedGameObject(m_hiddenSkipButton.gameObject);
				}
			}
			else
			{
				m_hiddenSkipButton.gameObject.SetActive(value: false);
				if (EventSystem.current.currentSelectedGameObject == m_hiddenSkipButton.gameObject)
				{
					EventSystem.current.SetSelectedGameObject(null);
				}
			}
		}
		else
		{
			m_hiddenSkipButton.gameObject.SetActive(value: false);
			if (EventSystem.current.currentSelectedGameObject == m_hiddenSkipButton.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || m_skipAction.action.WasPressedThisFrame())
		{
			Skip();
		}
	}
}
