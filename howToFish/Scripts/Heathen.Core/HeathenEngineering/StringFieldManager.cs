using System.Collections.Generic;
using UnityEngine;

namespace HeathenEngineering
{
	[CreateAssetMenu(menuName = "System Core/Application/String Field Manager")]
	public class StringFieldManager : ScriptableObject
	{
		public static StringFieldManager activeManager;

		public static StringSet activeSet;

		public List<StringLibrary> availableSets = new List<StringLibrary>();

		[Tooltip("Use the context menu to populate this list with all fields.\n\nThis list is used by Library serialization at runtime, if its empty serialization (save and load from disk) will not work.")]
		public List<StringField> availableFields = new List<StringField>();

		[ContextMenu("Activate Manager")]
		public void Activate()
		{
			activeManager = this;
		}

		public static void ApplyStringSet(StringSet set)
		{
			activeSet = set;
			foreach (StringFieldValue value in set.Values)
			{
				value.Field.Value = value.value;
			}
		}

		public void applyStringSet(StringSet set)
		{
			ApplyStringSet(set);
		}
	}
}
