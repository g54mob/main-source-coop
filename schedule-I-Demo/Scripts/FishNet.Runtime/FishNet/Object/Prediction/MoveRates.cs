using System.Runtime.CompilerServices;

namespace FishNet.Object.Prediction
{
	internal struct MoveRates
	{
		public float Position;

		public float Rotation;

		public float Scale;

		public const float UNSET_VALUE = float.NegativeInfinity;

		public const float INSTANT_VALUE = float.PositiveInfinity;

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

		public MoveRates(float value)
		{
			Position = value;
			Rotation = value;
			Scale = value;
		}

		public MoveRates(float position, float rotation)
		{
			Position = position;
			Rotation = rotation;
			Scale = float.PositiveInfinity;
		}

		public MoveRates(float position, float rotation, float scale)
		{
			Position = position;
			Rotation = rotation;
			Scale = scale;
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
	}
}
