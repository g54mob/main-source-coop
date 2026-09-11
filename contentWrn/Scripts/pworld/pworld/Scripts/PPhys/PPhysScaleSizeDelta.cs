using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts.PPhys
{
	public class PPhysScaleSizeDelta : PPhysSpringBase
	{
		private RectTransform rectT;

		public override Vector3 Current
		{
			get
			{
				return rectT.sizeDelta;
			}
			set
			{
				rectT.sizeDelta = value.xy();
			}
		}

		public override void Awake()
		{
			rectT = GetComponent<RectTransform>();
			base.Awake();
		}
	}
}
