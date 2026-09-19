using System;

namespace Obi
{
	[Serializable]
	public class ObiMassDataChannel : ObiPathDataChannelIdentity<float>
	{
		public ObiMassDataChannel()
			: base((ObiInterpolator<float>)new ObiCatmullRomInterpolator())
		{
		}
	}
}
