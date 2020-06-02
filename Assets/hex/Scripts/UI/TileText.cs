using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileText : MonoBehaviour
{
    private Text label = null;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    public void UpdateText(string text)
    {
        label.text = text;
    }

    public void SetPosition(Vector3 position)
    {
        position.z = 0;
        transform.position = position;
    }
}
