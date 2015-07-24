using UnityEngine;
using System.Collections;

public class SwordEasterEgg : MonoBehaviour 
{
    public GameObject sword;
    public GameObject swordParticles;
    public Animator animator;
    public AnimationClip animation;

    private bool isDone;

    void OnTriggerStay(Collider other)
    {
        if (/*isDone == false && */other.gameObject.tag == "player")
        {
            //isDone = true;
            animator.SetBool("isPlaying", true);
            //StartCoroutine(EndOfAnimation());
        }
    }

    void OnTriggerExit()
    {
        animator.SetBool("isPlaying", false);
    }

    IEnumerator EndOfAnimation()
    {
        yield return new WaitForSeconds(animation.length);

       
    }
}