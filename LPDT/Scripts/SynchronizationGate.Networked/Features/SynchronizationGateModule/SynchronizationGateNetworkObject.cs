using System;
using System.Collections.Generic;
using Fusion;

namespace Features.SynchronizationGateModule
{
	[NetworkBehaviourWeaved(320)]
	public class SynchronizationGateNetworkObject : NetworkBehaviour, ISynchronizationGateLanes
	{
		public const int LANE_CAPACITY = 64;

		[WeaverGenerated]
		[DefaultForProperty("CurrentVisit", 0, 64)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int[] _CurrentVisit;

		[WeaverGenerated]
		[DefaultForProperty("PassedOnVisit", 64, 64)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int[] _PassedOnVisit;

		[WeaverGenerated]
		[DefaultForProperty("WaitStartTickPlusOne", 128, 64)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int[] _WaitStartTickPlusOne;

		[WeaverGenerated]
		[DefaultForProperty("WindowTicks", 192, 64)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int[] _WindowTicks;

		[WeaverGenerated]
		[DefaultForProperty("OpenedOnEpoch", 256, 64)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int[] _OpenedOnEpoch;

		private readonly Dictionary<int, int> _pendingStepOpens = new Dictionary<int, int>();

		private SynchronizationGateAuthority _authority;

		[Networked]
		[Capacity(64)]
		[NetworkedWeaved(0, 64)]
		[NetworkedWeavedArray(64, 1, typeof(ElementReaderWriterUnmanaged<int, MetaConstant1>))]
		private unsafe NetworkArray<int> CurrentVisit
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateNetworkObject.CurrentVisit. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<int>((byte*)Ptr + 0, 64, ElementReaderWriterUnmanaged<int, MetaConstant1>.GetInstance());
			}
		}

		[Networked]
		[Capacity(64)]
		[NetworkedWeaved(64, 64)]
		[NetworkedWeavedArray(64, 1, typeof(ElementReaderWriterUnmanaged<int, MetaConstant1>))]
		private unsafe NetworkArray<int> PassedOnVisit
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateNetworkObject.PassedOnVisit. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<int>((byte*)Ptr + 256, 64, ElementReaderWriterUnmanaged<int, MetaConstant1>.GetInstance());
			}
		}

		[Networked]
		[Capacity(64)]
		[NetworkedWeaved(128, 64)]
		[NetworkedWeavedArray(64, 1, typeof(ElementReaderWriterUnmanaged<int, MetaConstant1>))]
		private unsafe NetworkArray<int> WaitStartTickPlusOne
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateNetworkObject.WaitStartTickPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<int>((byte*)Ptr + 512, 64, ElementReaderWriterUnmanaged<int, MetaConstant1>.GetInstance());
			}
		}

