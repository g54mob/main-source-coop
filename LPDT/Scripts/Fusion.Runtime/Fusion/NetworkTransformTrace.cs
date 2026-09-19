using UnityEngine;

namespace Fusion
{
	public class NetworkTransformTrace
	{
		public struct BufferedDataReader<T> where T : struct
		{
			private int _readIndex;

			private BufferedData<T> _bufferedData;

			public readonly T Recent => _bufferedData?.Recent ?? default(T);

			public BufferedDataReader(BufferedData<T> bufferedData)
			{
				_bufferedData = bufferedData;
				_readIndex = bufferedData?.SamplesWritten ?? 0;
			}

			public bool Read(out T value)
			{
				if (_bufferedData == null)
				{
					value = default(T);
					return false;
				}
				if (_bufferedData.TryGetSample(_readIndex, out value))
				{
					_readIndex++;
					return true;
				}
				int num = Mathf.Max(0, _bufferedData.SamplesWritten - 30);
				if (_readIndex < num)
				{
					_readIndex = num;
					if (_bufferedData.TryGetSample(_readIndex, out value))
					{
						_readIndex++;
						return true;
					}
				}
				return false;
			}
		}

		public class BufferedData<T> where T : struct
		{
			internal const int BufferSize = 30;

			private T[] DataBuffer = new T[30];

			private int BufferWriteIndex;

			private int TotalSamplesWritten;

			internal int SamplesWritten => TotalSamplesWritten;

			public bool HasData => TotalSamplesWritten > 0;

			public T Recent
			{
				get
				{
					if (!HasData)
					{
						return default(T);
					}
					int num = (BufferWriteIndex - 1 + 30) % 30;
					return DataBuffer[num];
				}
			}

			internal void Write(T data)
			{
				DataBuffer[BufferWriteIndex] = data;
				BufferWriteIndex = (BufferWriteIndex + 1) % 30;
				TotalSamplesWritten++;
			}

			public int GetSamplesInOrder(T[] output)
			{
				int num = Mathf.Min(TotalSamplesWritten, 30);
				if (num == 0)
				{
					return 0;
				}
				int num2 = ((TotalSamplesWritten > 30) ? BufferWriteIndex : 0);
				for (int i = 0; i < num && i < output.Length; i++)
				{
					output[i] = DataBuffer[(num2 + i) % 30];
				}
				return Mathf.Min(num, output.Length);
			}

			public bool TryGetSample(int sampleIndex, out T value)
			{
				value = default(T);
				if (sampleIndex >= TotalSamplesWritten || sampleIndex < 0)
				{
					return false;
				}
				int num = Mathf.Max(0, TotalSamplesWritten - 30);
				if (sampleIndex < num)
				{
					return false;
				}
				int num2 = sampleIndex % 30;
				value = DataBuffer[num2];
				return true;
			}
		}

		public struct StallData
		{
			public float StallScore;

			public float StallProgress;
		}

		public struct StallHeuristicData
		{
			public float ErrorSimilarity;

			public float CorrectionProgress;
		}

		public struct ForecastData
		{
			public Vector3 PreviousVelocity;

			public Vector3 NewVelocity;

			public Vector3 DesiredVelocity;

			public float LerpAlpha;
		}

		public struct CollisionEnterData
		{
			public float ImpactAlignment;

			public float RelativeVelocity;
		}

		public readonly BufferedData<ForecastData> ForecastBuffer = new BufferedData<ForecastData>();

		public readonly BufferedData<StallHeuristicData> StallHeuristicBuffer = new BufferedData<StallHeuristicData>();

		public readonly BufferedData<StallData> StallBuffer = new BufferedData<StallData>();

		public readonly BufferedData<CollisionEnterData> CollisionEnterBuffer = new BufferedData<CollisionEnterData>();
	}
}
