using System.Runtime.CompilerServices;

namespace Fusion
{
	public static class GenerateFlagsExtensionsExtensions_public
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkTransform.NetworkTransformFlags flag, NetworkTransform.NetworkTransformFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkTransform.NetworkTransformFlags flag, NetworkTransform.NetworkTransformFlags value, NetworkTransform.NetworkTransformFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkTransform.NetworkTransformFlags flag, NetworkTransform.NetworkTransformFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkTransform.NetworkTransformFlags flag, NetworkTransform.NetworkTransformFlags value, NetworkTransform.NetworkTransformFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkTransform.NetworkTransformFlags flag, NetworkTransform.NetworkTransformFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkTransform.NetworkTransformFlags flag, NetworkTransform.NetworkTransformFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkRigidbodyFlags flag, NetworkRigidbodyFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkRigidbodyFlags flag, NetworkRigidbodyFlags value, NetworkRigidbodyFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkRigidbodyFlags flag, NetworkRigidbodyFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkRigidbodyFlags flag, NetworkRigidbodyFlags value, NetworkRigidbodyFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkRigidbodyFlags flag, NetworkRigidbodyFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkRigidbodyFlags flag, NetworkRigidbodyFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this HitOptions flag, HitOptions value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this HitOptions flag, HitOptions value, HitOptions mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this HitOptions flag, HitOptions value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this HitOptions flag, HitOptions value, HitOptions mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this HitOptions flag, HitOptions value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this HitOptions flag, HitOptions value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this HitboxRoot.ConfigFlags flag, HitboxRoot.ConfigFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this HitboxRoot.ConfigFlags flag, HitboxRoot.ConfigFlags value, HitboxRoot.ConfigFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this HitboxRoot.ConfigFlags flag, HitboxRoot.ConfigFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this HitboxRoot.ConfigFlags flag, HitboxRoot.ConfigFlags value, HitboxRoot.ConfigFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this HitboxRoot.ConfigFlags flag, HitboxRoot.ConfigFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this HitboxRoot.ConfigFlags flag, HitboxRoot.ConfigFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectFlags flag, NetworkObjectFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectFlags flag, NetworkObjectFlags value, NetworkObjectFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectFlags flag, NetworkObjectFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectFlags flag, NetworkObjectFlags value, NetworkObjectFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkObjectFlags flag, NetworkObjectFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkObjectFlags flag, NetworkObjectFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectHeaderFlags flag, NetworkObjectHeaderFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkObjectHeaderFlags flag, NetworkObjectHeaderFlags value, NetworkObjectHeaderFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectHeaderFlags flag, NetworkObjectHeaderFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkObjectHeaderFlags flag, NetworkObjectHeaderFlags value, NetworkObjectHeaderFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkObjectHeaderFlags flag, NetworkObjectHeaderFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkObjectHeaderFlags flag, NetworkObjectHeaderFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this RpcLocalInvokeResult flag, RpcLocalInvokeResult value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this RpcLocalInvokeResult flag, RpcLocalInvokeResult value, RpcLocalInvokeResult mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this RpcLocalInvokeResult flag, RpcLocalInvokeResult value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this RpcLocalInvokeResult flag, RpcLocalInvokeResult value, RpcLocalInvokeResult mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this RpcLocalInvokeResult flag, RpcLocalInvokeResult value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this RpcLocalInvokeResult flag, RpcLocalInvokeResult value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this RpcSendMessageResult flag, RpcSendMessageResult value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this RpcSendMessageResult flag, RpcSendMessageResult value, RpcSendMessageResult mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this RpcSendMessageResult flag, RpcSendMessageResult value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this RpcSendMessageResult flag, RpcSendMessageResult value, RpcSendMessageResult mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this RpcSendMessageResult flag, RpcSendMessageResult value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this RpcSendMessageResult flag, RpcSendMessageResult value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this RpcSources flag, RpcSources value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this RpcSources flag, RpcSources value, RpcSources mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this RpcSources flag, RpcSources value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this RpcSources flag, RpcSources value, RpcSources mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this RpcSources flag, RpcSources value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this RpcSources flag, RpcSources value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this RpcTargets flag, RpcTargets value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this RpcTargets flag, RpcTargets value, RpcTargets mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this RpcTargets flag, RpcTargets value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this RpcTargets flag, RpcTargets value, RpcTargets mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this RpcTargets flag, RpcTargets value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this RpcTargets flag, RpcTargets value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkSpawnFlags flag, NetworkSpawnFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkSpawnFlags flag, NetworkSpawnFlags value, NetworkSpawnFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkSpawnFlags flag, NetworkSpawnFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkSpawnFlags flag, NetworkSpawnFlags value, NetworkSpawnFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkSpawnFlags flag, NetworkSpawnFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkSpawnFlags flag, NetworkSpawnFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkSceneInfoDefaultFlags flag, NetworkSceneInfoDefaultFlags value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkSceneInfoDefaultFlags flag, NetworkSceneInfoDefaultFlags value, NetworkSceneInfoDefaultFlags mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkSceneInfoDefaultFlags flag, NetworkSceneInfoDefaultFlags value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkSceneInfoDefaultFlags flag, NetworkSceneInfoDefaultFlags value, NetworkSceneInfoDefaultFlags mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkSceneInfoDefaultFlags flag, NetworkSceneInfoDefaultFlags value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkSceneInfoDefaultFlags flag, NetworkSceneInfoDefaultFlags value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkSceneInfoChangeSource flag, NetworkSceneInfoChangeSource value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkSceneInfoChangeSource flag, NetworkSceneInfoChangeSource value, NetworkSceneInfoChangeSource mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkSceneInfoChangeSource flag, NetworkSceneInfoChangeSource value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkSceneInfoChangeSource flag, NetworkSceneInfoChangeSource value, NetworkSceneInfoChangeSource mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkSceneInfoChangeSource flag, NetworkSceneInfoChangeSource value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkSceneInfoChangeSource flag, NetworkSceneInfoChangeSource value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this Topologies flag, Topologies value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this Topologies flag, Topologies value, Topologies mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this Topologies flag, Topologies value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this Topologies flag, Topologies value, Topologies mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this Topologies flag, Topologies value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this Topologies flag, Topologies value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkConfiguration.ReliableDataTransfers flag, NetworkConfiguration.ReliableDataTransfers value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this NetworkConfiguration.ReliableDataTransfers flag, NetworkConfiguration.ReliableDataTransfers value, NetworkConfiguration.ReliableDataTransfers mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkConfiguration.ReliableDataTransfers flag, NetworkConfiguration.ReliableDataTransfers value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this NetworkConfiguration.ReliableDataTransfers flag, NetworkConfiguration.ReliableDataTransfers value, NetworkConfiguration.ReliableDataTransfers mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this NetworkConfiguration.ReliableDataTransfers flag, NetworkConfiguration.ReliableDataTransfers value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this NetworkConfiguration.ReliableDataTransfers flag, NetworkConfiguration.ReliableDataTransfers value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this SimulationModes flag, SimulationModes value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this SimulationModes flag, SimulationModes value, SimulationModes mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this SimulationModes flag, SimulationModes value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this SimulationModes flag, SimulationModes value, SimulationModes mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this SimulationModes flag, SimulationModes value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this SimulationModes flag, SimulationModes value)
		{
			return (flag & value) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this SimulationStages flag, SimulationStages value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this SimulationStages flag, SimulationStages value, SimulationStages mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this SimulationStages flag, SimulationStages value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this SimulationStages flag, SimulationStages value, SimulationStages mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this SimulationStages flag, SimulationStages value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this SimulationStages flag, SimulationStages value)
		{
			return (flag & value) == 0;
		}
	}
}
