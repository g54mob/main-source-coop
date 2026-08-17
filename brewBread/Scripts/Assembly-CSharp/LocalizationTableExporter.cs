using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class LocalizationTableExporter : MonoBehaviour
{
	public LocalizationTable table;

	public Locale english;

	public Locale spanish;

	public bool t;

	private void Update()
	{
		if (t)
		{
			t = false;
			StartCoroutine(CrearArchivoCSV("excelprueba"));
		}
	}

	private IEnumerator CrearArchivoCSV(string nombreArchivo)
	{
		string ruta = Application.persistentDataPath + "/" + nombreArchivo + ".csv";
		if (File.Exists(ruta))
		{
			File.Delete(ruta);
		}
		StreamWriter streamWriter = File.CreateText(ruta);
		List<SharedTableData.SharedTableEntry> entries = table.SharedData.Entries;
		string text = "";
		for (int i = 0; i < entries.Count; i++)
		{
			text = text + LocalizationSettings.StringDatabase.GetLocalizedString(table.TableCollectionName, entries[i].Key, english, FallbackBehavior.UseProjectSettings) + ",";
			text += LocalizationSettings.StringDatabase.GetLocalizedString(table.TableCollectionName, entries[i].Key, spanish, FallbackBehavior.UseProjectSettings);
			text += Environment.NewLine;
		}
		streamWriter.WriteLine(text);
		new FileInfo(ruta).IsReadOnly = false;
		streamWriter.Close();
		yield return new WaitForSeconds(0.5f);
		Application.OpenURL(ruta);
	}
}
