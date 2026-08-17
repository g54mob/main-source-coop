using System.Collections.Generic;
using UnityEngine;

namespace Ami.BroAudio.Data
{
	[CreateAssetMenu(menuName = "BroAudio/Global Parameters", fileName = "BroAudioGlobalParameters")]
	public class BroAudioGlobalParameters : ScriptableObject
	{
		[SerializeField]
		private List<AudioParameterDefinition> parameters = new List<AudioParameterDefinition>();

		public IReadOnlyList<AudioParameterDefinition> Parameters => parameters;

		public static BroAudioGlobalParameters ActiveInstance { get; private set; }

		public void SetActive()
		{
			ActiveInstance = this;
		}

		public void ClearActive()
		{
			if ((object)ActiveInstance == this)
			{
				ActiveInstance = null;
			}
		}

		public static HashSet<string> GetReferencedGlobalIds(AudioEntity entity)
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (entity == null)
			{
				return hashSet;
			}
			BroAudioGlobalParameters activeInstance = ActiveInstance;
			if (activeInstance == null || activeInstance.parameters == null || activeInstance.parameters.Count == 0)
			{
				return hashSet;
			}
			HashSet<string> hashSet2 = new HashSet<string>();
			if (entity.Parameters != null)
			{
				for (int i = 0; i < entity.Parameters.Count; i++)
				{
					string id = entity.Parameters[i].Id;
					if (!string.IsNullOrEmpty(id))
					{
						hashSet2.Add(id);
					}
				}
			}
			AudioParameterDefinition definition;
			if (entity.Transitions != null)
			{
				for (int j = 0; j < entity.Transitions.Count; j++)
				{
					string parameterId = entity.Transitions[j].ParameterId;
					if (!string.IsNullOrEmpty(parameterId) && !hashSet2.Contains(parameterId) && activeInstance.TryFindParameterById(parameterId, out definition))
					{
						hashSet.Add(parameterId);
					}
				}
			}
			if (entity.Bindings != null)
			{
				for (int k = 0; k < entity.Bindings.Count; k++)
				{
					string parameterId2 = entity.Bindings[k].ParameterId;
					if (!string.IsNullOrEmpty(parameterId2) && !hashSet2.Contains(parameterId2) && activeInstance.TryFindParameterById(parameterId2, out definition))
					{
						hashSet.Add(parameterId2);
					}
				}
			}
			return hashSet;
		}

		public bool TryFindParameterById(string id, out AudioParameterDefinition definition)
		{
			if (!string.IsNullOrEmpty(id) && parameters != null)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					if (parameters[i].Id == id)
					{
						definition = parameters[i];
						return true;
					}
				}
			}
			definition = default(AudioParameterDefinition);
			return false;
		}

		public bool TryFindParameterByName(string name, out AudioParameterDefinition definition)
		{
			if (!string.IsNullOrEmpty(name) && parameters != null)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					if (parameters[i].Name == name)
					{
						definition = parameters[i];
						return true;
					}
				}
			}
			definition = default(AudioParameterDefinition);
			return false;
		}
	}
}
