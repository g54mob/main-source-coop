namespace Portningsbolaget
{
	public struct BlockedPlayer
	{
		public string Nickname;

		public int AccountHash;

		public bool OnPlatform;

		public string Serialize()
		{
			return $"{Nickname}_{AccountHash}";
		}

		public void Deserialize(string data)
		{
			string[] array = data.Split('_');
			Nickname = array[0];
			AccountHash = int.Parse(array[1]);
		}

		public bool Has(string nickname, int accountHash, bool onPlatform)
		{
			if (nickname == Nickname && accountHash == AccountHash)
			{
				return onPlatform == OnPlatform;
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}", Nickname, AccountHash, OnPlatform ? "This Platform" : "Other Platform");
		}
	}
}
