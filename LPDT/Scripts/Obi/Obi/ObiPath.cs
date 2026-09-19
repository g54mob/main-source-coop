using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Obi
{
	[Serializable]
	public class ObiPath
	{
		[HideInInspector]
		[SerializeField]
		private List<string> m_Names = new List<string>();

		[HideInInspector]
		[SerializeField]
		public ObiPointsDataChannel m_Points = new ObiPointsDataChannel();

		[HideInInspector]
		[SerializeField]
		private ObiNormalDataChannel m_Normals = new ObiNormalDataChannel();

		[HideInInspector]
		[SerializeField]
		private ObiColorDataChannel m_Colors = new ObiColorDataChannel();

		[HideInInspector]
		[SerializeField]
		private ObiThicknessDataChannel m_Thickness = new ObiThicknessDataChannel();

		[HideInInspector]
		[SerializeField]
		private ObiMassDataChannel m_Masses = new ObiMassDataChannel();

		[HideInInspector]
		[SerializeField]
		private ObiRotationalMassDataChannel m_RotationalMasses = new ObiRotationalMassDataChannel();

		[FormerlySerializedAs("m_Phases")]
		[HideInInspector]
		[SerializeField]
		private ObiPhaseDataChannel m_Filters = new ObiPhaseDataChannel();

		[HideInInspector]
		[SerializeField]
		private bool m_Closed;

		protected bool dirty;

		protected const int arcLenghtSamples = 20;

		[HideInInspector]
		[SerializeField]
		protected List<float> m_ArcLengthTable = new List<float>();

		[HideInInspector]
		[SerializeField]
		protected float m_TotalSplineLenght;

		public UnityEvent OnPathChanged = new UnityEvent();

		public PathControlPointEvent OnControlPointAdded = new PathControlPointEvent();

		public PathControlPointEvent OnControlPointRemoved = new PathControlPointEvent();

		public PathControlPointEvent OnControlPointRenamed = new PathControlPointEvent();

		public ObiPointsDataChannel points => m_Points;

		public ObiNormalDataChannel normals => m_Normals;

		public ObiColorDataChannel colors => m_Colors;

		public ObiThicknessDataChannel thicknesses => m_Thickness;

		public ObiMassDataChannel masses => m_Masses;

		public ObiRotationalMassDataChannel rotationalMasses => m_RotationalMasses;

		public ObiPhaseDataChannel filters => m_Filters;

		public ReadOnlyCollection<float> ArcLengthTable => m_ArcLengthTable.AsReadOnly();

		public float Length => m_TotalSplineLenght;

		public int ArcLengthSamples => 20;

		public int ControlPointCount => m_Points.Count;

		public bool Closed
		{
			get
			{
				return m_Closed;
			}
			set
			{
				if (value != m_Closed)
				{
					m_Closed = value;
					dirty = true;
				}
			}
		}

		private IEnumerable<IObiPathDataChannel> GetDataChannels()
		{
			yield return m_Points;
			yield return m_Normals;
			yield return m_Colors;
			yield return m_Thickness;
			yield return m_Masses;
			yield return m_RotationalMasses;
			yield return m_Filters;
		}

		public int GetSpanCount()
		{
			return m_Points.GetSpanCount(m_Closed);
		}

		public int GetSpanControlPointForMu(float mu, out float spanMu)
		{
			return m_Points.GetSpanControlPointAtMu(m_Closed, mu, out spanMu);
		}

		public int GetClosestControlPointIndex(float mu)
		{
			float spanMu;
			int spanControlPointForMu = GetSpanControlPointForMu(mu, out spanMu);
			if (spanMu > 0.5f)
			{
				return (spanControlPointForMu + 1) % ControlPointCount;
			}
			return spanControlPointForMu % ControlPointCount;
		}

		public float GetMuAtLenght(float length)
		{
			if (length <= 0f)
			{
				return 0f;
			}
			if (length >= m_TotalSplineLenght)
			{
				return 1f;
			}
			int i;
			for (i = 1; i < m_ArcLengthTable.Count && !(length < m_ArcLengthTable[i]); i++)
			{
			}
			float num = (float)(i - 1) / (float)(m_ArcLengthTable.Count - 1);
			float num2 = (float)i / (float)(m_ArcLengthTable.Count - 1);
			float num3 = (length - m_ArcLengthTable[i - 1]) / (m_ArcLengthTable[i] - m_ArcLengthTable[i - 1]);
			return num + (num2 - num) * num3;
		}

		public float RecalculateLenght(Matrix4x4 referenceFrame, float acc, int maxevals)
		{
			m_TotalSplineLenght = 0f;
			m_ArcLengthTable.Clear();
			m_ArcLengthTable.Add(0f);
			float num = 1f / 21f;
			int controlPointCount = ControlPointCount;
			if (controlPointCount >= 2)
			{
				int spanCount = GetSpanCount();
				for (int i = 0; i < spanCount; i++)
				{
					int i2 = (i + 1) % controlPointCount;
					ObiWingedPoint obiWingedPoint = m_Points[i];
					ObiWingedPoint obiWingedPoint2 = m_Points[i2];
					Vector3 vector = referenceFrame.MultiplyPoint3x4(obiWingedPoint.position);
					Vector3 vector2 = referenceFrame.MultiplyPoint3x4(obiWingedPoint.outTangentEndpoint);
					Vector3 vector3 = referenceFrame.MultiplyPoint3x4(obiWingedPoint2.inTangentEndpoint);
					Vector3 vector4 = referenceFrame.MultiplyPoint3x4(obiWingedPoint2.position);
					for (int j = 0; j <= Mathf.Max(1, 20); j++)
					{
						float num2 = (float)j * num;
						float num3 = (float)(j + 1) * num;
						float num4 = GaussLobattoIntegrationStep(vector, vector2, vector3, vector4, num2, num3, m_Points.EvaluateFirstDerivative(vector, vector2, vector3, vector4, num2).magnitude, m_Points.EvaluateFirstDerivative(vector, vector2, vector3, vector4, num3).magnitude, 0, maxevals, acc);
						m_TotalSplineLenght += num4;
						m_ArcLengthTable.Add(m_TotalSplineLenght);
					}
				}
			}
			else
			{
				Debug.LogWarning("A path needs at least 2 control points to be defined.");
			}
			return m_TotalSplineLenght;
		}

		private float GaussLobattoIntegrationStep(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float a, float b, float fa, float fb, int nevals, int maxevals, float acc)
		{
			if (nevals >= maxevals)
			{
				return 0f;
			}
			float num = Mathf.Sqrt(2f / 3f);
			float num2 = 1f / Mathf.Sqrt(5f);
			float num3 = (b - a) / 2f;
			float num4 = (a + b) / 2f;
			float num5 = num4 - num * num3;
			float num6 = num4 - num2 * num3;
			float num7 = num4 + num2 * num3;
			float num8 = num4 + num * num3;
			nevals += 5;
			float magnitude = m_Points.EvaluateFirstDerivative(p1, p2, p3, p4, num5).magnitude;
			float magnitude2 = m_Points.EvaluateFirstDerivative(p1, p2, p3, p4, num6).magnitude;
			float magnitude3 = m_Points.EvaluateFirstDerivative(p1, p2, p3, p4, num4).magnitude;
			float magnitude4 = m_Points.EvaluateFirstDerivative(p1, p2, p3, p4, num7).magnitude;
			float magnitude5 = m_Points.EvaluateFirstDerivative(p1, p2, p3, p4, num8).magnitude;
			float num9 = num3 / 6f * (fa + fb + 5f * (magnitude2 + magnitude4));
			float num10 = num3 / 1470f * (77f * (fa + fb) + 432f * (magnitude + magnitude5) + 625f * (magnitude2 + magnitude4) + 672f * magnitude3);
			if (num9 - num10 < acc || num5 <= a || b <= num8)
			{
				if (!(num4 > a) || !(b > num4))
				{
					Debug.LogError("Spline integration reached an interval with no more machine numbers");
				}
				return num10;
			}
			return GaussLobattoIntegrationStep(p1, p2, p3, p4, a, num5, fa, magnitude, nevals, maxevals, acc) + GaussLobattoIntegrationStep(p1, p2, p3, p4, num5, num6, magnitude, magnitude2, nevals, maxevals, acc) + GaussLobattoIntegrationStep(p1, p2, p3, p4, num6, num4, magnitude2, magnitude3, nevals, maxevals, acc) + GaussLobattoIntegrationStep(p1, p2, p3, p4, num4, num7, magnitude3, magnitude4, nevals, maxevals, acc) + GaussLobattoIntegrationStep(p1, p2, p3, p4, num7, num8, magnitude4, magnitude5, nevals, maxevals, acc) + GaussLobattoIntegrationStep(p1, p2, p3, p4, num8, b, magnitude5, fb, nevals, maxevals, acc);
		}

		public void SetName(int index, string name)
		{
			m_Names[index] = name;
			if (OnControlPointRenamed != null)
			{
				OnControlPointRenamed.Invoke(index);
			}
			dirty = true;
		}

		public string GetName(int index)
		{
			return m_Names[index];
		}

		public void AddControlPoint(Vector3 position, Vector3 inTangentVector, Vector3 outTangentVector, Vector3 normal, float mass, float rotationalMass, float thickness, int filter, Color color, string name)
		{
			InsertControlPoint(ControlPointCount, position, inTangentVector, outTangentVector, normal, mass, rotationalMass, thickness, filter, color, name);
		}

		public void InsertControlPoint(int index, Vector3 position, Vector3 inTangentVector, Vector3 outTangentVector, Vector3 normal, float mass, float rotationalMass, float thickness, int filter, Color color, string name)
		{
			m_Points.data.Insert(index, new ObiWingedPoint(inTangentVector, position, outTangentVector));
			m_Colors.data.Insert(index, color);
			m_Normals.data.Insert(index, normal);
			m_Thickness.data.Insert(index, thickness);
			m_Masses.data.Insert(index, mass);
			m_RotationalMasses.data.Insert(index, rotationalMass);
			m_Filters.data.Insert(index, filter);
			m_Names.Insert(index, name);
			if (OnControlPointAdded != null)
			{
				OnControlPointAdded.Invoke(index);
			}
			dirty = true;
		}

		public int InsertControlPoint(float mu)
		{
			int controlPointCount = ControlPointCount;
			if (controlPointCount >= 2 && !float.IsNaN(mu))
			{
				float spanMu;
				int spanControlPointForMu = GetSpanControlPointForMu(mu, out spanMu);
				int i = (spanControlPointForMu + 1) % controlPointCount;
				ObiWingedPoint value = m_Points[spanControlPointForMu];
				ObiWingedPoint value2 = m_Points[i];
				Vector3 vector = (1f - spanMu) * value.position + spanMu * value.outTangentEndpoint;
				Vector3 vector2 = (1f - spanMu) * value.outTangentEndpoint + spanMu * value2.inTangentEndpoint;
				Vector3 vector3 = (1f - spanMu) * value2.inTangentEndpoint + spanMu * value2.position;
				Vector3 vector4 = (1f - spanMu) * vector + spanMu * vector2;
				Vector3 vector5 = (1f - spanMu) * vector2 + spanMu * vector3;
				Vector3 vector6 = (1f - spanMu) * vector4 + spanMu * vector5;
				value.SetOutTangentEndpoint(vector);
				value2.SetInTangentEndpoint(vector3);
				m_Points[spanControlPointForMu] = value;
				m_Points[i] = value2;
				Color color = m_Colors.Evaluate(m_Colors[spanControlPointForMu], m_Colors[spanControlPointForMu], m_Colors[i], m_Colors[i], spanMu);
				Vector3 normal = m_Normals.Evaluate(m_Normals[spanControlPointForMu], m_Normals[spanControlPointForMu], m_Normals[i], m_Normals[i], spanMu);
				float thickness = m_Thickness.Evaluate(m_Thickness[spanControlPointForMu], m_Thickness[spanControlPointForMu], m_Thickness[i], m_Thickness[i], spanMu);
				float mass = m_Masses.Evaluate(m_Masses[spanControlPointForMu], m_Masses[spanControlPointForMu], m_Masses[i], m_Masses[i], spanMu);
				float rotationalMass = m_RotationalMasses.Evaluate(m_RotationalMasses[spanControlPointForMu], m_RotationalMasses[spanControlPointForMu], m_RotationalMasses[i], m_RotationalMasses[i], spanMu);
				int filter = m_Filters.Evaluate(m_Filters[spanControlPointForMu], m_Filters[spanControlPointForMu], m_Filters[i], m_Filters[i], spanMu);
				InsertControlPoint(spanControlPointForMu + 1, vector6, vector4 - vector6, vector5 - vector6, normal, mass, rotationalMass, thickness, filter, color, GetName(spanControlPointForMu));
				return spanControlPointForMu + 1;
			}
			return -1;
		}

		public void Clear()
		{
			for (int num = ControlPointCount - 1; num >= 0; num--)
			{
				RemoveControlPoint(num);
			}
			m_TotalSplineLenght = 0f;
			m_ArcLengthTable.Clear();
			m_ArcLengthTable.Add(0f);
		}

		public void RemoveControlPoint(int index)
		{
			foreach (IObiPathDataChannel dataChannel in GetDataChannels())
			{
				dataChannel.RemoveAt(index);
			}
			m_Names.RemoveAt(index);
			if (OnControlPointRemoved != null)
			{
				OnControlPointRemoved.Invoke(index);
			}
			dirty = true;
		}

		public void FlushEvents()
		{
			bool flag = dirty;
			foreach (IObiPathDataChannel dataChannel in GetDataChannels())
			{
				flag |= dataChannel.Dirty;
				dataChannel.Clean();
			}
			if (OnPathChanged != null && flag)
			{
				dirty = false;
				OnPathChanged.Invoke();
			}
		}
	}
}
