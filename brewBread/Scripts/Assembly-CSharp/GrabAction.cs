using UnityEngine;

[CreateAssetMenu(fileName = "PenguinGrab", menuName = "Actions/Grab", order = 1)]
public class GrabAction : PenguinAction
{
	[SerializeField]
	protected float _grabTimer;

	[SerializeField]
	private ParticleSystem _sweatParticlesPrefab;

	[SerializeField]
	protected float _grabRange;

	[SerializeField]
	private AudioObjectManager _audioObjectManager;

	private EffectPlayer _effectPlayer;

	private ParticleSystem _sweatParticles;

	protected Countdown _countdown;

	public float GetTimer()
	{
		return _countdown.CurrentTime;
	}

	public float GetPercentileAnimation()
	{
		return 1f - _countdown.CurrentTime / _grabTimer;
	}

	protected void RestartTimer()
	{
		StaticInstance<TwoWayCountdown>.Instance.RestartTimer(_countdown);
	}

	public override void Initialize()
	{
		base.Initialize();
		Controller.OnGrounded.AddListener(RestartTimer);
		_sweatParticles = Object.Instantiate(_sweatParticlesPrefab, _controllerTransform);
		_countdown = null;
		_effectPlayer = Controller.gameObject.GetComponent<EffectPlayer>();
	}

	public override void Enter()
	{
		base.Enter();
		RaycastHit2D raycastHit2D = Physics2D.Raycast(_controllerTransform.position, Vector2.left, _grabRange, Controller.GroundLayer);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(_controllerTransform.position, Vector2.right, _grabRange, Controller.GroundLayer);
		Vector2 zero = Vector2.zero;
		float num = 0f;
		if ((bool)raycastHit2D)
		{
			num = 0.4f;
			_controllerTransform.localScale = new Vector2(-1f, 1f);
			zero = new Vector2(raycastHit2D.point.x + num, _controllerTransform.position.y);
		}
		else
		{
			if (!raycastHit2D2)
			{
				Fall();
				return;
			}
			num = -0.4f;
			_controllerTransform.localScale = new Vector2(1f, 1f);
			zero = new Vector2(raycastHit2D2.point.x + num, _controllerTransform.position.y);
		}
		Controller.SetPlayerMobility(value: false);
		_controllerTransform.position = zero;
		if (_countdown == null)
		{
			_countdown = StaticInstance<TwoWayCountdown>.Instance.StartNewTimer(_grabTimer);
		}
		StaticInstance<TwoWayCountdown>.Instance.StartTimer(_countdown);
		_countdown.Ended.AddListener(GrabTimerRunOut);
		float num2 = _countdown.CurrentTime / _grabTimer;
		_sweatParticles.Play();
		ParticleSystem.EmissionModule emission = _sweatParticles.emission;
		emission.rateOverTime = 2f + num2 * 6f;
		Controller.JumpEnabled = false;
		if (_effectPlayer != null && _audioObjectManager != null)
		{
			_effectPlayer.PlaySound(_audioObjectManager);
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (_countdown != null)
		{
			if (_countdown.IsRunning)
			{
				Controller.SetMaterialFloatProperty("_redValue", 1f - _countdown.CurrentTime / _grabTimer);
			}
			RaycastHit2D raycastHit2D = Physics2D.Raycast(_controllerTransform.position, Vector2.left, 1f, Controller.GroundLayer);
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(_controllerTransform.position, Vector2.right, 1f, Controller.GroundLayer);
			if (!raycastHit2D && !raycastHit2D2)
			{
				Fall();
			}
		}
	}

	private void GrabTimerRunOut()
	{
		Fall();
		Controller.SendPlayerMakerEvent("GrabTimmer");
	}

	public override void Exit()
	{
		base.Exit();
		StaticInstance<TwoWayCountdown>.Instance.StopTimer(_countdown);
		_sweatParticles.Stop();
		Controller.SetPlayerMobility(value: true);
	}

	public override void Clean()
	{
		base.Clean();
		Controller.OnGrounded.RemoveListener(RestartTimer);
	}
}
