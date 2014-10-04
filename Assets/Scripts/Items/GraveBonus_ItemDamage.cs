using UnityEngine;
using System.Collections;

public class GraveBonus_ItemDamage : Item {

	public int
		m_damageBonus = 0;

	public override IEnumerator Activate ()
	{
		string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		int damagebonus = 0;
		foreach (GameManager.GraveSlot gs in GameManager.m_gameManager.grave)
		{
			if (gs.type == GameManager.GraveSlot.ObjectType.Item && gs.item != null)
			{
				damagebonus += m_damageBonus;
			}
		}

		UIManager.m_uiManager.SpawnFloatingText("+" + damagebonus.ToString(), UIManager.Icon.MeleeDamage, Player.m_player.m_playerMesh.transform);
	
		if (damagebonus > 0)
		{
			Player.m_player.turnDamage += damagebonus;	

			//Update Effect Stack
			EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
			newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
			newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
			string desc = m_name + ": " + damagebonus.ToString() + "$ until next turn.";
			newEffect.m_description = desc;
			newEffect.m_spriteName = "Effect_DamageBonus";
			newEffect.m_affectedItem = this;
			EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		}

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		InputManager.m_inputManager.cardsMoving = false;
		
		yield return true;
	}
}
