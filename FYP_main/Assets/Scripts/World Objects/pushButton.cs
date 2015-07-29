// This script goes on the button that has to be pressed by the player to open a door

using UnityEngine;
using System.Collections;

public class pushButton : MonoBehaviour
{
    public bool buttonActivated = false;
    float timer;
    public float defaultTimer;

    void Start()
    {
        timer = defaultTimer;
    }
    void Update()
    {
        if(buttonActivated == true)
        {
            timer--;
            if(timer <= 0)
            {
                buttonActivated = false;
                timer = defaultTimer;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "enemy")
        {
            buttonActivated = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "enemy")
        {
            timer = defaultTimer;
        }
    }
}
