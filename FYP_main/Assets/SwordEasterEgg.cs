using UnityEngine;
using System.Collections;

public class SwordEasterEgg : MonoBehaviour 
{
    public GameObject sword;
    public Animator animator;

    private bool isDone;

    void Start()
    {
        animator = sword.GetComponent<Animator>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            animator.SetBool("isPlaying", true);
        }
    }

    void OnTriggerExit()
    {
        animator.SetBool("isPlaying", false);
    }
}