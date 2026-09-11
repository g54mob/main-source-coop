using System;
using System.IO;
using System.Text;

namespace Photon.Voice.Unity.UtilityScripts
{
	public class WaveWriter : IDisposable
	{
		private readonly long _waveStartPosition;

		private int _dataLength;

		private bool _isDisposed;

		private Stream _stream;

		private BinaryWriter _writer;

		private int _sampleRate;

		private int _bitsPerSample;

		private int _channels;

		private readonly bool _closeStream;

		public WaveWriter(string fileName, int sampleRate, int bits, int channels)
			: this(File.OpenWrite(fileName), sampleRate, bits, channels)
		{
			_closeStream = true;
		}

		public WaveWriter(Stream stream, int sampleRate, int bitsPerSample, int channels)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException("Stream not writeable.", "stream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException("Stream not seekable.", "stream");
			}
			_sampleRate = sampleRate;
			_bitsPerSample = bitsPerSample;
			_channels = channels;
			_stream = stream;
			_waveStartPosition = stream.Position;
			_writer = new BinaryWriter(stream);
			for (int i = 0; i < 44; i++)
			{
				_writer.Write((byte)0);
			}
			WriteHeader();
			_closeStream = false;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public void WriteSample(float sample)
		{
			if (sample < -1f || sample > 1f)
			{
				sample = Math.Max(-1f, Math.Min(1f, sample));
			}
			switch (_bitsPerSample)
			{
			case 8:
				Write((byte)(255f * sample));
				break;
			case 16:
				Write((short)(32767f * sample));
				break;
			case 24:
			{
				byte[] bytes = BitConverter.GetBytes((int)(8388607f * sample));
				Write(new byte[3]
				{
					bytes[0],
					bytes[1],
					bytes[2]
				}, 0, 3);
				break;
			}
			case 32:
				Write((int)(2.1474836E+09f * sample));
				break;
			default:
				throw new InvalidOperationException("Invalid Waveformat", new InvalidOperationException("Invalid BitsPerSample while using PCM encoding."));
			}
		}

		public void WriteSamples(float[] samples, int offset, int count)
		{
			for (int i = offset; i < offset + count; i++)
			{
				WriteSample(samples[i]);
			}
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			_stream.Write(buffer, offset, count);
			_dataLength += count;
		}

		public void Write(byte value)
		{
			_writer.Write(value);
			_dataLength++;
		}

		public void Write(short value)
		{
			_writer.Write(value);
			_dataLength += 2;
		}

		public void Write(int value)
		{
			_writer.Write(value);
			_dataLength += 4;
		}

		public void Write(float value)
		{
			_writer.Write(value);
			_dataLength += 4;
		}

		private void WriteHeader()
		{
			_writer.Flush();
			long position = _stream.Position;
			_stream.Position = _waveStartPosition;
			int num = _bitsPerSample / 8 * _channels;
			int value = num * _sampleRate;
			_writer.Write(Encoding.UTF8.GetBytes("RIFF"));
			_writer.Write((int)(_stream.Length - 8));
			_writer.Write(Encoding.UTF8.GetBytes("WAVE"));
			short value2 = 1;
			_writer.Write(Encoding.UTF8.GetBytes("fmt "));
			_writer.Write(16);
			_writer.Write(value2);
			_writer.Write((short)_channels);
			_writer.Write(_sampleRate);
			_writer.Write(value);
			_writer.Write((short)num);
			_writer.Write((short)_bitsPerSample);
			_writer.Write(Encoding.UTF8.GetBytes("data"));
			_writer.Write(_dataLength);
			_writer.Flush();
			_stream.Position = position;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed || !disposing)
			{
				return;
			}
			try
			{
				WriteHeader();
			}
			catch (Exception)
			{
			}
			finally
			{
				if (_closeStream)
				{
					if (_writer != null)
					{
						_writer.Close();
						_writer = null;
					}
					if (_stream != null)
					{
						_stream.Dispose();
						_stream = null;
					}
				}
			}
			_isDisposed = true;
		}

		~WaveWriter()
		{
			Dispose(disposing: false);
		}
	}
}
