using UnityEngine;
using System.Collections;

public class EnableCounterAttack : Item {

	public override IEnumerator Activate ()
	{
		string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		Player.m_player.doCounterAttack = true;	
		UIManager.m_uiManager.SpawnAbilityName("+Counter Attack", Player.m_player.transform);

		newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 gains Counter Attack";
		UIManager.m_uiManager.UpdateActions (newString);

		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.CounterAttack;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = "Counter Attack until next turn.";
		newEffect.m_description = desc;
		newEffect.m_stackable = false;
		newEffect.m_spriteName = "Effect_CounterAttack";
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
