// This script goes on the button that has to be pressed by the player to open a door

using UnityEngine;
using System.Collections;

public class pushButton : MonoBehaviour
{
    public bool buttonActivated = false;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "enemy")
        {
            print("Colliding ?");
            buttonActivated = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        print("Not colliding.");
        if (other.gameObject.tag == "player" || other.gameObject.tag == "enemy")
        {
            buttonActivated = false;
        }
    }
}
