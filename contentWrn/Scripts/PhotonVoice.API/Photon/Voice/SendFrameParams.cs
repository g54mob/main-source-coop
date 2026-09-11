namespace Photon.Voice
{
	public readonly struct SendFrameParams
	{
		public bool TargetMe { get; }

		public int[] TargetPlayers { get; }

		public byte InterestGroup { get; }

		public bool Reliable { get; }

		public bool Encrypt { get; }

		public SendFrameParams(bool targetMe, int[] targetPlayers, byte interestGroup, bool reliable, bool encrypt)
		{
			TargetMe = targetMe;
			TargetPlayers = targetPlayers;
			InterestGroup = interestGroup;
			Reliable = reliable;
			Encrypt = encrypt;
		}
	}
}
