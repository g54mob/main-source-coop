using System;
using UnityEngine;

namespace NomadDrive.Features.SaveSystem
{
	[DisallowMultipleComponent]
	public class PersistentId : MonoBehaviour
	{
		private string _guid;

		public string Guid => _guid;

		public bool HasGuid => !string.IsNullOrEmpty(_guid);

		public void ServerAssignGuid(string guid)
		{
			_guid = guid;
			PersistentIdRegistry.Register(this);
		}

		public void ServerEnsureGuid()
		{
			if (string.IsNullOrEmpty(_guid))
			{
				_guid = System.Guid.NewGuid().ToString("N");
			}
			PersistentIdRegistry.Register(this);
		}

		private void OnDestroy()
		{
			PersistentIdRegistry.Unregister(this);
		}
	}
}
