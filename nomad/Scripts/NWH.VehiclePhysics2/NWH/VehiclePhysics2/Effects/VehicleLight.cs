using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class VehicleLight
	{
		[Tooltip("    All the light sources representing the vehicle light.\r\n    E.g. low beam can be represented by a directional light to represent light beam and\r\n    and emissive mesh to represent light optics.")]
		public List<LightSource> lightSources = new List<LightSource>();

		protected bool isOn;

		public bool On
		{
			get
			{
				return isOn;
			}
			set
			{
				isOn = value;
			}
		}

		public void SetState(bool state)
		{
			if (state)
			{
				TurnOn();
			}
			else
			{
				TurnOff();
			}
		}

		public void Toggle()
		{
			if (isOn)
			{
				TurnOff();
			}
			else
			{
				TurnOn();
			}
		}

		public void TurnOff()
		{
			for (int i = 0; i < lightSources.Count; i++)
			{
				lightSources[i].TurnOff();
			}
			isOn = false;
		}

		public void TurnOn()
		{
			for (int i = 0; i < lightSources.Count; i++)
			{
				lightSources[i].TurnOn();
			}
			isOn = true;
		}
	}
}
