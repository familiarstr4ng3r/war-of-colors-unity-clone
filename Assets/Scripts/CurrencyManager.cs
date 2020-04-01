using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] string oldString;
    //[SerializeField] private Currency currentAmount;
    //[SerializeField] private Currency price;
    [SerializeField] private double someDouble = 0;
    [SerializeField] private string formattedText = string.Empty;
    [SerializeField] private string formattedText1 = string.Empty;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var now = DateTime.Now;
            Debug.Log(now.ToString());
            now = now.AddDays(-1);
            Debug.Log(now.ToString());

            //oldString = now.ToBinary().ToString();
            oldString = now.ToString();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //long temp = Convert.ToInt64(oldString);
            //DateTime old = DateTime.FromBinary(temp);
            DateTime old = DateTime.Parse(oldString);

            var now = DateTime.Now;
            Debug.Log(now.ToString());

            TimeSpan difference = now.Subtract(old);
            Debug.Log(difference.TotalSeconds);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        //if (pause) //save date
    }

    //public void Add() => currentAmount += price;
    //public void Buy() => currentAmount -= price;

    public void Convert()
    {
        //same
        formattedText = Currency.FormatNumber(someDouble);
        formattedText1 = FormatNumber(someDouble);
    }

    private string FormatNumber(double value)
    {
        if (value < 1000d)
        {
            value = Math.Round(value);
            return value.ToString();
        }

        int num = 0;
        while (value >= 1000)
        {
            num++;
            value /= 1000;
        }

        var arr = Currency.GenerateArray();
        if (num < arr.Length)
        {
            string format = arr[num];
            value = Math.Round(value, 2);
            return $"{value} {format}";
        }
        else
        {
            return "out of range";
        }
    }
}
