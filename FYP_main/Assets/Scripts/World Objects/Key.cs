/*  numberOfKeys (int) has to be added to Temporary Movement
 *  keyPossessed[] (array) has to be added to Temporary movement
 * 
 */

using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{
    public int keyNumber; // the number of the key (will open the door with the same number)

    private int i;  // used to add a key in the good array

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            i = other.GetComponent<TemporaryMovement>().numberOfKeys;
            other.GetComponent<TemporaryMovement>().numberOfKeys += 1;
            other.GetComponent<TemporaryMovement>().keyPossessed[i] = keyNumber;
            Destroy(this.gameObject, 0.1f);
        }


    }
}