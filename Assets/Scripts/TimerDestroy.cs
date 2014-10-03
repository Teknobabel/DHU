using UnityEngine;
using System.Collections;

public class TimerDestroy : MonoBehaviour {
	
	public float 
		m_lifeTime = 5;
	
	private float
		m_lifeTimer = 0;

	public Transform[]
		m_objects;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		m_lifeTimer += Time.deltaTime;
		
		if (m_lifeTimer >= m_lifeTime)
		{
			Destroy(this.gameObject);
		}
	}
}