		[Networked]
		[Capacity(64)]
		[NetworkedWeaved(192, 64)]
		[NetworkedWeavedArray(64, 1, typeof(ElementReaderWriterUnmanaged<int, MetaConstant1>))]
		private unsafe NetworkArray<int> WindowTicks
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateNetworkObject.WindowTicks. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<int>((byte*)Ptr + 768, 64, ElementReaderWriterUnmanaged<int, MetaConstant1>.GetInstance());
			}
		}

		[Networked]
		[Capacity(64)]
		[NetworkedWeaved(256, 64)]
		[NetworkedWeavedArray(64, 1, typeof(ElementReaderWriterUnmanaged<int, MetaConstant1>))]
		private unsafe NetworkArray<int> OpenedOnEpoch
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateNetworkObject.OpenedOnEpoch. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<int>((byte*)Ptr + 1024, 64, ElementReaderWriterUnmanaged<int, MetaConstant1>.GetInstance());
			}
		}

		public int CurrentTick => base.Runner.Tick;

		public int LaneCount => 64;

		public bool IsReady { get; private set; }

		public override void Spawned()
		{
			_authority = new SynchronizationGateAuthority(base.Runner, this);
			_pendingStepOpens.Clear();
			IsReady = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			IsReady = false;
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				_authority.ResolveLanes();
				DrainPendingStepOpens();
			}
		}

		public bool AdvanceVisit(int lane)
		{
			if (!base.HasStateAuthority || lane < 0 || lane >= 64)
			{
				return false;
			}
			CurrentVisit.Set(lane, CurrentVisit[lane] + 1);
			return true;
		}

		public bool SetVisit(int lane, int visit)
		{
			if (!base.HasStateAuthority || lane < 0 || lane >= 64)
			{
				return false;
			}
			if (visit > CurrentVisit[lane])
			{
				CurrentVisit.Set(lane, visit);
			}
			return true;
		}

		public int GetCurrentVisit(int lane)
		{
			return CurrentVisit[lane];
		}

		public int GetPassedOnVisit(int lane)
		{
			return PassedOnVisit[lane];
		}

		public void SetPassedOnVisit(int lane, int visit)
		{
			PassedOnVisit.Set(lane, visit);
		}

		public int GetWindowTicks(int lane)
		{
			return WindowTicks[lane];
		}

		public int GetWaitStartTick(int lane)
		{
			return WaitStartTickPlusOne[lane] - 1;
		}

		public void SetWaitStartTick(int lane, int tick)
		{
			WaitStartTickPlusOne.Set(lane, (tick >= 0) ? (tick + 1) : 0);
		}

		public bool SetWindowTicks(int lane, int windowTicks)
		{
			if (!base.HasStateAuthority || lane < 0 || lane >= 64)
			{
				return false;
			}
			WindowTicks.Set(lane, windowTicks);
			return true;
		}

		public bool IsPassed(int lane)
		{
			return SynchronizationGateResolver.IsPassed(CurrentVisit[lane], PassedOnVisit[lane]);
		}

		public int CountPresentParticipants()
		{
			return _authority.CountPresentParticipants();
		}

		public int CountArrivals(int lane)
		{
			return _authority.CountArrivals(lane);
		}

		public bool IsStepOpen(int step, int epoch)
		{
			if (step < 0 || step >= 64)
			{
				return false;
			}
			return AuthorityGateResolver.IsOpen(OpenedOnEpoch[step], epoch);
		}

		public int GetOpenedOnEpoch(int step)
		{
			if (step < 0 || step >= 64)
			{
				return 0;
			}
			return OpenedOnEpoch[step];
		}

		public void RequestStepOpen(int step, int epoch)
		{
			if (step >= 0 && step < 64 && (!_pendingStepOpens.TryGetValue(step, out var value) || value < epoch))
			{
				_pendingStepOpens[step] = epoch;
			}
		}

		private void DrainPendingStepOpens()
		{
			if (_pendingStepOpens.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<int, int> pendingStepOpen in _pendingStepOpens)
			{
				int num = AuthorityGateResolver.NextOpened(OpenedOnEpoch[pendingStepOpen.Key], pendingStepOpen.Value);
				if (num != OpenedOnEpoch[pendingStepOpen.Key])
				{
					OpenedOnEpoch.Set(pendingStepOpen.Key, num);
				}
			}
			_pendingStepOpens.Clear();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkBehaviourUtils.InitializeNetworkArray(CurrentVisit, _CurrentVisit, "CurrentVisit");
			NetworkBehaviourUtils.InitializeNetworkArray(PassedOnVisit, _PassedOnVisit, "PassedOnVisit");
			NetworkBehaviourUtils.InitializeNetworkArray(WaitStartTickPlusOne, _WaitStartTickPlusOne, "WaitStartTickPlusOne");
			NetworkBehaviourUtils.InitializeNetworkArray(WindowTicks, _WindowTicks, "WindowTicks");
			NetworkBehaviourUtils.InitializeNetworkArray(OpenedOnEpoch, _OpenedOnEpoch, "OpenedOnEpoch");
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			NetworkBehaviourUtils.CopyFromNetworkArray(CurrentVisit, ref _CurrentVisit);
			NetworkBehaviourUtils.CopyFromNetworkArray(PassedOnVisit, ref _PassedOnVisit);
			NetworkBehaviourUtils.CopyFromNetworkArray(WaitStartTickPlusOne, ref _WaitStartTickPlusOne);
			NetworkBehaviourUtils.CopyFromNetworkArray(WindowTicks, ref _WindowTicks);
			NetworkBehaviourUtils.CopyFromNetworkArray(OpenedOnEpoch, ref _OpenedOnEpoch);
		}
	}
}
