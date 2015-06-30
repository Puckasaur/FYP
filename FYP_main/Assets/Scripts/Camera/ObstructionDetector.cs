using UnityEngine;
using System.Collections;

public class ObstructionDetector : MonoBehaviour {

	public Transform playerTransform;
	private Wall m_LastWall;

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
				Wall wall = rayCastHit.collider.gameObject.GetComponentInChildren<Wall>();
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
