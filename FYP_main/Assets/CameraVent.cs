using UnityEngine;
using System.Collections;

public class CameraVent : MonoBehaviour 
{
    public Camera mainCam;
    public Camera scriptedCam;

    public Animator animator; // scripted camera's movement animator
    public AnimationClip animation; // the movement of the cam

    private float durationOfAnim; // duration of the cam anim

    private bool isActivated = false; // security variable
    private bool isDone = false; // if scripted camera's movement is over or not

    private TemporaryMovement playerMovement;

	void Start ()
    {
        scriptedCam.enabled = false;
        durationOfAnim = animation.length;
        playerMovement = GameObject.Find("Char_Cat").GetComponent<TemporaryMovement>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDone == false)
        {
            if(other.gameObject.tag == "player")
            {
                if (isDone == false) isActivated = true;

                if (isActivated == true && isDone == false)
                {
                    //scriptedCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);

                    playerMovement.GetComponent<Animator>().enabled = false;
                    playerMovement.enabled = false;
                    scriptedCam.enabled = true;
                    mainCam.enabled = false;
                    animator.SetBool("cameraMovement", true);
                    StartCoroutine(EndOfAnimation());
                }
            }
        }
    }

    IEnumerator EndOfAnimation()
    {
        yield return new WaitForSeconds(durationOfAnim);

        playerMovement.enabled = true;
        playerMovement.GetComponent<Animator>().enabled = true;
        scriptedCam.enabled = false;
        mainCam.enabled = true;
        animator.SetBool("cameraMovement", false);
        isDone = true;
    }
}