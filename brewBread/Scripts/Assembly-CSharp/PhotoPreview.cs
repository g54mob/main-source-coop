using System.Collections;
using Rewired;
using UnityEngine;

public class PhotoPreview : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private float _turnanimationTime;

	private float _animatorCooldown;

	private void Start()
	{
		if (StaticInstance<Pause>.Instance != null)
		{
			StaticInstance<Pause>.Instance?.SetPause(value: true);
			StaticInstance<Pause>.Instance.SetEnablePause(v: false);
		}
		StaticInstance<UIManager>.Instance.UIConfirm.AddListener(FlipPhoto);
		StaticInstance<UIManager>.Instance.UICancel.AddListener(ClosePhoto);
	}

	private void Update()
	{
		if (_animatorCooldown > 0f)
		{
			_animatorCooldown -= Time.unscaledDeltaTime;
		}
	}

	private void ClosePhoto(Controller cont)
	{
		_animator.SetTrigger("Close");
		StartCoroutine(DestroyPhoto());
	}

	private void FlipPhoto(Controller cont)
	{
		if (!(_animatorCooldown > 0f))
		{
			_animator.SetTrigger("Flip");
			_animatorCooldown = _turnanimationTime;
		}
	}

	private IEnumerator DestroyPhoto()
	{
		yield return new WaitForSecondsRealtime(0.5f);
		Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (StaticInstance<Pause>.Instance != null)
		{
			StaticInstance<Pause>.Instance.SetEnablePause(v: true);
			StaticInstance<Pause>.Instance?.SetPause(value: false);
		}
		if (StaticInstance<UIManager>.Instance != null)
		{
			StaticInstance<UIManager>.Instance.UIConfirm.RemoveListener(FlipPhoto);
			StaticInstance<UIManager>.Instance.UICancel.RemoveListener(ClosePhoto);
		}
	}
}
