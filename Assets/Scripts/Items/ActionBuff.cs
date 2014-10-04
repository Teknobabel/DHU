using UnityEngine;
using System.Collections;

public class ActionBuff : Item {

	public int
		m_actionBonus = 0;
	
	public override IEnumerator Activate ()
	{
		string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		Player.m_player.GainActionPoints(m_actionBonus);
		UIManager.m_uiManager.SpawnFloatingText("+" + m_actionBonus.ToString(), UIManager.Icon.Actions, Player.m_player.m_playerMesh.transform);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
