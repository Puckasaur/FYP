using UnityEngine;
using System.Collections;

public class decreaseSmell : MonoBehaviour
{

    public float value;
    GameObject player;
    ringOfSmell script;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        script = player.GetComponentInChildren<ringOfSmell>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "smell")
        {
            script.decreaseSmell(value);
        }
    }

    void OnTriggerExit()
    {
        script.radius = Mathf.Ceil(script.radius);
    }
}