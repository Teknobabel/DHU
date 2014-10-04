using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageInRange : Item {

	public int
		m_damage = 0,
		m_range = 0;

	public TargetType
		m_targetType = TargetType.Target;

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
			if (c.enemy != null && c.cardState != Card.CardState.Hidden)
			{
				enemies.Add(c.enemy);
			}
		}

		if (enemies.Count > 0) {
			foreach (Enemy e in enemies)
			{
				string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name + "\\0 on \\3" + e.m_displayName;
				UIManager.m_uiManager.UpdateActions (newString);

				yield return StartCoroutine(e.TakeDamage(m_damage));
				yield return new WaitForSeconds(0.25f);
			}
		}

		yield return StartCoroutine( PayForCard());
		yield return StartCoroutine (SendToGrave ());

		yield return true;
	}
}
