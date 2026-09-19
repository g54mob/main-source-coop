using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(24)]
	public class LevelPlayersGameStatisticsNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0PlayerIdPlusOne", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0PlayerIdPlusOne;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0Deaths", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0Deaths;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0Kills", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0Kills;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0Revives", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0Revives;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot0Cents", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot0Cents;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1PlayerIdPlusOne", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1PlayerIdPlusOne;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1Deaths", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1Deaths;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1Kills", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1Kills;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1Revives", 8, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1Revives;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot1Cents", 9, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot1Cents;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2PlayerIdPlusOne", 10, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2PlayerIdPlusOne;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2Deaths", 11, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2Deaths;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2Kills", 12, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2Kills;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2Revives", 13, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2Revives;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot2Cents", 14, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot2Cents;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3PlayerIdPlusOne", 15, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3PlayerIdPlusOne;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3Deaths", 16, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3Deaths;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3Kills", 17, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3Kills;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3Revives", 18, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3Revives;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Slot3Cents", 19, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Slot3Cents;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CouldroneCount", 20, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CouldroneCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CartCount", 21, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CartCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("DeadPartCount", 22, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _DeadPartCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SpawnedNewItemId", 23, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _SpawnedNewItemId;

		[Networked]
		[OnChangedRender("OnSlot0PlayerIdPlusOneChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int Slot0PlayerIdPlusOne
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0PlayerIdPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0PlayerIdPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot0DeathsChangedRender")]
		[NetworkedWeaved(1, 1)]
		public unsafe int Slot0Deaths
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0Deaths. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0Deaths. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot0KillsChangedRender")]
		[NetworkedWeaved(2, 1)]
		public unsafe int Slot0Kills
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0Kills. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0Kills. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot0RevivesChangedRender")]
		[NetworkedWeaved(3, 1)]
		public unsafe int Slot0Revives
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0Revives. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0Revives. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot0CentsChangedRender")]
		[NetworkedWeaved(4, 1)]
		public unsafe int Slot0Cents
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0Cents. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[4];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot0Cents. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[4] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1PlayerIdPlusOneChangedRender")]
		[NetworkedWeaved(5, 1)]
		public unsafe int Slot1PlayerIdPlusOne
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1PlayerIdPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[5];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1PlayerIdPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[5] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1DeathsChangedRender")]
		[NetworkedWeaved(6, 1)]
		public unsafe int Slot1Deaths
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1Deaths. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[6];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1Deaths. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[6] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1KillsChangedRender")]
		[NetworkedWeaved(7, 1)]
		public unsafe int Slot1Kills
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1Kills. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[7];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1Kills. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[7] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1RevivesChangedRender")]
		[NetworkedWeaved(8, 1)]
		public unsafe int Slot1Revives
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1Revives. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[8];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1Revives. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[8] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot1CentsChangedRender")]
		[NetworkedWeaved(9, 1)]
		public unsafe int Slot1Cents
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1Cents. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[9];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot1Cents. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[9] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2PlayerIdPlusOneChangedRender")]
		[NetworkedWeaved(10, 1)]
		public unsafe int Slot2PlayerIdPlusOne
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2PlayerIdPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[10];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2PlayerIdPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[10] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2DeathsChangedRender")]
		[NetworkedWeaved(11, 1)]
		public unsafe int Slot2Deaths
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2Deaths. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[11];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2Deaths. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[11] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2KillsChangedRender")]
		[NetworkedWeaved(12, 1)]
		public unsafe int Slot2Kills
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2Kills. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[12];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2Kills. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[12] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2RevivesChangedRender")]
		[NetworkedWeaved(13, 1)]
		public unsafe int Slot2Revives
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2Revives. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[13];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2Revives. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[13] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot2CentsChangedRender")]
		[NetworkedWeaved(14, 1)]
		public unsafe int Slot2Cents
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2Cents. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[14];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot2Cents. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[14] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3PlayerIdPlusOneChangedRender")]
		[NetworkedWeaved(15, 1)]
		public unsafe int Slot3PlayerIdPlusOne
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3PlayerIdPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[15];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3PlayerIdPlusOne. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[15] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3DeathsChangedRender")]
		[NetworkedWeaved(16, 1)]
		public unsafe int Slot3Deaths
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3Deaths. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[16];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3Deaths. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[16] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3KillsChangedRender")]
		[NetworkedWeaved(17, 1)]
		public unsafe int Slot3Kills
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3Kills. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[17];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3Kills. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[17] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3RevivesChangedRender")]
		[NetworkedWeaved(18, 1)]
		public unsafe int Slot3Revives
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3Revives. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[18];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3Revives. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[18] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSlot3CentsChangedRender")]
		[NetworkedWeaved(19, 1)]
		public unsafe int Slot3Cents
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3Cents. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[19];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.Slot3Cents. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[19] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnCouldroneCountChangedRender")]
		[NetworkedWeaved(20, 1)]
		public unsafe int CouldroneCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.CouldroneCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[20];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.CouldroneCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[20] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnCartCountChangedRender")]
		[NetworkedWeaved(21, 1)]
		public unsafe int CartCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.CartCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[21];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.CartCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[21] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnDeadPartCountChangedRender")]
		[NetworkedWeaved(22, 1)]
		public unsafe int DeadPartCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.DeadPartCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[22];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.DeadPartCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[22] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnSpawnedNewItemIdChangedRender")]
		[NetworkedWeaved(23, 1)]
		public unsafe int SpawnedNewItemId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.SpawnedNewItemId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[23];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelPlayersGameStatisticsNetworkObject.SpawnedNewItemId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[23] = value;
			}
		}

		public event Action<int> OnNetworkedSlot0PlayerIdPlusOneChanged;

		public event Action<int> OnNetworkedSlot0DeathsChanged;

		public event Action<int> OnNetworkedSlot0KillsChanged;

		public event Action<int> OnNetworkedSlot0RevivesChanged;

		public event Action<int> OnNetworkedSlot0CentsChanged;

		public event Action<int> OnNetworkedSlot1PlayerIdPlusOneChanged;

		public event Action<int> OnNetworkedSlot1DeathsChanged;

		public event Action<int> OnNetworkedSlot1KillsChanged;

		public event Action<int> OnNetworkedSlot1RevivesChanged;

		public event Action<int> OnNetworkedSlot1CentsChanged;

		public event Action<int> OnNetworkedSlot2PlayerIdPlusOneChanged;

		public event Action<int> OnNetworkedSlot2DeathsChanged;

		public event Action<int> OnNetworkedSlot2KillsChanged;

		public event Action<int> OnNetworkedSlot2RevivesChanged;

		public event Action<int> OnNetworkedSlot2CentsChanged;

		public event Action<int> OnNetworkedSlot3PlayerIdPlusOneChanged;

		public event Action<int> OnNetworkedSlot3DeathsChanged;

		public event Action<int> OnNetworkedSlot3KillsChanged;

		public event Action<int> OnNetworkedSlot3RevivesChanged;

		public event Action<int> OnNetworkedSlot3CentsChanged;

		public event Action<int> OnNetworkedCouldroneCountChanged;

		public event Action<int> OnNetworkedCartCountChanged;

		public event Action<int> OnNetworkedDeadPartCountChanged;

		public event Action<int> OnNetworkedSpawnedNewItemIdChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		public event Action<int> OnReviveSignalReceived;

		public event Action OnResetSignalReceived;

		[Inject]
		public void InjectDependencies(INetworkedModelInstanceProvider networkedModelInstanceProvider)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
		}

		public override void Spawned()
		{
			_networkedModelInstanceProvider?.Register(this);
			this.OnNetworkedSlot0PlayerIdPlusOneChanged?.Invoke(Slot0PlayerIdPlusOne);
			this.OnNetworkedSlot0DeathsChanged?.Invoke(Slot0Deaths);
			this.OnNetworkedSlot0KillsChanged?.Invoke(Slot0Kills);
			this.OnNetworkedSlot0RevivesChanged?.Invoke(Slot0Revives);
			this.OnNetworkedSlot0CentsChanged?.Invoke(Slot0Cents);
			this.OnNetworkedSlot1PlayerIdPlusOneChanged?.Invoke(Slot1PlayerIdPlusOne);
			this.OnNetworkedSlot1DeathsChanged?.Invoke(Slot1Deaths);
			this.OnNetworkedSlot1KillsChanged?.Invoke(Slot1Kills);
			this.OnNetworkedSlot1RevivesChanged?.Invoke(Slot1Revives);
			this.OnNetworkedSlot1CentsChanged?.Invoke(Slot1Cents);
			this.OnNetworkedSlot2PlayerIdPlusOneChanged?.Invoke(Slot2PlayerIdPlusOne);
			this.OnNetworkedSlot2DeathsChanged?.Invoke(Slot2Deaths);
			this.OnNetworkedSlot2KillsChanged?.Invoke(Slot2Kills);
			this.OnNetworkedSlot2RevivesChanged?.Invoke(Slot2Revives);
			this.OnNetworkedSlot2CentsChanged?.Invoke(Slot2Cents);
			this.OnNetworkedSlot3PlayerIdPlusOneChanged?.Invoke(Slot3PlayerIdPlusOne);
			this.OnNetworkedSlot3DeathsChanged?.Invoke(Slot3Deaths);
			this.OnNetworkedSlot3KillsChanged?.Invoke(Slot3Kills);
			this.OnNetworkedSlot3RevivesChanged?.Invoke(Slot3Revives);
			this.OnNetworkedSlot3CentsChanged?.Invoke(Slot3Cents);
			this.OnNetworkedCouldroneCountChanged?.Invoke(CouldroneCount);
			this.OnNetworkedCartCountChanged?.Invoke(CartCount);
			this.OnNetworkedDeadPartCountChanged?.Invoke(DeadPartCount);
			this.OnNetworkedSpawnedNewItemIdChanged?.Invoke(SpawnedNewItemId);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_networkedModelInstanceProvider?.Unregister(this);
			this.OnDespawned?.Invoke();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				this.OnAuthoritativeTick?.Invoke();
			}
		}

		public bool TryWriteSlot0PlayerIdPlusOne(int slot0PlayerIdPlusOne)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0PlayerIdPlusOne = slot0PlayerIdPlusOne;
			return true;
		}

		public bool TryWriteSlot0Deaths(int slot0Deaths)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0Deaths = slot0Deaths;
			return true;
		}

		public bool TryWriteSlot0Kills(int slot0Kills)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0Kills = slot0Kills;
			return true;
		}

		public bool TryWriteSlot0Revives(int slot0Revives)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0Revives = slot0Revives;
			return true;
		}

		public bool TryWriteSlot0Cents(int slot0Cents)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot0Cents = slot0Cents;
			return true;
		}

		public bool TryWriteSlot1PlayerIdPlusOne(int slot1PlayerIdPlusOne)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1PlayerIdPlusOne = slot1PlayerIdPlusOne;
			return true;
		}

		public bool TryWriteSlot1Deaths(int slot1Deaths)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1Deaths = slot1Deaths;
			return true;
		}

		public bool TryWriteSlot1Kills(int slot1Kills)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1Kills = slot1Kills;
			return true;
		}

		public bool TryWriteSlot1Revives(int slot1Revives)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1Revives = slot1Revives;
			return true;
		}

		public bool TryWriteSlot1Cents(int slot1Cents)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot1Cents = slot1Cents;
			return true;
		}

		public bool TryWriteSlot2PlayerIdPlusOne(int slot2PlayerIdPlusOne)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2PlayerIdPlusOne = slot2PlayerIdPlusOne;
			return true;
		}

		public bool TryWriteSlot2Deaths(int slot2Deaths)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2Deaths = slot2Deaths;
			return true;
		}

		public bool TryWriteSlot2Kills(int slot2Kills)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2Kills = slot2Kills;
			return true;
		}

		public bool TryWriteSlot2Revives(int slot2Revives)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2Revives = slot2Revives;
			return true;
		}

		public bool TryWriteSlot2Cents(int slot2Cents)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot2Cents = slot2Cents;
			return true;
		}

		public bool TryWriteSlot3PlayerIdPlusOne(int slot3PlayerIdPlusOne)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3PlayerIdPlusOne = slot3PlayerIdPlusOne;
			return true;
		}

		public bool TryWriteSlot3Deaths(int slot3Deaths)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3Deaths = slot3Deaths;
			return true;
		}

		public bool TryWriteSlot3Kills(int slot3Kills)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3Kills = slot3Kills;
			return true;
		}

		public bool TryWriteSlot3Revives(int slot3Revives)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3Revives = slot3Revives;
			return true;
		}

		public bool TryWriteSlot3Cents(int slot3Cents)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Slot3Cents = slot3Cents;
			return true;
		}

		public bool TryWriteCouldroneCount(int couldroneCount)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			CouldroneCount = couldroneCount;
			return true;
		}

		public bool TryWriteCartCount(int cartCount)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			CartCount = cartCount;
			return true;
		}

		public bool TryWriteDeadPartCount(int deadPartCount)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			DeadPartCount = deadPartCount;
			return true;
		}

		public bool TryWriteSpawnedNewItemId(int spawnedNewItemId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			SpawnedNewItemId = spawnedNewItemId;
			return true;
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 673202562u)]
		public void RpcRaiseReviveSignal([RpcPayload(4)] int value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(673202562u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PlayersStatisticsModule.Scripts.Networked.LevelPlayersGameStatisticsNetworkObject::RpcRaiseReviveSignal(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(value, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			this.OnReviveSignalReceived?.Invoke(value);
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 4115518741u)]
		public void RpcRaiseResetSignal()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4115518741u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PlayersStatisticsModule.Scripts.Networked.LevelPlayersGameStatisticsNetworkObject::RpcRaiseResetSignal()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			this.OnResetSignalReceived?.Invoke();
		}

		private void OnSlot0PlayerIdPlusOneChangedRender()
		{
			this.OnNetworkedSlot0PlayerIdPlusOneChanged?.Invoke(Slot0PlayerIdPlusOne);
		}

		private void OnSlot0DeathsChangedRender()
		{
			this.OnNetworkedSlot0DeathsChanged?.Invoke(Slot0Deaths);
		}

		private void OnSlot0KillsChangedRender()
		{
			this.OnNetworkedSlot0KillsChanged?.Invoke(Slot0Kills);
		}

		private void OnSlot0RevivesChangedRender()
		{
			this.OnNetworkedSlot0RevivesChanged?.Invoke(Slot0Revives);
		}

		private void OnSlot0CentsChangedRender()
		{
			this.OnNetworkedSlot0CentsChanged?.Invoke(Slot0Cents);
		}

		private void OnSlot1PlayerIdPlusOneChangedRender()
		{
			this.OnNetworkedSlot1PlayerIdPlusOneChanged?.Invoke(Slot1PlayerIdPlusOne);
		}

		private void OnSlot1DeathsChangedRender()
		{
			this.OnNetworkedSlot1DeathsChanged?.Invoke(Slot1Deaths);
		}

		private void OnSlot1KillsChangedRender()
		{
			this.OnNetworkedSlot1KillsChanged?.Invoke(Slot1Kills);
		}

		private void OnSlot1RevivesChangedRender()
		{
			this.OnNetworkedSlot1RevivesChanged?.Invoke(Slot1Revives);
		}

		private void OnSlot1CentsChangedRender()
		{
			this.OnNetworkedSlot1CentsChanged?.Invoke(Slot1Cents);
		}

		private void OnSlot2PlayerIdPlusOneChangedRender()
		{
			this.OnNetworkedSlot2PlayerIdPlusOneChanged?.Invoke(Slot2PlayerIdPlusOne);
		}

		private void OnSlot2DeathsChangedRender()
		{
			this.OnNetworkedSlot2DeathsChanged?.Invoke(Slot2Deaths);
		}

		private void OnSlot2KillsChangedRender()
		{
			this.OnNetworkedSlot2KillsChanged?.Invoke(Slot2Kills);
		}

		private void OnSlot2RevivesChangedRender()
		{
			this.OnNetworkedSlot2RevivesChanged?.Invoke(Slot2Revives);
		}

		private void OnSlot2CentsChangedRender()
		{
			this.OnNetworkedSlot2CentsChanged?.Invoke(Slot2Cents);
		}

		private void OnSlot3PlayerIdPlusOneChangedRender()
		{
			this.OnNetworkedSlot3PlayerIdPlusOneChanged?.Invoke(Slot3PlayerIdPlusOne);
		}

		private void OnSlot3DeathsChangedRender()
		{
			this.OnNetworkedSlot3DeathsChanged?.Invoke(Slot3Deaths);
		}

		private void OnSlot3KillsChangedRender()
		{
			this.OnNetworkedSlot3KillsChanged?.Invoke(Slot3Kills);
		}

		private void OnSlot3RevivesChangedRender()
		{
			this.OnNetworkedSlot3RevivesChanged?.Invoke(Slot3Revives);
		}

		private void OnSlot3CentsChangedRender()
		{
			this.OnNetworkedSlot3CentsChanged?.Invoke(Slot3Cents);
		}

		private void OnCouldroneCountChangedRender()
		{
			this.OnNetworkedCouldroneCountChanged?.Invoke(CouldroneCount);
		}

		private void OnCartCountChangedRender()
		{
			this.OnNetworkedCartCountChanged?.Invoke(CartCount);
		}

		private void OnDeadPartCountChangedRender()
		{
			this.OnNetworkedDeadPartCountChanged?.Invoke(DeadPartCount);
		}

		private void OnSpawnedNewItemIdChangedRender()
		{
			this.OnNetworkedSpawnedNewItemIdChanged?.Invoke(SpawnedNewItemId);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Slot0PlayerIdPlusOne = _Slot0PlayerIdPlusOne;
			Slot0Deaths = _Slot0Deaths;
			Slot0Kills = _Slot0Kills;
			Slot0Revives = _Slot0Revives;
			Slot0Cents = _Slot0Cents;
			Slot1PlayerIdPlusOne = _Slot1PlayerIdPlusOne;
			Slot1Deaths = _Slot1Deaths;
			Slot1Kills = _Slot1Kills;
			Slot1Revives = _Slot1Revives;
			Slot1Cents = _Slot1Cents;
			Slot2PlayerIdPlusOne = _Slot2PlayerIdPlusOne;
			Slot2Deaths = _Slot2Deaths;
			Slot2Kills = _Slot2Kills;
			Slot2Revives = _Slot2Revives;
			Slot2Cents = _Slot2Cents;
			Slot3PlayerIdPlusOne = _Slot3PlayerIdPlusOne;
			Slot3Deaths = _Slot3Deaths;
			Slot3Kills = _Slot3Kills;
			Slot3Revives = _Slot3Revives;
			Slot3Cents = _Slot3Cents;
			CouldroneCount = _CouldroneCount;
			CartCount = _CartCount;
			DeadPartCount = _DeadPartCount;
			SpawnedNewItemId = _SpawnedNewItemId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Slot0PlayerIdPlusOne = Slot0PlayerIdPlusOne;
			_Slot0Deaths = Slot0Deaths;
			_Slot0Kills = Slot0Kills;
			_Slot0Revives = Slot0Revives;
			_Slot0Cents = Slot0Cents;
			_Slot1PlayerIdPlusOne = Slot1PlayerIdPlusOne;
			_Slot1Deaths = Slot1Deaths;
			_Slot1Kills = Slot1Kills;
			_Slot1Revives = Slot1Revives;
			_Slot1Cents = Slot1Cents;
			_Slot2PlayerIdPlusOne = Slot2PlayerIdPlusOne;
			_Slot2Deaths = Slot2Deaths;
			_Slot2Kills = Slot2Kills;
			_Slot2Revives = Slot2Revives;
			_Slot2Cents = Slot2Cents;
			_Slot3PlayerIdPlusOne = Slot3PlayerIdPlusOne;
			_Slot3Deaths = Slot3Deaths;
			_Slot3Kills = Slot3Kills;
			_Slot3Revives = Slot3Revives;
			_Slot3Cents = Slot3Cents;
			_CouldroneCount = CouldroneCount;
			_CartCount = CartCount;
			_DeadPartCount = DeadPartCount;
			_SpawnedNewItemId = SpawnedNewItemId;
		}

		[NetworkRpcWeavedInvoker(673202562u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcRaiseReviveSignal_0040Invoker673202562([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((LevelPlayersGameStatisticsNetworkObject)context.TargetBehaviour).RpcRaiseReviveSignal(value);
		}

		[NetworkRpcWeavedInvoker(4115518741u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcRaiseResetSignal_0040Invoker4115518741([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((LevelPlayersGameStatisticsNetworkObject)context.TargetBehaviour).RpcRaiseResetSignal();
		}
	}
}
