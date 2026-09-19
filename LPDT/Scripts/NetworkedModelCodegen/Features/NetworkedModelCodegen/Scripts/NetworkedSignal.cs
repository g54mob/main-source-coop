using System;

namespace Features.NetworkedModelCodegen.Scripts
{
	public sealed class NetworkedSignal : NetworkedSignalBase
	{
		private Action _sender;

		public event Action Received;

		public void BindSender(Action sender, string label)
		{
			_sender = sender;
			RetainLabel(label);
		}

		public void Raise()
		{
			if (_sender == null)
			{
				ReportDetachedRaise();
			}
			else
			{
				_sender();
			}
		}

		public void RaiseReceived()
		{
			this.Received?.Invoke();
		}
	}
	public sealed class NetworkedSignal<T> : NetworkedSignalBase
	{
		private Action<T> _sender;

		public event Action<T> Received;

		public void BindSender(Action<T> sender, string label)
		{
			_sender = sender;
			RetainLabel(label);
		}

		public void Raise(T payload)
		{
			if (_sender == null)
			{
				ReportDetachedRaise();
			}
			else
			{
				_sender(payload);
			}
		}

		public void RaiseReceived(T payload)
		{
			this.Received?.Invoke(payload);
		}
	}
}
