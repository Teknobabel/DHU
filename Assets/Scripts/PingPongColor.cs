using UnityEngine;
using System.Collections;

public class PingPongColor : MonoBehaviour {

	public float duration = 1;

	public Color 
		colorStart = Color.blue,
		colorEnd = Color.red;

	private TypogenicText
		m_text = null;

	void Start ()
	{
		m_text = (TypogenicText) this.gameObject.GetComponent ("TypogenicText");
	}

	void Update() {

		if (this.gameObject.activeSelf) {
			float lerp = Mathf.PingPong(Time.time, duration) / duration;
			Color newcolor = Color.Lerp(colorStart, colorEnd, lerp);
			m_text.ColorTopLeft = newcolor;
			m_text.ColorTopRight = newcolor;
			m_text.ColorBottomLeft = newcolor;
			m_text.ColorBottomRight = newcolor;
		}
	}
}
