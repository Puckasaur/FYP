using UnityEngine;
using System.Collections;

public class temporaryTP : MonoBehaviour 
{
    public GameObject tp;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            other.transform.position = tp.transform.position;
        }
    }
}
