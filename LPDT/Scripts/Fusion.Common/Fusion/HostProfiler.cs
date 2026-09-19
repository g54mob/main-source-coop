using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IL2CPP.CompilerServices;
using Unity.Profiling;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;

namespace Fusion
{
	[Il2CppEagerStaticClassConstruction]
	public sealed class HostProfiler
	{
		public static class Markers
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope InvokeFixedUpdateNetwork()
			{
				return _markerInvokeFixedUpdateNetwork.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope InvokeOnBeforeHitboxRegistration()
			{
				return _markerInvokeOnBeforeHitboxRegistration.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope ClientBeforeSimulation()
			{
				return _markerClientBeforeSimulation.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope RunClientSideResimulationLoop()
			{
				return _markerRunClientSideResimulationLoop.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope NetworkSend()
			{
				return _markerNetworkSend.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope NetworkRecv()
			{
				return _markerNetworkRecv.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope DeliverMessages()
			{
				return _markerDeliverMessages.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope WriteUsingAreaOfInterest()
			{
				return _markerWriteUsingAreaOfInterest.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope WriteUsingAllObjects()
			{
				return _markerWriteUsingAllObjects.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope StepSimulation()
			{
				return _markerStepSimulation.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope InvokeOnBeforeSimulation()
			{
				return _markerInvokeOnBeforeSimulation.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope InvokeOnAfterSimulation()
			{
				return _markerInvokeOnAfterSimulation.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope InvokeOnBeforeAllTicks()
			{
				return _markerInvokeOnBeforeAllTicks.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope InvokeOnAfterAllTicks()
			{
				return _markerInvokeOnAfterAllTicks.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope SimulationUpdate()
			{
				return _markerSimulationUpdate.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope BeforeSimulation()
			{
				return _markerBeforeSimulation.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope AfterSimulation()
			{
				return _markerAfterSimulation.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope UpdateAreaOfInterest()
			{
				return _markerUpdateAreaOfInterest.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope PreparePackets()
			{
				return _markerPreparePackets.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope SendPackets()
			{
				return _markerSendPackets.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope SimulationBeforeTick()
			{
				return _markerSimulationBeforeTick.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope SimulationAfterTick()
			{
				return _markerSimulationAfterTick.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope EncryptionWrap()
			{
				return _markerEncryptionWrap.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope EncryptionUnwrap()
			{
				return _markerEncryptionUnwrap.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope ReplaceDataFromBlockWithTemp()
			{
				return _markerReplaceDataFromBlockWithTemp.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope EncryptionSocketReceive()
			{
				return _markerEncryptionSocketReceive.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope EncryptionSocketSend()
			{
				return _markerEncryptionSocketSend.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope RpcBuilderDispose()
			{
				return _markerRpcBuilderDispose.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope RpcBuilderSend()
			{
				return _markerRpcBuilderSend.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope RpcBuilderPrepare()
			{
				return _markerRpcBuilderPrepare.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope UpdateRemotePrefabs()
			{
				return _markerUpdateRemotePrefabs.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope INetworkObjectProviderAcquireInstance()
			{
				return _markerINetworkObjectProviderAcquireInstance.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope InitializeNetworkObjectInstance()
			{
				return _markerInitializeNetworkObjectInstance.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope BehaviourUpdaterAddObject()
			{
				return _markerBehaviourUpdaterAddObject.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope UpdateObjectTimelines()
			{
				return _markerUpdateObjectTimelines.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope UpdateInterpolationParams()
			{
				return _markerUpdateInterpolationParams.Start();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HostProfilerMarkerScope UpdateObjectInterpolationParams()
			{
				return _markerUpdateObjectInterpolationParams.Start();
			}
		}

		public static class Counters
		{
			internal static HostProfilerCounter<float>? InterpolationOffset
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInterpolationOffset : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? InterpolationOffsetDeviation
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInterpolationOffsetDeviation : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? RoundTripTime
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterRoundTripTime : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? InputRecvDelta
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInputRecvDelta : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? InputRecvDeltaDeviation
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInputRecvDeltaDeviation : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? StateRecvDelta
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterStateRecvDelta : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? StateRecvDeltaDeviation
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterStateRecvDeltaDeviation : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? SimulationInputDelay
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationInputDelay : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? SimulationOffset
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationOffset : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? SimulationOffsetDeviation
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationOffsetDeviation : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? SimulationSpeed
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationSpeed : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<float>? InterpolationSpeed
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInterpolationSpeed : default(HostProfilerCounter<float>);
				}
			}

			internal static HostProfilerCounter<int>? RpcOut
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterRpcOut : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? RpcBytesOut
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterRpcBytesOut : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? ObjectsOut
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterObjectsOut : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? ObjectsBytesOut
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterObjectsBytesOut : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? DestroysOut
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterDestroysOut : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? InputSize
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInputSize : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? FragmentsOut
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterFragmentsOut : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? BytesOut
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterBytesOut : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? InputQueue
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInputQueue : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? BytesQueued
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterBytesQueued : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? FragmentsQueued
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterFragmentsQueued : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? FragmentsLost
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterFragmentsLost : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? FragmentsDelivered
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterFragmentsDelivered : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? RpcIn
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterRpcIn : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? RpcBytesIn
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterRpcBytesIn : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? ObjectsIn
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterObjectsIn : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? ObjectsBytesIn
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterObjectsBytesIn : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? DestroysIn
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterDestroysIn : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? InputSizeIn
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInputSizeIn : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? FragmentsIn
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterFragmentsIn : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? BytesIn
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterBytesIn : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? SimulationMessageActive
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationMessageActive : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? SimulationPacketEnvelopeActive
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationPacketEnvelopeActive : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? NetworkObjectMetaActive
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterNetworkObjectMetaActive : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? NetworkObjectHeaderSnapshotActive
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterNetworkObjectHeaderSnapshotActive : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? AreaOfInterestCellActive
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterAreaOfInterestCellActive : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? SimulationInputActive
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationInputActive : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? ByteLists
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterByteLists : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? SimulationConnectionLists
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationConnectionLists : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? NetworkObjectPacketDataLists
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterNetworkObjectPacketDataLists : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? SimulationMessagePacketDataLists
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterSimulationMessagePacketDataLists : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? ObjectDataBytes
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterObjectDataBytes : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? ObjectChangesBytes
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterObjectChangesBytes : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? ObjectSnapshotBytes
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterObjectSnapshotBytes : default(HostProfilerCounter<int>);
				}
			}

			internal static HostProfilerCounter<int>? InputDataBytes
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return IsEnabled ? _counterInputDataBytes : default(HostProfilerCounter<int>);
				}
			}
		}

		private static readonly bool IsEnabled;

		private static readonly HostProfilerCounter<float> _counterInterpolationOffset;

		private static readonly HostProfilerCounter<float> _counterInterpolationOffsetDeviation;

		private static readonly HostProfilerCounter<float> _counterRoundTripTime;

		private static readonly HostProfilerCounter<float> _counterInputRecvDelta;

		private static readonly HostProfilerCounter<float> _counterInputRecvDeltaDeviation;

		private static readonly HostProfilerCounter<float> _counterStateRecvDelta;

		private static readonly HostProfilerCounter<float> _counterStateRecvDeltaDeviation;

		private static readonly HostProfilerCounter<float> _counterSimulationInputDelay;

		private static readonly HostProfilerCounter<float> _counterSimulationOffset;

		private static readonly HostProfilerCounter<float> _counterSimulationOffsetDeviation;

		private static readonly HostProfilerCounter<float> _counterSimulationSpeed;

		private static readonly HostProfilerCounter<float> _counterInterpolationSpeed;

		private static readonly HostProfilerCounter<int> _counterRpcOut;

		private static readonly HostProfilerCounter<int> _counterRpcBytesOut;

		private static readonly HostProfilerCounter<int> _counterObjectsOut;

		private static readonly HostProfilerCounter<int> _counterObjectsBytesOut;

		private static readonly HostProfilerCounter<int> _counterDestroysOut;

		private static readonly HostProfilerCounter<int> _counterInputSize;

		private static readonly HostProfilerCounter<int> _counterFragmentsOut;

		private static readonly HostProfilerCounter<int> _counterBytesOut;

		private static readonly HostProfilerCounter<int> _counterInputQueue;

		private static readonly HostProfilerCounter<int> _counterBytesQueued;

		private static readonly HostProfilerCounter<int> _counterFragmentsQueued;

		private static readonly HostProfilerCounter<int> _counterFragmentsLost;

		private static readonly HostProfilerCounter<int> _counterFragmentsDelivered;

		private static readonly HostProfilerCounter<int> _counterRpcIn;

		private static readonly HostProfilerCounter<int> _counterRpcBytesIn;

		private static readonly HostProfilerCounter<int> _counterObjectsIn;

		private static readonly HostProfilerCounter<int> _counterObjectsBytesIn;

		private static readonly HostProfilerCounter<int> _counterDestroysIn;

		private static readonly HostProfilerCounter<int> _counterInputSizeIn;

		private static readonly HostProfilerCounter<int> _counterFragmentsIn;

		private static readonly HostProfilerCounter<int> _counterBytesIn;

		private static readonly HostProfilerCounter<int> _counterSimulationMessageActive;

		private static readonly HostProfilerCounter<int> _counterSimulationPacketEnvelopeActive;

		private static readonly HostProfilerCounter<int> _counterNetworkObjectMetaActive;

		private static readonly HostProfilerCounter<int> _counterNetworkObjectHeaderSnapshotActive;

		private static readonly HostProfilerCounter<int> _counterAreaOfInterestCellActive;

		private static readonly HostProfilerCounter<int> _counterSimulationInputActive;

		private static readonly HostProfilerCounter<int> _counterByteLists;

		private static readonly HostProfilerCounter<int> _counterSimulationConnectionLists;

		private static readonly HostProfilerCounter<int> _counterNetworkObjectPacketDataLists;

		private static readonly HostProfilerCounter<int> _counterSimulationMessagePacketDataLists;

		private static readonly HostProfilerCounter<int> _counterObjectDataBytes;

		private static readonly HostProfilerCounter<int> _counterObjectChangesBytes;

		private static readonly HostProfilerCounter<int> _counterObjectSnapshotBytes;

		private static readonly HostProfilerCounter<int> _counterInputDataBytes;

		private static readonly HostProfilerMarker _markerInvokeFixedUpdateNetwork;

		private static readonly HostProfilerMarker _markerInvokeOnBeforeHitboxRegistration;

		private static readonly HostProfilerMarker _markerClientBeforeSimulation;

		private static readonly HostProfilerMarker _markerRunClientSideResimulationLoop;

		private static readonly HostProfilerMarker _markerNetworkSend;

		private static readonly HostProfilerMarker _markerNetworkRecv;

		private static readonly HostProfilerMarker _markerDeliverMessages;

		private static readonly HostProfilerMarker _markerWriteUsingAreaOfInterest;

		private static readonly HostProfilerMarker _markerWriteUsingAllObjects;

		private static readonly HostProfilerMarker _markerStepSimulation;

		private static readonly HostProfilerMarker _markerInvokeOnBeforeSimulation;

		private static readonly HostProfilerMarker _markerInvokeOnAfterSimulation;

		private static readonly HostProfilerMarker _markerInvokeOnBeforeAllTicks;

		private static readonly HostProfilerMarker _markerInvokeOnAfterAllTicks;

		private static readonly HostProfilerMarker _markerSimulationUpdate;

		private static readonly HostProfilerMarker _markerBeforeSimulation;

		private static readonly HostProfilerMarker _markerAfterSimulation;

		private static readonly HostProfilerMarker _markerUpdateAreaOfInterest;

		private static readonly HostProfilerMarker _markerPreparePackets;

		private static readonly HostProfilerMarker _markerSendPackets;

		private static readonly HostProfilerMarker _markerSimulationBeforeTick;

		private static readonly HostProfilerMarker _markerSimulationAfterTick;

		private static readonly HostProfilerMarker _markerEncryptionWrap;

		private static readonly HostProfilerMarker _markerEncryptionUnwrap;

		private static readonly HostProfilerMarker _markerReplaceDataFromBlockWithTemp;

		private static readonly HostProfilerMarker _markerEncryptionSocketReceive;

		private static readonly HostProfilerMarker _markerEncryptionSocketSend;

		private static readonly HostProfilerMarker _markerRpcBuilderDispose;

		private static readonly HostProfilerMarker _markerRpcBuilderSend;

		private static readonly HostProfilerMarker _markerRpcBuilderPrepare;

		private static readonly HostProfilerMarker _markerUpdateRemotePrefabs;

		private static readonly HostProfilerMarker _markerINetworkObjectProviderAcquireInstance;

		private static readonly HostProfilerMarker _markerInitializeNetworkObjectInstance;

		private static readonly HostProfilerMarker _markerBehaviourUpdaterAddObject;

		private static readonly HostProfilerMarker _markerUpdateObjectTimelines;

		private static readonly HostProfilerMarker _markerUpdateInterpolationParams;

		private static readonly HostProfilerMarker _markerUpdateObjectInterpolationParams;

		[Obsolete("No longer supported")]
		public static HostProfilerMarkerScope Start(string name)
		{
			return default(HostProfilerMarkerScope);
		}

		[Obsolete("No longer supported")]
		public static HostProfilerMarkerScope Begin(string name)
		{
			return default(HostProfilerMarkerScope);
		}

		[Obsolete("No longer supported")]
		public static void End()
		{
		}

		public static HostProfilerMarker CreateMarker(string name, in HostProfilerCategory? category = null)
		{
			if (!IsEnabled)
			{
				return default(HostProfilerMarker);
			}
			return InternalCreateMarker(name, category ?? InternalGetDefaultCategory());
		}

		public static HostProfilerCounter<T> CreateCounter<T>(string name, HostProfilerDataUnit unit, in HostProfilerCategory? category = null, bool autoReset = false) where T : unmanaged
		{
			if (!IsEnabled)
			{
				return default(HostProfilerCounter<T>);
			}
			return InternalCreateCounter<T>(name, unit, category ?? InternalGetDefaultCategory(), autoReset);
		}

		public static HostProfilerCategory CreateCategory(string name)
		{
			if (!IsEnabled)
			{
				return InternalGetDefaultCategory();
			}
			return InternalCreateCategory(name);
		}

		private static bool InternalCheckIfEnabled()
		{
			return FusionPlatform.GetAnyLoadedAssemblyAttribute<MarkProfilerAsEnabledIfEnableProfilerDefinedAttribute>() != null;
		}

		private static HostProfilerMarker InternalCreateMarker(string name, in HostProfilerCategory category)
		{
			return new HostProfilerMarker(new ProfilerMarker(category.InternalCategory, name).Handle);
		}

		private unsafe static HostProfilerCounter<T> InternalCreateCounter<T>(string name, HostProfilerDataUnit unit, in HostProfilerCategory category, bool autoReset) where T : unmanaged
		{
			ProfilerCounterOptions counterOptions = (ProfilerCounterOptions)(2 | (autoReset ? 4 : 0));
			IntPtr counterPtr;
			void* ptr = ProfilerUnsafeUtility.CreateCounterValue(out counterPtr, name, category.InternalCategory, MarkerFlags.Default, (byte)GetMarkerDataType<T>(), (byte)GetProfilerMarkerDataUnit(unit), UnsafeUtility.SizeOf<T>(), counterOptions);
			return new HostProfilerCounter<T>(ptr);
		}

		private static HostProfilerCategory InternalCreateCategory(string name)
		{
			ProfilerCategory category = new ProfilerCategory(name);
			return new HostProfilerCategory(category);
		}

		private static HostProfilerCategory InternalGetDefaultCategory()
		{
			return new HostProfilerCategory(ProfilerCategory.Scripts);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ProfilerMarkerDataType GetMarkerDataType<T>()
		{
			if (typeof(T) == typeof(float))
			{
				return ProfilerMarkerDataType.Float;
			}
			if (typeof(T) == typeof(double))
			{
				return ProfilerMarkerDataType.Double;
			}
			return ProfilerMarkerDataType.Int32;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ProfilerMarkerDataUnit GetProfilerMarkerDataUnit(HostProfilerDataUnit dataUnit)
		{
			if (1 == 0)
			{
			}
			ProfilerMarkerDataUnit result = dataUnit switch
			{
				HostProfilerDataUnit.Bytes => ProfilerMarkerDataUnit.Bytes, 
				HostProfilerDataUnit.Count => ProfilerMarkerDataUnit.Count, 
				HostProfilerDataUnit.Percent => ProfilerMarkerDataUnit.Percent, 
				HostProfilerDataUnit.FrequencyHz => ProfilerMarkerDataUnit.FrequencyHz, 
				HostProfilerDataUnit.TimeNanoseconds => ProfilerMarkerDataUnit.TimeNanoseconds, 
				_ => ProfilerMarkerDataUnit.Undefined, 
			};
			if (1 == 0)
			{
			}
			return result;
		}

		static HostProfiler()
		{
			IsEnabled = InternalCheckIfEnabled();
			if (IsEnabled)
			{
				HostProfilerCategory category = InternalCreateCategory("Fusion");
				_counterInterpolationOffset = InternalCreateCounter<float>("Stats/Interp Offset", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterInterpolationOffsetDeviation = InternalCreateCounter<float>("Stats/interp Offset Dev", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterRoundTripTime = InternalCreateCounter<float>("Stats/Client RTT", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterInputRecvDelta = InternalCreateCounter<float>("Stats/Input Recv Delta", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterInputRecvDeltaDeviation = InternalCreateCounter<float>("Stats/Input Recv Delta Dev", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterStateRecvDelta = InternalCreateCounter<float>("Stats/State Recv Delta", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterStateRecvDeltaDeviation = InternalCreateCounter<float>("Stats/State Recv Delta Dev", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterSimulationInputDelay = InternalCreateCounter<float>("Stats/Simulation Input Delay", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterSimulationOffset = InternalCreateCounter<float>("Stats/Simulation Offset", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterSimulationOffsetDeviation = InternalCreateCounter<float>("Stats/Simulation Offset Dev", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterSimulationSpeed = InternalCreateCounter<float>("Stats/Simulation Speed", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterInterpolationSpeed = InternalCreateCounter<float>("Stats/Interpolation Speed", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterRpcOut = InternalCreateCounter<int>("Send/RPCs", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterRpcBytesOut = InternalCreateCounter<int>("Send/RPC Bytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterObjectsOut = InternalCreateCounter<int>("Send/Objects", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterObjectsBytesOut = InternalCreateCounter<int>("Send/Objects Bytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterDestroysOut = InternalCreateCounter<int>("Send/Destroys", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterInputSize = InternalCreateCounter<int>("Send/Input Bytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterFragmentsOut = InternalCreateCounter<int>("Send/Fragments Out", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterBytesOut = InternalCreateCounter<int>("Send/Bytes Out", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterInputQueue = InternalCreateCounter<int>("Send/Input Queue", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterBytesQueued = InternalCreateCounter<int>("Send/Bytes Queued", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterFragmentsQueued = InternalCreateCounter<int>("Send/Fragments Queued", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterFragmentsLost = InternalCreateCounter<int>("Send/Fragments Lost", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterFragmentsDelivered = InternalCreateCounter<int>("Send/Fragments Delivered", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterRpcIn = InternalCreateCounter<int>("Recv/RPCs", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterRpcBytesIn = InternalCreateCounter<int>("Recv/RPC Bytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterObjectsIn = InternalCreateCounter<int>("Recv/Objects", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterObjectsBytesIn = InternalCreateCounter<int>("Recv/Objects Bytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterDestroysIn = InternalCreateCounter<int>("Recv/Destroys", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterInputSizeIn = InternalCreateCounter<int>("Recv/Input Bytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterFragmentsIn = InternalCreateCounter<int>("Recv/Fragments In", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterBytesIn = InternalCreateCounter<int>("Recv/Bytes In", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterSimulationMessageActive = InternalCreateCounter<int>("Mem/Message", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterSimulationPacketEnvelopeActive = InternalCreateCounter<int>("Mem/Packet", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterNetworkObjectMetaActive = InternalCreateCounter<int>("Mem/ObjectMeta", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterNetworkObjectHeaderSnapshotActive = InternalCreateCounter<int>("Mem/ObjectSnapshot", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterAreaOfInterestCellActive = InternalCreateCounter<int>("Mem/AOICell", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterSimulationInputActive = InternalCreateCounter<int>("Mem/Input", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterByteLists = InternalCreateCounter<int>("Mem/ByteLists", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterSimulationConnectionLists = InternalCreateCounter<int>("Mem/ConnectionLists", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterNetworkObjectPacketDataLists = InternalCreateCounter<int>("Mem/ObjectPacketDataLists", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterSimulationMessagePacketDataLists = InternalCreateCounter<int>("Mem/MessagePacketDataLists", HostProfilerDataUnit.Count, in category, autoReset: true);
				_counterObjectDataBytes = InternalCreateCounter<int>("Mem/ObjectDataBytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterObjectChangesBytes = InternalCreateCounter<int>("Mem/ObjectChangesBytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterObjectSnapshotBytes = InternalCreateCounter<int>("Mem/ObjectSnapshotBytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_counterInputDataBytes = InternalCreateCounter<int>("Mem/InputDataBytes", HostProfilerDataUnit.Bytes, in category, autoReset: true);
				_markerInvokeFixedUpdateNetwork = InternalCreateMarker("SimulationBehaviourUpdater.InvokeFixedUpdateNetwork", in category);
				_markerInvokeOnBeforeHitboxRegistration = InternalCreateMarker("NetworkRunner.InvokeOnBeforeHitboxRegistration", in category);
				_markerClientBeforeSimulation = InternalCreateMarker("Simulation.Client.BeforeSimulation", in category);
				_markerRunClientSideResimulationLoop = InternalCreateMarker("Simulation.Client.RunClientSideResimulationLoop", in category);
				_markerNetworkSend = InternalCreateMarker("Simulation.NetworkSend", in category);
				_markerNetworkRecv = InternalCreateMarker("Simulation.NetworkRecv", in category);
				_markerDeliverMessages = InternalCreateMarker("Simulation.DeliverMessages", in category);
				_markerWriteUsingAreaOfInterest = InternalCreateMarker("WriteUsingAreaOfInterest", in category);
				_markerWriteUsingAllObjects = InternalCreateMarker("WriteUsingAllObjects", in category);
				_markerStepSimulation = InternalCreateMarker("Simulation.StepSimulation", in category);
				_markerInvokeOnBeforeSimulation = InternalCreateMarker("InvokeOnBeforeSimulation", in category);
				_markerInvokeOnAfterSimulation = InternalCreateMarker("InvokeOnAfterSimulation", in category);
				_markerInvokeOnBeforeAllTicks = InternalCreateMarker("InvokeOnBeforeAllTicks", in category);
				_markerInvokeOnAfterAllTicks = InternalCreateMarker("InvokeOnAfterAllTicks", in category);
				_markerSimulationUpdate = InternalCreateMarker("Simulation.Update", in category);
				_markerBeforeSimulation = InternalCreateMarker("BeforeSimulation", in category);
				_markerAfterSimulation = InternalCreateMarker("AfterSimulation", in category);
				_markerUpdateAreaOfInterest = InternalCreateMarker("UpdateAreaOfInterest", in category);
				_markerPreparePackets = InternalCreateMarker("PreparePackets", in category);
				_markerSendPackets = InternalCreateMarker("SendPackets", in category);
				_markerSimulationBeforeTick = InternalCreateMarker("Simulation.BeforeTick", in category);
				_markerSimulationAfterTick = InternalCreateMarker("Simulation.AfterTick", in category);
				_markerEncryptionWrap = InternalCreateMarker("Encryption.Wrap", in category);
				_markerEncryptionUnwrap = InternalCreateMarker("Encryption.Unwrap", in category);
				_markerReplaceDataFromBlockWithTemp = InternalCreateMarker("ReplaceDataFromBlockWithTemp", in category);
				_markerEncryptionSocketReceive = InternalCreateMarker("Encryption.Socket.Receive", in category);
				_markerEncryptionSocketSend = InternalCreateMarker("Encryption.Socket.Send", in category);
				_markerRpcBuilderDispose = InternalCreateMarker("RpcBuilder.Dispose", in category);
				_markerRpcBuilderSend = InternalCreateMarker("RpcBuilder.Send", in category);
				_markerRpcBuilderPrepare = InternalCreateMarker("RpcBuilder.Prepare", in category);
				_markerUpdateRemotePrefabs = InternalCreateMarker("UpdateRemotePrefabs", in category);
				_markerINetworkObjectProviderAcquireInstance = InternalCreateMarker("INetworkObjectProvider.AcquireInstance", in category);
				_markerInitializeNetworkObjectInstance = InternalCreateMarker("InitializeNetworkObjectInstance", in category);
				_markerBehaviourUpdaterAddObject = InternalCreateMarker("SimulationBehaviourUpdater.AddObject", in category);
				_markerUpdateObjectTimelines = InternalCreateMarker("UpdateObjectTimelines", in category);
				_markerUpdateInterpolationParams = InternalCreateMarker("UpdateInterpolationParams", in category);
				_markerUpdateObjectInterpolationParams = InternalCreateMarker("UpdateObjectInterpolationParams", in category);
			}
		}
	}
}
