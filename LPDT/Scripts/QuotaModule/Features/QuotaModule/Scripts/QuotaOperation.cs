using System;

namespace Features.QuotaModule.Scripts
{
	[Serializable]
	public enum QuotaOperation
	{
		None = 0,
		Add = 1,
		Remove = 2,
		Replace = 3
	}
}
