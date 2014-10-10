using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {
	
	public enum TreasureType
	{
		Key,
		Health,
		Energy,
		None,
		Enemy,
	}
	
	public MeshRenderer
		m_chestMesh;
	
	public bool
		m_doStorage = false;
	
	private Card
		m_currentCard;
	
	private TreasureType
		m_treasure = TreasureType.None;
	
	private int
		m_lootValue = 0,
		m_level = 1;
	
	private bool
		m_itemTaken = false,
		m_bossChest = false,
		m_setItem = false;
	
	private Item
		m_savedItem = null;

	private GameObject
			m_enemy = null;

	// Use this for initialization
	public void Initialize (GameObject enemy, int lootValue)
	{
		m_treasure = TreasureType.Enemy;
		m_enemy = enemy;

		m_lootValue = lootValue;
		m_level = lootValue;

		BoxCollider bc = (BoxCollider) transform.GetComponent("BoxCollider");
		bc.enabled = false;
	}

	public void InitializeWithItem (int item)
	{
		m_lootValue = item;
		m_level = 0;
		m_setItem = true;
		BoxCollider bc = (BoxCollider) transform.GetComponent("BoxCollider");
		bc.enabled = false;
	}

	public void Initialize (bool doKey, int lootValue) {
		if (doKey)
		{
			//Debug.Log("SETTING KEY");
			m_treasure = TreasureType.Key;
		} else 
		{
			m_lootValue = lootValue;
			m_level = lootValue;
		}
		
		BoxCollider bc = (BoxCollider) transform.GetComponent("BoxCollider");
		bc.enabled = false;
		
		//temp disabled
		
		//set chest facing
//		Card[] linkedCards = m_currentCard.linkedCards;
//		for (int i=0; i < linkedCards.Length; i++)
//		{
//			Card lc = linkedCards[i];
//			if (lc != null)
//			{
//				Vector3 rot = m_chestMesh.transform.eulerAngles;
//				if (i==1)
//				{
//					rot.y = 180;
//				} else if (i==2)
//				{
//					rot.y = 270;
//				} else if (i==3)
//				{
//					rot.y = 90;
//				}
//				m_chestMesh.transform.eulerAngles = rot;
//				break;	
//			}
//		}
	}
	
	public void InitializeBossChest (int lootValue) 
	{
		m_lootValue = lootValue;
		m_bossChest = true;
	}

	public IEnumerator ActivateChest ()
	{
		if (m_treasure == TreasureType.Enemy) 
		{
			m_itemTaken = true;

			GameObject go = (GameObject)Instantiate(m_enemy, m_currentCard.transform.position, m_enemy.transform.rotation);
			Enemy e = (Enemy)go.GetComponent("Enemy");
			GameManager.m_gameManager.currentMap.m_enemies.Add(e);
			e.Initialize(Enemy.EnemyState.Idle, m_currentCard);
			m_currentCard.enemy = e;

			Instantiate(AssetManager.m_assetManager.m_particleFX[0], this.transform.position, Quaternion.identity);

		} else {

			if (!m_doStorage)
			{
				UIManager.MenuMode currentMode = UIManager.m_uiManager.menuMode;
				UIManager.m_uiManager.menuMode = UIManager.MenuMode.Chest;
			
//				if (m_bossChest)
//				{
//					if (m_lootValue >= GameManager.m_gameManager.m_bossLootTable.Length)
//					{
//						m_lootValue = GameManager.m_gameManager.m_bossLootTable.Length-1;
//					}
//				} else 
//				{
//					if (m_lootValue >= GameManager.m_gameManager.m_lootTable.Length)
//					{
//						m_lootValue = GameManager.m_gameManager.m_lootTable.Length-1;
//					}
//				}
//
//				GameObject[] table = GameManager.m_gameManager.m_lootTable[m_lootValue].m_lootTable;
//				if (m_bossChest)
//				{
//					table = GameManager.m_gameManager.m_bossLootTable[m_lootValue].m_lootTable;
//				}
//				GameObject item = (GameObject)table[Random.Range(0, table.Length)];
//				//GameObject item = (GameObject)GameManager.m_gameManager.m_itemBank[3];
//				if (m_setItem)
//				{
//					item = GameManager.m_gameManager.m_itemBank[m_lootValue];
//				}
//
//				if (m_treasure == TreasureType.Key)
//				{
//					item = GameManager.m_gameManager.m_key;
//					int numKeys = Player.m_player.numKeys;
//					numKeys = Mathf.Clamp(numKeys+1, 0, 3);
//					Player.m_player.numKeys = numKeys;
//				}

//				GameObject randItem = (GameObject)Instantiate(item, Vector3.zero, Quaternion.identity);
//				Item thisItem = (Item)randItem.GetComponent("Item");

				Item thisItem = null;

				if (m_treasure == TreasureType.Key)
				{
					GameObject keyGO = (GameObject)Instantiate(GameManager.m_gameManager.m_key, Vector3.zero, Quaternion.identity);
					thisItem = (Item)keyGO.GetComponent("Item");
					Player.m_player.numKeys ++;
				} else {
					thisItem = (Item)GameManager.m_gameManager.GetItemFromChest();
				}

				
				string newString = "\\8" + thisItem.m_name + "\\0 added to \\1" + GameManager.m_gameManager.currentFollower.m_nameText + " \\0's Hand";
				UIManager.m_uiManager.UpdateActions (newString);

				GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_itemCard, UIManager.m_uiManager.m_itemCard.transform.position, UIManager.m_uiManager.m_itemCard.transform.rotation);	
				UICard cardUI = (UICard)fCard.GetComponent("UICard");
				UIManager.m_uiManager.chestCard = cardUI;
				cardUI.SetCard(thisItem, false);
				Transform cardParent = UIManager.m_uiManager.m_chestUI.transform.Find("InventoryPanel");
				cardUI.transform.parent = cardParent;
				cardUI.transform.localPosition = Vector3.zero;
//				StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
				UIManager.m_uiManager.m_chestUI.SetActive(true);	

				yield return new WaitForSeconds(0.75f);

				GameManager.m_gameManager.inventory.Add(thisItem);

				//animated card moving
				float t = 0;
				float time = 0.3f;
				Vector3 startPos = cardUI.transform.position;
				Vector3 startScale = cardUI.transform.localScale;

				while (t < time)
				{
					t += Time.deltaTime;
					Vector3 newPos = Vector3.Lerp(startPos, AssetManager.m_assetManager.m_props[7].transform.position , t / time);
					Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
					cardUI.transform.position = newPos;
					cardUI.transform.localScale = newScale;
					yield return null;
				}
				Destroy(fCard.gameObject);
				UIManager.m_uiManager.RefreshInventoryMenu();

				UIManager.m_uiManager.menuMode = currentMode;
			} else {
				yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Storage));
			}

