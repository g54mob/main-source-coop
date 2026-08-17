namespace EvilCore.Networking
{
	public interface IUniqueNetworkComponent
	{
		bool IsActive { get; set; }

		void Activate()
		{
			IsActive = true;
		}

		void Deactivate()
		{
			IsActive = false;
		}
	}
}
