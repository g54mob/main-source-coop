namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	public class TryGetDataFromSaveFileResultData
	{
		public TryGetDataFromSaveFileResult Result { get; }

		public string SaveData { get; }

		public TryGetDataFromSaveFileResultData(TryGetDataFromSaveFileResult result, string saveData)
		{
			Result = result;
			SaveData = saveData;
		}
	}
}
