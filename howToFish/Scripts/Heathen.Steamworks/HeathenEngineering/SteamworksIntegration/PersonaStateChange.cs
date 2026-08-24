using System;
using Steamworks;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct PersonaStateChange
	{
		public PersonaStateChange_t data;

		public CSteamID SubjectId => new CSteamID(data.m_ulSteamID);

		public EPersonaChange Flags => data.m_nChangeFlags;

		public static implicit operator PersonaStateChange(PersonaStateChange_t native)
		{
			return new PersonaStateChange
			{
				data = native
			};
		}

		public static implicit operator PersonaStateChange_t(PersonaStateChange heathen)
		{
			return heathen.data;
		}
	}
}
