namespace Mirror
{
	public static class PredictedSyncDataReadWrite
	{
		public static void WritePredictedSyncData(this NetworkWriter writer, PredictedSyncData data)
		{
			writer.WriteBlittable(data);
		}

		public static PredictedSyncData ReadPredictedSyncData(this NetworkReader reader)
		{
			return reader.ReadBlittable<PredictedSyncData>();
		}
	}
}
