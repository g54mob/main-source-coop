using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using Features.EntitiesSoundOcclusionModule.Scripts;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class PorterNoiseEmitter
	{
		private readonly MonkeyPorterContext _monkeyPorterContext;

		private readonly MonkeyPorterSettings _monkeyPorterSettings;

		private readonly IEntitiesSoundOcclusionService _entitiesSoundOcclusionService;

		private float _moveNoiseCooldown;

		public PorterNoiseEmitter(MonkeyPorterContext monkeyPorterContext, MonkeyPorterSettings monkeyPorterSettings, IEntitiesSoundOcclusionService entitiesSoundOcclusionService)
		{
			_monkeyPorterContext = monkeyPorterContext;
			_monkeyPorterSettings = monkeyPorterSettings;
			_entitiesSoundOcclusionService = entitiesSoundOcclusionService;
		}

		public void Tick(float deltaTime, bool isCarrying)
		{
			_moveNoiseCooldown -= deltaTime;
			if (!(_monkeyPorterContext.AgentVelocity < _monkeyPorterSettings.NoiseMoveSpeedThreshold) && !(_moveNoiseCooldown > 0f))
			{
				_moveNoiseCooldown = _monkeyPorterSettings.MoveNoiseInterval;
				Emit(isCarrying ? _monkeyPorterSettings.CarryNoiseRadius : _monkeyPorterSettings.MoveNoiseRadius);
			}
		}

		public void EmitPanic()
		{
			_moveNoiseCooldown = _monkeyPorterSettings.MoveNoiseInterval;
			Emit(_monkeyPorterSettings.PanicNoiseRadius);
		}

		private void Emit(float radius)
		{
			if (!(radius <= 0f) && !(_monkeyPorterContext.SoundSourceBehaviour == null))
			{
				_entitiesSoundOcclusionService.TriggerOcclusionForEntities(_monkeyPorterContext.transform.position, radius, _monkeyPorterContext.SoundSourceBehaviour);
			}
		}
	}
}
