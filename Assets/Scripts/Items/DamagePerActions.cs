﻿using UnityEngine;
using System.Collections;

public class DamagePerActions : Item {

	public int
		m_bonus = 0;

	public override IEnumerator Activate ()
	{
		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		int damage = Player.m_player.turnDamage + (Player.m_player.currentActionPoints * m_bonus);
//		int damage = Player.m_player.turnDamage;
//		damage += m_attackBonus;
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": " + (Player.m_player.currentActionPoints * m_bonus).ToString() + "$ until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_DamageBonus";
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		
		Player.m_player.turnDamage = damage;
		UIManager.m_uiManager.SpawnFloatingText("+" + (Player.m_player.currentActionPoints * m_bonus).ToString(), UIManager.Icon.MeleeDamage, Player.m_player.m_playerMesh.transform);
		yield return new WaitForSeconds(0.5f);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
