using System;
using AeLa.EasyFeedback.UI.Interfaces;
using UnityEngine;

namespace AeLa.EasyFeedback.UI
{
	internal abstract class UIInteropWrapper<T> : IUIInteropWrapper where T : Component
	{
		protected readonly T InternalTarget;

		public static Type TargetType => typeof(T);

		public Component Target => InternalTarget;

		public static T GetTarget(GameObject go)
		{
			return go.GetComponent<T>();
		}

		internal UIInteropWrapper(T internalTarget)
		{
			InternalTarget = internalTarget;
		}
	}
}
