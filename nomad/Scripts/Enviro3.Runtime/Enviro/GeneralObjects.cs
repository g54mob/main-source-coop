using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class GeneralObjects
	{
		public GameObject sun;

		public GameObject moon;

		public GameObject stars;

		public Light directionalLight;

		public Light additionalDirectionalLight;

		public EnviroReflectionProbe globalReflectionProbe;

		public GameObject effects;

		public GameObject audio;

		public WindZone windZone;

		public GameObject worldAnchor;
	}
}
