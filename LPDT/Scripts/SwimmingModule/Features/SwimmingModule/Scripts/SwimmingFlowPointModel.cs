using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace Features.SwimmingModule.Scripts
{
	public class SwimmingFlowPointModel : ISessionCleanup
	{
		public Dictionary<GameObject, Dictionary<SwimmingPointType, Transform>> Points { get; } = new Dictionary<GameObject, Dictionary<SwimmingPointType, Transform>>();

		public void RegisterPoint(GameObject source, SwimmingPointType swimmingPointType, Transform transform)
		{
			if (!Points.TryGetValue(source, out var value))
			{
				value = new Dictionary<SwimmingPointType, Transform>();
				Points[source] = value;
			}
			value[swimmingPointType] = transform;
		}

		public bool TryGetPoint(GameObject source, SwimmingPointType swimmingPointType, out Transform transform)
		{
			transform = null;
			if (source == null)
			{
				return false;
			}
			if (!Points.TryGetValue(source, out var value))
			{
				return false;
			}
			return value.TryGetValue(swimmingPointType, out transform);
		}

		public void Cleanup()
		{
			Points.Clear();
		}
	}
}
