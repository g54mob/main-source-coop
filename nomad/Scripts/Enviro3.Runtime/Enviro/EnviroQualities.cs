using System;
using System.Collections.Generic;

namespace Enviro
{
	[Serializable]
	public class EnviroQualities
	{
		public EnviroQuality defaultQuality;

		public List<EnviroQuality> Qualities = new List<EnviroQuality>();
	}
}
