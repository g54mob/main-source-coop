using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts
{
	public class NavigationModel
	{
		internal readonly List<GameObject> NavigationQuery = new List<GameObject>();

		public EventSystem EventSystem { get; internal set; }

		public GameObject CurrentSelectedObject { get; internal set; }

		public GameObject LastSelectedObject { get; internal set; }
	}
}
