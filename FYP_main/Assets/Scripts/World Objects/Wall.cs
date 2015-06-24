using UnityEngine;
using System.Collections;

<<<<<<< HEAD
public class Wall : MonoBehaviour
{
	GameObject[] parentChild;

    public Color transparenColor;
    private Color m_InitialColor;
	//public GameObject test;
=======
public class wall : MonoBehaviour
{


    public Color transparenColor;
    private Color m_InitialColor;
>>>>>>> origin/Toni_Sound&Vision

    void Start()
    {
        m_InitialColor = GetComponent<Renderer>().material.color;
<<<<<<< HEAD
        m_InitialColor = GetComponentInChildren<Renderer>().material.color;

		parentChild = Resources.LoadAll<GameObject> ("PARENT");
=======
>>>>>>> origin/Toni_Sound&Vision
    }

    public void SetTransparent()
    {

        GetComponent<Renderer>().material.color = transparenColor;
<<<<<<< HEAD
		GetComponentInChildren<Renderer>().material.color = transparenColor;

		Renderer selectChild = GetComponentInChildren<Renderer> ();

		selectChild.material.color = transparenColor;
=======
>>>>>>> origin/Toni_Sound&Vision
    }

    public void SetToNormal()
    {

        GetComponent<Renderer>().material.color = m_InitialColor;
<<<<<<< HEAD
		GetComponentInChildren<Renderer>().material.color = m_InitialColor;

		if (GetComponentInChildren<Renderer> ()) 
		{
			Debug.Log ("Normal");
		}
=======
>>>>>>> origin/Toni_Sound&Vision
    }
}
