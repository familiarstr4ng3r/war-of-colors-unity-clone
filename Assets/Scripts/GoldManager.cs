using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public string oldString;
    [SerializeField] private Gold currentAmount;
    [SerializeField] private Gold price;

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

    [ContextMenu("Plus")]
    private void Plus()
    {
        currentAmount += price;
    }

    private void Minus()
    {
        currentAmount -= price;
    }
}
