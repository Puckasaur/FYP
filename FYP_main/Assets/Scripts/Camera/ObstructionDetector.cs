using UnityEngine;
using System.Collections;

public class obstructionDetector : MonoBehaviour {
			
	public Transform playerTransform;
	private wall m_LastWall;
			
	void Start ()
	{
		StartCoroutine (DetectPlayerObstructions());
	}
			
	IEnumerator DetectPlayerObstructions()
	{
		while (true) 
		{
			yield return new WaitForSeconds(0.1f);
					
			Vector3 direction = (playerTransform.position - Camera.main.transform.position).normalized;
			RaycastHit rayCastHit;
					
			if (Physics.Raycast(Camera.main.transform.position, direction, out rayCastHit, Mathf.Infinity))
			{
				wall wall = rayCastHit.collider.gameObject.GetComponentInChildren<wall>();
				if (wall)
				{
					wall.SetTransparent();
					m_LastWall = wall;
				}
				//Working
				else 
				{
					if (m_LastWall)
					{
						m_LastWall.SetToNormal();
						m_LastWall = null;
					}
					//Working
				}
						
			}
		}
	}		
}
