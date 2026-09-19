using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine;

namespace Features.StatsUsageModule.Scripts.Entities
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	[NetworkStructWeaved(3)]
	public struct StatModifierStruct : INetworkStruct
	{
		[FieldOffset(0)]
		[FixedBufferProperty(typeof(ModifierType), typeof(UnityValueSurrogate_0040RW_8C0A294E), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ModifierType", 0, 1)]
		private FixedStorage_00401 _ModifierType;

		[FieldOffset(4)]
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate_0040RW_C8563331), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Value", 1, 1)]
		private FixedStorage_00401 _Value;

		[FieldOffset(8)]
		[FixedBufferProperty(typeof(int), typeof(UnityValueSurrogate_0040RW_44DDCC79), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Order", 2, 1)]
		private FixedStorage_00401 _Order;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe ModifierType ModifierType
		{
			readonly get
			{
				return *(ModifierType*)FusionUnsafe.ReferenceToPointer(ref _ModifierType);
			}
			set
			{
				*(ModifierType*)FusionUnsafe.ReferenceToPointer(ref _ModifierType) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe float Value
		{
			readonly get
			{
				return *(float*)FusionUnsafe.ReferenceToPointer(ref _Value);
			}
			set
			{
				*(float*)FusionUnsafe.ReferenceToPointer(ref _Value) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe int Order
		{
			readonly get
			{
				return *(int*)FusionUnsafe.ReferenceToPointer(ref _Order);
			}
			set
			{
				*(int*)FusionUnsafe.ReferenceToPointer(ref _Order) = value;
			}
		}

		public StatModifierStruct(float value, ModifierType modifierType, int order)
		{
			Value = value;
			ModifierType = modifierType;
			Order = order;
		}
	}
}
