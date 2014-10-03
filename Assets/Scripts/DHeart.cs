using UnityEngine;
using System.Collections;

public class DHeart : MonoBehaviour {
	
	public static DHeart
		m_dHeart;
	
	public Animation
		m_anim;
	
	public UILabel
		m_level;
	
	private bool
		m_isBeating = false;
	
	void Awake () {
		m_dHeart = this;	
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void StartHeartBeat () {

		if (GameManager.m_gameManager.doHeartBeat)
		{
			Debug.Log("STARTING HEART BEAT");
			m_anim.Play("HeartBeat");
			m_isBeating = true;
		}
	}
	
	public void StopHeartBeat () {

		if (GameManager.m_gameManager.doHeartBeat)
		{
			Debug.Log("STOPPING HEART BEAT");
			this.animation.Stop();	
			this.animation["HeartBeat"].time = 0;
		}
	}
	
	public void SetLevel (int level)
	{
//		string newLevel = "DEPTH " + (level+1).ToString();
//		m_level.text = newLevel;
		AssetManager.m_assetManager.m_typogenicText [12].Text = (level + 1).ToString ();
	}
	
	public bool isBeating {get{return m_isBeating;}}
}
