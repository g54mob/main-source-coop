using System;
using System.Collections.Generic;
using System.Reflection;
using Fusion;
using Photon.Client;
using Photon.Realtime;

namespace Features.MultiplayerSessionServices.Scripts
{
	public static class NetworkRunnerRoomPropertyExtensions
	{
		private const string CLOUD_SERVICES_MEMBER = "_cloudServices";

		private const string COMMUNICATOR_MEMBER = "_communicator";

		private const string CLIENT_MEMBER = "Client";

		private static readonly Dictionary<(Type, string), MemberInfo> _reflectedMembers = new Dictionary<(Type, string), MemberInfo>();

		public static bool TrySetRoomCustomProperty(this NetworkRunner runner, string propertyKey, object propertyValue)
		{
			Room currentRoom = ((RealtimeClient)ReadMember(ReadMember(ReadMember(runner, "_cloudServices"), "_communicator"), "Client")).CurrentRoom;
			if (currentRoom == null)
			{
				return false;
			}
			PhotonHashtable propertiesToSet = new PhotonHashtable { { propertyKey, propertyValue } };
			return currentRoom.SetCustomProperties(propertiesToSet);
		}

		private static object ReadMember(object instance, string memberName)
		{
			Type type = instance.GetType();
			if (!_reflectedMembers.TryGetValue((type, memberName), out var value))
			{
				value = ResolveMember(type, memberName);
				if (value == null)
				{
					throw new InvalidOperationException("Fusion room-property reflection: member '" + memberName + "' not found on '" + type.FullName + "'. Fusion internals changed — IsVisible/IsOpen join-protection still works, but custom session properties will not.");
				}
				_reflectedMembers[(type, memberName)] = value;
			}
			if (!(value is FieldInfo fieldInfo))
			{
				return ((PropertyInfo)value).GetValue(instance);
			}
			return fieldInfo.GetValue(instance);
		}

		private static MemberInfo ResolveMember(Type type, string memberName)
		{
			Type type2 = type;
			while (type2 != null)
			{
				FieldInfo field = type2.GetField(memberName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					return field;
				}
				PropertyInfo property = type2.GetProperty(memberName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					return property;
				}
				type2 = type2.BaseType;
			}
			return null;
		}
	}
}
