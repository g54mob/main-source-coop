#define TRACE
#define DEBUG
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Fusion
{
	public static class NetworkTransformHelpers
	{
		public readonly struct NormalizedStallHeuristic
		{
			private readonly float _minimumLinearError;

			private readonly float _directionSimilarityThreshold;

			private readonly float _correctionFractionThreshold;

			private readonly float _errorRatioThreshold;

			public readonly NetworkTransform.CustomErrorDetectionDelegate Delegate;

			public NormalizedStallHeuristic(float minimumLinearError = 0.01f, float directionSimilarityThreshold = 0.95f, float correctionFractionThreshold = 0.05f, float errorRatioThreshold = 0.95f)
			{
				Delegate = null;
				_minimumLinearError = minimumLinearError;
				_directionSimilarityThreshold = directionSimilarityThreshold;
				_correctionFractionThreshold = correctionFractionThreshold;
				_errorRatioThreshold = errorRatioThreshold;
				Delegate = StalledErrorCorrectionDetection;
			}

			private float StalledErrorCorrectionDetection(NetworkTransform networkTransform, float physicsDt, AbstractPhysicsBody physicsBody, Vector3 previousLocalPosition, Vector3 previousExtrapolatedPosition, ref KinematicSnapshot localSnapshot, ref KinematicSnapshot extrapolatedSnapshot)
			{
				Vector3 vector = previousExtrapolatedPosition - previousLocalPosition;
				Vector3 vector2 = extrapolatedSnapshot.WorldPosition - localSnapshot.WorldPosition;
				float magnitude = vector.magnitude;
				float magnitude2 = vector2.magnitude;
				if (magnitude < _minimumLinearError || magnitude2 < _minimumLinearError)
				{
					return -1f;
				}
				if (magnitude < 1E-05f || magnitude2 < 1E-05f)
				{
					return 0f;
				}
				Vector3 rhs = vector / magnitude;
				Vector3 lhs = vector2 / magnitude2;
				float num = Vector3.Dot(lhs, rhs);
				float num2 = Vector3.Dot(localSnapshot.WorldPosition - previousLocalPosition, rhs) / magnitude;
				float num3 = magnitude2 / magnitude;
				bool flag = num > _directionSimilarityThreshold;
				bool flag2 = num2 < _correctionFractionThreshold;
				bool flag3 = num3 > _errorRatioThreshold;
				if (flag && flag2 && flag3)
				{
					return 1f;
				}
				return -1f;
			}
		}

		private const float _fallingEpsilon = 0.0001f;

		public static float DefaultStalledErrorCorrectionDetection(NetworkTransform networkTransform, float physicsDt, AbstractPhysicsBody physicsBody, Vector3 previousLocalPosition, Vector3 previousExtrapolatedPosition, ref KinematicSnapshot localSnapshot, ref KinematicSnapshot extrapolatedSnapshot)
		{
			Vector3 rhs = previousExtrapolatedPosition - previousLocalPosition;
			Vector3 lhs = extrapolatedSnapshot.WorldPosition - localSnapshot.WorldPosition;
			float num = Vector3.Dot(lhs, rhs);
			float num2 = Vector3.Dot(localSnapshot.WorldPosition - previousLocalPosition, rhs.normalized);
			networkTransform.RecordStallHeuristicData(num, num2);
			if (num > networkTransform.PhysicsSettings.HighErrorSimilarityThreshold && num2 < networkTransform.PhysicsSettings.LowCorrectionProgressThreshold)
			{
				return 1f;
			}
			return -1f;
		}

		public static void ComputeExtrapolatedSnapshot(NetworkTransform nt, float physicsDt, AbstractPhysicsBody physicsBody, float ticksToExtrapolate, ref KinematicSnapshot local, ref KinematicSnapshot remote, out KinematicSnapshot result)
		{
			result = default(KinematicSnapshot);
			ref PhysicsSettings physicsSettings = ref nt.PhysicsSettings;
			float deltaTime = nt.Runner.DeltaTime;
			Vector3 worldPosition = remote.WorldPosition;
			Quaternion worldRotation = remote.WorldRotation;
			Vector3 linearVelocity = remote.LinearVelocity;
			Vector3 angularVelocity = remote.AngularVelocity;
			Vector3 vector = physicsBody.Gravity;
			if (physicsSettings.GravityForecast == GravityForecast.None)
			{
				vector = Vector3.zero;
			}
			else if (physicsSettings.GravityForecast == GravityForecast.Auto)
			{
				float num = vector.magnitude * physicsDt;
				float num2 = Vector3.Dot(linearVelocity, vector.normalized);
				vector = ((num2 >= num - 0.0001f) ? vector : Vector3.zero);
			}
			if (remote.IsSleeping)
			{
				if (remote.LinearVelocity != Vector3.zero)
				{
					InternalLogStreams.LogTraceForecast?.Log(nt, $"Sleeping, but non-Zero LinearVelocity: {remote.LinearVelocity:F3}");
				}
				if (remote.AngularVelocity != Vector3.zero)
				{
					InternalLogStreams.LogTraceForecast?.Log(nt, $"Sleeping, but non-Zero AngularVelocity: {remote.AngularVelocity:F3}");
				}
				result.WorldPosition = worldPosition;
				result.WorldRotation = worldRotation;
				result.LinearVelocity = linearVelocity;
				result.AngularVelocity = angularVelocity;
			}
			else
			{
				float num3 = ticksToExtrapolate * deltaTime;
				float drag = physicsBody.Drag;
				Vector3 worldPosition2 = FloatTrajectoryPredictor.PredictPosition(deltaTime, ticksToExtrapolate, worldPosition, linearVelocity, vector, drag);
				float magnitude = angularVelocity.magnitude;
				Vector3 normalized = angularVelocity.normalized;
				Quaternion quaternion = Quaternion.AngleAxis(magnitude * num3, normalized);
				Quaternion worldRotation2 = quaternion * worldRotation;
				Vector3 linearVelocity2 = FloatTrajectoryPredictor.PredictVelocity(deltaTime, ticksToExtrapolate, linearVelocity, vector, drag);
				Vector3 angularVelocity2 = angularVelocity + normalized * (magnitude * num3);
				result.WorldPosition = worldPosition2;
				result.WorldRotation = worldRotation2;
				result.LinearVelocity = linearVelocity2;
				result.AngularVelocity = angularVelocity2;
			}
		}

		public static void ApplyCorrection(NetworkTransform nt, float physicsDt, float physicsFps, AbstractPhysicsBody physicsBody, float timeSinceLastOnCollisionEnter, ref KinematicSnapshot localSnapshot, ref KinematicSnapshot remoteSnapshot, ref KinematicSnapshot extrapolatedSnapshot)
		{
			ref PhysicsSettings physicsSettings = ref nt.PhysicsSettings;
			if (timeSinceLastOnCollisionEnter < physicsSettings.ImpactStartCorrectionTime)
			{
				return;
			}
			float num = Mathf.Clamp((timeSinceLastOnCollisionEnter - physicsSettings.ImpactStartCorrectionTime) / (physicsSettings.ImpactCorrectionTimeComplete - physicsSettings.ImpactStartCorrectionTime), 0f, 1f);
			if (physicsSettings.ErrorCorrectionType == PhysicsCorrection.Velocity || physicsSettings.ErrorCorrectionType == PhysicsCorrection.PositionRotation)
			{
				if (physicsSettings.ErrorCorrectionType == PhysicsCorrection.PositionRotation)
				{
					Vector3 b = Vector3.Lerp(localSnapshot.WorldPosition, extrapolatedSnapshot.WorldPosition, physicsSettings.PositionCorrectionLerp);
					physicsBody.Position = Vector3.Lerp(physicsBody.Position, b, num);
				}
				Quaternion b2 = Quaternion.Slerp(localSnapshot.WorldRotation, extrapolatedSnapshot.WorldRotation, physicsSettings.RotationCorrectionLerp);
				physicsBody.Rotation = Quaternion.Slerp(physicsBody.Rotation, b2, num);
				Vector3 linearVelocity = physicsBody.LinearVelocity;
				Vector3 vector = CalculateLinearVelocity(nt, physicsFps, ref localSnapshot, ref extrapolatedSnapshot);
				physicsBody.LinearVelocity = Vector3.Lerp(physicsBody.LinearVelocity, vector, num);
				Vector3 b3 = CalculateAngularVelocity(nt, ref remoteSnapshot, ref extrapolatedSnapshot);
				physicsBody.AngularVelocity = Vector3.Lerp(physicsBody.AngularVelocity, b3, num);
				nt.RecordForecastData(linearVelocity, physicsBody.LinearVelocity, vector, num);
			}
			else if (physicsSettings.ErrorCorrectionType == PhysicsCorrection.SpringDamping)
			{
				physicsBody.Rotation = Quaternion.Slerp(localSnapshot.WorldRotation, extrapolatedSnapshot.WorldRotation, physicsSettings.RotationCorrectionLerp);
				physicsBody.LinearVelocity = extrapolatedSnapshot.LinearVelocity;
				physicsBody.AngularVelocity = CalculateAngularVelocity(nt, ref remoteSnapshot, ref extrapolatedSnapshot);
				Vector3 vector2 = nt.PositionError * physicsSettings.Spring;
				Vector3 vector3 = default(Vector3);
				if (nt.PositionError != default(Vector3))
				{
					Vector3 vector4 = (nt.PositionError - nt.PreviousPositionError) * physicsFps;
					vector3 = vector4 * physicsSettings.Damper;
				}
				physicsBody.AddForce((vector2 + vector3) * physicsBody.Mass);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector3 CalculateAngularVelocity(NetworkTransform nt, [In][RequiresLocation] ref KinematicSnapshot remoteSnapshot, [In][RequiresLocation] ref KinematicSnapshot extrapolatedSnapshot)
		{
			ref PhysicsSettings physicsSettings = ref nt.PhysicsSettings;
			return remoteSnapshot.AngularVelocity + (extrapolatedSnapshot.AngularVelocity - remoteSnapshot.AngularVelocity) * physicsSettings.AngularVelCorrectionMul;
		}

		private static Vector3 CalculateLinearVelocity(NetworkTransform nt, float physicsFps, [In][RequiresLocation] ref KinematicSnapshot localSnapshot, [In][RequiresLocation] ref KinematicSnapshot extrapolatedSnapshot)
		{
			ref PhysicsSettings physicsSettings = ref nt.PhysicsSettings;
			Vector3 vector = extrapolatedSnapshot.WorldPosition - localSnapshot.WorldPosition;
			float magnitude = vector.magnitude;
			float value = magnitude * physicsSettings.LinearVelCorrectionMul;
			float max = magnitude * physicsFps;
			value = Mathf.Clamp(value, 0f, max);
			return extrapolatedSnapshot.LinearVelocity + value * vector.normalized;
		}
	}
}
