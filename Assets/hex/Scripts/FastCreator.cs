using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastCreator : MonoBehaviour
{
    [SerializeField] private int playersCount = 3;

    private void Start()
    {
        var manager = FindObjectOfType<MovesManager>();

        for (int i = 0; i < playersCount; i++)
        {
            manager.AddPlayer(new Player(i.ToString(), Random.ColorHSV(), i));
        }
        
        manager.Init();
    }
}
