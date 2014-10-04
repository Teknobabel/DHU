using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbsorbHealthPerEnemyInRange : Item {

	public int
		m_healthBonus = 0,
		m_range = 0;


	public override IEnumerator Activate ()
	{

		List<GameManager.Direction> directions = new List<GameManager.Direction>();
		directions.Add(GameManager.Direction.North);
		directions.Add(GameManager.Direction.South);
		directions.Add(GameManager.Direction.East);
		directions.Add(GameManager.Direction.West);
		List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_range, directions, false, false, false);
		
		int bonusHealth = 0;
		List <Enemy> enemies = new List<Enemy> ();
		foreach (Card c in validCards)
		{
			if (c.enemy != null && c.cardState != Card.CardState.Hidden)
			{
				if (c.enemy.currentHealth < m_healthBonus)
				{
					bonusHealth += c.enemy.currentHealth;
				} else {
					bonusHealth += m_healthBonus;
				}
				enemies.Add(c.enemy);
			}
		}

		if (enemies.Count > 0) {

			string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
			UIManager.m_uiManager.UpdateActions (newString);

			yield return new WaitForSeconds(0.5f);

			foreach (Enemy e in enemies)
			{
				yield return StartCoroutine(e.TakeDirectDamage(m_healthBonus));
				yield return new WaitForSeconds(0.5f);
			}

			Player.m_player.GainHealth(bonusHealth);	
			UIManager.m_uiManager.SpawnFloatingText("+" + bonusHealth.ToString(), UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);

			yield return StartCoroutine( PayForCard());
			yield return new WaitForSeconds(0.5f);
			yield return StartCoroutine (SendToGrave ());

		}


		yield return true;
	}
}
