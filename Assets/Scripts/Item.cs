using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {
	
	public enum Keyword
	{
		Craftable,
		Consumeable,
		Equippable,
		Pet,
		Chain,
		UseFromInv,
		Skill,
		Limbo,
		WhileInHand,
		LostSoul,
		Key,
	}

	public enum TargetType
	{
		Target,
		All,
		Direction,
	}

	public enum GraveBonus
	{
		Attack,
		Health,
		Energy,
		Armor,
		Actions,
	}
	
	public enum ItemState
	{
		Normal,
		Spent,
	}
	
	public string
		m_name,
//		m_storageName, //temp workaround because current save system doesn't like spaces in names
		m_description,
		m_portraitSpriteName = "Card_Portrait_Item02";
//		m_fullPortraitSpriteName = "Card_ItemPH_Full",
//		m_shortcutText = "none",
//		m_keywordText = "none";
	
	public int
//		m_healthBonus = 0,
//		m_energyBonus = 0,
//		m_attackBonus = 0,
//		m_armorBonus = 0,
//		m_actionBonus = 0,
		m_energyCost = 0,
		m_healthCost = 0;
//		m_chainLevel = 0,
//		m_adjacentDamage = 0,
//		m_XPBonus = 0,
//		m_goldBonus = 0,
//		m_reflectMeleeDamage = 0,
//		m_actionPerHiddenBonus = 0,
//		m_lifeTapBonus = 0,
//		m_soulTapBonus = 0,
//		m_refreshDamage = 0,
//		m_trapDestroyRange = 0,
//		m_moveDistance = 0,
//		m_levelsSkipped = 0,
//		m_stunDuration = 0;
//	
	public bool
		m_canDiscard = true;	
//		m_refreshParty = false,
//		m_rangedAttack = false,
//		m_curePoison = false,
//		m_enableCounterAttack = false,
//		m_enablePoison = false,
//		m_enableArmorPierce = false,
//		m_flipAdjacent = false,
//		m_knockbackAdjacent = false,
//		m_stun = false,
//		m_skipTurn = false,
//		m_doRage = false,
//		m_doSoulArmor = false,
//		m_enableSacrifice = false,
//		m_enableCover = false,
//		m_doSpellbook = false,
//		m_reverseAdjacent = false,
//		m_graveHealth = false,
//		m_graveEnergy = false,
//		m_graveBomb = false,
//		m_graveItemDamage = false,
//		m_rezItems = false,
//		m_clearAllWounds = false,
//		m_clearAllCorruption = false;
//	
//	public int
//		m_attackRange = 0,
//		m_attackDamage = 0;
//
	public Follower.FollowerClass m_class = Follower.FollowerClass.None;
//	public int m_itemLevel = 1;
//	public EffectsPanel.Effect.Duration
//		m_duration = EffectsPanel.Effect.Duration.EndOfTurn;
//	
//	public GameManager.Dice[]
//		m_attackBonusDice,
//		m_armorBonusDice;
//	
	public Keyword[]
		m_keywords;

	public GraveBonus[]
		m_graveBonus;
//	
//	public GameObject
//		m_craftResult;
//	
//	private ItemState
//		m_itemState = ItemState.Normal;

	private UICard
		m_card;
//	
	private int
		m_id = -99,
//		m_attackDiceRoll = 0,
//		m_armorDiceRoll = 0,
		m_adjustedEnergyCost = 0,
		m_adjustedHealthCost = 0;
//
//	private Follower
//		m_attachedFollower = null;


	// Use this for initialization
	void Start () {
		m_id = GameManager.m_gameManager.GetNewID();
	}
	
	public bool HasKeyword (Keyword thisKeyword)
	{
		if (m_keywords.Length > 0)
		{
			foreach (Keyword kw in m_keywords)
			{
				if (kw == thisKeyword)
				{
					return true;	
				}
			}
		}
		
		return false;
	}

	public virtual IEnumerator Activate ()
	{
		yield return true;
	}

	public virtual IEnumerator Deactivate ()
	{
		yield return true;
	}

	public IEnumerator PayForCard ()
	{
		if (m_adjustedEnergyCost > 0)
		{
			Player.m_player.GainEnergy(m_adjustedEnergyCost * -1);
		} else if (m_adjustedHealthCost > 0)
		{
			yield return StartCoroutine(Player.m_player.TakeDirectDamage(m_adjustedHealthCost));
		}

		yield return true;
	}

	public IEnumerator CenterCard ()
	{
		// move card to center
		float t = 0;
		float time = 0.3f;
		Vector3 startPos = card.transform.position;
		Vector3 endPos = UIManager.m_uiManager.m_HUD.transform.position;
		endPos.x -= 0.5f;

		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			card.transform.position = nPos;
			yield return null;
		}

		card.transform.position = endPos;

		yield return new WaitForSeconds (0.5f);
		yield return true;
	}

	public IEnumerator SendToGrave ()
	{
		float t = 0;
		float time = 0.5f;
		Vector3 startPos = card.transform.position;
		Vector3 startScale = card.transform.localScale;
		Vector3 endPos = UIManager.m_uiManager.m_backpackButton.transform.position;
		Vector3 endScale = Vector3.one * 0.25f;
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			card.transform.position = nPos;
			card.transform.localScale = newScale;
			yield return null;
		}
		//card.transform.position = endPos;
		//card.transform.localScale = endScale;
		card.gameObject.SetActive (false);
		for (int i=0; i < GameManager.m_gameManager.inventory.Count; i++) {
			if (GameManager.m_gameManager.inventory[i].m_id == this.id)
			{
				GameManager.m_gameManager.inventory.RemoveAt(i);
				break;
			}
		}
		GameManager.m_gameManager.SendToGrave (this);
		UIManager.m_uiManager.RefreshInventoryMenu ();
		yield return new WaitForSeconds (0.5f);
		yield return true;
	}

	
//	public ItemState itemState {get{return m_itemState;}}
	public int id {get{return m_id;}}
	public int adjustedEnergyCost {get{return m_adjustedEnergyCost;} set {m_adjustedEnergyCost = value;}}
	public int adjustedHealthCost {get{return m_adjustedHealthCost;} set {m_adjustedHealthCost = value;}}
	public UICard card {get{return m_card;}set{m_card = value;}}
//	public Follower attachedFollower {get {return m_attachedFollower;} set {m_attachedFollower = value;}}

}
