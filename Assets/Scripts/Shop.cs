using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour {

	public enum ShopBonus
	{
		None,
		DamageBonus1, //+1$ and -1 Hand Size
		DamageBonus2, //+2$ and -1 Hand Size
		HealthBonus1, //+1& and -1 Hand Size
		HealthBonus2, //+2& and -1 Hand Size
		HealthBonus3, //+1 Health and -2 Energy *
		EnergyBonus1, //+1# and -1 Hand Size
		EnergyBonus2, //+2# and -1 Hand Size
		EnergyBonus3, //+2 Energy for free *
		ArmorBonus1, //+1% and -1 Hand Size
		HandBonus1, //+1 Hand Size and -2 Energy *
		HandBonus2, //+1 Hand Size and -1 Health *
		HealthBonus4, //+2 Health and -1 Damage *
		HealthBonus5, //+1 Health for free *
		DamageBonus3, //+1 Damage for free *
		DamageBonus4, //+1 Damage for 10 Netherite(accrued) *
		EnergyBonus4, //+2 Energy for 5 Netherite(accrued) *
		HealthBonus6, // +1 Health for 10 Netherite(accrued) *
		HandBonus3, //+1 Hand Size for 12 Netherite(accrued) *
		HandBonus4, //+1 Hand Size for free *
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

	private int
		m_handBonus3NCost = 12,
		m_healthBonus6NCost = 10,
		m_energyBonus4NCost = 5,
		m_damageBonus4NCost = 10;

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
			if (Player.m_player.maxEnergy >=2){ l.Add(ShopBonus.HealthBonus3);}
			if (Player.m_player.maxEnergy >=2){ l.Add(ShopBonus.HandBonus1);}
			if (Player.m_player.maxHealth >=2){ l.Add(ShopBonus.HandBonus2);}
			l.Add(ShopBonus.HandBonus4);
			l.Add(ShopBonus.EnergyBonus3);
			if (GameManager.m_gameManager.accruedXP >= m_handBonus3NCost) {l.Add(ShopBonus.HandBonus3);}
			if (GameManager.m_gameManager.accruedXP >= m_energyBonus4NCost) {l.Add(ShopBonus.EnergyBonus4);}

		} else if (level < 20) {
			l.Add(ShopBonus.DamageBonus1);
			l.Add(ShopBonus.EnergyBonus2);
			l.Add(ShopBonus.HealthBonus1);
			l.Add(ShopBonus.HealthBonus2);
			l.Add(ShopBonus.ArmorBonus1);
			if (Player.m_player.maxEnergy >=2){ l.Add(ShopBonus.HandBonus1);}
			if (Player.m_player.maxEnergy >=2){ l.Add(ShopBonus.HandBonus2);}
			l.Add(ShopBonus.HandBonus4);
			l.Add(ShopBonus.EnergyBonus3);
			if (Player.m_player.maxEnergy >=2){ l.Add(ShopBonus.HealthBonus3);}
			l.Add(ShopBonus.HealthBonus4);
			l.Add(ShopBonus.HealthBonus5);
			l.Add(ShopBonus.DamageBonus3);
			if (GameManager.m_gameManager.accruedXP >= m_handBonus3NCost) {l.Add(ShopBonus.HandBonus3);}
			if (GameManager.m_gameManager.accruedXP >= healthBonus6NCost) {l.Add(ShopBonus.HealthBonus6);}
			if (GameManager.m_gameManager.accruedXP >= m_energyBonus4NCost) {l.Add(ShopBonus.EnergyBonus4);}
			if (GameManager.m_gameManager.accruedXP >= damageBonus4NCost) {l.Add(ShopBonus.DamageBonus4);}
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

			//apply hand size effects
			GameManager.m_gameManager.BPbonus --;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.DamageBonus2:
			desc = "+2$ and -1 Hand Size";
			Player.m_player.permDamage += 2;

			//apply hand size effects
			GameManager.m_gameManager.BPbonus --;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.HealthBonus1:
			desc = "+1& and -1 Hand Size";
			Player.m_player.permHealth += 1;

			//apply hand size effects
			GameManager.m_gameManager.BPbonus --;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.HealthBonus2:
			desc = "+2& and -1 Hand Size";
			Player.m_player.permHealth += 2;

			//apply hand size effects
			GameManager.m_gameManager.BPbonus --;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.EnergyBonus1:
			desc = "+1# and -1 Hand Size";
			Player.m_player.permEnergy += 1;

			//apply hand size effects
			GameManager.m_gameManager.BPbonus --;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.EnergyBonus2:
			desc = "+2# and -1 Hand Size";
			Player.m_player.permEnergy += 2;

			//apply hand size effects
			GameManager.m_gameManager.BPbonus --;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.ArmorBonus1:
			desc = "+1% and -1 Hand Size";
			Player.m_player.permArmor += 1;

			//apply hand size effects
			GameManager.m_gameManager.BPbonus --;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.HandBonus3:
			desc = "+1 Hand Size";
			
			//apply hand size effects
			GameManager.m_gameManager.BPbonus ++;
			UIManager.m_uiManager.UpdateHandSize ();

			GameManager.m_gameManager.accruedXP -= 12;
			break;
		case Shop.ShopBonus.HandBonus4:
			desc = "+1 Hand Size";

			//apply hand size effects
			GameManager.m_gameManager.BPbonus ++;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.HealthBonus6:
			desc = "+1&";
			Player.m_player.permHealth += 1;
			
			GameManager.m_gameManager.accruedXP -= 10;
			break;
		case Shop.ShopBonus.EnergyBonus4:
			desc = "+2#";
			Player.m_player.permEnergy += 2;
			
			GameManager.m_gameManager.accruedXP -= 5;
			break;
		case Shop.ShopBonus.DamageBonus4:
			desc = "+1 Attack Damage";
			Player.m_player.permDamage += 1;
			
			GameManager.m_gameManager.accruedXP -= 10;
			break;
		case Shop.ShopBonus.DamageBonus3:
			desc = "+1 Attack Damage";
			Player.m_player.permDamage += 1;
			break;
		case Shop.ShopBonus.HealthBonus5:
			desc = "+1 Health";
			Player.m_player.permHealth += 1;
			break;
		case Shop.ShopBonus.HealthBonus4:
			desc = "+2 Health and -1 Attack Damage";
			Player.m_player.permDamage -= 1;
			Player.m_player.permHealth += 2;
			break;
		case Shop.ShopBonus.HandBonus2:
			desc = "+1 Hand Size and -1&";
			Player.m_player.permHealth -= 1;
			
			GameManager.m_gameManager.BPbonus ++;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.HandBonus1:
			desc = "+1 Hand Size and -2#";
			Player.m_player.permEnergy -= 2;
			
			GameManager.m_gameManager.BPbonus ++;
			UIManager.m_uiManager.UpdateHandSize ();
			break;
		case Shop.ShopBonus.EnergyBonus3:
			desc = "+2#";
			Player.m_player.permEnergy += 2;
			break;
		case Shop.ShopBonus.HealthBonus3:
			desc = "+1& and -2#";
			Player.m_player.permEnergy -= 2;
			Player.m_player.permHealth += 1;
			break;
		}

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
	public int handBonus3NCost { get {return m_handBonus3NCost;}}
	public int healthBonus6NCost {get {return m_healthBonus6NCost;}}
	public int energyBonus4NCost { get {return m_energyBonus4NCost;}}
	public int damageBonus4NCost { get {return m_damageBonus4NCost;}}
}
