using System.Collections.Generic;
using Features.NavigationModule.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModule.Scripts.AttractionZone
{
	public class EnemyAttractionZoneService : IEnemyAttractionZoneService
	{
		private readonly EnemyAttractionZoneListenerModel _model;

		public EnemyAttractionZoneService(EnemyAttractionZoneListenerModel model)
		{
			_model = model;
		}

		public void RaiseZone(AttractionZoneData zone)
		{
			_model.AddZone(zone);
			List<IEnemyAttractionZoneCallbackListener> allListeners = _model.GetAllListeners();
			for (int i = 0; i < allListeners.Count; i++)
			{
				allListeners[i]?.OnAttractionZoneRaised(zone);
			}
		}

		public void EndZone(int zoneId)
		{
			if (_model.RemoveZone(zoneId))
			{
				List<IEnemyAttractionZoneCallbackListener> allListeners = _model.GetAllListeners();
				for (int i = 0; i < allListeners.Count; i++)
				{
					allListeners[i]?.OnAttractionZoneEnded(zoneId);
				}
			}
		}

		public bool TrySampleApproachPoint(INavigationService navigationService, Vector3 origin, float radius, int attempts, out Vector3 point)
		{
			point = origin;
			if (navigationService == null)
			{
				return false;
			}
			int num = Mathf.Max(1, attempts);
			for (int i = 0; i < num; i++)
			{
				Vector2 vector = Random.insideUnitCircle * radius;
				Vector3 point2 = origin + new Vector3(vector.x, 0f, vector.y);
				if (navigationService.TryGetPointOnNavMeshProjected(point2, out point))
				{
					return true;
				}
			}
			return false;
		}

		public bool TrySampleApproachPointDirect(Vector3 origin, float radius, int attempts, out Vector3 point)
		{
			point = origin;
			float num = Mathf.Max(0.5f, radius);
			int num2 = Mathf.Max(1, attempts);
			for (int i = 0; i < num2; i++)
			{
				Vector2 vector = Random.insideUnitCircle * num;
				if (NavMesh.SamplePosition(origin + new Vector3(vector.x, 0f, vector.y), out var hit, num, -1))
				{
					point = hit.position;
					return true;
				}
			}
			return false;
		}
	}
}
