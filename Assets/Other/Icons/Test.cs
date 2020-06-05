using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        SelectIconManager.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectIconManager.ShowModal();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelectIconManager.CloseModal();
        }
    }
}
