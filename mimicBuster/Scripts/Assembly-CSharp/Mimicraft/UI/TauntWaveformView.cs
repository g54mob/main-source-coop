using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class TauntWaveformView : MaskableGraphic
	{
		[Tooltip("En fazla kaç çubuk çizilecek. Daha fazla örnek verilirse aralarından seçilir. Çok yüksek değerler ince çizgilerden oluşan bir bulanıklık üretir.")]
		[SerializeField]
		[Range(8f, 256f)]
		private int maxBars = 64;

		[Tooltip("Çubuklar arasındaki boşluğun çubuk genişliğine oranı. 0 = bitişik.")]
		[SerializeField]
		[Range(0f, 0.9f)]
		private float barGap = 0.25f;

		[Tooltip("Sessizlikte bile görünen en ince çubuğun yüksekliği (0..1). Tamamen sessiz bir kayıt boş bir kutu yerine düz bir çizgi olarak görünsün diye.")]
		[SerializeField]
		[Range(0f, 0.2f)]
		private float minBarHeight = 0.04f;

		[Tooltip("Kaydedilirken henüz doldurulmamış kısmın çizilip çizilmeyeceği. Kapalıysa dalga soldan sağa dolar; açıksa boş kısım da ince çizgiyle gösterilir.")]
		[SerializeField]
		private bool drawEmptyTail = true;

		[Tooltip("Ses çalarken geride kalan (çalınmış) kısmın rengi. Çalma bitince dalga tek renge döner.")]
		[SerializeField]
		private Color playedColor = new Color(0.35f, 0.85f, 0.4f);

		private readonly List<float> peaks = new List<float>();

		private float expected;

		private float progress;

		public float Progress
		{
			get
			{
				return progress;
			}
			set
			{
				float b = Mathf.Clamp01(value);
				if (!Mathf.Approximately(progress, b) && !SameBar(progress, b))
				{
					progress = b;
					SetVerticesDirty();
				}
			}
		}

		private bool SameBar(float a, float b)
		{
			if (Mathf.FloorToInt(a * (float)maxBars) == Mathf.FloorToInt(b * (float)maxBars))
			{
				return a > 0f == b > 0f;
			}
			return false;
		}

		public void Show(IReadOnlyList<float> values, int expectedCount = 0)
		{
			peaks.Clear();
			if (values != null)
			{
				for (int i = 0; i < values.Count; i++)
				{
					peaks.Add(Mathf.Clamp01(values[i]));
				}
			}
			expected = Mathf.Max(expectedCount, peaks.Count);
			SetVerticesDirty();
		}

		public void Clear()
		{
			if (peaks.Count != 0)
			{
				peaks.Clear();
				expected = 0f;
				SetVerticesDirty();
			}
		}

		protected override void OnPopulateMesh(VertexHelper helper)
		{
			helper.Clear();
			Rect pixelAdjustedRect = GetPixelAdjustedRect();
			if (pixelAdjustedRect.width <= 0f || pixelAdjustedRect.height <= 0f)
			{
				return;
			}
			int num = Mathf.Clamp(Mathf.RoundToInt((expected > 0f) ? Mathf.Min(expected, maxBars) : ((float)maxBars)), 1, maxBars);
			float num2 = pixelAdjustedRect.width / (float)num;
			float num3 = num2 * (1f - barGap);
			float num4 = pixelAdjustedRect.y + pixelAdjustedRect.height * 0.5f;
			for (int i = 0; i < num; i++)
			{
				float num5 = Mathf.Max(minBarHeight, PeakFor(i, num)) * pixelAdjustedRect.height * 0.5f;
				if (drawEmptyTail || peaks.Count <= 0 || Sample(i, num) < peaks.Count)
				{
					float num6 = pixelAdjustedRect.x + num2 * (float)i + (num2 - num3) * 0.5f;
					bool flag = progress > 0f && ((float)i + 0.5f) / (float)num <= progress;
					AddBar(helper, num6, num6 + num3, num4 - num5, num4 + num5, flag ? playedColor : color);
				}
			}
		}

		private float PeakFor(int bar, int bars)
		{
			if (peaks.Count == 0)
			{
				return 0f;
			}
			int num = Sample(bar, bars);
			int num2 = Mathf.Max(num + 1, Sample(bar + 1, bars));
			float num3 = 0f;
			for (int i = num; i < num2 && i < peaks.Count; i++)
			{
				num3 = Mathf.Max(num3, peaks[i]);
			}
			return num3;
		}

		private int Sample(int bar, int bars)
		{
			return Mathf.FloorToInt((float)bar / (float)bars * Mathf.Max(peaks.Count, expected));
		}

		private static void AddBar(VertexHelper helper, float left, float right, float bottom, float top, Color tint)
		{
			int currentVertCount = helper.currentVertCount;
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = tint;
			simpleVert.position = new Vector3(left, bottom);
			helper.AddVert(simpleVert);
			simpleVert.position = new Vector3(left, top);
			helper.AddVert(simpleVert);
			simpleVert.position = new Vector3(right, top);
			helper.AddVert(simpleVert);
			simpleVert.position = new Vector3(right, bottom);
			helper.AddVert(simpleVert);
			helper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			helper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}
	}
}
