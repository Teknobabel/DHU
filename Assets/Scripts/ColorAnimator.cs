using UnityEngine;
using System.Collections;

public class ColorAnimator : MonoBehaviour {
	
	public float 
		colorR = 1,
		colorG = 1,
		colorB = 1;
	
	public string 
		m_animName = "PartyMemberSelect";
	
	public Animation
		m_animation;
	
	private UISprite
		m_sprite;
	
	private Color
		m_startColor = Color.white;
	
	private bool
		m_animPlaying = false;

	// Use this for initialization
	void Awake () {
		m_sprite = (UISprite)this.GetComponent("UISprite");
		m_startColor = m_sprite.color;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (m_animation.IsPlaying(m_animName))
		{
			m_animPlaying = true;
			Color newColor = new Color(colorR, colorG, colorB, 1.0f);
			m_sprite.color = newColor;
		} else if (m_animPlaying && !m_animation.IsPlaying(m_animName))
		{
			m_animPlaying = false;
			m_sprite.color = m_startColor;
		}
	}
}
