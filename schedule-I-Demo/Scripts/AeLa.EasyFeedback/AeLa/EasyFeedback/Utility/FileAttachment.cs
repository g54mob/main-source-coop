using System;
using System.IO;

namespace AeLa.EasyFeedback.Utility
{
	[Serializable]
	public class FileAttachment
	{
		private string name;

		private byte[] data;

		private string mimeType;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (value.Length > 256)
				{
					throw new Exception("File name is too long!");
				}
				name = value;
			}
		}

		public byte[] Data
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
			}
		}

		public string MimeType
		{
			get
			{
				return mimeType;
			}
			set
			{
				mimeType = value;
			}
		}

		public FileAttachment(string name, byte[] data, string mimeType = null)
		{
			this.name = name;
			this.data = data;
			this.mimeType = mimeType;
		}

		public FileAttachment(string filePath, string mimeType = null)
		{
			name = Path.GetFileName(filePath);
			data = File.ReadAllBytes(filePath);
			this.mimeType = mimeType;
		}

		public FileAttachment(string name, string filePath, string mimeType = null)
		{
			this.name = name;
			data = File.ReadAllBytes(filePath);
			this.mimeType = mimeType;
		}
	}
}
