using UnityEngine;
using System.Collections;

public class Sniffing : MonoBehaviour
{
    AudioSource audio;
    enemyPathfinding enemyPathfindingScript;

    void Start()
    {
        enemyPathfindingScript = this.transform.parent.GetComponent<enemyPathfinding>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player" && !enemyPathfindingScript.States.Equals(enumStates.chase))
        {
            audio = GetComponent<AudioSource>();
            audio.Play();
            print("OnTriggerEnter");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            if (!audio.isPlaying) audio.Play();
            print("OnTriggerStay");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            if (audio.isPlaying) audio.Stop();
            print("OnTriggerExit");
        }
    }
}