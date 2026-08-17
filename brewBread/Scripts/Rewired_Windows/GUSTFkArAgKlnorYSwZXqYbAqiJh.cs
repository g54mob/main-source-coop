using System;
using System.Runtime.CompilerServices;
using System.Threading;

internal abstract class GUSTFkArAgKlnorYSwZXqYbAqiJh : IDisposable
{
	[CompilerGenerated]
	private EventHandler<EventArgs> m_YqtRhKzJcjaxINOoXhITEgbwKeJP;

	[CompilerGenerated]
	private EventHandler<EventArgs> m_jkXIjKMAuMWfwbrBHnYFDRzshibU;

	[CompilerGenerated]
	private bool uLUAPnUhWBOzcfeZyADSRkcaVfbX;

	public bool DYqWQXvHniLnDgbHYdHhMRjcFIrn
	{
		[CompilerGenerated]
		get
		{
			return uLUAPnUhWBOzcfeZyADSRkcaVfbX;
		}
		[CompilerGenerated]
		private set
		{
			uLUAPnUhWBOzcfeZyADSRkcaVfbX = flag;
		}
	}

	public event EventHandler<EventArgs> YqtRhKzJcjaxINOoXhITEgbwKeJP
	{
		[CompilerGenerated]
		add
		{
			EventHandler<EventArgs> eventHandler = this.m_YqtRhKzJcjaxINOoXhITEgbwKeJP;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, b);
				eventHandler = Interlocked.CompareExchange(ref this.m_YqtRhKzJcjaxINOoXhITEgbwKeJP, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<EventArgs> eventHandler = this.m_YqtRhKzJcjaxINOoXhITEgbwKeJP;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value3);
				eventHandler = Interlocked.CompareExchange(ref this.m_YqtRhKzJcjaxINOoXhITEgbwKeJP, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<EventArgs> jkXIjKMAuMWfwbrBHnYFDRzshibU
	{
		[CompilerGenerated]
		add
		{
			EventHandler<EventArgs> eventHandler = this.m_jkXIjKMAuMWfwbrBHnYFDRzshibU;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, b);
				eventHandler = Interlocked.CompareExchange(ref this.m_jkXIjKMAuMWfwbrBHnYFDRzshibU, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<EventArgs> eventHandler = this.m_jkXIjKMAuMWfwbrBHnYFDRzshibU;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value3);
				eventHandler = Interlocked.CompareExchange(ref this.m_jkXIjKMAuMWfwbrBHnYFDRzshibU, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void zNJVymYugIbeeZuNgMrKxyYWbziV()
	{
		try
		{
			lxSxJmnRsfzrqrLsogKtIiLkYKBg(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	public void Dispose()
	{
		lxSxJmnRsfzrqrLsogKtIiLkYKBg(true);
	}

	private void lxSxJmnRsfzrqrLsogKtIiLkYKBg(bool P_0)
	{
		if (!DYqWQXvHniLnDgbHYdHhMRjcFIrn)
		{
			this.YqtRhKzJcjaxINOoXhITEgbwKeJP?.Invoke(this, EventArgs.Empty);
			lDxnsjCDTQrmresvWgbliNUVruIc(P_0);
			GC.SuppressFinalize(this);
			DYqWQXvHniLnDgbHYdHhMRjcFIrn = true;
			this.jkXIjKMAuMWfwbrBHnYFDRzshibU?.Invoke(this, EventArgs.Empty);
		}
	}

	protected abstract void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0);
}
