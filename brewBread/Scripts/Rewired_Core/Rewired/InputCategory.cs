using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public class InputCategory
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		protected string _name;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		protected string _descriptiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		protected string _tag;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		protected int _id;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		protected bool _userAssignable;

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

		public string tag
		{
			get
			{
				return _tag;
			}
			internal set
			{
				_tag = text;
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

		public bool userAssignable
		{
			get
			{
				return _userAssignable;
			}
			internal set
			{
				_userAssignable = flag;
			}
		}

		public InputCategory()
		{
		}

		public InputCategory(InputCategory P_0)
		{
			_name = P_0._name;
			_descriptiveName = P_0._descriptiveName;
			_tag = P_0._tag;
			_id = P_0._id;
			_userAssignable = P_0._userAssignable;
		}
	}
}
