using UnityEngine;

public class PlatformAnimator : MonoBehaviour
{
	[SerializeField]
	private Animator _cartAnimator;

	[SerializeField]
	private Animator _wheelAnimator;

	private int _childCount;

	private bool _landed;

	private void Start()
	{
		_landed = false;
		_childCount = base.transform.childCount;
	}

	private void Update()
	{
		if (_childCount != base.transform.childCount)
		{
			if (_childCount > base.transform.childCount)
			{
				_childCount = base.transform.childCount;
				_cartAnimator.SetBool("Landed", value: false);
			}
			else if (_childCount < base.transform.childCount)
			{
				_childCount = base.transform.childCount;
				_landed = true;
				_cartAnimator.SetBool("Landed", value: true);
			}
		}
	}

	public void StartWeels(bool value)
	{
		_wheelAnimator.SetBool("Moving", value);
	}
}
