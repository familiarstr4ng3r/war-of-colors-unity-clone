using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public struct Currency
{
    public static readonly string Word = "abcd";
    private const int lettersCount = 2;

    [SerializeField] private float value;
    [SerializeField] private int index;

    public Currency(float _value, int _index)
    {
        value = _value;
        index = _index;
    }

    //public static Currency operator +(Currency a, Currency b)
    //{
    //    return new Currency(0, 0);
    //}

    //public static Currency operator -(Currency a, Currency b)
    //{
    //    return new Currency(0, 0);
    //}

    public static string FormatNumber(double value)
    {
        //https://gram.gs/gramlog/formatting-big-numbers-aa-notation/

        if (value < 1000d)
        {
            value = Math.Round(value);
            return value.ToString();
        }

        int n = (int)Math.Log(value, 1000);
        double m = value / Math.Pow(1000, n);

        string format = string.Empty;
        var arr = GenerateArray();

        if (n < arr.Length)
        {
            format = arr[n];
            //Debug.Log($"index {n}");
        }
        else
        {
            //int unitInt = n - arr.Length;
            //int secondUnit = unitInt % 26;
            //int firstUnit = unitInt / 26;
            //int charA = Convert.ToInt32('a');
            //format = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();

            return "ваш кошелек полон (out of range)";            
        }

        m = Math.Round(m * 100) / 100;//fixes rounding errors
        string formatted = m.ToString("0.##") + format;
        formatted = m.ToString() + format; //result is same i dont found difference

        return formatted;
    }

    //old method array filler by max value 2, ex. "aa"
    public static string[] GenerateArray(string word)
    {
        int count = word.Length;

        int first = count;
        int second = count * count;
        //int _3rd = Mathf.FloorToInt(Mathf.Pow(count, 3));

        string[] array = new string[first + second + 1];

        array[0] = "Empty";

        for (int i = 0; i < count; i++)
        {
            array[i + 1] = word[i].ToString();
        }

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                int currentIndex = i * count + j;
                array[currentIndex + count + 1] = word[i].ToString() + word[j].ToString();
            }
        }

        return array;
    }

    public static string[] GenerateArray()
    {
        int count = Word.Length;
        List<string> list = new List<string>();

        for (int i = 0; i < count; i++)
        {
            list.Add(Word[i].ToString());

            for (int j = 0; j < count; j++)
            {
                list.Add(Word[i].ToString() + Word[j].ToString());

                if (lettersCount != 3) continue;
                //its okay
                for (int k = 0; k < count; k++)
                {
                    list.Add(Word[i].ToString() + Word[j].ToString() + Word[k].ToString());
                }
            }
        }

        //Debug.Log(list.Count);
        list = list.OrderBy(x => x.Length).ThenBy(x => x).ToList();
        list.Insert(0, "Empty");

        return list.ToArray();
    }
}
