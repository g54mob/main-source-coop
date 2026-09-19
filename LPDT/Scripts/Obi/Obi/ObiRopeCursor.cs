using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Rope Cursor", 883)]
	[RequireComponent(typeof(ObiRope))]
	public class ObiRopeCursor : MonoBehaviour
	{
		private ObiRope rope;

		[Range(0f, 1f)]
		[HideInInspector]
		[SerializeField]
		private float m_CursorMu;

		[Range(0f, 1f)]
		[HideInInspector]
		[SerializeField]
		private float m_SourceMu;

		public bool direction = true;

		private ObiStructuralElement m_CursorElement;

		private int m_SourceIndex = -1;

		private float lengthChange;

		public float cursorMu
		{
			get
			{
				return m_CursorMu;
			}
			set
			{
				m_CursorMu = value;
				UpdateCursor();
			}
		}

		public float sourceMu
		{
			get
			{
				return m_SourceMu;
			}
			set
			{
				m_SourceMu = value;
				UpdateSource();
			}
		}

		public ObiStructuralElement cursorElement
		{
			get
			{
				if (m_CursorElement == null)
				{
					UpdateCursor();
				}
				return m_CursorElement;
			}
		}

		public int sourceParticleIndex
		{
			get
			{
				if (m_SourceIndex < 0)
				{
					UpdateSource();
				}
				return m_SourceIndex;
			}
		}

		private void OnEnable()
		{
			rope = GetComponent<ObiRope>();
			rope.OnElementsGenerated += Actor_OnElementsGenerated;
			rope.OnSimulationStart += Rope_OnSimulate;
			if (rope.elements != null && rope.elements.Count > 0)
			{
				Actor_OnElementsGenerated(rope);
			}
		}

		private void OnDisable()
		{
			rope.OnElementsGenerated -= Actor_OnElementsGenerated;
			rope.OnSimulationStart -= Rope_OnSimulate;
		}

		private void Actor_OnElementsGenerated(ObiActor actor)
		{
			UpdateCursor();
			UpdateSource();
		}

		private void Rope_OnSimulate(ObiActor actor, float simulatedTime, float substepTime)
		{
			if (!rope.isLoaded || Mathf.Abs(lengthChange) < 1E-07f)
			{
				return;
			}
			ObiSolver solver = rope.solver;
			if (lengthChange < 0f)
			{
				lengthChange = 0f - lengthChange;
				while (lengthChange > m_CursorElement.restLength)
				{
					lengthChange -= m_CursorElement.restLength;
					if (rope.elements.Count == 1)
					{
						break;
					}
					int num = rope.elements.IndexOf(m_CursorElement);
					if (num < 0)
					{
						continue;
					}
					if (direction)
					{
						RemoveParticleAt(solver.particleToActor[m_CursorElement.particle2].indexInActor);
						rope.elements.RemoveAt(num);
						if (num < rope.elements.Count)
						{
							if (rope.elements[num].particle1 == m_CursorElement.particle2)
							{
								rope.elements[num].particle1 = m_CursorElement.particle1;
							}
							m_CursorElement = rope.elements[num];
						}
						else
						{
							m_CursorElement = rope.elements[Mathf.Max(0, num - 1)];
						}
						continue;
					}
					RemoveParticleAt(solver.particleToActor[m_CursorElement.particle1].indexInActor);
					rope.elements.RemoveAt(num);
					if (num > 0)
					{
						if (rope.elements[num - 1].particle2 == m_CursorElement.particle1)
						{
							rope.elements[num - 1].particle2 = m_CursorElement.particle2;
						}
						m_CursorElement = rope.elements[num - 1];
					}
					else
					{
						m_CursorElement = rope.elements[0];
					}
				}
				if (lengthChange > 0f)
				{
					m_CursorElement.restLength = Mathf.Max(0f, m_CursorElement.restLength - lengthChange);
				}
			}
			else
			{
				float num2 = Mathf.Min(lengthChange, Mathf.Max(0f, rope.ropeBlueprint.interParticleDistance - m_CursorElement.restLength));
				if (num2 > 0f)
				{
					m_CursorElement.restLength += num2;
					lengthChange -= num2;
				}
				while (rope.activeParticleCount < rope.sourceBlueprint.particleCount && m_CursorElement.restLength + lengthChange > rope.ropeBlueprint.interParticleDistance)
				{
					num2 = Mathf.Min(lengthChange, rope.ropeBlueprint.interParticleDistance);
					lengthChange -= num2;
					if (direction)
					{
						int num3 = AddParticleAt(solver.particleToActor[m_CursorElement.particle1].indexInActor);
						solver.positions[num3] = solver.positions[m_CursorElement.particle1] + (solver.positions[m_CursorElement.particle2] - solver.positions[m_CursorElement.particle1]) * num2;
						ObiStructuralElement obiStructuralElement = new ObiStructuralElement();
						obiStructuralElement.restLength = num2;
						obiStructuralElement.particle1 = m_CursorElement.particle1;
						obiStructuralElement.particle2 = num3;
						m_CursorElement.particle1 = num3;
						int index = rope.elements.IndexOf(m_CursorElement);
						rope.elements.Insert(index, obiStructuralElement);
						m_CursorElement = obiStructuralElement;
					}
					else
					{
						int num4 = AddParticleAt(solver.particleToActor[m_CursorElement.particle2].indexInActor);
						solver.positions[num4] = solver.positions[m_CursorElement.particle2] + (solver.positions[m_CursorElement.particle1] - solver.positions[m_CursorElement.particle2]) * num2;
						ObiStructuralElement obiStructuralElement2 = new ObiStructuralElement();
						obiStructuralElement2.restLength = num2;
						obiStructuralElement2.particle1 = num4;
						obiStructuralElement2.particle2 = m_CursorElement.particle2;
						m_CursorElement.particle2 = num4;
						int num5 = rope.elements.IndexOf(m_CursorElement);
						rope.elements.Insert(num5 + 1, obiStructuralElement2);
						m_CursorElement = obiStructuralElement2;
					}
				}
				if (lengthChange > 0f)
				{
					m_CursorElement.restLength += lengthChange;
				}
			}
			rope.RecalculateRestPositions();
			rope.RecalculateRestLength();
			rope.RebuildConstraintsFromElements();
			lengthChange = 0f;
		}

		public void UpdateCursor()
		{
			rope = GetComponent<ObiRope>();
			m_CursorElement = null;
			if (rope.isLoaded)
			{
				m_CursorElement = rope.GetElementAt(cursorMu, out var _);
			}
		}

		public void UpdateSource()
		{
			rope = GetComponent<ObiRope>();
			m_SourceIndex = -1;
			if (rope.isLoaded)
			{
				float elementMu;
				ObiStructuralElement elementAt = rope.GetElementAt(sourceMu, out elementMu);
				if (elementAt != null && rope.solver != null)
				{
					m_SourceIndex = ((elementMu < 0.5f) ? elementAt.particle1 : elementAt.particle2);
				}
			}
		}

		private int AddParticleAt(int index)
		{
			int activeParticleCount = rope.activeParticleCount;
			rope.CopyParticle(rope.solver.particleToActor[m_SourceIndex].indexInActor, activeParticleCount);
			rope.TeleportParticle(activeParticleCount, rope.solver.positions[rope.solverIndices[index]]);
			rope.ActivateParticle();
			rope.SetRenderingDirty(Oni.RenderingSystemType.AllRopes);
			return rope.solverIndices[activeParticleCount];
		}

		private void RemoveParticleAt(int index)
		{
			rope.DeactivateParticle(index);
			rope.SetRenderingDirty(Oni.RenderingSystemType.AllRopes);
		}

		public float ChangeLength(float lengthChange)
		{
			this.lengthChange += lengthChange;
			return this.lengthChange + rope.restLength;
		}
	}
}
