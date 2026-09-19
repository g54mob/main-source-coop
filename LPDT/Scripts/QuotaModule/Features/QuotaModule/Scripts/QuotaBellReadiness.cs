namespace Features.QuotaModule.Scripts
{
	public static class QuotaBellReadiness
	{
		public static bool IsQuotaMet(QuotaCompletionModel completionModel, QuotaSynchronizedModel quotaModel)
		{
			if (completionModel.IsQuotaCompleted.Value)
			{
				return true;
			}
			if (quotaModel.MaxQuota.Value > 0f)
			{
				return quotaModel.CurrentQuota.Value >= quotaModel.MaxQuota.Value;
			}
			return false;
		}
	}
}
