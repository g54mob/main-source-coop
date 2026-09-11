using Tayx.Graphy.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Ram
{
	public class G_RamGraph : G_Graph
	{
		[SerializeField]
		private Image m_imageAllocated;

		[SerializeField]
		private Image m_imageReserved;

		[SerializeField]
		private Image m_imageMono;

		[SerializeField]
		private Shader ShaderFull;

		[SerializeField]
		private Shader ShaderLight;

		[SerializeField]
		private bool m_isInitialized;

		private GraphyManager m_graphyManager;

		private G_RamMonitor m_ramMonitor;

		private int m_resolution = 150;

		private G_GraphShader m_shaderGraphAllocated;

		private G_GraphShader m_shaderGraphReserved;

		private G_GraphShader m_shaderGraphMono;

		private float[] m_allocatedArray;

		private float[] m_reservedArray;

		private float[] m_monoArray;

		private float m_highestMemory;

		private void Update()
		{
			UpdateGraph();
		}

		public void UpdateParameters()
		{
			if (m_shaderGraphAllocated != null && m_shaderGraphReserved != null && m_shaderGraphMono != null)
			{
				switch (m_graphyManager.GraphyMode)
				{
				case GraphyManager.Mode.FULL:
					m_shaderGraphAllocated.ArrayMaxSize = 512;
					m_shaderGraphReserved.ArrayMaxSize = 512;
					m_shaderGraphMono.ArrayMaxSize = 512;
					m_shaderGraphAllocated.Image.material = new Material(ShaderFull);
					m_shaderGraphReserved.Image.material = new Material(ShaderFull);
					m_shaderGraphMono.Image.material = new Material(ShaderFull);
					break;
				case GraphyManager.Mode.LIGHT:
					m_shaderGraphAllocated.ArrayMaxSize = 128;
					m_shaderGraphReserved.ArrayMaxSize = 128;
					m_shaderGraphMono.ArrayMaxSize = 128;
					m_shaderGraphAllocated.Image.material = new Material(ShaderLight);
					m_shaderGraphReserved.Image.material = new Material(ShaderLight);
					m_shaderGraphMono.Image.material = new Material(ShaderLight);
					break;
				}
				m_shaderGraphAllocated.InitializeShader();
				m_shaderGraphReserved.InitializeShader();
				m_shaderGraphMono.InitializeShader();
				m_resolution = m_graphyManager.RamGraphResolution;
				CreatePoints();
			}
		}

		protected override void UpdateGraph()
		{
			if (!m_isInitialized)
			{
				Init();
			}
			float allocatedRam = m_ramMonitor.AllocatedRam;
			float reservedRam = m_ramMonitor.ReservedRam;
			float monoRam = m_ramMonitor.MonoRam;
			m_highestMemory = 0f;
			for (int i = 0; i <= m_resolution - 1; i++)
			{
				if (i >= m_resolution - 1)
				{
					m_allocatedArray[i] = allocatedRam;
					m_reservedArray[i] = reservedRam;
					m_monoArray[i] = monoRam;
				}
				else
				{
					m_allocatedArray[i] = m_allocatedArray[i + 1];
					m_reservedArray[i] = m_reservedArray[i + 1];
					m_monoArray[i] = m_monoArray[i + 1];
				}
				if (m_highestMemory < m_reservedArray[i])
				{
					m_highestMemory = m_reservedArray[i];
				}
			}
			for (int j = 0; j <= m_resolution - 1; j++)
			{
				m_shaderGraphAllocated.ShaderArrayValues[j] = m_allocatedArray[j] / m_highestMemory;
				m_shaderGraphReserved.ShaderArrayValues[j] = m_reservedArray[j] / m_highestMemory;
				m_shaderGraphMono.ShaderArrayValues[j] = m_monoArray[j] / m_highestMemory;
			}
			m_shaderGraphAllocated.UpdatePoints();
			m_shaderGraphReserved.UpdatePoints();
			m_shaderGraphMono.UpdatePoints();
		}

		protected override void CreatePoints()
		{
			if (m_shaderGraphAllocated.ShaderArrayValues == null || m_shaderGraphAllocated.ShaderArrayValues.Length != m_resolution)
			{
				m_allocatedArray = new float[m_resolution];
				m_reservedArray = new float[m_resolution];
				m_monoArray = new float[m_resolution];
				m_shaderGraphAllocated.ShaderArrayValues = new float[m_resolution];
				m_shaderGraphReserved.ShaderArrayValues = new float[m_resolution];
				m_shaderGraphMono.ShaderArrayValues = new float[m_resolution];
			}
			for (int i = 0; i < m_resolution; i++)
			{
				m_shaderGraphAllocated.ShaderArrayValues[i] = 0f;
				m_shaderGraphReserved.ShaderArrayValues[i] = 0f;
				m_shaderGraphMono.ShaderArrayValues[i] = 0f;
			}
			m_shaderGraphAllocated.GoodColor = m_graphyManager.AllocatedRamColor;
			m_shaderGraphAllocated.CautionColor = m_graphyManager.AllocatedRamColor;
			m_shaderGraphAllocated.CriticalColor = m_graphyManager.AllocatedRamColor;
			m_shaderGraphAllocated.UpdateColors();
			m_shaderGraphReserved.GoodColor = m_graphyManager.ReservedRamColor;
			m_shaderGraphReserved.CautionColor = m_graphyManager.ReservedRamColor;
			m_shaderGraphReserved.CriticalColor = m_graphyManager.ReservedRamColor;
			m_shaderGraphReserved.UpdateColors();
			m_shaderGraphMono.GoodColor = m_graphyManager.MonoRamColor;
			m_shaderGraphMono.CautionColor = m_graphyManager.MonoRamColor;
			m_shaderGraphMono.CriticalColor = m_graphyManager.MonoRamColor;
			m_shaderGraphMono.UpdateColors();
			m_shaderGraphAllocated.GoodThreshold = 0f;
			m_shaderGraphAllocated.CautionThreshold = 0f;
			m_shaderGraphAllocated.UpdateThresholds();
			m_shaderGraphReserved.GoodThreshold = 0f;
			m_shaderGraphReserved.CautionThreshold = 0f;
			m_shaderGraphReserved.UpdateThresholds();
			m_shaderGraphMono.GoodThreshold = 0f;
			m_shaderGraphMono.CautionThreshold = 0f;
			m_shaderGraphMono.UpdateThresholds();
			m_shaderGraphAllocated.UpdateArrayValuesLength();
			m_shaderGraphReserved.UpdateArrayValuesLength();
			m_shaderGraphMono.UpdateArrayValuesLength();
			m_shaderGraphAllocated.Average = 0f;
			m_shaderGraphReserved.Average = 0f;
			m_shaderGraphMono.Average = 0f;
			m_shaderGraphAllocated.UpdateAverage();
			m_shaderGraphReserved.UpdateAverage();
			m_shaderGraphMono.UpdateAverage();
		}

		private void Init()
		{
			m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			m_ramMonitor = GetComponent<G_RamMonitor>();
			m_shaderGraphAllocated = new G_GraphShader();
			m_shaderGraphReserved = new G_GraphShader();
			m_shaderGraphMono = new G_GraphShader();
			m_shaderGraphAllocated.Image = m_imageAllocated;
			m_shaderGraphReserved.Image = m_imageReserved;
			m_shaderGraphMono.Image = m_imageMono;
			UpdateParameters();
			m_isInitialized = true;
		}
	}
}
