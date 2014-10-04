using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stun : Item {

	public int
		m_range = 0,
		m_duration = 0;

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
//					UIManager.m_uiManager.SpawnAbilityName("Headbutt", Player.m_player.transform);
//					yield return new WaitForSeconds(0.5f);


					Enemy thisEnemy = GameManager.m_gameManager.selectedCard.enemy;
					thisEnemy.stunDuration += m_duration;

					string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name + "\\0 on \\4" + thisEnemy.m_displayName;
					UIManager.m_uiManager.UpdateActions (newString);

					UIManager.m_uiManager.SpawnAbilityName("Stun +" + m_duration.ToString(), thisEnemy.transform);
					yield return new WaitForSeconds(0.5f);

					newString = "\\4" + thisEnemy.m_displayName + "\\0 is Stunned for " + m_duration.ToString() + " Turns";
					UIManager.m_uiManager.UpdateActions (newString);
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
