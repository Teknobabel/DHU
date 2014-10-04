using UnityEngine;
using System.Collections;

public class DescendLevel : Item {

	public override IEnumerator Activate ()
	{
		string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());


		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		InputManager.m_inputManager.cardsMoving = false;

		StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.EndLevel));
		yield return true;
	}
}
