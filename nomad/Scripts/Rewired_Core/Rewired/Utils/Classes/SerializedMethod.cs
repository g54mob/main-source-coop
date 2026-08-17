using System;
using System.Collections.Generic;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired.Utils.Classes
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public abstract class SerializedMethod : ScriptableObject
	{
		private const int QVkviqCdBEATYvgXwCqKjzSBuGJI = 3;

		[NonSerialized]
		private bool voXJsAlqYKECWfpEKETHOIYntqGcA;

		[NonSerialized]
		internal List<TypeWrapper> _data;

		[NonSerialized]
		internal TypeWrapper _result;

		[NonSerialized]
		internal bool _resultIsValid;

		internal abstract TypeWrapper.DataType ResultType { get; }

		internal int DataCount
		{
			get
			{
				if (_data == null)
				{
					return 0;
				}
				return _data.Count;
			}
		}

		internal TypeWrapper Result => _result;

		internal bool ResultIsValid => _resultIsValid;

		internal TypeWrapper GetData(int index)
		{
			if (index < 0 || index >= DataCount)
			{
				throw new IndexOutOfRangeException();
			}
			return _data[index];
		}

		internal void AddData(byte item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(sbyte item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(char item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(int item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(uint item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(long item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(ulong item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(float item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(double item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(bool item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(string item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(object item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(TypeWrapper item)
		{
			if (!voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				NPYfyFLBJYXUfMQOjfiQbVZJXMCaA();
			}
			_data.Add(item);
		}

		internal void ClearData()
		{
			if (voXJsAlqYKECWfpEKETHOIYntqGcA)
			{
				_data.Clear();
			}
		}

		internal void ClearResult()
		{
			_resultIsValid = false;
			_result.Clear();
		}

		internal abstract bool Process();

		private void NPYfyFLBJYXUfMQOjfiQbVZJXMCaA()
		{
			_data = new List<TypeWrapper>(3);
			voXJsAlqYKECWfpEKETHOIYntqGcA = true;
		}
	}
}
