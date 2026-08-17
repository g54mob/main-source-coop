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
		private const int SUlQDrwIcmZFfFJVXofSHwGuSqLC = 3;

		[NonSerialized]
		private bool ddfLFhLiJODfDhahYrcCbLnLCiLi;

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
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(sbyte item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(char item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(int item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(uint item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(long item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(ulong item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(float item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(double item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(bool item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(string item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(object item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(new TypeWrapper(item));
		}

		internal void AddData(TypeWrapper item)
		{
			if (!ddfLFhLiJODfDhahYrcCbLnLCiLi)
			{
				cAVeQbCddgXrFutcXSGhiEPCUNkhA();
			}
			_data.Add(item);
		}

		internal void ClearData()
		{
			if (ddfLFhLiJODfDhahYrcCbLnLCiLi)
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

		private void cAVeQbCddgXrFutcXSGhiEPCUNkhA()
		{
			_data = new List<TypeWrapper>(3);
			ddfLFhLiJODfDhahYrcCbLnLCiLi = true;
		}
	}
}
