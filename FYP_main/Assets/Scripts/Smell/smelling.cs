using UnityEngine;
using System.Collections;

public class smelling : MonoBehaviour {
	
	public ParticleSystem enterPoint;
	public ParticleSystem exitPoint;

	public BoxCollider colliderCheck;

	public GameObject newBoneComo;
	public GameObject boneSpawnerComo;
	public GameObject boneComo;
	
	public bool isEnterBone = false;
	public bool isEnterBag = false;
	public bool smellArea = false;

	public float boneLimit = 1;
	public float boneCount;


	//Bone
	IEnumerator boneSmell()
	{
		yield return new WaitForSeconds (1);
		
		enterPoint.GetComponent<ParticleSystem>().enableEmission = true;

		yield return new WaitForSeconds (4);
		
		exitPoint.GetComponent<ParticleSystem>().enableEmission = true;	
		
		newBoneComo = (GameObject)Instantiate (boneComo, boneSpawnerComo.transform
            , Quaternion.identity);
		
		yield return new WaitForSeconds (4);
		
		isEnterBone = false;
	}

	//Bag
	IEnumerator bagSmell()
	{
		yield return new WaitForSeconds (1);
		
		enterPoint.GetComponent<ParticleSystem> ().enableEmission = true;
		enterPoint.GetComponent<ParticleSystem> ().startColor = new Color (1, 0, 1, 0.5f);
		
		yield return new WaitForSeconds (4);

		smellArea = true;
		
		exitPoint.GetComponent<ParticleSystem> ().enableEmission = true;	
		exitPoint.GetComponent<ParticleSystem> ().startColor = new Color (1, 0, 1, 0.5f);

		yield return new WaitForSeconds (4);

		isEnterBag = false;

		yield return new WaitForSeconds (15);

		smellArea = false;
	}

	void Start()
	{
        // If we add this line the game will throw no error but works without so, I don't know. Bisous cordial.
        colliderCheck = GetComponent<BoxCollider>();

		enterPoint.GetComponent<ParticleSystem>().enableEmission = false;
		exitPoint.GetComponent<ParticleSystem>().enableEmission = false;
		//colliderCheck.GetComponent<BoxCollider> ().enabled = false;

		boneSpawnerComo = GameObject.FindGameObjectWithTag("boneSpawnerRoom");
	}

	void OnTriggerEnter(Collider boneTrigger)
	{
		if (boneTrigger.tag == "bone")
		{
			isEnterBone = true;	

			if (isEnterBone == true)
			{
				StopCoroutine(bagSmell ());
				StartCoroutine(boneSmell ());
			}
		}

		if (boneTrigger.tag == "bagOfAir") 
		{
			isEnterBag = true;

			if (isEnterBag == true)
			{	
				StopCoroutine(boneSmell ());
				StartCoroutine(bagSmell ());
			}
		}
	}

	void Update()
	{	
		if (isEnterBone == false && isEnterBag == false)
		{
			enterPoint.GetComponent<ParticleSystem>().enableEmission = false;
			exitPoint.GetComponent<ParticleSystem>().enableEmission = false;
			enterPoint.GetComponent<ParticleSystem>().startColor = new Color (1, 1, 1, 0.5f);
			exitPoint.GetComponent<ParticleSystem>().startColor = new Color (1, 1, 1, 0.5f);
		}

        /*
		if (smellArea == true) 
		{
			colliderCheck.GetComponent<BoxCollider> ().enabled = true;
			//Debug.Log ("Area Trigger");
		}

		else if (smellArea == false) 
		{
			colliderCheck.GetComponent<BoxCollider> ().enabled = false;
			//Debug.Log ("Area Untrigger");
		}
        */
	}
}
