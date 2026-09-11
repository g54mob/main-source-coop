using System;
using EasyTextEffects.Editor.MyBoxCopy.Tools.Internal;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyTextEffects.Editor.MyBoxCopy.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	[PublicAPI]
	public class ButtonMethodAttribute : PropertyAttribute
	{
		public readonly ButtonMethodDrawOrder DrawOrder;

		public readonly ConditionalData Condition;

		public ButtonMethodAttribute(ButtonMethodDrawOrder drawOrder = ButtonMethodDrawOrder.AfterInspector)
		{
			DrawOrder = drawOrder;
		}

		public ButtonMethodAttribute(ButtonMethodDrawOrder drawOrder, string fieldToCheck, bool inverse = false, params object[] compareValues)
		{
			ConditionalData condition = new ConditionalData(fieldToCheck, inverse, compareValues);
			DrawOrder = drawOrder;
			Condition = condition;
		}

		public ButtonMethodAttribute(ButtonMethodDrawOrder drawOrder, string[] fieldToCheck, bool[] inverse = null, params object[] compare)
		{
			ConditionalData condition = new ConditionalData(fieldToCheck, inverse, compare);
			DrawOrder = drawOrder;
			Condition = condition;
		}

		public ButtonMethodAttribute(ButtonMethodDrawOrder drawOrder, params string[] fieldToCheck)
		{
			ConditionalData condition = new ConditionalData(fieldToCheck);
			DrawOrder = drawOrder;
			Condition = condition;
		}

		public ButtonMethodAttribute(ButtonMethodDrawOrder drawOrder, bool useMethod, string method, bool inverse = false)
		{
			ConditionalData condition = new ConditionalData(useMethod, method, inverse);
			DrawOrder = drawOrder;
			Condition = condition;
		}
	}
}
