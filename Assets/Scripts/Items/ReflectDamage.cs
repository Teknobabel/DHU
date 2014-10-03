using UnityEngine;
using System.Collections;

public class ReflectDamage : Item {

	public int
		m_reflectAmount = 0;

	public override IEnumerator Activate ()
	{
		string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		Player.m_player.reflectDamage += m_reflectAmount;
		UIManager.m_uiManager.SpawnFloatingText("Reflect +" + m_reflectAmount.ToString(), UIManager.Icon.None, Player.m_player.m_playerMesh.transform);

		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.ReflectMeleeDamage;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": Reflect " + m_reflectAmount.ToString() + "$ until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_ArmorBonus";
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		InputManager.m_inputManager.cardsMoving = false;
		
		yield return true;
	}
}
