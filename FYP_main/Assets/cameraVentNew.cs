using UnityEngine;
using System.Collections;

public class CameraVentNew : MonoBehaviour
{
    public Camera mainCam;
    public Camera scriptedCam;

    public Animator animator; // scripted camera's movement animator
    public AnimationClip animation; // the movement of the cam

    public GameObject button;

    private float durationOfAnim; // duration of the cam anim

    private bool isActivated = false; // security variable
    private bool isDone = false; // if scripted camera's movement is over or not

    private TemporaryMovement playerMovement;
    private ObstructionDetector obstructD;

    void Start()
    {
        scriptedCam.enabled = false;
        durationOfAnim = animation.length;
        playerMovement = GameObject.Find("Char_Cat").GetComponent<TemporaryMovement>();
        obstructD = GameObject.Find("Target 1").GetComponent<ObstructionDetector>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDone == false)
        {
            if (other.gameObject.tag == "player")
            {
                if (isDone == false) isActivated = true;

                if (isActivated == true && isDone == false)
                {
                    //scriptedCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
                    obstructD.enabled = false;
                    playerMovement.GetComponent<Animator>().enabled = false;
                    playerMovement.enabled = false;
                    scriptedCam.enabled = true;
                    mainCam.enabled = false;
                    animator.SetBool("cameraMovement", true);
                    StartCoroutine(ButtonPressure());
                    StartCoroutine(DoorOpening());
                    StartCoroutine(EndOfAnimation());
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            button.GetComponent<pushButton>().buttonActivated = true;
        }
    }

    IEnumerator ButtonPressure()
    {
        yield return new WaitForSeconds(durationOfAnim / 3f);
        button.GetComponent<pushButton>().buttonActivated = true;
    }

    IEnumerator DoorOpening()
    {
        yield return new WaitForSeconds(durationOfAnim / 1.2f);
        button.GetComponent<pushButton>().buttonActivated = true;
    }

    IEnumerator EndOfAnimation()
    {
        yield return new WaitForSeconds(durationOfAnim);

        obstructD.enabled = true;
        playerMovement.enabled = true;
        playerMovement.GetComponent<Animator>().enabled = true;
        scriptedCam.enabled = false;
        mainCam.enabled = true;
        button.GetComponent<pushButton>().buttonActivated = false;
        animator.SetBool("cameraMovement", false);
        isDone = true;
    }
}