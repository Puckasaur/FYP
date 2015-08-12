using UnityEngine;
using System.Collections;

public class uselessScript : MonoBehaviour 
{
    public fade fading;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            fading.enabled = true;
        }
    }
}
