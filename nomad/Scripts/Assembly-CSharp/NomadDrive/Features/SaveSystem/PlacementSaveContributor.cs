using EvilCore.EvilSave;
using EvilCore.Networking.Parenting;
using Mirror;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using UnityEngine;

namespace NomadDrive.Features.SaveSystem
{
	[DisallowMultipleComponent]
	public class PlacementSaveContributor : MonoBehaviour, INetworkSaveable
	{
		public string ContributorKey => "placement";

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			HeldItem component = GetComponent<HeldItem>();
			PlacementNetworkData placementNetworkData = ((component != null) ? component.PlacementNetworkData : default(PlacementNetworkData));
			if (component == null || placementNetworkData.ParentNetworkID == 0)
			{
				writer.Write(string.Empty);
				return;
			}
			string value = ctx.ToGuid(placementNetworkData.ParentNetworkID);
			if (string.IsNullOrEmpty(value))
			{
				writer.Write(string.Empty);
				return;
			}
			Vector3 value2 = Vector3.zero;
			Vector3 value3 = Vector3.zero;
			if (TryResolvePlane(placementNetworkData.ParentNetworkID, placementNetworkData.SnappingPlaneIndex, out var plane))
			{
				NetworkedTransform networkedTransform = plane.ResolveParentNetworkedTransform();
				if (networkedTransform != null)
				{
					Transform transform = networkedTransform.transform;
					value2 = Quaternion.Inverse(transform.rotation) * (component.transform.position - transform.position);
					value3 = (Quaternion.Inverse(transform.rotation) * component.transform.rotation).eulerAngles;
				}
			}
			writer.Write(value);
			writer.Write(placementNetworkData.SnappingPlaneIndex);
			writer.Write(value2);
			writer.Write(value3);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			string text = reader.ReadString();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			byte planeIndex = reader.ReadByte();
			Vector3 localPositionOffset = reader.ReadVector3();
			Vector3 localRotationOffsetEuler = reader.ReadVector3();
			if (NetworkServer.active && ctx.TryToNetId(text, out var netId) && TryResolvePlane(netId, planeIndex, out var plane))
			{
				HeldItem component = GetComponent<HeldItem>();
				if (component != null)
				{
					plane.ServerRestorePlacement(component, localPositionOffset, localRotationOffsetEuler);
				}
			}
		}

		private static bool TryResolvePlane(uint containerNetId, byte planeIndex, out SnappingPlane plane)
		{
			plane = null;
			if (!NetworkServer.spawned.TryGetValue(containerNetId, out var value) || value == null)
			{
				return false;
			}
			if (!value.TryGetComponent<ISnappingPlaneContainer>(out var component))
			{
				return false;
			}
			plane = component.GetSnappingPlaneByIndex(planeIndex);
			return plane != null;
		}
	}
}
