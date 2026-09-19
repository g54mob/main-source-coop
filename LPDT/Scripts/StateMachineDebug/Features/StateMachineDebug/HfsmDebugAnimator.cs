using System;
using UnityEngine;

namespace Features.StateMachineDebug
{
	public sealed class HfsmDebugAnimator : MonoBehaviour
	{
		[Tooltip("When off, this enemy is fully ignored by the HFSM debug previewer.")]
		[SerializeField]
		private bool _debugEnabled;

		[Tooltip("If null, uses Animator on this GameObject.")]
		[SerializeField]
		private Animator _animator;

		[Tooltip("Folder for generated .controller assets.")]
		[SerializeField]
		private string _outputFolder = "Assets/HfsmDebug";

		[Tooltip("Empty = {host type name}.controller")]
		[SerializeField]
		private string _controllerFileName = "";

		public bool IsEnabled => _debugEnabled;

		public Animator DebugAnimator
		{
			get
			{
				if (!(_animator != null))
				{
					return GetComponent<Animator>();
				}
				return _animator;
			}
		}

		public string OutputFolder
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(_outputFolder))
				{
					return _outputFolder;
				}
				return "Assets/HfsmDebug";
			}
		}

		public string GetControllerFileName(MonoBehaviour host)
		{
			if (host == null)
			{
				return "Unknown.controller";
			}
			if (!string.IsNullOrWhiteSpace(_controllerFileName))
			{
				if (!_controllerFileName.EndsWith(".controller", StringComparison.OrdinalIgnoreCase))
				{
					return _controllerFileName + ".controller";
				}
				return _controllerFileName;
			}
			return host.GetType().Name + ".controller";
		}

		private void Reset()
		{
			if (_animator == null)
			{
				_animator = GetComponent<Animator>();
			}
		}
	}
}
