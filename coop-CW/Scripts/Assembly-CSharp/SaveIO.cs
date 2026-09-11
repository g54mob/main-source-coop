using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class SaveIO
{
	public static Save LoadSave(FileInfo fileInfo, int saveIndex)
	{
		Save save = new Save(saveIndex);
		DateTime lastWriteTime = File.GetLastWriteTime(fileInfo.FullName);
		string data = File.ReadAllText(fileInfo.FullName).Split("\n")[1];
		save.Deserialize(data);
		save.Date = lastWriteTime;
		return save;
	}

	public static IEnumerable<FileInfo> GetSaveFiles()
	{
		return from f in new DirectoryInfo(SaveSystem.SAVE_LOCATION).GetFiles()
			where f.Extension == SaveSystem.BIN_EXTENSION
			select f;
	}

	public static void SaveTo(string saveLocation, Save currentSave)
	{
		string text = currentSave.Serialize();
		string contents = "version:" + SaveSystem.CURRENT_VERSION + "\n" + text;
		File.WriteAllText(saveLocation, contents);
	}
}
