using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rewired.Utils
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class EnumValueHelper<TEnum>
	{
		private static EnumValueHelper<TEnum> sKlAxiBmvyBxAjrDeOltNvBiEdkP;

		private TEnum[] VePwhFxSBkhXLQKWMwBJJwSoAsVl;

		private ReadOnlyCollection<TEnum> nwQIQIWlftoLBzjeLCyPSDiDJBcA;

		private string[] xpMAMnjGJQpGdAskRlzTTKLVUuDH;

		private ReadOnlyCollection<string> boHWIiwcNAnaNVwWcFgcjTvMEMkL;

		public static EnumValueHelper<TEnum> Default => sKlAxiBmvyBxAjrDeOltNvBiEdkP ?? (sKlAxiBmvyBxAjrDeOltNvBiEdkP = new EnumValueHelper<TEnum>());

		public IList<TEnum> values => nwQIQIWlftoLBzjeLCyPSDiDJBcA;

		public IList<string> names
		{
			get
			{
				if (boHWIiwcNAnaNVwWcFgcjTvMEMkL == null)
				{
					xpMAMnjGJQpGdAskRlzTTKLVUuDH = Enum.GetNames(typeof(TEnum));
					boHWIiwcNAnaNVwWcFgcjTvMEMkL = new ReadOnlyCollection<string>(xpMAMnjGJQpGdAskRlzTTKLVUuDH);
				}
				return boHWIiwcNAnaNVwWcFgcjTvMEMkL;
			}
		}

		public EnumValueHelper()
		{
			if (!EnumTools.IsEnum(typeof(TEnum)))
			{
				throw new ArgumentException("TEnum must be an enum type.");
			}
			VePwhFxSBkhXLQKWMwBJJwSoAsVl = (TEnum[])Enum.GetValues(typeof(TEnum));
			nwQIQIWlftoLBzjeLCyPSDiDJBcA = new ReadOnlyCollection<TEnum>(VePwhFxSBkhXLQKWMwBJJwSoAsVl);
		}
	}
}
