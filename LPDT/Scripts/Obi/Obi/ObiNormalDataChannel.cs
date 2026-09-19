using System;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiNormalDataChannel : ObiPathDataChannelIdentity<Vector3>
	{
		public ObiNormalDataChannel()
			: base((ObiInterpolator<Vector3>)new ObiCatmullRomInterpolator3D())
		{
		}
	}
}
