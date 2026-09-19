using System;
using System.Collections.Generic;

namespace Features.ObjectDespawnModule.Scripts
{
	public class ObjectDespawnModel
	{
		private readonly List<DespawnObjectData> _objectsToDespawn = new List<DespawnObjectData>();

		public event Action<DespawnObjectData> OnObjectAddedToDespawn;

		public void AddObjectToDespawn(DespawnObjectData networkObjectData)
		{
			if (networkObjectData != null && !_objectsToDespawn.Contains(networkObjectData))
			{
				_objectsToDespawn.Add(networkObjectData);
				this.OnObjectAddedToDespawn?.Invoke(networkObjectData);
			}
		}

		public void RemoveObject(DespawnObjectData networkObject)
		{
			_objectsToDespawn.Remove(networkObject);
		}
	}
}
