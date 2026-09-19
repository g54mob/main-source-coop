using System.Runtime.CompilerServices;

namespace Fusion
{
	internal static class GenerateFlagsExtensionsExtensions_internal
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this BehaviourCallbackFlags flag, BehaviourCallbackFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this BehaviourCallbackFlags flag, BehaviourCallbackFlags value, BehaviourCallbackFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this BehaviourCallbackFlags flag, BehaviourCallbackFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this BehaviourCallbackFlags flag, BehaviourCallbackFlags value, BehaviourCallbackFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this BehaviourCallbackFlags flag, BehaviourCallbackFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this BehaviourCallbackFlags flag, BehaviourCallbackFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this AnimatorSyncSettings flag, AnimatorSyncSettings value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this AnimatorSyncSettings flag, AnimatorSyncSettings value, AnimatorSyncSettings mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this AnimatorSyncSettings flag, AnimatorSyncSettings value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this AnimatorSyncSettings flag, AnimatorSyncSettings value, AnimatorSyncSettings mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this AnimatorSyncSettings flag, AnimatorSyncSettings value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this AnimatorSyncSettings flag, AnimatorSyncSettings value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkPropertyMetaFlags flag, NetworkPropertyMetaFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkPropertyMetaFlags flag, NetworkPropertyMetaFlags value, NetworkPropertyMetaFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkPropertyMetaFlags flag, NetworkPropertyMetaFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkPropertyMetaFlags flag, NetworkPropertyMetaFlags value, NetworkPropertyMetaFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkPropertyMetaFlags flag, NetworkPropertyMetaFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkPropertyMetaFlags flag, NetworkPropertyMetaFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this SimulationBehaviourRuntimeFlags flag, SimulationBehaviourRuntimeFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this SimulationBehaviourRuntimeFlags flag, SimulationBehaviourRuntimeFlags value, SimulationBehaviourRuntimeFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this SimulationBehaviourRuntimeFlags flag, SimulationBehaviourRuntimeFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this SimulationBehaviourRuntimeFlags flag, SimulationBehaviourRuntimeFlags value, SimulationBehaviourRuntimeFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this SimulationBehaviourRuntimeFlags flag, SimulationBehaviourRuntimeFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this SimulationBehaviourRuntimeFlags flag, SimulationBehaviourRuntimeFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectConnectionDataFlags flag, NetworkObjectConnectionDataFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectConnectionDataFlags flag, NetworkObjectConnectionDataFlags value, NetworkObjectConnectionDataFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectConnectionDataFlags flag, NetworkObjectConnectionDataFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectConnectionDataFlags flag, NetworkObjectConnectionDataFlags value, NetworkObjectConnectionDataFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkObjectConnectionDataFlags flag, NetworkObjectConnectionDataFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkObjectConnectionDataFlags flag, NetworkObjectConnectionDataFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectDestroyFlags flag, NetworkObjectDestroyFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectDestroyFlags flag, NetworkObjectDestroyFlags value, NetworkObjectDestroyFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectDestroyFlags flag, NetworkObjectDestroyFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectDestroyFlags flag, NetworkObjectDestroyFlags value, NetworkObjectDestroyFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkObjectDestroyFlags flag, NetworkObjectDestroyFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkObjectDestroyFlags flag, NetworkObjectDestroyFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectHeaderPlayerDataFlags flag, NetworkObjectHeaderPlayerDataFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectHeaderPlayerDataFlags flag, NetworkObjectHeaderPlayerDataFlags value, NetworkObjectHeaderPlayerDataFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectHeaderPlayerDataFlags flag, NetworkObjectHeaderPlayerDataFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectHeaderPlayerDataFlags flag, NetworkObjectHeaderPlayerDataFlags value, NetworkObjectHeaderPlayerDataFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkObjectHeaderPlayerDataFlags flag, NetworkObjectHeaderPlayerDataFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkObjectHeaderPlayerDataFlags flag, NetworkObjectHeaderPlayerDataFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectMetaFlags flag, NetworkObjectMetaFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectMetaFlags flag, NetworkObjectMetaFlags value, NetworkObjectMetaFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectMetaFlags flag, NetworkObjectMetaFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectMetaFlags flag, NetworkObjectMetaFlags value, NetworkObjectMetaFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkObjectMetaFlags flag, NetworkObjectMetaFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkObjectMetaFlags flag, NetworkObjectMetaFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectPacketFlags flag, NetworkObjectPacketFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectPacketFlags flag, NetworkObjectPacketFlags value, NetworkObjectPacketFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectPacketFlags flag, NetworkObjectPacketFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectPacketFlags flag, NetworkObjectPacketFlags value, NetworkObjectPacketFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkObjectPacketFlags flag, NetworkObjectPacketFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkObjectPacketFlags flag, NetworkObjectPacketFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectRuntimeFlags flag, NetworkObjectRuntimeFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectRuntimeFlags flag, NetworkObjectRuntimeFlags value, NetworkObjectRuntimeFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectRuntimeFlags flag, NetworkObjectRuntimeFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectRuntimeFlags flag, NetworkObjectRuntimeFlags value, NetworkObjectRuntimeFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkObjectRuntimeFlags flag, NetworkObjectRuntimeFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkObjectRuntimeFlags flag, NetworkObjectRuntimeFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkRunnerFlags flag, NetworkRunnerFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkRunnerFlags flag, NetworkRunnerFlags value, NetworkRunnerFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkRunnerFlags flag, NetworkRunnerFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkRunnerFlags flag, NetworkRunnerFlags value, NetworkRunnerFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkRunnerFlags flag, NetworkRunnerFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkRunnerFlags flag, NetworkRunnerFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkLoadSceneParametersFlags flag, NetworkLoadSceneParametersFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkLoadSceneParametersFlags flag, NetworkLoadSceneParametersFlags value, NetworkLoadSceneParametersFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkLoadSceneParametersFlags flag, NetworkLoadSceneParametersFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkLoadSceneParametersFlags flag, NetworkLoadSceneParametersFlags value, NetworkLoadSceneParametersFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkLoadSceneParametersFlags flag, NetworkLoadSceneParametersFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkLoadSceneParametersFlags flag, NetworkLoadSceneParametersFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this SimulationMessageHeaderFlags flag, SimulationMessageHeaderFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this SimulationMessageHeaderFlags flag, SimulationMessageHeaderFlags value, SimulationMessageHeaderFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this SimulationMessageHeaderFlags flag, SimulationMessageHeaderFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this SimulationMessageHeaderFlags flag, SimulationMessageHeaderFlags value, SimulationMessageHeaderFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this SimulationMessageHeaderFlags flag, SimulationMessageHeaderFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this SimulationMessageHeaderFlags flag, SimulationMessageHeaderFlags value)
		{
			return (flag & value) == 0;
		}
	}
}
