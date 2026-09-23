using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class ProjectorCookieChanger : MonoBehaviour
	{
		[Tooltip("Cookie'si değiştirilecek ışıklar. Boş bırakılırsa bu objenin üzerindeki ve altındaki ışıklar kullanılır - tek bir projektör için hiçbir şey atamana gerek yok.")]
		[SerializeField]
		private Light[] projectors;

		[Tooltip("Bir görselin ekranda kalma süresi, saniye. Sunucu saatinden hesaplandığı için bu değer HER MAKİNEDE AYNI olmalı - sahnede duran bir alan olduğu için zaten öyle.")]
		[SerializeField]
		[Min(0.1f)]
		private float secondsPerCookie = 6f;

		[Tooltip("Haritanın kendi görselleri yokken kullanılacak görseller. Boş bırakılırsa ışıkların sahnede authored edilmiş cookie'si olduğu gibi kalır.")]
		[SerializeField]
		private Texture[] fallbackCookies;

		private IReadOnlyList<Texture> cookies = Array.Empty<Texture>();

		private int shownIndex = -1;

		private static double Clock
		{
			get
			{
				NetworkManager singleton = NetworkManager.Singleton;
				if (!(singleton != null) || !singleton.IsListening)
				{
					return Time.unscaledTimeAsDouble;
				}
				return singleton.ServerTime.Time;
			}
		}

		private void Awake()
		{
			if (projectors == null || projectors.Length == 0)
			{
				projectors = GetComponentsInChildren<Light>(includeInactive: true);
			}
		}

		private void OnEnable()
		{
			MapLoader.MapChanged += OnMapChanged;
			OnMapChanged(MapLoader.CurrentMap);
		}

		private void OnDisable()
		{
			MapLoader.MapChanged -= OnMapChanged;
		}

		private void OnMapChanged(MapScriptableObject map)
		{
			IReadOnlyList<Texture> readOnlyList = ((map != null) ? map.ProjectorCookies : null);
			object obj;
			if (readOnlyList == null || readOnlyList.Count <= 0)
			{
				IReadOnlyList<Texture> readOnlyList2 = fallbackCookies;
				obj = readOnlyList2 ?? Array.Empty<Texture>();
			}
			else
			{
				obj = readOnlyList;
			}
			cookies = (IReadOnlyList<Texture>)obj;
			shownIndex = -1;
		}

		private void Update()
		{
			if (cookies.Count == 0)
			{
				return;
			}
			int num = (int)(Clock / (double)secondsPerCookie % (double)cookies.Count);
			if (num == shownIndex)
			{
				return;
			}
			shownIndex = num;
			Texture texture = cookies[num];
			if (texture == null)
			{
				return;
			}
			Light[] array = projectors;
			foreach (Light light in array)
			{
				if (light != null)
				{
					light.cookie = texture;
				}
			}
		}
	}
}
