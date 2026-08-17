using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rewired.Utils
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class EnumValueHelper<TEnum>
	{
		private static EnumValueHelper<TEnum> HPrUIZxoiaCKDcdSwriBQHdlvmDC;

		private TEnum[] japiHzdcSqOJHtviCeyDJGGsSabVA;

		private ReadOnlyCollection<TEnum> HDpCRhAXoCoXtEoSzZHLsHdDXIrM;

		private string[] nzaWNtFjwrRpBNIqhEuqeMhARJyU;

		private ReadOnlyCollection<string> GMrlPayppssWddvmbRmRZLdqaIpK;

		public static EnumValueHelper<TEnum> Default => HPrUIZxoiaCKDcdSwriBQHdlvmDC ?? (HPrUIZxoiaCKDcdSwriBQHdlvmDC = new EnumValueHelper<TEnum>());

		public IList<TEnum> values => HDpCRhAXoCoXtEoSzZHLsHdDXIrM;

		public IList<string> names
		{
			get
			{
				if (GMrlPayppssWddvmbRmRZLdqaIpK == null)
				{
					nzaWNtFjwrRpBNIqhEuqeMhARJyU = Enum.GetNames(typeof(TEnum));
					GMrlPayppssWddvmbRmRZLdqaIpK = new ReadOnlyCollection<string>(nzaWNtFjwrRpBNIqhEuqeMhARJyU);
				}
				return GMrlPayppssWddvmbRmRZLdqaIpK;
			}
		}

		public EnumValueHelper()
		{
			if (!EnumTools.IsEnum(typeof(TEnum)))
			{
				throw new ArgumentException("TEnum must be an enum type.");
			}
			japiHzdcSqOJHtviCeyDJGGsSabVA = (TEnum[])Enum.GetValues(typeof(TEnum));
			HDpCRhAXoCoXtEoSzZHLsHdDXIrM = new ReadOnlyCollection<TEnum>(japiHzdcSqOJHtviCeyDJGGsSabVA);
		}
	}
}
