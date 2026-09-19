using System;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;

namespace Features.QuotaModule.Scripts
{
	[Serializable]
	public class QuotaContainerItemData
	{
		public MonoItem Item { get; set; }

		public IPointGrabable PointGrabable { get; set; }
	}
}
