using UnityEngine;
using System.Collections;

public class ArmorBuff : Item {

	public int
		m_armorBonus = 0;
	
	public override IEnumerator Activate ()
	{
		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		Player.m_player.GainTurnArmor (m_armorBonus);
//		int armor = Player.m_player.turnArmor;
//		armor += m_armorBonus;

//		newString = GameManager.m_gameManager.currentFollower.m_nameText + " gains +" + m_armorBonus.ToString() + " Armor";
//		UIManager.m_uiManager.UpdateActions (newString);
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": " + m_armorBonus.ToString() + "% until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_ArmorBonus";
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		
		//Player.m_player.turnArmor = armor;
		UIManager.m_uiManager.SpawnFloatingText("+" + m_armorBonus.ToString(), UIManager.Icon.Armor, Player.m_player.m_playerMesh.transform);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);	
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
