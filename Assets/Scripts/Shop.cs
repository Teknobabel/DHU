using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour {

	public enum ShopBonus
	{
		None,
		DamageBonus1,
		DamageBonus2,
		HealthBonus1,
		HealthBonus2,
		HealthBonus3,
		EnergyBonus1,
		EnergyBonus2,
		EnergyBonus3,
		ArmorBonus1,
	}

	public static Shop
		m_shop;

	public MeshRenderer[]
		m_shopMesh;

	public string
		m_displayName = "Shop",
		m_abilityText = "Buy and sell cards.",
		m_portraitSprite = "Card_Portrait_Enemy";

	private ShopBonus
		m_bonus = ShopBonus.None;

	void Awake () {
		m_shop = this;
	}

	// Use this for initialization
	void Start () {
		}

	public void Initialize (int level)
	{
		//choose bonus based on level
		List<ShopBonus> l = new List<ShopBonus> ();
		if (level < 10) {
			l.Add(ShopBonus.DamageBonus1);
			l.Add(ShopBonus.EnergyBonus2);
			l.Add(ShopBonus.HealthBonus1);
		} else if (level < 20) {
			l.Add(ShopBonus.DamageBonus1);
			l.Add(ShopBonus.EnergyBonus2);
			l.Add(ShopBonus.HealthBonus2);
			l.Add(ShopBonus.ArmorBonus1);
			}

		if (l.Count > 0) {
			m_bonus = (ShopBonus)l[Random.Range(0, l.Count)];
		}
	}

	public void SetFacing (Card currentCard)
	{
		int minDistance = 999;
		Card nextCard = null;
		for (int i=0; i < currentCard.linkedCards.Length; i++)
		{
			Card linkedCard = currentCard.linkedCards[i];
			if (linkedCard != null)
			{
				if (linkedCard.distanceToPlayer < minDistance )
				{
					if (i ==0)
					{
						ChangeFacing(GameManager.Direction.North);	
					} else if (i == 1)
					{
						ChangeFacing(GameManager.Direction.South);	
					} else if (i == 2)
					{
						ChangeFacing(GameManager.Direction.East);	
					} else if (i == 3)
					{
						ChangeFacing(GameManager.Direction.West);	
					}
					minDistance = linkedCard.distanceToPlayer;
					nextCard = linkedCard;
				}
			}
		}
	}

	public void ApplyEffect ()
	{
		string desc = "null";

		switch (m_bonus)
		{
		case Shop.ShopBonus.DamageBonus1:
			desc = "+1$ and -1 Hand Size";
			Player.m_player.permDamage += 1;
			break;
		case Shop.ShopBonus.DamageBonus2:
			desc = "+2$ and -1 Hand Size";
			Player.m_player.permDamage += 2;
			break;
		case Shop.ShopBonus.HealthBonus1:
			desc = "+1& and -1 Hand Size";
			Player.m_player.permHealth += 1;
			break;
		case Shop.ShopBonus.HealthBonus2:
			desc = "+2& and -1 Hand Size";
			Player.m_player.permHealth += 2;
			break;
		case Shop.ShopBonus.EnergyBonus1:
			desc = "+1# and -1 Hand Size";
			Player.m_player.permEnergy += 1;
			break;
		case Shop.ShopBonus.EnergyBonus2:
			desc = "+2# and -1 Hand Size";
			Player.m_player.permEnergy += 2;
			break;
		case Shop.ShopBonus.ArmorBonus1:
			desc = "+1% and -1 Hand Size";
			Player.m_player.permArmor += 1;
			break;
		}

		//apply hand size effects
		GameManager.m_gameManager.BPbonus --;
		UIManager.m_uiManager.UpdateHandSize ();

		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.Faust;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfRun;
		newEffect.m_description = desc;
		newEffect.m_stackable = true;
		newEffect.m_spriteName = "Effect_Reaper";
		newEffect.m_affectedItem = null;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
	}

	public void ChangeFacing (GameManager.Direction dir)
	{

		//m_facing = dir;
		//Vector3 newDirection = 	m_shopMesh[0].transform.eulerAngles;
		Vector3 newDirection = 	this.transform.eulerAngles;
		switch (dir)
		{
		case GameManager.Direction.North:
			newDirection.y = 180;
			break;
		case GameManager.Direction.South:
			newDirection.y = 0;
			break;
		case GameManager.Direction.East:
			newDirection.y = 270;
			break;
		case GameManager.Direction.West:
			newDirection.y = 90;
			break;
		}

		Debug.Log (dir);
		this.transform.eulerAngles = newDirection;
	}

	public ShopBonus bonus {get{return m_bonus;}}
}
