using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{

    public Transform SpawnPoint;
    public TetrisBlock Prefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Instantiate(Prefab, SpawnPoint.position, Quaternion.identity);
        
    }

    public void InstantiateTetrisBlock()
    {
        GameObject.Instantiate(Prefab, SpawnPoint.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
