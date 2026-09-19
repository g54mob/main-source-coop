namespace Photon.Realtime
{
	public class TypedLobby
	{
		private static readonly TypedLobby DefaultLobby = new TypedLobby();

		public string Name { get; protected set; }

		public LobbyType Type { get; protected set; }

		public static TypedLobby Default => DefaultLobby;

		public bool IsDefault => string.IsNullOrEmpty(Name);

		public TypedLobby(string name, LobbyType type)
		{
			string.IsNullOrEmpty(name);
			Name = name;
			Type = type;
		}

		public TypedLobby(TypedLobby original = null)
		{
			if (original != null)
			{
				Name = original.Name;
				Type = original.Type;
			}
		}

		public override string ToString()
		{
			return $"'{Name}'[{Type}]";
		}
	}
}
