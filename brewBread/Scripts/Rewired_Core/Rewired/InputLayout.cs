using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public sealed class InputLayout
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _name;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private string _descriptiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _id;

		public string name
		{
			get
			{
				return _name;
			}
			internal set
			{
				_name = text;
			}
		}

		public string descriptiveName
		{
			get
			{
				return _descriptiveName;
			}
			internal set
			{
				_descriptiveName = text;
			}
		}

		public int id
		{
			get
			{
				return _id;
			}
			internal set
			{
				_id = num;
			}
		}

		public InputLayout()
		{
		}

		public InputLayout(InputLayout P_0)
		{
			_name = P_0._name;
			_descriptiveName = P_0._descriptiveName;
			_id = P_0._id;
		}

		public InputLayout Clone()
		{
			return new InputLayout(this);
		}
	}
}
