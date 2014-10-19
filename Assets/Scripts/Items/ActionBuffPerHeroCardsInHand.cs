using UnityEngine;
using System.Collections;

public class ActionBuffPerHeroCardsInHand : Item {

	public int
		m_actionBonus = 0;

	public Follower.FollowerClass
		m_heroClass = Follower.FollowerClass.None;

	public override IEnumerator Activate ()
	{
		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		int bonus = 0;

		foreach (Item i in GameManager.m_gameManager.inventory) {
			if (i.m_class == m_heroClass && i.id != this.id)
			{
				bonus += m_actionBonus;
			}
		}

		if (bonus > 0) {

			string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
			UIManager.m_uiManager.UpdateActions (newString);

			Player.m_player.GainActionPoints(bonus);
			UIManager.m_uiManager.SpawnFloatingText("+" + bonus.ToString(), UIManager.Icon.Actions, Player.m_player.m_playerMesh.transform);
		}

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
