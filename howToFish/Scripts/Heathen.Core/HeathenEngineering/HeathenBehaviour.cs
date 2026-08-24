using System.Collections.Generic;
using UnityEngine;

namespace HeathenEngineering
{
	public class HeathenBehaviour : MonoBehaviour
	{
		public List<ScriptableObject> tags;

		private Transform _selfTransform;

		public Transform SelfTransform
		{
			get
			{
				if (_selfTransform == null)
				{
					_selfTransform = GetComponent<Transform>();
				}
				return _selfTransform;
			}
		}

		public bool ContainsScriptableTag(ScriptableObject tag)
		{
			return tags.Contains(tag);
		}

		public bool ContainsScriptableTags(IEnumerable<ScriptableObject> tags)
		{
			foreach (ScriptableObject tag in tags)
			{
				if (!this.tags.Contains(tag))
				{
					return false;
				}
			}
			return true;
		}
	}
}
