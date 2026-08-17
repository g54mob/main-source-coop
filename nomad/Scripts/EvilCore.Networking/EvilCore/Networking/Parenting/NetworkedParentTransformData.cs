using System;

namespace EvilCore.Networking.Parenting
{
	[Serializable]
	public struct NetworkedParentTransformData
	{
		public uint ParentNetworkedTransformNetId;

		public byte SubTransformIndex;

		public NetworkedTransformParentingConfig ParentingConfig;

		public bool HasParent
		{
			get
			{
				if (ParentNetworkedTransformNetId != 0)
				{
					return SubTransformIndex != 255;
				}
				return false;
			}
		}
	}
}
