namespace Features.ItemsModule.Scripts
{
	public interface IDisplayedCurrencyOverride
	{
		object DisplayedCurrencyGroupKey { get; }

		bool TryGetDisplayedCurrency(out int currency);
	}
}
