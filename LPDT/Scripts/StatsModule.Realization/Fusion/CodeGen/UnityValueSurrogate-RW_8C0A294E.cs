using System;
using Fusion.Internal;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;

namespace Fusion.CodeGen
{
	[Serializable]
	[WeaverGenerated]
	internal class UnityValueSurrogate_0040RW_8C0A294E : UnityValueSurrogate<ModifierType, ElementReaderWriterUnmanaged<ModifierType, MetaConstant1>>
	{
		[WeaverGenerated]
		public ModifierType Data;

		[WeaverGenerated]
		public override ModifierType DataProperty
		{
			[WeaverGenerated]
			get
			{
				return Data;
			}
			[WeaverGenerated]
			set
			{
				Data = value;
			}
		}

		[WeaverGenerated]
		public UnityValueSurrogate_0040RW_8C0A294E()
		{
		}
	}
}
