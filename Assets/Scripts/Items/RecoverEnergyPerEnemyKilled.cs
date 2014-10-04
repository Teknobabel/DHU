using UnityEngine;
using System.Collections;

public class RecoverEnergyPerEnemyKilled : Item {

	public int
		m_energyBonus = 0;


	public override IEnumerator Activate ()
	{
		string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		Player.m_player.soulTap += m_energyBonus;
		UIManager.m_uiManager.SpawnFloatingText("+Soul Tap", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);


		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.SoulTap;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": Regain " + m_energyBonus.ToString() + "# for each enemy killed until next turn";
		newEffect.m_description = desc;
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
