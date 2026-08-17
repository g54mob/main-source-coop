using System;
using UnityEngine;

namespace Ami.BroAudio.Data
{
	[Serializable]
	public struct AudioParameter : IEquatable<AudioParameter>
	{
		[SerializeField]
		private AudioEntity _entity;

		[SerializeField]
		private string _parameterId;

		public AudioEntity Entity => _entity;

		public string ParameterId => _parameterId;

		public AudioParameter(AudioEntity entity, string parameterId)
		{
			_entity = entity;
			_parameterId = parameterId;
		}

		public bool IsValid()
		{
			AudioParameterDefinition definition;
			if (_entity != null && !string.IsNullOrEmpty(_parameterId))
			{
				return _entity.TryFindParameterById(_parameterId, out definition);
			}
			return false;
		}

		public bool TryGetDefinition(out AudioParameterDefinition definition)
		{
			definition = default(AudioParameterDefinition);
			if (_entity == null || string.IsNullOrEmpty(_parameterId))
			{
				return false;
			}
			return _entity.TryFindParameterById(_parameterId, out definition);
		}

		public override string ToString()
		{
			if (_entity == null)
			{
				return "AudioParameter(unset)";
			}
			if (TryGetDefinition(out var definition))
			{
				return _entity.Name + "/" + definition.Name;
			}
			return _entity.Name + "/<missing:" + _parameterId + ">";
		}

		public bool Equals(AudioParameter other)
		{
			if (_entity == other._entity)
			{
				return _parameterId == other._parameterId;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is AudioParameter other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = ((_entity != null) ? _entity.GetHashCode() : 0);
			int num2 = ((_parameterId != null) ? _parameterId.GetHashCode() : 0);
			return (num * 397) ^ num2;
		}

		public static bool operator ==(AudioParameter a, AudioParameter b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(AudioParameter a, AudioParameter b)
		{
			return !a.Equals(b);
		}
	}
}