//			if (m_bossChest)
//			{
//				if (m_lootValue >= GameManager.m_gameManager.m_bossLootTable.Length)
//				{
//					m_lootValue = GameManager.m_gameManager.m_bossLootTable.Length-1;
//				}
//			} else 
//			{
//				if (m_lootValue >= GameManager.m_gameManager.m_lootTable.Length)
//				{
//					m_lootValue = GameManager.m_gameManager.m_lootTable.Length-1;
//				}
//			}
//				
//			GameObject[] table = GameManager.m_gameManager.m_lootTable[m_lootValue].m_lootTable;
//			if (m_bossChest)
//			{
//				table = GameManager.m_gameManager.m_bossLootTable[m_lootValue].m_lootTable;
//			}
//			
//			GameObject randItem = (GameObject)Instantiate((GameObject)table[Random.Range(0, table.Length)], Vector3.zero, Quaternion.identity);
//			Item thisItem = (Item)randItem.GetComponent("Item");
//			
//			if (m_savedItem != null)
//			{
//				thisItem = m_savedItem;	
//			}
//			GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_itemCard, UIManager.m_uiManager.m_itemCard.transform.position, UIManager.m_uiManager.m_itemCard.transform.rotation);	
//			UICard cardUI = (UICard)fCard.GetComponent("UICard");
//			UIManager.m_uiManager.chestCard = cardUI;
//			cardUI.SetCard(thisItem, false);
//
//			Transform cardParent = UIManager.m_uiManager.m_chestUI.transform.Find("InventoryPanel");
//			cardUI.transform.parent = cardParent;
//			cardUI.transform.localPosition = Vector3.zero;
//
//			if (!m_doStorage)
//			{
//				yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Chest));
//			} else {
//				yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Storage));
//			}
//			
//			if (cardUI.m_itemData == null)
//			{
//				m_itemTaken = true;	
//			} else {
//				m_savedItem = thisItem;	
//			}
//			
//			Destroy(fCard.gameObject);
		}

//		if (m_itemTaken)
//		{
			m_currentCard.chest = null;
			Destroy(this.gameObject);
//		}
		
		yield return null;
	}
	
	public Card currentCard
	{
		get 
		{
			return m_currentCard;	
		}
		set
		{
			m_currentCard = value;	
		}
	}
	public int level {get{return m_level;}set{m_level = value;}}
}
