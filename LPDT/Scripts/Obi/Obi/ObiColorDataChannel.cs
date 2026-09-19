using System;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiColorDataChannel : ObiPathDataChannelIdentity<Color>
	{
		public ObiColorDataChannel()
			: base((ObiInterpolator<Color>)new ObiColorInterpolator3D())
		{
		}
	}
}
