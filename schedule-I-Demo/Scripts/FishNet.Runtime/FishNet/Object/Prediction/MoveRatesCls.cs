using System.Runtime.CompilerServices;
using GameKit.Utilities;

namespace FishNet.Object.Prediction
{
	internal class MoveRatesCls : IResettable
	{
		public float Position;

		public float Rotation;

		public float Scale;

		public const float UNSET_VALUE = float.NegativeInfinity;

		public const float INSTANT_VALUE = float.PositiveInfinity;

		public float LastMultiplier { get; private set; } = 1f;

		public bool PositionSet => Position != float.NegativeInfinity;

		public bool RotationSet => Rotation != float.NegativeInfinity;

		public bool ScaleSet => Scale != float.NegativeInfinity;

		public bool AnySet
		{
			get
			{
				if (!PositionSet && !RotationSet)
				{
					return ScaleSet;
				}
				return true;
			}
		}

		public bool InstantPosition => Position == float.PositiveInfinity;

		public bool InstantRotation => Rotation == float.PositiveInfinity;

		public bool InstantScale => Scale == float.PositiveInfinity;

		public MoveRatesCls(float value)
		{
			Position = value;
			Rotation = value;
			Scale = value;
		}

		public MoveRatesCls(float position, float rotation)
		{
			Position = position;
			Rotation = rotation;
			Scale = float.PositiveInfinity;
		}

		public MoveRatesCls(float position, float rotation, float scale)
		{
			Position = position;
			Rotation = rotation;
			Scale = scale;
		}

		public MoveRatesCls()
		{
			Update(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
		}

		public void Multiply(float value)
		{
			LastMultiplier = value;
			Position *= value;
			Rotation *= value;
			Scale *= value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetInstantRates()
		{
			Update(float.PositiveInfinity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Update(float value)
		{
			Update(value, value, value);
		}

		public void Update(float position, float rotation, float scale)
		{
			Position = position;
			Rotation = rotation;
			Scale = scale;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Update(MoveRatesCls mr)
		{
			Update(mr.Position, mr.Rotation, mr.Scale);
		}

		public void ResetState()
		{
			Position = float.NegativeInfinity;
			Rotation = float.NegativeInfinity;
			Scale = float.NegativeInfinity;
		}

		public void InitializeState()
		{
		}
	}
}
