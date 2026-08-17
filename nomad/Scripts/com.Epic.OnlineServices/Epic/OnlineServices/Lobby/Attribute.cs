using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Lobby
{
	public struct Attribute
	{
		[CompilerGenerated]
		private LobbyAttributeVisibility _003CVisibility_003Ek__BackingField;

		public AttributeData? Data { get; set; }

		public LobbyAttributeVisibility Visibility
		{
			[CompilerGenerated]
			set
			{
				_003CVisibility_003Ek__BackingField = value;
			}
		}
	}
}
