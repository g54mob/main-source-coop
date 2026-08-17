using System;
using System.Collections.Generic;
using Ami.BroAudio.Data;
using UnityEngine;

namespace Ami.BroAudio
{
	[Serializable]
	public struct SoundID : IEquatable<SoundID>, IComparable<SoundID>, IEqualityComparer<SoundID>, IComparer<SoundID>
	{
		[SerializeField]
		private AudioEntity _entity;

		[Obsolete("Raw entities are now used", true)]
		[SerializeField]
		private int ID;

		internal AudioEntity Entity
		{
			get
			{
				if (_entity == null)
				{
					_fixLegacyId();
				}
				return _entity;
			}
		}

		public static SoundID Invalid => default(SoundID);

		[Obsolete("Raw entities are now used")]
		private void _fixLegacyId()
		{
			if (!(_entity != null) && ID != 0 && ID != -1 && !SoundIDExtension.TryConvertIdToEntity(ID, out _entity))
			{
				Debug.LogError($"Could not find entity with ID {ID} to convert SoundID to entity with");
				_entity = null;
			}
		}

		public SoundID(AudioEntity entity)
		{
			this = default(SoundID);
			_entity = entity;
		}

		public override string ToString()
		{
			if (!(Entity != null))
			{
				return "not set";
			}
			return Entity.Name;
		}

		public override bool Equals(object obj)
		{
			if (obj is SoundID other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (!(Entity != null))
			{
				return 0;
			}
			return Entity.GetHashCode();
		}

		public bool Equals(SoundID other)
		{
			return other.Entity == Entity;
		}

		public bool Equals(SoundID x, SoundID y)
		{
			return x.Entity == y.Entity;
		}

		public int GetHashCode(SoundID obj)
		{
			if (!(obj.Entity != null))
			{
				return 0;
			}
			return obj.Entity.GetHashCode();
		}

		public int CompareTo(SoundID other)
		{
			if (!(Entity != null) || !(other.Entity != null))
			{
				return 0;
			}
			return StringComparer.OrdinalIgnoreCase.Compare(Entity.Name, other.Entity.Name);
		}

		public int Compare(SoundID x, SoundID y)
		{
			if (!(x.Entity != null) || !(y.Entity != null))
			{
				return 0;
			}
			return StringComparer.OrdinalIgnoreCase.Compare(x.Entity.Name, y.Entity.Name);
		}

		[Obsolete("legacy upgrade only", true)]
		public static void __setLegacyId(ref SoundID soundId, int id)
		{
			soundId.ID = id;
		}
	}
}
