using System;
using System.Collections.Generic;

namespace Enviro
{
	[Serializable]
	public class EnviroWeatherTypeAudioOverride
	{
		public List<EnviroAudioOverrideType> ambientOverride = new List<EnviroAudioOverrideType>();

		public List<EnviroAudioOverrideType> weatherOverride = new List<EnviroAudioOverrideType>();
	}
}
