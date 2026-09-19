using System.Runtime.InteropServices;
using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	[StructLayout(LayoutKind.Explicit, Size = 24)]
	[NetworkStructWeaved(6)]
	public struct RatsHoleContentSlot : INetworkStruct
	{
		[FieldOffset(0)]
		public NetworkPrefabId PrefabId;

		[FieldOffset(4)]
		public LevelObjectType LevelObjectType;

		[FieldOffset(8)]
		public ItemType ItemType;

		[FieldOffset(12)]
		public ushort CurrencyValue;

		[FieldOffset(16)]
		public ushort MaxCurrencyValue;

		[FieldOffset(20)]
		public NetworkBool IsCollectable;

		public RatsHoleContentEntry ToEntry()
		{
			return new RatsHoleContentEntry(PrefabId, LevelObjectType, ItemType, CurrencyValue, MaxCurrencyValue, IsCollectable);
		}
	}
}
