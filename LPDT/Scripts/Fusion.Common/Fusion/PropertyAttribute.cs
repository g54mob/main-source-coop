using System;
using System.Reflection;
using UnityEngine;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public abstract class PropertyAttribute : UnityEngine.PropertyAttribute
	{
		private static readonly Lazy<FieldInfo> ApplyToCollectionField = new Lazy<FieldInfo>(delegate
		{
			Type typeFromHandle = typeof(UnityEngine.PropertyAttribute);
			return typeFromHandle.GetField("<applyToCollection>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
		});

		public new bool applyToCollection { get; }

		public PropertyAttribute(bool applyToCollection = false)
		{
			this.applyToCollection = applyToCollection;
			if (applyToCollection)
			{
				ApplyToCollectionField.Value?.SetValue(this, true);
			}
		}
	}
}
