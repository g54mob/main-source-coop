using System.Collections.Generic;
using System.IO;
using EvilCore.EvilSave;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Restoration;
using UnityEngine;

namespace NomadDrive.Features.SaveSystem
{
	[RequireComponent(typeof(PersistentId))]
	[DisallowMultipleComponent]
	public class PersistentObject : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Addressable GUID used to respawn this prefab on load. Auto-filled by the save tooling; set manually if blank.")]
		private string addressableGuid;

		private PersistentId _persistentId;

		public PersistentId PersistentId
		{
			get
			{
				if (_persistentId == null)
				{
					_persistentId = GetComponent<PersistentId>();
				}
				return _persistentId;
			}
		}

		public string AddressableGuid => addressableGuid;

		public void ServerSetAddressableGuid(string guid)
		{
			if (!string.IsNullOrEmpty(guid))
			{
				addressableGuid = guid;
			}
		}

		public static PersistentObject ServerEnsure(GameObject instance, string addressableGuid, string assignGuid = null)
		{
			if (instance == null)
			{
				return null;
			}
			PersistentId persistentId = instance.GetComponent<PersistentId>();
			if (persistentId == null)
			{
				persistentId = instance.AddComponent<PersistentId>();
			}
			if (!string.IsNullOrEmpty(assignGuid))
			{
				persistentId.ServerAssignGuid(assignGuid);
			}
			else
			{
				persistentId.ServerEnsureGuid();
			}
			PersistentObject persistentObject = instance.GetComponent<PersistentObject>();
			if (persistentObject == null)
			{
				persistentObject = instance.AddComponent<PersistentObject>();
			}
			persistentObject.ServerSetAddressableGuid(addressableGuid);
			if (instance.GetComponent<HeldItem>() != null && instance.GetComponent<PlacementSaveContributor>() == null)
			{
				instance.AddComponent<PlacementSaveContributor>();
			}
			if (instance.GetComponentInChildren<RestorableVehicleSurface>(includeInactive: true) != null && instance.GetComponent<RestorationSaveContributor>() == null)
			{
				instance.AddComponent<RestorationSaveContributor>();
			}
			return persistentObject;
		}

		private void Awake()
		{
			_persistentId = GetComponent<PersistentId>();
		}

		private List<INetworkSaveable> CollectOwnedContributors()
		{
			List<INetworkSaveable> list = new List<INetworkSaveable>();
			INetworkSaveable[] componentsInChildren = GetComponentsInChildren<INetworkSaveable>(includeInactive: true);
			foreach (INetworkSaveable networkSaveable in componentsInChildren)
			{
				if (networkSaveable is Component component && !(component == null) && component.GetComponentInParent<PersistentObject>(includeInactive: true) == this)
				{
					list.Add(networkSaveable);
				}
			}
			return list;
		}

		public Dictionary<string, byte[]> CaptureContributors(ISaveContext ctx)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			foreach (INetworkSaveable item in CollectOwnedContributors())
			{
				using MemoryStream memoryStream = new MemoryStream();
				using (EvilWriter writer = new EvilWriter(memoryStream))
				{
					item.CaptureState(writer, ctx);
				}
				dictionary[item.ContributorKey] = memoryStream.ToArray();
			}
			return dictionary;
		}

		public void RestoreSelf(IReadOnlyDictionary<string, byte[]> blobs, ISaveContext ctx)
		{
			ApplyBlobs(blobs, ctx, links: false);
		}

		public void RestoreLinks(IReadOnlyDictionary<string, byte[]> blobs, ISaveContext ctx)
		{
			ApplyBlobs(blobs, ctx, links: true);
		}

		private void ApplyBlobs(IReadOnlyDictionary<string, byte[]> blobs, ISaveContext ctx, bool links)
		{
			foreach (INetworkSaveable item in CollectOwnedContributors())
			{
				if (!blobs.TryGetValue(item.ContributorKey, out var value) || value == null)
				{
					continue;
				}
				using MemoryStream stream = new MemoryStream(value);
				using EvilReader reader = new EvilReader(stream);
				if (links)
				{
					item.RestoreLinks(reader, ctx);
				}
				else
				{
					item.RestoreSelfState(reader, ctx);
				}
			}
		}
	}
}
