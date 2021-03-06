﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetLosesHealthPerArmor : Item {

	public int
		m_range = 0,
		m_damage = 0;

	private bool
		m_abort = false;

	public override IEnumerator Activate ()
	{

		List<GameManager.Direction> directions = new List<GameManager.Direction>();
		directions.Add(GameManager.Direction.North);
		directions.Add(GameManager.Direction.South);
		directions.Add(GameManager.Direction.East);
		directions.Add(GameManager.Direction.West);
		List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, (m_range + Player.m_player.currentCard.siteRangeBonus + Player.m_player.rangedBonus), directions, false, false, false);
		
		List<Card> eCards = new List<Card> ();
		
		foreach (Card c in validCards) {
			if (c.enemy != null)
			{
				eCards.Add(c);
				c.ChangeHighlightState(true);
			}
		}


		if (eCards.Count > 0)
		{
			GameManager.m_gameManager.selectMode = true;
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(eCards));	
			GameManager.m_gameManager.selectMode = false;
			
			if (GameManager.m_gameManager.selectedCard != null)
			{
				if (GameManager.m_gameManager.selectedCard.enemy != null)
				{
					string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name + " on " + GameManager.m_gameManager.selectedCard.enemy.m_displayName;
					UIManager.m_uiManager.UpdateActions (newString);

					int damage = Player.m_player.currentArmor + Player.m_player.tempArmor + Player.m_player.turnArmor;
					yield return StartCoroutine(GameManager.m_gameManager.selectedCard.enemy.TakeDirectDamage(damage));
				}
				GameManager.m_gameManager.selectedCard = null;
			} else {
				m_abort = true;
			}
			
			foreach (Card vc in eCards)
			{
				vc.ChangeHighlightState(false);	
			}
		} else {
			m_abort = true;
		}

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
