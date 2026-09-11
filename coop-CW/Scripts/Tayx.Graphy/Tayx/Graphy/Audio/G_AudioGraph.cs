using Tayx.Graphy.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Audio
{
	public class G_AudioGraph : G_Graph
	{
		[SerializeField]
		private Image m_imageGraph;

		[SerializeField]
		private Image m_imageGraphHighestValues;

		[SerializeField]
		private Shader ShaderFull;

		[SerializeField]
		private Shader ShaderLight;

		[SerializeField]
		private bool m_isInitialized;

		private GraphyManager m_graphyManager;

		private G_AudioMonitor m_audioMonitor;

		private int m_resolution = 40;

		private G_GraphShader m_shaderGraph;

		private G_GraphShader m_shaderGraphHighestValues;

		private float[] m_graphArray;

		private float[] m_graphArrayHighestValue;

		private void OnEnable()
		{
			m_audioMonitor = GetComponent<G_AudioMonitor>();
		}

		private void Update()
		{
			if (m_audioMonitor.SpectrumDataAvailable)
			{
				UpdateGraph();
			}
		}

		public void UpdateParameters()
		{
			if (m_shaderGraph != null)
			{
				switch (m_graphyManager.GraphyMode)
				{
				case GraphyManager.Mode.FULL:
					m_shaderGraph.ArrayMaxSize = 512;
					m_shaderGraph.Image.material = new Material(ShaderFull);
					m_shaderGraphHighestValues.ArrayMaxSize = 512;
					m_shaderGraphHighestValues.Image.material = new Material(ShaderFull);
					break;
				case GraphyManager.Mode.LIGHT:
					m_shaderGraph.ArrayMaxSize = 128;
					m_shaderGraph.Image.material = new Material(ShaderLight);
					m_shaderGraphHighestValues.ArrayMaxSize = 128;
					m_shaderGraphHighestValues.Image.material = new Material(ShaderLight);
					break;
				}
				m_shaderGraph.InitializeShader();
				m_shaderGraphHighestValues.InitializeShader();
				m_resolution = m_graphyManager.AudioGraphResolution;
				CreatePoints();
			}
		}

		protected override void UpdateGraph()
		{
			if (!m_isInitialized)
			{
				Init();
			}
			int num = Mathf.FloorToInt((float)m_audioMonitor.Spectrum.Length / (float)m_resolution);
			for (int i = 0; i <= m_resolution - 1; i++)
			{
				float num2 = 0f;
				for (int j = 0; j < num; j++)
				{
					num2 += m_audioMonitor.Spectrum[i * num + j];
				}
				if ((i + 1) % 3 == 0 && i > 1)
				{
					float num3 = (m_audioMonitor.dBNormalized(m_audioMonitor.lin2dB(num2 / (float)num)) + m_graphArray[i - 1] + m_graphArray[i - 2]) / 3f;
					m_graphArray[i] = num3;
					m_graphArray[i - 1] = num3;
					m_graphArray[i - 2] = -1f;
				}
				else
				{
					m_graphArray[i] = m_audioMonitor.dBNormalized(m_audioMonitor.lin2dB(num2 / (float)num));
				}
			}
			for (int k = 0; k <= m_resolution - 1; k++)
			{
				m_shaderGraph.ShaderArrayValues[k] = m_graphArray[k];
			}
			m_shaderGraph.UpdatePoints();
			for (int l = 0; l <= m_resolution - 1; l++)
			{
				float num4 = 0f;
				for (int m = 0; m < num; m++)
				{
					num4 += m_audioMonitor.SpectrumHighestValues[l * num + m];
				}
				if ((l + 1) % 3 == 0 && l > 1)
				{
					float num5 = (m_audioMonitor.dBNormalized(m_audioMonitor.lin2dB(num4 / (float)num)) + m_graphArrayHighestValue[l - 1] + m_graphArrayHighestValue[l - 2]) / 3f;
					m_graphArrayHighestValue[l] = num5;
					m_graphArrayHighestValue[l - 1] = num5;
					m_graphArrayHighestValue[l - 2] = -1f;
				}
				else
				{
					m_graphArrayHighestValue[l] = m_audioMonitor.dBNormalized(m_audioMonitor.lin2dB(num4 / (float)num));
				}
			}
			for (int n = 0; n <= m_resolution - 1; n++)
			{
				m_shaderGraphHighestValues.ShaderArrayValues[n] = m_graphArrayHighestValue[n];
			}
			m_shaderGraphHighestValues.UpdatePoints();
		}

		protected override void CreatePoints()
		{
			if (m_shaderGraph.ShaderArrayValues == null || m_shaderGraph.ShaderArrayValues.Length != m_resolution)
			{
				m_graphArray = new float[m_resolution];
				m_graphArrayHighestValue = new float[m_resolution];
				m_shaderGraph.ShaderArrayValues = new float[m_resolution];
				m_shaderGraphHighestValues.ShaderArrayValues = new float[m_resolution];
			}
			for (int i = 0; i < m_resolution; i++)
			{
				m_shaderGraph.ShaderArrayValues[i] = 0f;
				m_shaderGraphHighestValues.ShaderArrayValues[i] = 0f;
			}
			m_shaderGraph.GoodColor = m_graphyManager.AudioGraphColor;
			m_shaderGraph.CautionColor = m_graphyManager.AudioGraphColor;
			m_shaderGraph.CriticalColor = m_graphyManager.AudioGraphColor;
			m_shaderGraph.UpdateColors();
			m_shaderGraphHighestValues.GoodColor = m_graphyManager.AudioGraphColor;
			m_shaderGraphHighestValues.CautionColor = m_graphyManager.AudioGraphColor;
			m_shaderGraphHighestValues.CriticalColor = m_graphyManager.AudioGraphColor;
			m_shaderGraphHighestValues.UpdateColors();
			m_shaderGraph.GoodThreshold = 0f;
			m_shaderGraph.CautionThreshold = 0f;
			m_shaderGraph.UpdateThresholds();
			m_shaderGraphHighestValues.GoodThreshold = 0f;
			m_shaderGraphHighestValues.CautionThreshold = 0f;
			m_shaderGraphHighestValues.UpdateThresholds();
			m_shaderGraph.UpdateArrayValuesLength();
			m_shaderGraphHighestValues.UpdateArrayValuesLength();
			m_shaderGraph.Average = 0f;
			m_shaderGraph.UpdateAverage();
			m_shaderGraphHighestValues.Average = 0f;
			m_shaderGraphHighestValues.UpdateAverage();
		}

		private void Init()
		{
			m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			m_audioMonitor = GetComponent<G_AudioMonitor>();
			m_shaderGraph = new G_GraphShader
			{
				Image = m_imageGraph
			};
			m_shaderGraphHighestValues = new G_GraphShader
			{
				Image = m_imageGraphHighestValues
			};
			UpdateParameters();
			m_isInitialized = true;
		}
	}
}
