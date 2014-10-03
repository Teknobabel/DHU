using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour {
	public float scrollSpeed = 0.5f;

	private bool m_enabled = false;
	void Update() {
//		if (m_enabled) {
			float offset = Time.time * scrollSpeed;
			renderer.material.mainTextureOffset = new Vector2 (offset, 0);
//		}
	}

	public bool enabled {
		get{return m_enabled;}
		set{m_enabled = value;}
	}
}