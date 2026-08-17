using UnityEngine;

public class AnimationCTRL : MonoBehaviour
{
	private CharacterCTRL _characterCTRL;

	private Animator _animator;

	private PlayerState _previusState;

	public SpriteRenderer renderer;

	private void Start()
	{
		_characterCTRL = GetComponent<CharacterCTRL>();
		_animator = GetComponent<Animator>();
		_previusState = PlayerState.IDLE;
	}

	private void Update()
	{
		SetAnimation();
		if (_previusState == PlayerState.HANGING)
		{
			float z = Mathf.Atan2(_characterCTRL.g_companion.transform.position.y - base.transform.position.y, _characterCTRL.g_companion.transform.position.x - base.transform.position.x) * 57.29578f;
			renderer.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, z);
			renderer.flipX = false;
		}
		else
		{
			renderer.flipY = false;
			renderer.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		if (_characterCTRL.GetMovement() < 0f)
		{
			if (_characterCTRL._playerState != PlayerState.HANGING)
			{
				renderer.flipX = true;
			}
			else
			{
				renderer.flipY = true;
			}
		}
		else if (_characterCTRL.GetMovement() > 0f)
		{
			if (_characterCTRL._playerState != PlayerState.HANGING)
			{
				renderer.flipX = false;
			}
			else
			{
				renderer.flipY = false;
			}
		}
	}

	private void SetAnimation()
	{
		if (_characterCTRL._playerState != _previusState)
		{
			ResetAnimations();
			_previusState = _characterCTRL._playerState;
			switch (_characterCTRL._playerState)
			{
			case PlayerState.WALK:
				_animator.SetBool("Walk", value: true);
				break;
			case PlayerState.JUMPING:
				_animator.SetBool("Jump", value: true);
				break;
			case PlayerState.HANGING:
				_animator.SetBool("Hang", value: true);
				break;
			case PlayerState.FALLING:
				_animator.SetBool("Fall", value: true);
				break;
			case PlayerState.WEIGHT:
				_animator.SetBool("Weight", value: true);
				break;
			case PlayerState.ATTACHED:
				_animator.SetBool("Hang", value: true);
				break;
			case PlayerState.IDLE:
				break;
			}
		}
	}

	private void ResetAnimations()
	{
		_animator.SetBool("Walk", value: false);
		_animator.SetBool("Hang", value: false);
		_animator.SetBool("Weight", value: false);
		_animator.SetBool("Jump", value: false);
		_animator.SetBool("Fall", value: false);
	}
}
