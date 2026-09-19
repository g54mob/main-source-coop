using System;
using Photon.Client;

namespace Photon.Realtime
{
	public class OnRoomPropertiesUpdateMsg
	{
		public PhotonHashtable changedProps;

		[Obsolete("Use changedProps.")]
		public PhotonHashtable propertiesThatChanged
		{
			get
			{
				return changedProps;
			}
			set
			{
				changedProps = value;
			}
		}

		internal OnRoomPropertiesUpdateMsg(PhotonHashtable propertiesThatChanged)
		{
			changedProps = propertiesThatChanged;
		}
	}
}
