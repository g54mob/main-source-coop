using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class GraphColoring
	{
		private List<int> m_ParticleIndices;

		private List<int> m_ConstraintIndices;

		private List<List<int>> m_ConstraintsPerParticle;

		public IReadOnlyList<int> particleIndices => m_ParticleIndices.AsReadOnly();

		public IReadOnlyList<int> constraintIndices => m_ConstraintIndices.AsReadOnly();

		public GraphColoring(int particleCount = 0)
		{
			m_ParticleIndices = new List<int>();
			m_ConstraintIndices = new List<int>();
			m_ConstraintsPerParticle = new List<List<int>>(particleCount);
			for (int i = 0; i < particleCount; i++)
			{
				m_ConstraintsPerParticle.Add(new List<int>());
			}
		}

		public void Clear()
		{
			m_ParticleIndices.Clear();
			m_ConstraintIndices.Clear();
			for (int i = 0; i < m_ConstraintsPerParticle.Count; i++)
			{
				m_ConstraintsPerParticle[i].Clear();
			}
		}

		public void AddConstraint(int[] particles)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				while (particles[i] >= m_ConstraintsPerParticle.Count)
				{
					m_ConstraintsPerParticle.Add(new List<int>());
				}
				m_ConstraintsPerParticle[particles[i]].Add(m_ConstraintIndices.Count);
			}
			m_ConstraintIndices.Add(m_ParticleIndices.Count);
			m_ParticleIndices.AddRange(particles);
		}

		public IEnumerator Colorize(string progressDescription, List<int> colors)
		{
			m_ConstraintIndices.Add(m_ParticleIndices.Count);
			int constraintCount = Mathf.Max(0, m_ConstraintIndices.Count - 1);
			colors.Clear();
			if (constraintCount == 0)
			{
				yield break;
			}
			colors.Capacity = constraintCount;
			bool[] availability = new bool[constraintCount];
			for (int i = 0; i < constraintCount; i++)
			{
				colors.Add(-1);
				availability[i] = true;
			}
			int i2 = 0;
			while (i2 < constraintCount)
			{
				for (int j = m_ConstraintIndices[i2]; j < m_ConstraintIndices[i2 + 1]; j++)
				{
					foreach (int item in m_ConstraintsPerParticle[m_ParticleIndices[j]])
					{
						if (i2 != item && colors[item] >= 0)
						{
							availability[colors[item]] = false;
						}
					}
				}
				colors[i2] = 0;
				int value;
				while (colors[i2] < constraintCount && !availability[colors[i2]])
				{
					int index = i2;
					value = colors[index] + 1;
					colors[index] = value;
				}
				for (int k = 0; k < constraintCount; k++)
				{
					availability[k] = true;
				}
				if (i2 % 250 == 0)
				{
					yield return new CoroutineJob.ProgressInfo(progressDescription, (float)i2 / (float)constraintCount);
				}
				value = i2 + 1;
				i2 = value;
			}
		}
	}
}
