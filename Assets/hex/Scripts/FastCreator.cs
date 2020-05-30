using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastCreator : MonoBehaviour
{
    private void Start()
    {
        var manager = FindObjectOfType<MovesManager>();
        manager.AddPlayer(new Player("1", Color.green));
        manager.AddPlayer(new Player("2", Color.yellow));
        manager.AddPlayer(new Player("3", Color.red));
        manager.Init();
    }
}
