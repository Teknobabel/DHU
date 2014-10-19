using UnityEngine;
using System.Collections;

public class EnableArmorPiercing : Item {

	public override IEnumerator Activate ()
	{
		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		Player.m_player.doArmorPierce = true;
		UIManager.m_uiManager.SpawnAbilityName("+Armor Pierce", Player.m_player.transform);

		newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 gains Armor Piercing";
		UIManager.m_uiManager.UpdateActions (newString);

		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.ArmorPierce;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": Gain Armor Piercing until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_ArmorPierce";
		newEffect.m_stackable = false;
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
