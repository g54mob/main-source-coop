using System;
using UnityEngine;

namespace Fusion
{
	public static class FloatTrajectoryPredictor
	{
		public static Vector3 PredictPosition(float dt, float ticksAhead, Vector3 initPos, Vector3 initVel, Vector3 gravity, float drag)
		{
			if (ticksAhead < 0f)
			{
				throw new InvalidOperationException(string.Format("'{0}' must be positive or zero, but is: {1}", "ticksAhead", ticksAhead));
			}
			if (drag <= 0f)
			{
				float num = dt * ticksAhead;
				return initPos + initVel * num + gravity * (num * (num + dt) * 0.5f);
			}
			float num2 = Mathf.Pow(1f - drag * dt, ticksAhead);
			Vector3 vector = gravity * (dt - 1f / drag);
			float num3 = (1f - num2) / drag;
			Vector3 vector2 = gravity * (ticksAhead * dt / drag);
			return initPos + (initVel + vector) * num3 + vector2;
		}

		public static Vector2 PredictPosition(float dt, float ticksAhead, Vector2 initPos, Vector2 initVel, Vector2 gravity, float drag)
		{
			if (ticksAhead < 0f)
			{
				throw new InvalidOperationException(string.Format("'{0}' must be positive or zero, but is: {1}", "ticksAhead", ticksAhead));
			}
			if (drag <= 0f)
			{
				float num = dt * ticksAhead;
				return initPos + initVel * num + gravity * (num * (num + dt) * 0.5f);
			}
			float num2 = Mathf.Pow(1f - drag * dt, ticksAhead);
			Vector2 vector = gravity * (dt - 1f / drag);
			float num3 = (1f - num2) / drag;
			Vector2 vector2 = gravity * (ticksAhead * dt / drag);
			return initPos + (initVel + vector) * num3 + vector2;
		}

		public static Vector2 PredictVelocity(float dt, float ticksAhead, Vector2 initVel, Vector2 gravity, float drag)
		{
			if (ticksAhead < 0f)
			{
				throw new InvalidOperationException(string.Format("'{0}' must be positive or zero, but is: {1}", "ticksAhead", ticksAhead));
			}
			Vector3 vector;
			if (drag <= 0f)
			{
				float num = dt * ticksAhead;
				vector = initVel + gravity * num;
			}
			else
			{
				float num2 = Mathf.Pow(1f - drag * dt, ticksAhead);
				Vector2 vector2 = gravity * (dt - 1f / drag);
				vector = (initVel + vector2) * num2 - vector2;
			}
			return vector;
		}

		public static Vector3 PredictVelocity(float dt, float ticksAhead, Vector3 initVel, Vector3 gravity, float drag)
		{
			if (ticksAhead < 0f)
			{
				throw new InvalidOperationException(string.Format("'{0}' must be positive or zero, but is: {1}", "ticksAhead", ticksAhead));
			}
			if (drag <= 0f)
			{
				float num = dt * ticksAhead;
				return initVel + gravity * num;
			}
			float num2 = Mathf.Pow(1f - drag * dt, ticksAhead);
			Vector3 vector = gravity * (dt - 1f / drag);
			return (initVel + vector) * num2 - vector;
		}
	}
}
