using System.Collections.Generic;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;

namespace Mimicraft.UI
{
	public class KillFeedView : MonoBehaviour
	{
		[Tooltip("Satırların ekleneceği obje. Ekranın sağ üstüne yerleştirilir; dikey bir LayoutGroup koyarsan satırlar kendiliğinden dizilir.")]
		[SerializeField]
		private RectTransform container;

		[Tooltip("Elle hazırlanmış satır şablonu - üzerinde bir TextMeshProUGUI olmalı. PASİF bırakılır; klonları aktif edilir. Yazı tipi, renk ve hizalama tamamen buradan gelir.")]
		[SerializeField]
		private GameObject entryTemplate;

		[Tooltip("Bir satırın ekranda kalma süresi (saniye).")]
		[SerializeField]
		[Min(0.5f)]
		private float entrySeconds = 6f;

		[Tooltip("Aynı anda en fazla kaç satır durur. Fazlası en eskisini düşürür.")]
		[SerializeField]
		[Min(1f)]
		private int maxEntries = 5;

		[Tooltip("Bir Avcı bir Modelciyi bulduğunda. {0} bulan, {1} bulunan.")]
		[SerializeField]
		private string foundFormat = "{0} found {1}";

		[Tooltip("Bir Avcı kendi ıskalarıyla elendiğinde. {0} elenen oyuncu.")]
		[SerializeField]
		private string selfEliminationFormat = "{0} eliminated himself";

		private readonly List<GameObject> entries = new List<GameObject>();

		private readonly List<float> expiryTimes = new List<float>();

		public static KillFeedView Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
			entryTemplate.SetActive(value: false);
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void ReportFound(string finderName, string victimName)
		{
			Add(string.Format(foundFormat, finderName, victimName));
		}

		public void ReportSelfElimination(string playerName)
		{
			Add(string.Format(selfEliminationFormat, playerName));
		}

		public void ReportKill(string killerName, string victimName)
		{
			Add(string.Format(Loc.Get("KillFeed.Killed"), killerName, victimName));
		}

		public void Add(string text)
		{
			if (!(container == null) && !(entryTemplate == null) && !string.IsNullOrEmpty(text))
			{
				GameObject gameObject = Object.Instantiate(entryTemplate, container);
				gameObject.name = "KillFeedEntry";
				TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
				if (componentInChildren != null)
				{
					componentInChildren.text = text;
				}
				gameObject.SetActive(value: true);
				entries.Add(gameObject);
				expiryTimes.Add(Time.unscaledTime + entrySeconds);
				while (entries.Count > maxEntries)
				{
					RemoveAt(0);
				}
			}
		}

		private void Update()
		{
			while (entries.Count > 0 && Time.unscaledTime >= expiryTimes[0])
			{
				RemoveAt(0);
			}
		}

		private void RemoveAt(int index)
		{
			if (entries[index] != null)
			{
				Object.Destroy(entries[index]);
			}
			entries.RemoveAt(index);
			expiryTimes.RemoveAt(index);
		}
	}
}
