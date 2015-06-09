using UnityEngine;
using System.Collections;

public class ObstructionDetector : MonoBehaviour {

	public Transform playerTrans;
	private Platform m_LastPlatform;

	void Start () {
		StartCoroutine (DetectPlayerObstructions ());
	}
	
	IEnumerator DetectPlayerObstructions()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);

			Vector3 Direction = (playerTrans.position - Camera.main.transform.position);
			RaycastHit rayCastHit;
			Debug.Log("Platform1");

			if (Physics.Raycast(Camera.main.transform.position, Direction, out rayCastHit, Mathf.Infinity))
			{	
				Debug.Log("Platform2");
				Platform platform = rayCastHit.collider.gameObject.GetComponent<Platform>();
				Debug.Log("Platform3");

				if (platform)
				{	
					Debug.Log("Platform4");
					platform.SetTransparent ();
					m_LastPlatform = platform;
				}
				else 
				{
					if (m_LastPlatform)
					{
						m_LastPlatform.SetToNormal();
						m_LastPlatform = null;
					}
				}
			}
		}
	}

	public void StartRayCast()
	{
		StopCoroutine ("DetectPlayerObstructions");
		StartCoroutine (DetectPlayerObstructions());
	}

	public void StopRayCast()
	{
		StopCoroutine ("DetectPlayerObstructions");
	}
}
