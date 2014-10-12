using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour {

	public bool Shaking; 
	private float ShakeDecay; 
	private float ShakeIntensity;

	private Vector3 
		OriginalPos,
		OriginalLocalPos;
	private Quaternion OriginalRot;
	
	void Start()
	{
		Shaking = false;    
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		if(ShakeIntensity > 0)
		{
			transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
			transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                                    OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                                    OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                                    OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f);
			
			ShakeIntensity -= ShakeDecay;
		}
		else if (Shaking)
		{
			transform.localPosition = OriginalLocalPos;
			transform.rotation = OriginalRot;
//			if (gameObject.name == "Main Camera") {
//				Debug.Log(gameObject.transform.localPosition);
//			}
			Shaking = false;    
		}
	}
	
	public void DoShake(float shakeIntensity, float shakeDecay)
	{
//		if (gameObject.name == "Main Camera") {
//			Debug.Log(gameObject.transform.localPosition);
//		}
		if (!Shaking) {
			OriginalPos = transform.position;
			OriginalLocalPos = transform.localPosition;
			OriginalRot = transform.rotation;
		

		}

		ShakeIntensity = shakeIntensity;
		ShakeDecay = shakeDecay;
		Shaking = true;
	}

}
