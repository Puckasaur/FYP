using UnityEngine;
using System.Collections;

public class smelling : MonoBehaviour {

	public ParticleSystem enterPoint;
	public ParticleSystem exitPoint;

	public BoxCollider colliderCheck;

	GameObject boneSpawnerComo;
	public GameObject newBoneComo;
	public GameObject boneComo;
	
	public bool isEnter = false;
	public bool smellArea = false;

	public float boneLimit = 1;
	public float boneCount;
	
	IEnumerator particleStartTimer()
	{
		//Debug.Log ("Before Timer Start");

		yield return new WaitForSeconds (1);

		enterPoint.GetComponent<ParticleSystem>().enableEmission = true;

		yield return new WaitForSeconds (4);

		smellArea = true;

		exitPoint.GetComponent<ParticleSystem>().enableEmission = true;	

		newBoneComo = (GameObject)Instantiate (boneComo, boneSpawnerComo.transform.position, Quaternion.identity);
	
		//Debug.Log ("After Timer End"); 

		yield return new WaitForSeconds (4);

		isEnter = false;
		smellArea = false;
	}

	void Start()
	{
		enterPoint.GetComponent<ParticleSystem>().enableEmission = false;
		exitPoint.GetComponent<ParticleSystem>().enableEmission = false;
		colliderCheck.GetComponent<BoxCollider> ().enabled = false;

		boneSpawnerComo = GameObject.FindGameObjectWithTag("boneSpawnerRoom");
	}

	void OnTriggerEnter(Collider boneTrigger)
	{
		if (boneTrigger.tag == "bone") 
		{
			isEnter = true;
		}

		if (isEnter == true || Input.GetKey (KeyCode.E)) 
		{
			StartCoroutine (particleStartTimer ()); 	
		} 
	}

	void Update()
	{	

		
		//Debug.Log ("isEnter :" + isEnter);
		//Debug.Log ("smallArea :" + smellArea);


		if (isEnter == false)
		{
			enterPoint.GetComponent<ParticleSystem>().enableEmission = false;
			exitPoint.GetComponent<ParticleSystem>().enableEmission = false;
		}

		if (smellArea == true) 
		{
			colliderCheck.GetComponent<BoxCollider> ().enabled = true;

		
		}

		else if (smellArea == false) 
		{
			colliderCheck.GetComponent<BoxCollider> ().enabled = false;
			
		}
			

	}

}
