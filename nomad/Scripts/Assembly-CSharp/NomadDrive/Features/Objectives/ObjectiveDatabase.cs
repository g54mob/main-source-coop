using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[CreateAssetMenu(menuName = "NomadDrive/Objectives/Objective Database", fileName = "ObjectiveDatabase")]
	public class ObjectiveDatabase : SerializedScriptableObject
	{
		[SerializeField]
		private List<ObjectiveDefinition> _objectives = new List<ObjectiveDefinition>();

		private Dictionary<string, ObjectiveDefinition> _byId;

		public IReadOnlyList<ObjectiveDefinition> All => _objectives;

		private void OnEnable()
		{
			BuildLookup();
		}

		public ObjectiveDefinition GetById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			if (_byId == null)
			{
				BuildLookup();
			}
			if (_byId == null || !_byId.TryGetValue(id, out var value))
			{
				return null;
			}
			return value;
		}

		private void BuildLookup()
		{
			_byId = new Dictionary<string, ObjectiveDefinition>(_objectives?.Count ?? 0);
			if (_objectives == null)
			{
				return;
			}
			foreach (ObjectiveDefinition objective in _objectives)
			{
				if (!(objective == null) && !string.IsNullOrEmpty(objective.ObjectiveId) && !_byId.ContainsKey(objective.ObjectiveId))
				{
					_byId[objective.ObjectiveId] = objective;
				}
			}
		}
	}
}
