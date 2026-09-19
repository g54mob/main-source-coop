using System;

namespace Obi
{
	[Serializable]
	public class ObiThicknessDataChannel : ObiPathDataChannelIdentity<float>
	{
		public ObiThicknessDataChannel()
			: base((ObiInterpolator<float>)new ObiCatmullRomInterpolator())
		{
		}
	}
}
