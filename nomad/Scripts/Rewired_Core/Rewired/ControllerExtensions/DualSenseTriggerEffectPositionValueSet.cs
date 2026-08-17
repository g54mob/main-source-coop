using System;
using System.Collections.Generic;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[Serializable]
	[Preserve]
	public struct DualSenseTriggerEffectPositionValueSet
	{
		public const int Count = 10;

		[SerializeField]
		private byte _position0;

		[SerializeField]
		private byte _position1;

		[SerializeField]
		private byte _position2;

		[SerializeField]
		private byte _position3;

		[SerializeField]
		private byte _position4;

		[SerializeField]
		private byte _position5;

		[SerializeField]
		private byte _position6;

		[SerializeField]
		private byte _position7;

		[SerializeField]
		private byte _position8;

		[SerializeField]
		private byte _position9;

		public byte this[int index]
		{
			get
			{
				return index switch
				{
					0 => _position0, 
					1 => _position1, 
					2 => _position2, 
					3 => _position3, 
					4 => _position4, 
					5 => _position5, 
					6 => _position6, 
					7 => _position7, 
					8 => _position8, 
					9 => _position9, 
					_ => throw new ArgumentOutOfRangeException("index"), 
				};
			}
			set
			{
				switch (index)
				{
				case 0:
					_position0 = value;
					break;
				case 1:
					_position1 = value;
					break;
				case 2:
					_position2 = value;
					break;
				case 3:
					_position3 = value;
					break;
				case 4:
					_position4 = value;
					break;
				case 5:
					_position5 = value;
					break;
				case 6:
					_position6 = value;
					break;
				case 7:
					_position7 = value;
					break;
				case 8:
					_position8 = value;
					break;
				case 9:
					_position9 = value;
					break;
				default:
					throw new ArgumentOutOfRangeException("index");
				}
			}
		}

		public DualSenseTriggerEffectPositionValueSet(IList<byte> P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (P_0.Count != 10)
			{
				throw new ArgumentException("collection count must be " + 10);
			}
			_position0 = P_0[0];
			_position1 = P_0[1];
			_position2 = P_0[2];
			_position3 = P_0[3];
			_position4 = P_0[4];
			_position5 = P_0[5];
			_position6 = P_0[6];
			_position7 = P_0[7];
			_position8 = P_0[8];
			_position9 = P_0[9];
		}

		public byte[] ToArray()
		{
			byte[] array = new byte[10];
			CopyTo(array);
			return array;
		}

		public void CopyTo(byte[] destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (destination.Length < 10)
			{
				throw new ArgumentException("destination.Length must be " + 10 + "or greater.");
			}
			destination[0] = _position0;
			destination[1] = _position1;
			destination[2] = _position2;
			destination[3] = _position3;
			destination[4] = _position4;
			destination[5] = _position5;
			destination[6] = _position6;
			destination[7] = _position7;
			destination[8] = _position8;
			destination[9] = _position9;
		}

		internal void prLCBAAOSyauBrnZEgWKljaIbEQpA(byte P_0, byte P_1)
		{
			bool flag = false;
			for (int i = 0; i < 10; i++)
			{
				byte b = this[i];
				if (b < P_0)
				{
					this[i] = P_0;
					flag = true;
				}
				else if (b > P_1)
				{
					flag = true;
					this[i] = P_1;
				}
			}
			if (flag)
			{
				Logger.LogWarning("One or more values in trigger effect position value set was outside the allowed range and was clamped.", requiredThreadSafety: true);
			}
		}
	}
}
