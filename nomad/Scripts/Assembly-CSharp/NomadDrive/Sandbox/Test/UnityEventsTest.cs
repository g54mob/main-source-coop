using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NomadDrive.Sandbox.Test
{
	public class UnityEventsTest : MonoBehaviour
	{
		[FormerlySerializedAs("OnLeftClick")]
		public UnityEvent onLeftClick;

		[FormerlySerializedAs("OnRightClick")]
		public UnityEvent onRightClick;

		public Event OnEnter;

		private void Start()
		{
			onLeftClick.AddListener(LeftClick);
			onRightClick.AddListener(RightClick);
			Collider component = GetComponent<Collider>();
			OnTriggerEnter(component);
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				onLeftClick.Invoke();
			}
			if (Input.GetMouseButtonDown(1))
			{
				onRightClick.Invoke();
			}
		}

		private void LeftClick()
		{
		}

		public void RightClick()
		{
		}

		private void OnTriggerEnter(Collider other)
		{
		}

		private async UniTask Test()
		{
			await UniTask.WaitForSeconds(2f);
			await UniTask.Yield();
		}
	}
}
