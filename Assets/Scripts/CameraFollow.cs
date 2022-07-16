using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private void Start() 
    {
        offset = this.transform.position;
    }
    void Update () 
    {
        transform.position = player.transform.position + offset;
    }
}
