using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            Destroy(this.gameObject, 0.1f);
        }
    }
}
