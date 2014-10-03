using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionBuffPerHiddenInRange : Item {

	public int
		m_actionBonus = 0,
		m_range = 0;
	
	public override IEnumerator Activate ()
	{
		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		List<GameManager.Direction> directions = new List<GameManager.Direction>();
		directions.Add(GameManager.Direction.North);
		directions.Add(GameManager.Direction.South);
		directions.Add(GameManager.Direction.East);
		directions.Add(GameManager.Direction.West);
		List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_range, directions, false, false, true);
		
		int bonusActions = 0;
//		List <Enemy> enemies = new List<Enemy> ();
		foreach (Card c in validCards)
		{
			if (c.cardState == Card.CardState.Hidden)
			{
				bonusActions += m_actionBonus;
			}
		}
		
		if (bonusActions > 0) {
			string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name;
			UIManager.m_uiManager.UpdateActions (newString);

			UIManager.m_uiManager.SpawnFloatingText ("+" + bonusActions.ToString (), UIManager.Icon.Actions, Player.m_player.m_playerMesh.transform);
			Player.m_player.GainActionPoints(bonusActions);
		}

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}

}
