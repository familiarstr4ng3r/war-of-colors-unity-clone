using System;

public class CallbackObserver : IObserverType
{
    public int Sort { get; set; }

    private Action callback = null;

    public CallbackObserver(Action _callback)
    {
        callback = _callback;
    }

    public void Run()
    {
        callback?.Invoke();
    }
}
