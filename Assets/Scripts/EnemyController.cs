using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    //public GameObject ragdoll;
    private void Awake()
    {
        setRigidbodyState(true);
        setColliderState(false);  
    }

    private void Start() 
    {
  
    }

    public void die()
    {
        Destroy(gameObject, 5f);
        GetComponentInChildren<Animator>().enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
    }

    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        //GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

}
