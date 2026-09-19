using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Services;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class PorterThreatDetector
	{
		private readonly MonkeyPorterContext _context;

		private readonly MonkeyPorterSettings _settings;

		private readonly IEnemyTrackingService _enemyTrackingService;

		private float _scanElapsed;

		private IEnemyTrackable _threat;

		public bool HasThreat => IsThreatAlive(_threat);

		public IEnemyTrackable Threat
		{
			get
			{
				if (!HasThreat)
				{
					return null;
				}
				return _threat;
			}
		}

		public Vector3 ThreatPosition
		{
			get
			{
				if (!HasThreat)
				{
					return _context.transform.position;
				}
				return _threat.Transform.position;
			}
		}

		public PorterThreatDetector(MonkeyPorterContext context, MonkeyPorterSettings settings, IEnemyTrackingService enemyTrackingService)
		{
			_context = context;
			_settings = settings;
			_enemyTrackingService = enemyTrackingService;
		}

		public void Reset()
		{
			_threat = null;
			_scanElapsed = 0f;
		}

		public void Tick(float deltaTime, bool isHidden)
		{
			_scanElapsed -= deltaTime;
			if (!(_scanElapsed > 0f))
			{
				_scanElapsed = _settings.ThreatScanInterval;
				float radius = ((_threat != null) ? _settings.ThreatForgetRadius : _settings.ThreatDetectRadius);
				if (!_enemyTrackingService.TryGetNearestThreat(_context.transform.position, radius, out var threat, out var _))
				{
					_threat = null;
				}
				else if (!isHidden && !_context.HasLineOfSightTo(threat.Transform))
				{
					_threat = null;
				}
				else
				{
					_threat = threat;
				}
			}
		}

		public bool IsAnyThreatWithin(float radius)
		{
			IEnemyTrackable threat;
			float distance;
			return _enemyTrackingService.TryGetNearestThreat(_context.transform.position, radius, out threat, out distance);
		}

		public bool IsThreatWithin(IEnemyTrackable threat, float radius)
		{
			if (!IsThreatAlive(threat))
			{
				return false;
			}
			return (threat.Transform.position - _context.transform.position).sqrMagnitude <= radius * radius;
		}

		public Vector3 GetThreatPosition(IEnemyTrackable threat)
		{
			if (!IsThreatAlive(threat))
			{
				return ThreatPosition;
			}
			return threat.Transform.position;
		}

		public static bool IsThreatAlive(IEnemyTrackable threat)
		{
			if (threat != null && threat.Transform != null)
			{
				return threat.IsTrackable;
			}
			return false;
		}
	}
}
