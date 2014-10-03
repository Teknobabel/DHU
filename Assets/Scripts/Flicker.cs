using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {

	private Light
		m_light;
	
	public float
	m_flickerTime = 0.1f;
	
	private float
		m_intensity = 0,
		m_timer = 0;
		
	// Use this for initialization
	void Awake () {
		m_light = (Light)transform.GetComponent("Light");
		m_intensity = m_light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
	
		m_timer += Time.deltaTime;
		if (m_timer >= m_flickerTime)
		{
			m_timer = 0;
			m_light.intensity = m_intensity * Random.Range(0.75f, 1.25f);
		}
	}
}
