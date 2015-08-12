// This script goes on the button that has to be pressed by the player or an enemy to open a door

using UnityEngine;
using System.Collections;

public class pushButton : MonoBehaviour
{
    public bool buttonActivated = false;
    float timer;
    public float defaultTimer;
    Animator anim;

    void Start()
    {
        timer = defaultTimer;
        anim = GetComponent<Animator>();
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

        else if (buttonActivated == false) GetComponentInParent<Animator>().SetBool("isDown", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "enemy" || other.gameObject.tag == "fatDog")
        {
            buttonActivated = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "enemy" || other.gameObject.tag == "fatDog")
        {
            timer = defaultTimer;
            //anim.SetBool("isDown", true);
            GetComponentInParent<Animator>().SetBool("isDown", true);
        }
    }

    void OnTriggerExit()
    {
        //anim.SetBool("isDown", false);
        //GetComponentInParent<Animator>().SetBool("isDown", false);
    }
}