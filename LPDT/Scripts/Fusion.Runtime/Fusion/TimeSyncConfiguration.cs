using System;
using UnityEngine;

namespace Fusion
{
	[Serializable]
	public class TimeSyncConfiguration : IConfigurationSanityCheck
	{
		public enum Selection
		{
			Default = 0,
			Custom = 1
		}

		public enum Unit
		{
			Milliseconds = 0,
			Ticks = 1
		}

		internal const float DEFAULT_MAX_LATE_SNAPSHOTS_PERCENTILE = 5f;

		internal const float DEFAULT_REDUNDANT_SNAPSHOTS = 1f;

		internal const int DEFAULT_STATE_BUFFER_DELAY_ADDED = 0;

		internal const Unit DEFAULT_STATE_BUFFER_DELAY_ADDED_UNIT = Unit.Milliseconds;

		internal const float DEFAULT_MAX_LATE_INPUTS_PERCENTILE = 5f;

		internal const float DEFAULT_REDUNDANT_INPUTS = 1f;

		internal const int DEFAULT_INPUT_BUFFER_DELAY_ADDED = 0;

		internal const Unit DEFAULT_INPUT_BUFFER_DELAY_ADDED_UNIT = Unit.Milliseconds;

		internal const int DEFAULT_MAX_RESIMS_PER_FRAME = -1;

		internal const int DEFAULT_MINIMUM_INPUT_DELAY = 0;

		internal const Unit DEFAULT_MINIMUM_INPUT_DELAY_UNIT = Unit.Milliseconds;

		[InlineHelp]
		public Selection StateReceiveBufferSettings = Selection.Default;

		[InlineHelp]
		[Unit(Units.Percentage)]
		[RangeEx(0.10000000149011612, 20.0)]
		[DrawIf("StateReceiveBufferSettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public float MaxLateSnapshots = 5f;

		[InlineHelp]
		[Unit(Units.Packets)]
		[RangeEx(-1.0, 10.0)]
		[DrawIf("StateReceiveBufferSettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public float RedundantSnapshots = 1f;

		[InlineHelp]
		[Unit(Units.None)]
		[RangeEx(0.0, 1000.0)]
		[DrawIf("StateReceiveBufferSettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public int StateReceiveBufferDelayAdded = 0;

		[InlineHelp]
		[DrawIf("StateReceiveBufferSettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public Unit StateReceiveBufferDelayAddedUnit = Unit.Milliseconds;

		[InlineHelp]
		public Selection InputReceiveBufferSettings = Selection.Default;

		[InlineHelp]
		[Unit(Units.Percentage)]
		[RangeEx(0.10000000149011612, 20.0)]
		[DrawIf("InputReceiveBufferSettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public float MaxLateInputs = 5f;

		[InlineHelp]
		[Unit(Units.Packets)]
		[RangeEx(0.0, 10.0)]
		[DrawIf("InputReceiveBufferSettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public float RedundantInputs = 1f;

		[InlineHelp]
		[Unit(Units.None)]
		[RangeEx(0.0, 1000.0)]
		[DrawIf("InputReceiveBufferSettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public int InputReceiveBufferDelayAdded = 0;

		[InlineHelp]
		[DrawIf("InputReceiveBufferSettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public Unit InputReceiveBufferDelayAddedUnit = Unit.Milliseconds;

		[InlineHelp]
		public Selection InputDelaySettings = Selection.Default;

		[InlineHelp]
		[Unit(Units.None)]
		[RangeEx(0.0, 1000.0)]
		[DrawIf("InputDelaySettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public int MinimumInputDelay = 0;

		[InlineHelp]
		[DrawIf("InputDelaySettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public Unit MinimumInputDelayUnit = Unit.Milliseconds;

		[InlineHelp]
		[ToggleLeft]
		[DrawIf("InputDelaySettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		public bool ClientsCanAddInputDelayToLimitResims = false;

		[InlineHelp]
		[Unit(Units.Ticks)]
		[RangeEx(-1.0, 1000.0)]
		[DrawIf("InputDelaySettings", 1L, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = true)]
		[DrawIf("ClientsCanAddInputDelayToLimitResims", true, CompareOperator.Equal, DrawIfMode.ReadOnly, Hide = false)]
		public int ClientMaxResimsPerFrame = -1;

		public void SanityCheck()
		{
			if (StateReceiveBufferSettings == Selection.Custom)
			{
				MaxLateSnapshots = Mathf.Clamp(MaxLateSnapshots, 0.1f, 20f);
				RedundantSnapshots = Mathf.Clamp(RedundantSnapshots, -1f, 10f);
				StateReceiveBufferDelayAdded = Maths.Clamp(StateReceiveBufferDelayAdded, 0, 1000);
			}
			else
			{
				MaxLateSnapshots = 5f;
				RedundantSnapshots = 1f;
				StateReceiveBufferDelayAdded = 0;
				StateReceiveBufferDelayAddedUnit = Unit.Milliseconds;
			}
			if (InputReceiveBufferSettings == Selection.Custom)
			{
				MaxLateInputs = Mathf.Clamp(MaxLateInputs, 0.1f, 20f);
				RedundantInputs = Mathf.Clamp(RedundantInputs, 0f, 10f);
				InputReceiveBufferDelayAdded = Maths.Clamp(InputReceiveBufferDelayAdded, 0, 1000);
			}
			else
			{
				MaxLateInputs = 5f;
				RedundantInputs = 1f;
				InputReceiveBufferDelayAdded = 0;
				InputReceiveBufferDelayAddedUnit = Unit.Milliseconds;
			}
			if (InputDelaySettings == Selection.Custom)
			{
				MinimumInputDelay = Maths.Clamp(MinimumInputDelay, 0, 1000);
				ClientMaxResimsPerFrame = (ClientsCanAddInputDelayToLimitResims ? Maths.Clamp(ClientMaxResimsPerFrame, -1, 1000) : (-1));
			}
			else
			{
				MinimumInputDelay = 0;
				MinimumInputDelayUnit = Unit.Milliseconds;
				ClientMaxResimsPerFrame = -1;
			}
		}
	}
}
