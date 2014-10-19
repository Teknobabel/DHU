using UnityEngine;
using System.Collections;

public class ActionsHitReact : Item {

	public override IEnumerator Activate ()
	{
		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		Player.m_player.berserkerActive = true;
		UIManager.m_uiManager.SpawnFloatingText("+Rage", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);

		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.Rage;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": +1 Action when damaged until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_CounterAttack";
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
