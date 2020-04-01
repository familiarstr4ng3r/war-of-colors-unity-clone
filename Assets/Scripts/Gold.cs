using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Gold
{
    [SerializeField] private float value;
    [SerializeField] private int index;

    public Gold(float _value, int _index)
    {
        value = _value;
        index = _index;
    }

    public static Gold operator +(Gold a, Gold b)
    {
        //bool add = a.index < b.index;

        int diff = Mathf.Abs(a.index - b.index);

        for (int i = 0; i < diff; i++)
        {
            a.index++;
        }

        //while (a.index != b.index)
        //{
        //    a.index++;
        //}

        a.value = (a.value + b.value) % ((diff + 1) * 1000);

        //while(a.value + b.value > 1000)
        //{
        //    a.value -= 1000;
        //    //a.index++;
        //}

        return new Gold(a.value + b.value, a.index);
    }

    public static Gold operator -(Gold a, Gold b)
    {
        return new Gold(0, 0);
    }
}
