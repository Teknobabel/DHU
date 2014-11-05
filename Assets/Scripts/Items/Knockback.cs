using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Knockback : Item {

	public int
		m_range = 0;

	public TargetType
		m_targetType = TargetType.Target;

	private bool
		m_abort = false;

	public override IEnumerator Activate ()
	{

		List<GameManager.Direction> directions = new List<GameManager.Direction>();
		directions.Add(GameManager.Direction.North);
		directions.Add(GameManager.Direction.South);
		directions.Add(GameManager.Direction.East);
		directions.Add(GameManager.Direction.West);
		List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_range, directions, false, false, false);
		
		List <Enemy> enemies = new List<Enemy> ();
		foreach (Card c in validCards)
		{
			if (c.enemy != null)
			{
				enemies.Add(c.enemy);
				c.ChangeHighlightState(true);
			}
		}
		
		if (enemies.Count > 0) {

			GameManager.m_gameManager.selectMode = true;
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
			GameManager.m_gameManager.selectMode = false;
			
			if (GameManager.m_gameManager.selectedCard != null)
			{
				if (GameManager.m_gameManager.selectedCard.enemy != null)
				{
					string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
					UIManager.m_uiManager.UpdateActions (newString);

					Card c = GameManager.m_gameManager.selectedCard;
					Card pC = Player.m_player.currentCard;
					GameManager.Direction dir = GameManager.Direction.North;
					// determine direction relative to player
					if (c.column > pC.column && c.row == pC.row)
					{
						dir = GameManager.Direction.East;
					} 
					else if (c.column < pC.column && c.row == pC.row)
					{
						dir = GameManager.Direction.West;
					} 
					else if (c.column == pC.column && c.row > pC.row)
					{
						dir = GameManager.Direction.South;
					} 

					if (GameManager.m_gameManager.selectedCard.enemy.CanBeKnockedBack(dir))
					{
						newString = "\\3" + GameManager.m_gameManager.selectedCard.enemy.m_displayName + "\\0 is knocked back";
						UIManager.m_uiManager.UpdateActions (newString);
						yield return StartCoroutine(GameManager.m_gameManager.selectedCard.enemy.DoKnockback(dir));
					}
				}
				GameManager.m_gameManager.selectedCard = null;

			} else {m_abort = true;}

			foreach (Card vc in validCards)
			{
				vc.ChangeHighlightState(false);	
			}
			
		} else {m_abort = true;}

		if (!m_abort) {
			InputManager.m_inputManager.cardsMoving = true;
			yield return StartCoroutine (PayForCard ());
			yield return new WaitForSeconds (0.5f);
			yield return StartCoroutine (SendToGrave ());
			InputManager.m_inputManager.cardsMoving = false;
		}

		yield return true;
	}
}
