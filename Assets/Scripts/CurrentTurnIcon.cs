using UnityEngine;
using System.Collections;

public class CurrentTurnIcon : MonoBehaviour {

	public enum TurnIconState
	{
		Player,
		Enemy,
		World,
	}

	public static CurrentTurnIcon m_currentTurnIcon;

	public Transform
		m_tipParent;

	public UILabel
		m_currentTurn,
		m_turnNum;

	private UISprite m_sprite = null;

	private TurnIconState m_turnIconState = TurnIconState.Player;

	// Use this for initialization
	void Awake () {
		m_currentTurnIcon = this;
	}

	void Start () {
		m_sprite = (UISprite)transform.GetComponent ("UISprite");
		//m_turnNum.text = "Turns this Level: " + GameManager.m_gameManager.currentTurnNum.ToString ();
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void ChangeTurn (GameManager.Turn newTurn)
	{

//		switch (newTurn)
//		{
//		case GameManager.Turn.Player:
//			m_sprite.spriteName = "Effect_Brand";
////			m_currentTurn.text = "Current Turn: Player";
//			m_turnIconState = TurnIconState.Player;
//			break;
//		case GameManager.Turn.Enemy:
//			m_sprite.spriteName = "Effect_Enemy";
////			m_currentTurn.text = "Current Turn: Enemy";
//			m_turnIconState = TurnIconState.Enemy;
//			break;
//		case GameManager.Turn.Environment:
//			m_sprite.spriteName = "Effect_RazorVine";
////			m_currentTurn.text = "Current Turn: World";
//			m_turnIconState = TurnIconState.World;
//			break;
//		}
	}

	public void DisplayTip (bool displayTip)
	{
		if (displayTip)
		{
			m_tipParent.gameObject.SetActive (true);
		} else {
			m_tipParent.gameObject.SetActive (false);
		}
	}

	public TurnIconState turnIconState {get{return m_turnIconState;}}
	public bool tipDisplayed {get {
			if (m_tipParent.gameObject.activeSelf)
			{
				return true;
			} else {
				return false;
			}
		}}
}
