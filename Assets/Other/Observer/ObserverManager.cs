using System.Collections.Generic;

public class ObserverManager
{
    private static Dictionary<string, List<IObserverType>> observers = new Dictionary<string, List<IObserverType>>();

    public static void RunEvent(string eventName)
    {
        if (observers.ContainsKey(eventName))
        {
            var list = observers[eventName];

            for (int i = 0, length = list.Count; i < length; i++)
            {
                list[i].Run();
            }
        }
    }

    public static void AddObserver(IObserverType observer, string eventName, int sort = 10)
    {
        if (observers.ContainsKey(eventName))
        {
            observer.Sort = sort;
            var list = observers[eventName];
            list.Add(observer);

            list.Sort((x, y) => y.Sort.CompareTo(x.Sort));//higher to lower
        }
        else
        {
            observer.Sort = sort;
            var list = new List<IObserverType>
            {
                observer
            };

            observers.Add(eventName, list);
        }
    }
}
