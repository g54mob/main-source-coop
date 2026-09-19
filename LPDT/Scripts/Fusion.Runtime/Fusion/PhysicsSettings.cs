using System;
using UnityEngine;

namespace Fusion
{
	[Serializable]
	public struct PhysicsSettings
	{
		[Space]
		[InlineHelp]
		public bool ForecastEnabled;

		[NonSerialized]
		[InlineHelp]
		public bool SetForecastDisabledProxyRigidbodiesToKinematic;

		[NonSerialized]
		[InlineHelp]
		public bool SetPhysicsInterpolationToDisabledWhenKinematic;

		[InlineHelp]
		public bool DebugPhysics;

		[InlineHelp]
		public GravityForecast GravityForecast;

		[InlineHelp]
		public float MaxExtrapolationTime;

		[InlineHelp]
		public float MinLinearDetectedError;

		[InlineHelp]
		public float MinAngularDetectedError;

		[InlineHelp]
		public float MaxLinearError;

		[InlineHelp]
		public float MaxAngularError;

		[InlineHelp]
		public float LowCorrectionProgressThreshold;

		[InlineHelp]
		public float HighErrorSimilarityThreshold;

		[InlineHelp]
		public float MaxErrorTotalTime;

		[InlineHelp]
		public float MaxRemoteSleepIgnoreTime;

		[InlineHelp]
		public PhysicsCorrection ErrorCorrectionType;

		[InlineHelp]
		[DrawIf("ErrorCorrectionType", 1L, CompareOperator.LessOrEqual, DrawIfMode.Hide)]
		public float LinearVelCorrectionMul;

		[InlineHelp]
		public float AngularVelCorrectionMul;

		[InlineHelp]
		[DrawIf("ErrorCorrectionType", 1L, CompareOperator.Equal, DrawIfMode.Hide)]
		public float PositionCorrectionLerp;

		[InlineHelp]
		public float RotationCorrectionLerp;

		[InlineHelp]
		[DrawIf("ErrorCorrectionType", 2L, CompareOperator.Equal, DrawIfMode.Hide)]
		public float Spring;

		[InlineHelp]
		[DrawIf("ErrorCorrectionType", 2L, CompareOperator.Equal, DrawIfMode.Hide)]
		public float Damper;

		[InlineHelp]
		[Range(0f, 1f)]
		public float MinImpactfulCollisionAlignment;

		[InlineHelp]
		public float ImpactStartCorrectionTime;

		[InlineHelp]
		public float ImpactCorrectionTimeComplete;

		public PhysicsSettings()
		{
			ForecastEnabled = true;
			SetForecastDisabledProxyRigidbodiesToKinematic = true;
			SetPhysicsInterpolationToDisabledWhenKinematic = true;
			DebugPhysics = false;
			GravityForecast = GravityForecast.Auto;
			MaxExtrapolationTime = 0.15f;
			MinLinearDetectedError = 0.02f;
			MinAngularDetectedError = 3f;
			MaxLinearError = 0f;
			MaxAngularError = 0f;
			LowCorrectionProgressThreshold = 0.015f;
			HighErrorSimilarityThreshold = 1f;
			MaxErrorTotalTime = 0.5f;
			MaxRemoteSleepIgnoreTime = 0.5f;
			ErrorCorrectionType = PhysicsCorrection.Velocity;
			LinearVelCorrectionMul = 1f;
			AngularVelCorrectionMul = 1f;
			PositionCorrectionLerp = 0.15f;
			RotationCorrectionLerp = 0.15f;
			Spring = 75f;
			Damper = 2f;
			MinImpactfulCollisionAlignment = 0.3f;
			ImpactStartCorrectionTime = 0.1f;
			ImpactCorrectionTimeComplete = 0.3f;
		}
	}
}
