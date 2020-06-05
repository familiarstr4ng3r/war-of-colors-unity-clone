using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPointer : MonoBehaviour
{
    [SerializeField] private GameObject gfx = null;

    public void Activate(Vector3 pos)
    {
        transform.position = pos;
        gfx.SetActive(true);
    }

    public void Deactivate()
    {
        gfx.SetActive(false);
    }
}
