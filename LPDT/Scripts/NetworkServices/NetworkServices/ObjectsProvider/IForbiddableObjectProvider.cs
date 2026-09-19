namespace NetworkServices.ObjectsProvider
{
	public interface IForbiddableObjectProvider
	{
		bool IsAcquireInstanceAllowed { get; }

		void ForbidAcquireInstance();

		void AllowAcquireInstance();
	}
}
