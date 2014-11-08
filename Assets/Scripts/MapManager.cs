using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

	[System.Serializable]
	public class EnemyList
	{
		public GameObject[] m_enemyList;
	}
	
	[System.Serializable]
	public class TrapList
	{
		public Card.CardType[] m_availableTraps;
	}
	
	public class Map
	{
		public enum MapType
		{
			Plains,
			Dungeon,
			Cave,
			Molten,
			Crystal,
			Frozen,
			Tutorial,
		}
		
		public int
			m_id = -1,
			m_difficulty = 0;
		
		public MapType
			m_mapType;
		
		public Transform
			m_root;
		
		public Card
			m_entrance = null,
			m_exit = null;
		
		public List<Card>
			m_cards = new List<Card>();
		
		public List<Card>
			m_dungeonEntrances = new List<Card>();
		
		public List<Enemy>
		m_enemies = new List<Enemy>();

		public List<Pet>
			m_pets = new List<Pet>();
		
		public List<Chest>
			m_chests = new List<Chest>();

		public Follower 
			m_follower = null;

		public Trial
			m_trial = null;
	}

	public class Trial
	{
		public int
			m_columns,
			m_rows,
			m_levels,
			m_gpPrize,
			m_xpPrize;

		public int[]
			m_cardMap,
			m_enemyMap,
			m_trapMap,
			m_lootMap;
	}
	
	public static MapManager
		m_mapManager;
	
	public bool doSpawnEnemies = true;
	
	public GameObject
		m_card,
		m_player,
		m_enemy,
		m_chest,
		m_storageChest,
		m_goal,
		m_follower;
	
	public GameObject[]
		m_cardTypes,
		m_enemies,
		m_bosses;
	
	public EnemyList[]
		m_subBiomeEnemies;
	
	public Material[]
		m_plainsVariants,
		m_magmaVariants;
	
	public TrapList[]
		m_trapList;
	
	private int
		m_currentFollower = 0,
		m_chapter1Shop = -1,
		m_chapter2Shop = -1;
//		m_currentTutorial = 0;

	private List<GameObject> m_tempProps = new List<GameObject>();

	private List<string>
		m_eBank = null,
		m_bBank = null;
	
	
	
	
	
	
	
	
	
//
//				New Map Gen Work
//
	
	public struct GridCell
	{
		public bool m_isOccupied;	
		public int
			m_xPos,
			m_yPos;
	}
	
	public enum ChunkType
	{
		Entrance,
		Exit,
		Standard,
		Boss,
		Tutorial,
		Single,
		SingleLarge,
		SingleMed,
	}
	
	
	
	
	
	
	
	
	
	// Use this for initialization
	void Awake () {
		m_mapManager = this;
		m_chapter1Shop = Random.Range (5, 10);
		m_chapter2Shop = Random.Range (15, 20);
	}

	private void DrawMap (List<Card> cards, MapManager.Map thisMap)
	{
		foreach(Card thisCard in cards)
		{
			switch (thisCard.type)
			{
			case Card.CardType.Normal:
				if (thisMap.m_mapType == Map.MapType.Plains)
				{
					float chance = 0;
					if (thisMap.m_difficulty == 6)
					{
						chance = 0.3f;
					} else if (thisMap.m_difficulty == 7)
					{
						chance = 0.4f;
					} else if (thisMap.m_difficulty == 8)
					{
						chance = 0.5f;
					} else if (thisMap.m_difficulty == 9)
					{
						chance = 0.75f;
					}

					if (Random.Range(0.0f, 1.0f) < chance)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[16].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					} else {
						//apply variants
						float randVariant = Random.Range(0.0f, 1.0f);
						if (randVariant > 0.4f)
						{
							Material randMat = m_plainsVariants[Random.Range(1, m_plainsVariants.Length)];
							thisCard.m_cardMesh.material = randMat;
						}
					}
				}
				else if (thisMap.m_mapType == Map.MapType.Dungeon)
				{
					if (thisCard.subType == Card.SubType.Broken)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[27].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					}
					else if (thisCard.subType == Card.SubType.Catacomb)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[25].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					}
					else if (thisCard.subType == Card.SubType.Web)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[15].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					} else {
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[4].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					}
				} else if (thisMap.m_mapType == Map.MapType.Cave)
				{
					if (thisCard.subType == Card.SubType.Bugs)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[21].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					} else if (thisCard.subType == Card.SubType.Hive)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[20].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					} 
//					else if (thisCard.subType == Card.SubType.Mushroom)
//					{
//						
//					}
					else {

						float chance = 1;
						if (thisMap.m_difficulty == 1)
						{
							chance = 0.75f;
						} else if (thisMap.m_difficulty == 2)
						{
							chance = 0.85f;
						} else if (thisMap.m_difficulty == 3)
						{
							chance = 0.9f;
						} else if (thisMap.m_difficulty == 5)
						{
							chance = 0.9f;
						} else if (thisMap.m_difficulty == 6)
						{
							chance = 0.75f;
						}

						if (Random.Range(0.0f, 1.0f) > chance)
						{
							if (thisMap.m_difficulty < 4)
							{
								Material randMat = m_plainsVariants[Random.Range(1, m_plainsVariants.Length)];
								thisCard.m_cardMesh.material = randMat;
							} else {
								thisCard.m_cardMesh.material =	((Card)m_cardTypes[27].transform.GetComponent("Card")).cardMesh.sharedMaterial;
							}
						} else {
							thisCard.m_cardMesh.material =	((Card)m_cardTypes[16].transform.GetComponent("Card")).cardMesh.sharedMaterial;
						}
					}
				} else if (thisMap.m_mapType == Map.MapType.Molten)
				{
					if (thisCard.subType == Card.SubType.Ash)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[29].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					} else if (thisCard.subType == Card.SubType.Ash02)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[32].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					}
					else
					{

						//apply variants
						float randVariant = Random.Range(0.0f, 1.0f);
						if (randVariant > 0.65f)
						{
							Material randMat = m_magmaVariants[Random.Range(0, m_magmaVariants.Length)];
							thisCard.m_cardMesh.material = randMat;
						} else {
							thisCard.m_cardMesh.material =	((Card)m_cardTypes[17].transform.GetComponent("Card")).cardMesh.sharedMaterial;
						}
					}
				} else if (thisMap.m_mapType == Map.MapType.Frozen)
				{
					if (thisCard.subType == Card.SubType.Snow01)
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[34].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					} else
					{
						thisCard.m_cardMesh.material =	((Card)m_cardTypes[18].transform.GetComponent("Card")).cardMesh.sharedMaterial;
					}
				} else if (thisMap.m_mapType == Map.MapType.Crystal)
				{
					thisCard.m_cardMesh.material =	((Card)m_cardTypes[19].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				}
				break;
			case Card.CardType.Entrance:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[1].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				break;
			case Card.CardType.DungeonEntrance:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[3].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				break;
			case Card.CardType.Exit:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[2].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[2].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[2].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[2].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Portal:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[5].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				break;
			case Card.CardType.Trap_Flipper:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[6].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				break;
			case Card.CardType.Gate:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[7].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[7].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[7].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[7].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Quicksand:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[8].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[8].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[8].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[8].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Trap_Razorvine:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[9].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[9].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[9].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[9].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Warren:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[10].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_spawn = ((Card)m_cardTypes[10].transform.GetComponent("Card")).m_spawn;
				thisCard.m_displayName = ((Card)m_cardTypes[10].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[10].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[10].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Fort:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[11].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[11].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[11].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[11].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Stalactite:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[24].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[24].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[24].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[24].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Spore:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[23].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[23].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[23].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[23].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Spikes:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[22].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[22].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[22].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[22].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Unholy:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[26].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_spawn = ((Card)m_cardTypes[26].transform.GetComponent("Card")).m_spawn;
				thisCard.m_displayName = ((Card)m_cardTypes[26].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[26].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[26].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.AshCloud:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[28].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[28].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[28].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[28].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Magma:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[30].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[30].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[30].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[30].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Fire:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[14].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[14].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[14].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[14].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Mine:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[31].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[31].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[31].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[31].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Darkness:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[33].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[33].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[33].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[33].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.ClawingHands:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[35].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[35].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[35].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[35].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Whispers:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[36].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[36].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[36].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[36].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.FrostSnap:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[37].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[37].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[37].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[37].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.RazorGlade:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[38].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[38].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[38].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[38].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.ManaBurn:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[39].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[39].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[39].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[39].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
				case Card.CardType.BrokenGround:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[40].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[40].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[40].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[40].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			case Card.CardType.Tower:
				Card newCard = ((Card)m_cardTypes[12].transform.GetComponent("Card"));
				thisCard.m_cardMesh.material = newCard.cardMesh.sharedMaterial;
				float enemyChance = 0.75f;
				if (Random.Range(0.0f, 1.0f) <= enemyChance && !thisCard.isOccupied && !SettingsManager.m_settingsManager.trial)
				{
					GameObject thisEnemy = ((Card)m_cardTypes[12].transform.GetComponent("Card")).m_spawn;
					Vector3 pos = thisCard.m_actorBase.transform.position;
					Vector3 rot = thisEnemy.transform.eulerAngles;
					rot.z = 180;
					Enemy newEnemy = (Enemy)((GameObject)Instantiate(thisEnemy, pos, Quaternion.Euler(rot))).transform.GetComponent("Enemy");
					newEnemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
					//newEnemy.gameObject.transform.parent = thisCard.m_cardMesh.transform;
					thisCard.enemy = newEnemy;
					thisMap.m_enemies.Add(newEnemy);
				}
				thisCard.m_displayName = newCard.m_displayName;
				thisCard.m_abilityText = newCard.m_abilityText;
				thisCard.m_portraitSpriteName = newCard.m_portraitSpriteName;
				break;
			case Card.CardType.HighGround:
				thisCard.m_cardMesh.material = ((Card)m_cardTypes[13].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				thisCard.m_displayName = ((Card)m_cardTypes[13].transform.GetComponent("Card")).m_displayName;
				thisCard.m_abilityText = ((Card)m_cardTypes[13].transform.GetComponent("Card")).m_abilityText;
				thisCard.m_portraitSpriteName = ((Card)m_cardTypes[13].transform.GetComponent("Card")).m_portraitSpriteName;
				break;
			}
			
		}
	}
	
//	private int[] GetNewChunk (Map.MapType mapType)
//	{
//		List<int[]> chunkArray = new List<int[]>();
//		
//		if (mapType == Map.MapType.Plains)
//		{
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,1,0,1,0,0,2,1,1,1,1,1,1,1,1,1,0,1,0,0,1,0,1,0,1,0,0,1,0,0,0,0,1,0,1,0,0,1,0,1,1,1,1,0,1,0,0,1,0,1,0,0,0,0,1,0,1,1,1,1,1,1,1,1,1,1,0,0,1,0,0,1,0,1,0,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,2,1,1,1,1,0,0,0,1,1,0,0,0,0,1,0,0,1,1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,1,1,0,0,1,1,0,0,0,0,0,1,0,0,0,1,0,1,1,0,0,1,1,1,1,1,1,0,1,1,1,1,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,2,1,1,0,0,1,1,1,1,1,0,1,0,0,0,1,0,1,0,0,0,1,0,0,1,1,0,0,0,0,0,1,0,0,0,1,0,1,1,0,0,1,1,1,1,1,0,1,0,0,1,1,0,0,0,1,0,1,1,1,0,0,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,1,0,2,1,1,1,0,0,0,0,1,1,0,0,0,1,1,1,1,0,1,0,0,0,0,1,0,0,1,1,1,0,0,1,1,1,0,0,1,0,0,0,0,1,0,1,1,1,1,0,0,0,1,1,0,0,0,0,1,1,1,1,0,1,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {1,1,1,0,0,0,0,1,0,0,0,0,1,0,1,0,0,1,0,0,2,1,1,0,1,1,1,1,1,1,0,0,1,1,1,0,0,0,1,0,0,0,1,0,1,0,1,1,1,0,0,1,1,0,1,0,0,0,1,0,0,0,1,0,1,1,0,0,0,0,1,1,1,1,1,0,0,1,1,1,0,0,1,0,1,1,1,1,0,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,2,1,1,0,0,0,0,1,1,1,0,0,1,1,1,1,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,0,1,1,0,1,0,0,1,1,1,0,1,1,0,1,1,1,0,0,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,0,0,1,1,0,0,0,0,1,0,0,0,0,1,0,0,2,1,1,1,1,0,0,1,1,1,0,1,0,0,1,1,0,0,1,0,0,0,0,1,1,1,1,1,1,0,0,1,1,1,1,1,1,0,0,0,0,1,0,0,1,1,0,0,1,0,1,1,1,0,0,1,1,1,1,1,0,0,1,0,0,0,0,1,0,0,0,0,1,1,0,0,1,1,0,0});
//			
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,2,1,1,1,0,0,1,1,1,1,0,1,1,1,1,1,1,1,1,0,0,1,1,1,0,0,1,1,1,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,1,1,1,1,0,0,1,1,1,1,0,1,1,1,0,0,1,1,1,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,0,0,1,1,0,0,0,0,0,1,0,1,1,0,0,0,2,1,0,1,0,0,1,1,1,1,0,1,0,1,0,0,1,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,1,0,0,1,0,0,1,1,0,1,1,1,0,1,0,1,1,1,0,0,1,0,0,1,0,0,0,1,1,1,1,0,0,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,0,1,1,0,0,1,1,0,0,2,1,1,0,0,0,0,1,1,1,0,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,1,1,1,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,1,1,0,0,0,0,1,1,0,2,1,1,1,0,0,1,1,1,1,0,1,0,1,1,1,1,0,1,0,1,1,0,1,1,1,1,0,1,1,0,1,0,0,1,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,1,1,1,0,0,1,1,1,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,2,1,0,0,1,1,0,0,1,1,1,0,0,0,1,1,0,0,0,1,1,1,0,1,1,1,1,0,1,1,1,1,0,1,1,1,1,0,1,1,1,0,0,0,1,1,0,0,0,1,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0,0,0,1,1,1,1,1,1,0,0});
//			
//			chunkArray.Add( new int[] {1,1,1,1,1,0,0,1,1,1,1,1,0,0,1,0,0,0,1,1,2,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,0,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,0,1,1,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,1,0,0,0,1,0,0,1,1,1,1,1,0,0,1,1,1,1,1});
//			chunkArray.Add( new int[] {0,1,1,1,1,1,1,1,1,1,0,0,1,0,0,0,1,0,1,1,2,1,1,1,0,0,0,0,0,1,0,0,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,0,1,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,0,1,1,1,0,0,1,0,1,1,0,0,1,1,1,1,1,1,1,1});
//			//chunkArray.Add( new int[] {});
//
//
//		} else if (mapType == Map.MapType.Dungeon)
//		{
//			
//			chunkArray.Add( new int[] {0,0,1,0,1,0,0,1,0,0,0,1,1,1,1,1,1,1,1,0,1,1,0,0,1,0,1,0,1,1,0,1,0,0,1,0,0,0,1,0,0,1,1,1,1,1,1,1,1,0,1,1,0,0,1,2,0,0,1,1,0,1,1,0,1,0,0,0,1,0,1,1,0,0,1,0,1,0,1,1,0,1,1,1,1,1,1,1,1,0,0,0,1,0,1,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,1,1,1,1,1,1,1,1,0,1,1,0,0,0,1,0,0,1,1,1,1,0,1,1,1,1,0,1,1,0,1,0,0,1,1,1,0,1,0,0,1,0,0,1,0,1,0,1,0,1,1,0,0,0,0,0,0,1,1,1,1,1,0,2,1,0,1,1,1,0,1,1,1,1,1,1,1,1,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,1,1,0,0,0,0,2,1,0,0,1,0,1,0,0,1,0,1,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,1,1,0,0,0,0,1,1,1,0,1,1,0,1,1,1,0,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,0,0,0,0,1,0,1,1,1,1,1,1,0,1,1,0,0,0,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,0,0,0,0,1,1,1,1,0,0,0,1,1,0,1,1,0,1,0,0,1,1,1,1,1,1,0,1,1,0,0,1,1,0,1,1,0,1,1,2,0,0,0,0,1,1,0,1,1,0,0,1,1,0,1,1,0,0,0,0,1,1,1,0,1,1,1,0,1,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,1,1,0,0,1,1,0,0,0,1,1,0,0,0,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0,1,0,1,1,1,1,0,1,0,1,1,0,1,0,0,1,0,1,1,0,0,0,1,1,1,1,0,0,0,0,2,0,0,1,1,0,0,1,0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,0,1,1,0,0,0,1,1,0,0,1,1,0,0});
//			chunkArray.Add( new int[] {1,0,1,0,0,0,0,1,0,1,1,0,1,1,1,1,1,1,0,1,1,1,1,0,1,1,0,1,1,1,0,0,0,1,1,1,1,0,0,0,1,1,0,1,2,0,1,0,1,1,1,0,0,1,0,1,1,0,0,1,1,0,0,1,1,1,1,0,0,1,1,1,1,0,1,1,0,1,1,1,0,0,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,0,1,1,2,0,0,1,1,1,0,1,1,1,0,0,0,0,1,1,1,1,0,1,1,1,1,1,1,0,1,0,0,1,0,1,1,0,1,0,0,0,0,1,1,1,1,1,1,0,0,1,0,1,1,1,1,1,1,0,1,1,1,1,0,0,0,0,1,1,1,0,1,1,1,0,0,1,1,1,0,0,0,1,0,0,0,0,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,1,1,1,1,1,1,0,0,1,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,1,0,1,0,0,0,1,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,1,0,0,0,1,0,1,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,1,0,0,1,2,1,1,1,1,1,1,1,0,0});
//
//		}
//		int[] randChunk = chunkArray[Random.Range(0, chunkArray.Count)];
//		
//		return(randChunk);
//	}
	
//	public List<Card> GetPathToTarget (Card startCard, Card endCard)
//	{
//		List<Card> newPath = new List<Card>();
//		Card[] lc = startCard.linkedCards;
//		
//		foreach (Card thisCard in lc)
//		{
//			if (thisCard != null)
//			{
//				if (!thisCard.isOccupied && thisCard.cardState == Card.CardState.Normal && thisCard != Player.m_player.currentCard)
//				{
//					newPath.Add(thisCard);	
//				}
//			}
//		}
//		return newPath;
//	}
	
	public void SetDistanceToPlayer ()
	{
		//Debug.Log("SETTING DISTANCE TO PLAYER");
		Card playerCard = Player.m_player.currentCard;
		
		List<Card> checkedCards = new List<Card>();
		Queue uncheckedCards = new Queue();
		int distance = 0;
		
		playerCard.distanceToPlayer = distance;
		uncheckedCards.Enqueue(playerCard);
		
		while (uncheckedCards.Count > 0)
		{
			//distance ++;
			
			Card thisCard = (Card)uncheckedCards.Dequeue();
			checkedCards.Add(thisCard);
			
			Card[]lc = thisCard.linkedCards;
			foreach (Card linkedCard in lc)
			{
				if (linkedCard != null)
				{
					bool alreadychecked = false;
					foreach (Card checkedCard in checkedCards)
					{
						if (linkedCard == checkedCard)
						{
							alreadychecked = true;	
						}
					}
					foreach (Card checkedCard in uncheckedCards)
					{
						if (linkedCard == checkedCard)
						{
							alreadychecked = true;	
						}
					}
					
					if (!alreadychecked)
					{
						linkedCard.distanceToPlayer = thisCard.distanceToPlayer + 1;
						uncheckedCards.Enqueue(linkedCard);
					}
				}
			}
		}
	}
	
	public List<Card> GetCardsInRange (Card start, int range,List<GameManager.Direction> directions, bool doBlockLOS, bool doHighlight, bool doHidden)
	{
		List<Card> validCards = new List<Card>();
		
		foreach (GameManager.Direction thisDirection in directions)
		{
			List<Card> uncheckedCards = new List<Card>();
			
			uncheckedCards.Add(start);
			
			while (uncheckedCards.Count > 0)
			{
				
				Card thisCard = uncheckedCards[0];
				
				uncheckedCards.RemoveAt(0);
				Card[] lc = thisCard.linkedCards;
				if (lc[0] != null && thisDirection == GameManager.Direction.North)
				{
					if ((lc[0].column == start.column && (Mathf.Abs(lc[0].row - start.row) <= range)) || (lc[0].row == start.row && (Mathf.Abs(lc[0].column - start.column) <= range)))
					{

						if (lc[0].cardState == Card.CardState.Normal && !lc[0].isOccupied)
						{
							validCards.Add(lc[0]);
							uncheckedCards.Add(lc[0]);
						} else if (lc[0].cardState == Card.CardState.Normal && lc[0].isOccupied && !doBlockLOS)
						{
							validCards.Add(lc[0]);
							uncheckedCards.Add(lc[0]);
						} else if (lc[0].cardState == Card.CardState.Normal && lc[0].isOccupied && doBlockLOS)
						{
							validCards.Add(lc[0]);
						} else if (lc[0].cardState == Card.CardState.Hidden && doHidden)
						{
							validCards.Add(lc[0]);
							uncheckedCards.Add(lc[0]);
						}


//						if (doHidden || (!doHidden && lc[0].cardState == Card.CardState.Normal))
//						{
//							validCards.Add(lc[0]);
//
//							if (!doBlockLOS || (doBlockLOS && !lc[0].isOccupied))
//							{
//								uncheckedCards.Add(lc[0]);
//							}
//						}



//						if ((!doBlockLOS || (doBlockLOS && !lc[0].isOccupied)) && (doHidden || (!doHidden && lc[0].cardState == Card.CardState.Normal)))
//						{
//							validCards.Add(lc[0]);
//							uncheckedCards.Add(lc[0]);
//						}
//						else  if (lc[0].cardState != Card.CardState.Hidden){validCards.Add(lc[0]);}
					}
				}
				if (lc[1] != null && thisDirection == GameManager.Direction.South)
				{

					if ((lc[1].column == start.column && (Mathf.Abs(lc[1].row - start.row) <= range)) || (lc[1].row == start.row && (Mathf.Abs(lc[1].column - start.column) <= range)))
					{

						if (lc[1].cardState == Card.CardState.Normal && !lc[1].isOccupied)
						{
							validCards.Add(lc[1]);
							uncheckedCards.Add(lc[1]);
						} else if (lc[1].cardState == Card.CardState.Normal && lc[1].isOccupied && !doBlockLOS)
						{
							validCards.Add(lc[1]);
							uncheckedCards.Add(lc[1]);
						} else if (lc[1].cardState == Card.CardState.Normal && lc[1].isOccupied && doBlockLOS)
						{
							validCards.Add(lc[1]);
						} else if (lc[1].cardState == Card.CardState.Hidden && doHidden)
						{
							validCards.Add(lc[1]);
							uncheckedCards.Add(lc[1]);
						}
						
						//						if (doHidden || (!doHidden && lc[1].cardState == Card.CardState.Normal))
//						{
//							validCards.Add(lc[1]);
//
//							if (!doBlockLOS || (doBlockLOS && !lc[1].isOccupied))
//							{
//								uncheckedCards.Add(lc[1]);
//							}
//						}
						

					}
				}
				if (lc[2] != null && thisDirection == GameManager.Direction.East)
				{
					if ((lc[2].column == start.column && (Mathf.Abs(lc[2].row - start.row) <= range)) || (lc[2].row == start.row && (Mathf.Abs(lc[2].column - start.column) <= range)))
					{

						if (lc[2].cardState == Card.CardState.Normal && !lc[2].isOccupied)
						{
							validCards.Add(lc[2]);
							uncheckedCards.Add(lc[2]);
						} else if (lc[2].cardState == Card.CardState.Normal && lc[2].isOccupied && !doBlockLOS)
						{
							validCards.Add(lc[2]);
							uncheckedCards.Add(lc[2]);
						} else if (lc[2].cardState == Card.CardState.Normal && lc[2].isOccupied && doBlockLOS)
						{
							validCards.Add(lc[2]);
						} else if (lc[2].cardState == Card.CardState.Hidden && doHidden)
						{
							validCards.Add(lc[2]);
							uncheckedCards.Add(lc[2]);
						}
						
						//						if (doHidden || (!doHidden && lc[2].cardState == Card.CardState.Normal))
//						{
//							validCards.Add(lc[2]);
//
//							if (!doBlockLOS || (doBlockLOS && !lc[2].isOccupied))
//							{
//								uncheckedCards.Add(lc[2]);
//							}
//						}
						

					}
				}
				if (lc[3] != null && thisDirection == GameManager.Direction.West)
				{
					if ((lc[3].column == start.column && (Mathf.Abs(lc[3].row - start.row) <= range)) || (lc[3].row == start.row && (Mathf.Abs(lc[3].column - start.column) <= range)))
					{

						if (lc[3].cardState == Card.CardState.Normal && !lc[3].isOccupied)
						{
							validCards.Add(lc[3]);
							uncheckedCards.Add(lc[3]);
						} else if (lc[3].cardState == Card.CardState.Normal && lc[3].isOccupied && !doBlockLOS)
						{
							validCards.Add(lc[3]);
							uncheckedCards.Add(lc[3]);
						} else if (lc[3].cardState == Card.CardState.Normal && lc[3].isOccupied && doBlockLOS)
						{
							validCards.Add(lc[3]);
						} else if (lc[3].cardState == Card.CardState.Hidden && doHidden)
						{
							validCards.Add(lc[3]);
							uncheckedCards.Add(lc[3]);
						}
						
						//						if (doHidden || (!doHidden && lc[3].cardState == Card.CardState.Normal))
//						{
//							validCards.Add(lc[3]);
//							if (!doBlockLOS || (doBlockLOS && !lc[3].isOccupied))
//							{
//								uncheckedCards.Add(lc[3]);
//							}
//						}
						

					}
				}
			}
		}
		
		if (doHighlight)
		{
			foreach(Card thisCard in validCards)
			{
				thisCard.ChangeHighlightState(true);	
			}
		}
		return validCards;
	}
	
	public List<Card> GetCardsInRange (Map map, Card start, int range)
	{
//		List<GameManager.Direction> directions = new List<GameManager.Direction>();
//		directions.Add(GameManager.Direction.North);
//		directions.Add(GameManager.Direction.South);
//		directions.Add(GameManager.Direction.East);
//		directions.Add(GameManager.Direction.West);
			
		List<Card> validCards = new List<Card>();
		float r = (float)range;
		float x1 = (float)start.column;
		float y1 = (float)start.row;
		
		foreach (Card thisCard in map.m_cards)
		{
			float x2 = (float)thisCard.column;
			float y2 = (float)thisCard.row;

			float d = Mathf.Sqrt(((x2 - x1)*(x2 - x1)) + ((y2 - y1)*(y2 - y1)));
//			if ((Mathf.Abs(thisCard.column)  <= start.column + range) && (Mathf.Abs(thisCard.row)  <= start.row + range))
			if (d <= r)
			{
				validCards.Add(thisCard);
			}
		}
		
//		foreach (GameManager.Direction thisDirection in directions)
//		{
//			List<Card> uncheckedCards = new List<Card>();
//			
//			uncheckedCards.Add(start);
//			
//			while (uncheckedCards.Count > 0)
//			{
//				
//				Card thisCard = uncheckedCards[0];
//				
//				uncheckedCards.RemoveAt(0);
//				Card[] lc = thisCard.linkedCards;
//				if (lc[0] != null && thisDirection == GameManager.Direction.North)
//				{
//					if ((lc[0].column == start.column && (Mathf.Abs(lc[0].row - start.row) <= range)) || (lc[0].row == start.row && (Mathf.Abs(lc[0].column - start.column) <= range)))
//					{
//						validCards.Add(lc[0]);
//						uncheckedCards.Add(lc[0]);
//					}
//				}
//				if (lc[1] != null && thisDirection == GameManager.Direction.South)
//				{
//					if ((lc[1].column == start.column && (Mathf.Abs(lc[1].row - start.row) <= range)) || (lc[1].row == start.row && (Mathf.Abs(lc[1].column - start.column) <= range)))
//					{
//						validCards.Add(thisCard.southCard);
//						uncheckedCards.Add(thisCard.southCard);
//					}
//				}
//				if (lc[2] != null && thisDirection == GameManager.Direction.East)
//				{
//					if ((lc[2].column == start.column && (Mathf.Abs(lc[2].row - start.row) <= range)) || (lc[2].row == start.row && (Mathf.Abs(lc[2].column - start.column) <= range)))
//					{
//						validCards.Add(lc[2]);
//						uncheckedCards.Add(lc[2]);
//					}
//				}
//				if (lc[3] != null && thisDirection == GameManager.Direction.West)
//				{
//					if ((lc[3].column == start.column && (Mathf.Abs(lc[3].row - start.row) <= range)) || (lc[3].row == start.row && (Mathf.Abs(lc[3].column - start.column) <= range)))
//					{
//						validCards.Add(lc[3]);	
//						uncheckedCards.Add(lc[3]);
//					}
//				}
//			}
//		}
		
		return validCards;
	}
	
	public void PlaceBiome (Map thisMap)
	{
		Debug.Log("PLACING BIOME");
		int biomeRange = Random.Range(1, 7);
		List<Card.SubBiomeType> types = new List<Card.SubBiomeType>();
		
		if (thisMap.m_mapType == Map.MapType.Cave)
		{
			types.Add(Card.SubBiomeType.Beehive);
			types.Add(Card.SubBiomeType.Mushrooms);
			types.Add(Card.SubBiomeType.Bugs);
			types.Add(Card.SubBiomeType.Stalactites);
			types.Add(Card.SubBiomeType.Shadows);
			
		} else if (thisMap.m_mapType == Map.MapType.Dungeon)
		{
			types.Add(Card.SubBiomeType.Spiderwebs);
			types.Add(Card.SubBiomeType.Spikes);
			types.Add(Card.SubBiomeType.Catacomb);
			//types.Add(Card.SubBiomeType.Warren);
		} else if (thisMap.m_mapType == Map.MapType.Molten)
		{
			types.Add(Card.SubBiomeType.AshCloud);
			types.Add(Card.SubBiomeType.MagmaFlow);
			types.Add(Card.SubBiomeType.Inferno);
			types.Add(Card.SubBiomeType.Minefield);
		} else if (thisMap.m_mapType == Map.MapType.Frozen)
		{
			types.Add(Card.SubBiomeType.Shadows);
			types.Add(Card.SubBiomeType.Hands);
			types.Add(Card.SubBiomeType.Whispers);
			types.Add(Card.SubBiomeType.Frost);
		} else if (thisMap.m_mapType == Map.MapType.Crystal)
		{
			types.Add(Card.SubBiomeType.RazorGlade);
			types.Add(Card.SubBiomeType.ManaBurn);
			types.Add(Card.SubBiomeType.CrystalField);
			types.Add(Card.SubBiomeType.Cliffs);
		}
		
		if (types.Count > 0)
		{
			Card.SubBiomeType thisBiomeType = types[Random.Range(0, types.Count)];
			
			Card biomeOrigin = thisMap.m_cards[Random.Range(0, thisMap.m_cards.Count)];
			List<Card> cardsInRange = GetCardsInRange(thisMap, biomeOrigin, biomeRange);
			cardsInRange.Add(biomeOrigin);
		
			foreach (Card thisCard in cardsInRange)
			{
				if (thisBiomeType == Card.SubBiomeType.Cliffs)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.BrokenGround;
						} else if (rand > 0.5f)
						{
							//thisCard.subType = Card.SubType.Snow01;	
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.CrystalField)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f && !thisCard.isOccupied)
						{
							GameObject newEnemy = GetBiomeEnemy(11);
							Vector3 cardPos = thisCard.m_actorBase.transform.position;
							Vector3 cardRot = thisCard.transform.eulerAngles;
							cardRot.z = 180;
							Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
							thisCard.enemy = enemy;
							enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
							//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
							thisMap.m_enemies.Add(enemy);
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.ManaBurn)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.ManaBurn;
						} else if (rand > 0.5f)
						{
							//thisCard.subType = Card.SubType.Snow01;	
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.RazorGlade)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.RazorGlade;
						} else if (rand > 0.5f)
						{
							thisCard.subType = Card.SubType.Snow01;	
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Frost)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.FrostSnap;
						} else if (rand > 0.5f)
						{
							thisCard.subType = Card.SubType.Snow01;	
						}
					}
				}
				if (thisBiomeType == Card.SubBiomeType.Whispers)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.Whispers;
						} else if (rand > 0.5f)
						{
							thisCard.subType = Card.SubType.Snow01;	
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Hands)
				{
					
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.ClawingHands;
						} else if (rand > 0.5f)
						{
							thisCard.subType = Card.SubType.Snow01;	
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Shadows)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.Darkness;
						} else if (rand > 0.5f)
						{
							thisCard.subType = Card.SubType.Snow01;	
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Minefield)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.Mine;	
						} else if (rand > 0.5f)
						{
							thisCard.subType = Card.SubType.Ash02;	
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Inferno)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin && !thisCard.isOccupied)
						{
							thisCard.type = Card.CardType.Fire;
							
							GameObject newEnemy = GetBiomeEnemy(10);
							Vector3 cardPos = thisCard.m_actorBase.transform.position;
							Vector3 cardRot = thisCard.transform.eulerAngles;
							cardRot.z = 180;
							Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
							thisCard.enemy = enemy;
							enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
							//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
							thisMap.m_enemies.Add(enemy);
							
						} else if (rand >= 0.8f)
						{
							thisCard.type = Card.CardType.Fire;	
						} else {
							if (rand >= 0.6f && !thisCard.isOccupied)
							{
								GameObject newEnemy = GetBiomeEnemy(9);
								Vector3 cardPos = thisCard.m_actorBase.transform.position;
								Vector3 cardRot = thisCard.transform.eulerAngles;
								cardRot.z = 180;
								Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
								thisCard.enemy = enemy;
								enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
								//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
								thisMap.m_enemies.Add(enemy);
							}
							thisCard.subType = Card.SubType.Ash;
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.MagmaFlow)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin)
						{
							thisCard.type = Card.CardType.Magma;
						} else if (rand >= 0.7f)
						{
							thisCard.type = Card.CardType.Magma;	
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.AshCloud)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin)
						{
							thisCard.type = Card.CardType.AshCloud;
						} else if (rand >= 0.6f)
						{
							thisCard.type = Card.CardType.AshCloud;	
						} else {
							thisCard.subType = Card.SubType.Ash;
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Warren)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin && !thisCard.isOccupied)
						{
							thisCard.subType = Card.SubType.Broken;
							
							GameObject newEnemy = GetBiomeEnemy(7);
							Vector3 cardPos = thisCard.m_actorBase.transform.position;
							Vector3 cardRot = thisCard.transform.eulerAngles;
							cardRot.z = 180;
							Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
							thisCard.enemy = enemy;
							enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
							//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
							thisMap.m_enemies.Add(enemy);
							
						} else if (rand >= 0.6f)
						{
							thisCard.subType = Card.SubType.None;	
						} else {
							
							rand = Random.Range(0.0f, 1.0f);
							if (rand >= 0.2f)
							{
								thisCard.subType = Card.SubType.Broken;
								if (rand >= 0.8f && !thisCard.isOccupied)
								{
									GameObject newEnemy = GetBiomeEnemy(8);
									Vector3 cardPos = thisCard.m_actorBase.transform.position;
									Vector3 cardRot = thisCard.transform.eulerAngles;
									cardRot.z = 180;
									Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
									thisCard.enemy = enemy;
									enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
									//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
									thisMap.m_enemies.Add(enemy);
								}
							} else 
							{
								thisCard.type = Card.CardType.Warren;
							}
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Catacomb)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin && !thisCard.isOccupied)
						{
							thisCard.subType = Card.SubType.Catacomb;
							
							GameObject newEnemy = GetBiomeEnemy(6);
							Vector3 cardPos = thisCard.m_actorBase.transform.position;
							Vector3 cardRot = thisCard.transform.eulerAngles;
							cardRot.z = 180;
							Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
							thisCard.enemy = enemy;
							enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
							//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
							thisMap.m_enemies.Add(enemy);
							
						} else if (rand >= 0.6f)
						{
							thisCard.subType = Card.SubType.None;	
						} else {
							
							rand = Random.Range(0.0f, 1.0f);
							if (rand >= 0.3f)
							{
								thisCard.subType = Card.SubType.Catacomb;
								if (rand >= 0.7f && !thisCard.isOccupied)
								{
									GameObject newEnemy = GetBiomeEnemy(5);
									Vector3 cardPos = thisCard.m_actorBase.transform.position;
									Vector3 cardRot = thisCard.transform.eulerAngles;
									cardRot.z = 180;
									Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
									thisCard.enemy = enemy;
									enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
									//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
									thisMap.m_enemies.Add(enemy);
								}
							} else 
							{
								thisCard.type = Card.CardType.Unholy;
							}
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Spikes)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin)
						{
							thisCard.type = Card.CardType.Spikes;
						} else if (rand >= 0.6f)
						{
							thisCard.subType = Card.SubType.None;	
						} else {
							thisCard.type = Card.CardType.Spikes;
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Beehive)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin && !thisCard.isOccupied)
						{
							thisCard.subType = Card.SubType.Hive;
							if (doSpawnEnemies && rand >= 0.65f && !thisCard.isOccupied)
							{
								GameObject newEnemy = GetBiomeEnemy(4);
								Vector3 cardPos = thisCard.m_actorBase.transform.position;
								Vector3 cardRot = thisCard.transform.eulerAngles;
								cardRot.z = 180;
								Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
								thisCard.enemy = enemy;
								enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
								//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
								thisMap.m_enemies.Add(enemy);
							}
							
						} else if (rand >= 0.5f)
						{
							thisCard.subType = Card.SubType.None;	
						} else {
							thisCard.subType = Card.SubType.Hive;
							//chance to place an extra enemy
							rand = Random.Range(0.0f, 1.0f);
							if (!thisCard.isOccupied && rand > 0.5f)
							{
								if (doSpawnEnemies)
								{
									GameObject newEnemy = GetBiomeEnemy(3);
									Vector3 cardPos = thisCard.m_actorBase.transform.position;
									Vector3 cardRot = thisCard.transform.eulerAngles;
									cardRot.z = 180;
									Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
									thisCard.enemy = enemy;
									enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
									//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
									thisMap.m_enemies.Add(enemy);
								}
							}
						}
					}
				} else if (thisBiomeType == Card.SubBiomeType.Stalactites)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin)
						{
							thisCard.type = Card.CardType.Stalactite;
						} else if (rand >= 0.5f)
						{
							thisCard.subType = Card.SubType.None;	
						} else {
							thisCard.type = Card.CardType.Stalactite;
						}
					}
				} else if (thisBiomeType == Card.SubBiomeType.Mushrooms)
				{
				
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin)
						{
							//thisCard.subType = Card.SubType.Mushroom;
							thisCard.type = Card.CardType.Spore;
						} else if (rand >= 0.5f)
						{
							thisCard.subType = Card.SubType.None;	
						} else {
							thisCard.type = Card.CardType.Spore;
							//thisCard.subType = Card.SubType.Mushroom;
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Bugs)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin)
						{
							thisCard.subType = Card.SubType.Bugs;
							
							if (doSpawnEnemies)
							{
								if (rand >= 0.5f && !thisCard.isOccupied)
								{
									GameObject newEnemy = GetBiomeEnemy(2);
									Vector3 cardPos = thisCard.m_actorBase.transform.position;
									Vector3 cardRot = thisCard.transform.eulerAngles;
									cardRot.z = 180;
									Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
									thisCard.enemy = enemy;
									enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
									//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
									thisMap.m_enemies.Add(enemy);
								}
							}
						}
						else if (rand >= 0.6f)
						{
							thisCard.subType = Card.SubType.None;	
						} else {
							thisCard.subType = Card.SubType.Bugs;
							
							//chance to place an extra enemy
							rand = Random.Range(0.0f, 1.0f);
							if (!thisCard.isOccupied && rand > 0.5f)
							{
								if (doSpawnEnemies)
								{
									GameObject newEnemy = GetBiomeEnemy(2);
									Vector3 cardPos = thisCard.m_actorBase.transform.position;
									Vector3 cardRot = thisCard.transform.eulerAngles;
									cardRot.z = 180;
									Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
									thisCard.enemy = enemy;
									enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
									//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
									thisMap.m_enemies.Add(enemy);
								}
							}
						}
					}
				}
				else if (thisBiomeType == Card.SubBiomeType.Spiderwebs)
				{
					if (thisCard.type == Card.CardType.Normal && !thisCard.doFlipTrap)
					{
						float rand = Random.Range(0.0f, 1.0f);
						if (thisCard == biomeOrigin)
						{
							thisCard.type = Card.CardType.Quicksand;
							
							if (doSpawnEnemies)
							{
								if (rand >= 0.65f && !thisCard.isOccupied)
								{
									GameObject newEnemy = GetBiomeEnemy(1);
									Vector3 cardPos = thisCard.m_actorBase.transform.position;
									Vector3 cardRot = thisCard.transform.eulerAngles;
									cardRot.z = 180;
									Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
									thisCard.enemy = enemy;
									enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
									//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
									thisMap.m_enemies.Add(enemy);
								}
							}
						}
						else if (rand >= 0.6f)
						{
							thisCard.type = Card.CardType.Quicksand;	
						} else {
							thisCard.subType = Card.SubType.Web;
							
							//chance to place an extra enemy
							rand = Random.Range(0.0f, 1.0f);
							if (!thisCard.isOccupied && rand > 0.25f)
							{
								if (doSpawnEnemies)
								{
									GameObject newEnemy = GetBiomeEnemy(0);
									Vector3 cardPos = thisCard.m_actorBase.transform.position;
									Vector3 cardRot = thisCard.transform.eulerAngles;
									cardRot.z = 180;
									Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
									thisCard.enemy = enemy;
									enemy.Initialize(Enemy.EnemyState.Inactive, thisCard);
									//enemy.gameObject.transform.parent = thisCard.cardMesh.transform;
									thisMap.m_enemies.Add(enemy);
								}
							}
						}
					}
				}
			}
		}
	}

	private int fTrapsActive = 0;
	public IEnumerator ActivateFlipTrap (Card trapCard)
	{
		bool first = false;
		if (fTrapsActive == 0 && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player)
		{
			Debug.Log("STARTING FLIP TRAP");
			fTrapsActive ++;
			first = true;
		}



		//trapCard.type = Card.CardType.Normal;
		List<GameManager.Direction> directions = new List<GameManager.Direction>();
		List<Card> northCards = new List<Card>();
		List<Card> southCards = new List<Card>();
		List<Card> eastCards = new List<Card>();
		List<Card> westCards = new List<Card>();
		
		
		Card[] linkedCards = trapCard.linkedCards;
		
		if (linkedCards[0] != null)
		{
			directions.Add(GameManager.Direction.North);	
		}
		if (linkedCards[1] != null)
		{
			directions.Add(GameManager.Direction.South);	
		}
		if (linkedCards[2] != null)
		{
			directions.Add(GameManager.Direction.East);	
		}
		if (linkedCards[3] != null)
		{
			directions.Add(GameManager.Direction.West);	
		}
		
		
		bool cardsToFlip = false;
		foreach (GameManager.Direction thisDirection in directions)
		{
			List<Card> uncheckedCards = new List<Card>();
			
			uncheckedCards.Add(trapCard);
			
			while (uncheckedCards.Count > 0)
			{
				
				Card thisCard = uncheckedCards[0];
				
				uncheckedCards.RemoveAt(0);
				Card[] lc = thisCard.linkedCards;
				if (lc[0] != null && thisDirection == GameManager.Direction.North)
				{
					northCards.Add(lc[0]);
					uncheckedCards.Add(lc[0]);
					
					if (lc[0].cardState == Card.CardState.Hidden)
					{
						lc[0].ChangeHighlightState(true);
						cardsToFlip = true;
					}
				}
				else if (lc[1] != null && thisDirection == GameManager.Direction.South)
				{
					southCards.Add(lc[1]);
					uncheckedCards.Add(lc[1]);
					
					if (lc[1].cardState == Card.CardState.Hidden)
					{
						lc[1].ChangeHighlightState(true);
						cardsToFlip = true;
					}
				}
				else if (lc[2] != null && thisDirection == GameManager.Direction.East)
				{
					eastCards.Add(lc[2]);
					uncheckedCards.Add(lc[2]);
					
					if (lc[2].cardState == Card.CardState.Hidden)
					{
						lc[2].ChangeHighlightState(true);
						cardsToFlip = true;
					}
				}
				else if (lc[3] != null && thisDirection == GameManager.Direction.West)
				{
					westCards.Add(lc[3]);
					uncheckedCards.Add(lc[3]);
					
					if (lc[3].cardState == Card.CardState.Hidden)
					{
						lc[3].ChangeHighlightState(true);
						cardsToFlip = true;
					}
				}
			}
		}

		if (cardsToFlip)
		{
			if (first)
			{
				GameManager.m_gameManager.acceptInput = false;
			}
			yield return new WaitForSeconds(0.4f);
		}
		
		//start flipping cards
		while (northCards.Count > 0 || southCards.Count > 0 || eastCards.Count > 0 || westCards.Count > 0)
		{
			yield return new WaitForSeconds(0.08f);
			
			if (northCards.Count > 0)
			{
				Card northCard = northCards[0];
				northCards.RemoveAt(0);
				if (northCard.cardState == Card.CardState.Hidden)
				{
					northCard.ChangeHighlightState(false);
					if (northCard.doFlipTrap){fTrapsActive++;}
					StartCoroutine(northCard.ChangeCardState(Card.CardState.Normal));
					Instantiate(AssetManager.m_assetManager.m_particleFX[2], northCard.transform.position, AssetManager.m_assetManager.m_particleFX[2].transform.rotation);
				}
			}
			if (southCards.Count > 0)
			{
				Card southCard = southCards[0];
				southCards.RemoveAt(0);
				if (southCard.cardState == Card.CardState.Hidden)
				{
					southCard.ChangeHighlightState(false);
					if (southCard.doFlipTrap){fTrapsActive++;}
					StartCoroutine(southCard.ChangeCardState(Card.CardState.Normal));
					Instantiate(AssetManager.m_assetManager.m_particleFX[2], southCard.transform.position, AssetManager.m_assetManager.m_particleFX[2].transform.rotation);

				}
			}
			if (eastCards.Count > 0)
			{
				Card eastCard = eastCards[0];
				eastCards.RemoveAt(0);
				if (eastCard.cardState == Card.CardState.Hidden)
				{
					eastCard.ChangeHighlightState(false);
					if (eastCard.doFlipTrap){fTrapsActive++;}
					StartCoroutine(eastCard.ChangeCardState(Card.CardState.Normal));
					Instantiate(AssetManager.m_assetManager.m_particleFX[2], eastCard.transform.position, AssetManager.m_assetManager.m_particleFX[2].transform.rotation);

				}
			}
			if (westCards.Count > 0)
			{
				Card westCard = westCards[0];
				westCards.RemoveAt(0);
				if (westCard.cardState == Card.CardState.Hidden)
				{
					westCard.ChangeHighlightState(false);
					if (westCard.doFlipTrap){fTrapsActive++;}
					StartCoroutine(westCard.ChangeCardState(Card.CardState.Normal));
					Instantiate(AssetManager.m_assetManager.m_particleFX[2], westCard.transform.position, AssetManager.m_assetManager.m_particleFX[2].transform.rotation);

				}
			}
			Debug.Log("FTRPS: " + fTrapsActive);
			yield return null;
		}

		fTrapsActive --;
		Debug.Log("FTRPS: " + fTrapsActive);
//		if (fTrapsActive == 0 && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player)
//		{
//			GameManager.m_gameManager.acceptInput = true;
//		}

		while (first && fTrapsActive > 0)
		{
			yield return null;
		}

		if (cardsToFlip && first)
		{
			yield return new WaitForSeconds (0.5f);
		}

		Debug.Log("ENDING FLIP TRAP");

		yield return null;
	}
	
//	public IEnumerator ActivateFlipTrap2 (Card trapCard)
//	{
//		List<GameManager.Direction> directions = new List<GameManager.Direction>();
//		List<Card> validCards = new List<Card>();
//
//		trapCard.type = Card.CardType.Normal;
//		
//		Card[] linkedCards = trapCard.linkedCards;
//		
//		if (linkedCards[0] != null)
//		{
//			directions.Add(GameManager.Direction.North);	
//		}
//		if (linkedCards[1] != null)
//		{
//			directions.Add(GameManager.Direction.South);	
//		}
//		if (linkedCards[2] != null)
//		{
//			directions.Add(GameManager.Direction.East);	
//		}
//		if (linkedCards[3] != null)
//		{
//			directions.Add(GameManager.Direction.West);	
//		}
//		
//		foreach (GameManager.Direction thisDirection in directions)
//		{
//			List<Card> uncheckedCards = new List<Card>();
//			
//			uncheckedCards.Add(trapCard);
//			
//			while (uncheckedCards.Count > 0)
//			{
//				
//				Card thisCard = uncheckedCards[0];
//				
//				uncheckedCards.RemoveAt(0);
//				Card[] lc = thisCard.linkedCards;
//				if (lc[0] != null && thisDirection == GameManager.Direction.North)
//				{
//					validCards.Add(lc[0]);
//					uncheckedCards.Add(lc[0]);
//				}
//				else if (lc[1] != null && thisDirection == GameManager.Direction.South)
//				{
//					validCards.Add(thisCard.southCard);
//					uncheckedCards.Add(thisCard.southCard);
//				}
//				else if (lc[2] != null && thisDirection == GameManager.Direction.East)
//				{
//					validCards.Add(lc[2]);
//					uncheckedCards.Add(lc[2]);
//				}
//				else if (lc[3] != null && thisDirection == GameManager.Direction.West)
//				{
//					validCards.Add(lc[3]);	
//					uncheckedCards.Add(lc[3]);
//				}
//			}
//		}
//		
//		foreach (Card validCard in validCards)
//		{
//			if (validCard.cardState == Card.CardState.Hidden)
//			{
//				yield return new WaitForSeconds(0.2f);
//				//Debug.Log("flip");
//				StartCoroutine(validCard.ChangeCardState(Card.CardState.Normal));	
//			}
//		}
//	}
	
//	public void RemoveMap () {
//		
//		for (int i=0; i < GameManager.m_gameManager.currentQuest.m_maps.Count; i++)
//		{
//			Map thisMap = GameManager.m_gameManager.currentMap;
//			thisMap.m_cards = null;
//			thisMap.m_enemies = null;
//			Destroy(thisMap.m_root.gameObject);
//		}
//		GameManager.m_gameManager.currentMap = null;
//	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	public Map BuildTrial (int trialNum)
	{
		Trial newTrial = GetTrial (trialNum);

		Map newMap = new Map();
		newMap.m_difficulty = trialNum;
		GameObject root = new GameObject();
		newMap.m_root = root.transform;
		float cardWidthOffset = 1.1f;
		float cardHeightOffset = 1.6f;
		int i=0;

		for (int x=0; x<newTrial.m_columns; x++)
		{
			for (int y=0; y<newTrial.m_rows; y++)
			{
				//Debug.Log(i);
				if (newTrial.m_cardMap[i] != 0)
				{

					Vector3 pos = new Vector3(y * cardWidthOffset,0,x * cardHeightOffset * -1);
//					pos.x += chunkXOffset - (chunkXSize/2);
//					pos.z += chunkYOffset - (chunkYSize/2);
					
//					int col = y + (chunkXSize * chunkX);
//					int row = x + (chunkYSize * (chunkY*-1));
					int col = y;
					int row = x;

					//skew placement slightly
					Vector3 rot = m_card.transform.eulerAngles;
					rot.y += Random.Range(-2.5f, 2.5f);
					
					Card newCard = (Card)((GameObject)Instantiate(m_cardTypes[0], pos, Quaternion.Euler(rot))).transform.GetComponent("Card");	
					newCard.gameObject.transform.parent = newMap.m_root;
					newMap.m_cards.Add(newCard);

					if (newTrial.m_cardMap[i] == 8)
					{
						newCard.Initialize(i, col, row, Card.CardState.Normal, Card.CardType.Entrance);
						newMap.m_entrance = newCard;
					} else if (newTrial.m_cardMap[i] == 9)
					{
						newCard.Initialize(i, col, row, Card.CardState.Hidden, Card.CardType.Exit);
						newMap.m_exit = newCard;
					}
					else {
						newCard.Initialize(i, col, row, Card.CardState.Hidden, Card.CardType.Normal);
					}
//					if (thisChunk[i] == 1)
//					{
//
//					}

					if (newTrial.m_enemyMap[i] != 0)
					{
						if (doSpawnEnemies)
						{
							GameObject newEnemy = MapManager.m_mapManager.m_enemies[newTrial.m_enemyMap[i]];
							Vector3 cardPos = newCard.m_actorBase.transform.position;
							Vector3 cardRot = newCard.transform.eulerAngles;
							cardRot.z = 180;
							Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
							newCard.enemy = enemy;
							enemy.Initialize(Enemy.EnemyState.Inactive, newCard);
							newMap.m_enemies.Add(enemy);
						}
						
					}

					if (newTrial.m_trapMap[i] == 1)
					{
						newCard.doFlipTrap = true;
					} else if (newTrial.m_trapMap[i] == 2)
					{
						newCard.type = Card.CardType.Tower;
					}  else if (newTrial.m_trapMap[i] == 3)
					{
						newCard.type = Card.CardType.Stalactite;
					} 

					if (newTrial.m_lootMap[i] == 1)
					{
						Vector3 cardPos = newCard.m_actorBase.position;
						Vector3 cardRot = newCard.transform.eulerAngles;
						cardRot.z = 180;
						
						Chest newChest = (Chest)((GameObject)Instantiate(m_chest, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Chest");
						newChest.currentCard = newCard;
						newChest.InitializeWithItem(1);
						newChest.m_chestMesh.renderer.enabled = false;
						newCard.chest = newChest;
						newChest.gameObject.transform.parent = newCard.m_actorBase.transform;
						newMap.m_chests.Add(newChest);
					}
				}

				i++;
			}
		}

		foreach (Card thisCard in newMap.m_cards)
		{
			Card[] linkedCards = new Card[4];
			foreach (Card tempCard in newMap.m_cards)
			{
				if (tempCard.column == thisCard.column+1 && tempCard.row == thisCard.row)
				{
					linkedCards[2] = tempCard;	
				}else if  (tempCard.column == thisCard.column-1 && tempCard.row == thisCard.row)
				{
					linkedCards[3] = tempCard;	
				}else if (tempCard.row == thisCard.row-1 && tempCard.column == thisCard.column)
				{
					linkedCards[0] = tempCard;	
				}else if (tempCard.row == thisCard.row+1 && tempCard.column == thisCard.column)
				{
					linkedCards[1] = tempCard;	
				}
			}	
			
			thisCard.SetLinkedCards(linkedCards);
		}

		newMap.m_trial = newTrial;
		DrawMap(newMap.m_cards, newMap);

		return newMap;
	}
	
	public Map BuildLevel (int difficulty)
	{
		
		//seed is reset for each level in case we need to recreate a level
		int randSeed = Random.Range(1, 100000000);
		Random.seed = randSeed;
		SettingsManager.m_settingsManager.lastRandomSeed = randSeed;
		Debug.Log("Seed: " + randSeed);
		
		// Set available enemies based on difficulty
		XMLManager xm = new XMLManager ();
		List<List<string>> banks  = xm.GetEnemyBank (difficulty);
		m_eBank = banks [0];
		if (banks.Count > 1)
		{
			m_bBank = banks[1];
		}
		xm = null;


		Map newMap = new Map();
		newMap.m_difficulty = difficulty;
		GameObject root = new GameObject();
		newMap.m_root = root.transform;
		float cardWidthOffset = 1.2f;
		float cardHeightOffset = 1.55f;
		int id = 0;
		
		int numChunks = 2;
		int cellsPlaced = 0;
//		int chunkXSize = 7;
//		int chunkYSize = 7;
		int chunkXSize = 5;
		int chunkYSize = 5;
		int lootLevel = 0;
		bool doGate = false;
		
		//set generation variables based on difficulty
		int adjustedDiff = difficulty+1;

		if (adjustedDiff > 8)
		{
//			int chunkXSize = 7;
//			int chunkYSize = 7;
		}

		if (difficulty < 0) {
			chunkXSize = 6;
			chunkYSize = 6;
			numChunks = 1;
		} else if (adjustedDiff % 10 == 0) {
			//boss fight
			chunkXSize = 9;
			chunkYSize = 9;
			numChunks = 1;
		} else if (adjustedDiff < 4) 
		{
			numChunks = 1;
		}
		else if (adjustedDiff <= 6)
		{
			numChunks = 1;
			chunkXSize = 7;
			chunkYSize = 7;

		} 
//		else if (adjustedDiff <= 9)
//		{
//			numChunks = 1;
//			chunkXSize = 9;
//			chunkYSize = 9;
//		}
		else if (adjustedDiff == 7 || adjustedDiff == 8)
		{
			numChunks = 2;	
		} else if (adjustedDiff == 9)
		{
			numChunks = 3;	
		}
		else if (adjustedDiff == 11)
		{
			numChunks = 1;	
			chunkXSize = 5;
			chunkYSize = 5;
		}
		else if (adjustedDiff == 12 || adjustedDiff == 13)
		{
			numChunks = 1;	
			chunkXSize = 7;
			chunkYSize = 7;
		}
		else if (adjustedDiff == 14 || adjustedDiff == 15)
		{
			numChunks = 1;	
			chunkXSize = 9;
			chunkYSize = 9;
		}
		else if (adjustedDiff == 16 || adjustedDiff == 17)
		{
			numChunks = 2;	
		}
		else if (adjustedDiff >= 18)
		{
			numChunks = 3;	
		} 
		else if (adjustedDiff == 19)
		{
			numChunks = 4;
		} 
//		else if (adjustedDiff == 18 || adjustedDiff == 19)
//		{
//			numChunks = 3;
//		} else if (adjustedDiff == 20 || adjustedDiff == 21 || adjustedDiff == 22)
//		{
//			numChunks = 4;
//		} else if (adjustedDiff == 23)
//		{
//			numChunks = 5;
//		}
		
		
		
		
		//set chest loot values
		if (difficulty == -1)
		{
			lootLevel = 8;	
		} else if (difficulty == -4)
		{
			lootLevel = 7;
		}
		else if (adjustedDiff == 4 || adjustedDiff == 5)
		{
			lootLevel = 1;	
		} else if (adjustedDiff == 6 || adjustedDiff == 7)
		{
			float rand = Random.Range(0.0f, 1.0f);
			if (rand > 0.5f)
			{
				lootLevel = 2;	
			} else {
				lootLevel = 1;
			}
		} else if (adjustedDiff == 8 || adjustedDiff == 9)
		{
			lootLevel = 2;
		} else if (adjustedDiff == 10 || adjustedDiff == 11)
		{
			float rand = Random.Range(0.0f, 1.0f);
			if (rand > 0.5f)
			{
				lootLevel = 2;	
			} else {
				lootLevel = 3;
			}
		}  else if (adjustedDiff == 12 || adjustedDiff == 13)
		{
			lootLevel = 3;
		} else if (adjustedDiff == 14|| adjustedDiff == 15)
		{
			float rand = Random.Range(0.0f, 1.0f);
			if (rand > 0.5f)
			{
				lootLevel = 3;	
			} else {
				lootLevel = 4;
			}
		}  else if (adjustedDiff == 16 || adjustedDiff == 17)
		{
			lootLevel = 4;
		} else if (adjustedDiff == 18 || adjustedDiff == 19)
		{
			float rand = Random.Range(0.0f, 1.0f);
			if (rand > 0.5f)
			{
				lootLevel = 4;	
			} else {
				lootLevel = 5;
			}
		}  else if (adjustedDiff == 20 || adjustedDiff == 21)
		{
			lootLevel = 5;
		} else if (adjustedDiff == 22 || adjustedDiff == 23 || adjustedDiff == 24)
		{
			float rand = Random.Range(0.0f, 1.0f);
			if (rand > 0.5f)
			{
				lootLevel = 5;	
			} else {
				lootLevel = 6;
			}
		}
		
		
	
		Debug.Log("NUM CHUNKS: " + numChunks);
//		Debug.Log("LOOT LEVEL: " + lootLevel);

		//check for gate
		if (adjustedDiff >= 9)
		{
			float gateChance = 0.1f;

			if (adjustedDiff >= 12 && adjustedDiff < 18)
			{
				gateChance = 0.15f;
			}else if (adjustedDiff >= 18)
			{
				gateChance = 0.2f;
			}

			if (Random.Range(0.0f, 1.0f) <= gateChance)
			{
				doGate = true;
			}
		}

		// debug
		//doGate = true;

		//set biomes
//		if (adjustedDiff <= 1)
//		{
//			newMap.m_mapType = Map.MapType.Plains;	
//		}
//		else if (adjustedDiff >=2 && adjustedDiff <= 8)
//		{
//			newMap.m_mapType = Map.MapType.Cave;	
//		} else if (adjustedDiff >= 9 && adjustedDiff <= 16)
//		{
//			newMap.m_mapType = Map.MapType.Dungeon;
//		} else if (adjustedDiff >= 17 && adjustedDiff <= 24)
//		{
//			newMap.m_mapType = Map.MapType.Molten;	
//		} else if (adjustedDiff >=25 && adjustedDiff <= 32)
//		{
//			newMap.m_mapType = Map.MapType.Crystal;	
//		} else
//		{
//			newMap.m_mapType = Map.MapType.Frozen;	
//		}

		if (adjustedDiff <= 10)
		{
			newMap.m_mapType = Map.MapType.Plains;	
		}
		else if (adjustedDiff > 10 && adjustedDiff <= 20)
		{
			newMap.m_mapType = Map.MapType.Cave;	
		} 
		else if (adjustedDiff >= 21 && adjustedDiff <= 30)
		{
			newMap.m_mapType = Map.MapType.Dungeon;
		} 
//		else if (adjustedDiff >= 17 && adjustedDiff <= 24)
//		{
//			newMap.m_mapType = Map.MapType.Molten;	
//		} else if (adjustedDiff >=25 && adjustedDiff <= 32)
//		{
//			newMap.m_mapType = Map.MapType.Crystal;	
//		} else
//		{
//			newMap.m_mapType = Map.MapType.Frozen;	
//		}
		

		
		
		
		
		
		
		
		//create a list of adjacent, unoccupied cells to pull from randomly
		List<GridCell> availableCells = new List<GridCell>();
		
		//create a list of occupied cells to place chunks in
		List<GridCell> occupiedCells = new List<GridCell>();
		
		//create grid with rows/columns = numChunks x 2 + 1
		//each grid cell holds one chunk
		
		int centerCell = ((numChunks*2)+1)/2;
		GridCell[,] chunkGrid = new GridCell[(numChunks*2)+1, (numChunks*2)+1];
		
		//initialize grid values
		
		for (int x=0; x < (numChunks*2)+1; x++)
		{
			for (int y=0; y < (numChunks*2)+1; y++)
			{
				GridCell thisCell = chunkGrid[x,y];
				thisCell.m_isOccupied = false;
				thisCell.m_xPos = x;
				thisCell.m_yPos = y * -1;
				
				//set middle cell to occupied as the starting point
				
				if (x == centerCell && y == centerCell)
				{
					thisCell.m_isOccupied = true;
					cellsPlaced ++;
					occupiedCells.Add(thisCell);
				}
				
				chunkGrid[x,y] = thisCell;
			}
		}
		
		
		
		
		
		
		//gather initial unoccupied cells
		
		GridCell currentCell = chunkGrid[centerCell, centerCell];
		availableCells.Add(chunkGrid[currentCell.m_xPos+1, currentCell.m_yPos*-1]);
		availableCells.Add(chunkGrid[currentCell.m_xPos-1, currentCell.m_yPos*-1]);
		availableCells.Add(chunkGrid[currentCell.m_xPos, (currentCell.m_yPos*-1)-1]);
		availableCells.Add(chunkGrid[currentCell.m_xPos, (currentCell.m_yPos*-1)+1]);

		while (cellsPlaced < numChunks)
		{
			//grab a random cell from the available list, remove it, and mark it occupied	
			
			int rand = Random.Range(0, availableCells.Count);
			GridCell randCell = availableCells[rand];
			availableCells.RemoveAt(rand);
			randCell.m_isOccupied = true;
			chunkGrid[randCell.m_xPos, randCell.m_yPos*-1] = randCell;
			cellsPlaced ++;
			occupiedCells.Add(randCell);
			//update list of available unoccupied cells
			
			currentCell = randCell;
			//Debug.Log(randCell.m_xPos + " / " + randCell.m_yPos);
			for (int i=0; i < 4; i++)
			{
				GridCell thisCell = chunkGrid[currentCell.m_xPos+1, currentCell.m_yPos*-1];
				if (i==1)
				{
					thisCell = chunkGrid[currentCell.m_xPos-1, currentCell.m_yPos*-1];
				} 
				else if (i == 2)
				{
					thisCell = chunkGrid[currentCell.m_xPos, (currentCell.m_yPos*-1)-1];
				} 
				else if (i == 3)
				{
					thisCell = chunkGrid[currentCell.m_xPos, (currentCell.m_yPos*-1)+1];
				}
				
				if (!thisCell.m_isOccupied)
				{
					//make sure it isn't already in the list
					
					bool inList = false;
					for (int j=0; j < availableCells.Count; j++)
					{
						GridCell cell = availableCells[j];
						if (cell.m_xPos == thisCell.m_xPos &&  cell.m_yPos == thisCell.m_yPos)
						{
							inList = true;	
						}
					}
					if (!inList)
					{
						
						availableCells.Add(thisCell);	
					}
				}
			}
		}
		
		
		

		//debug drawing
//		for (int x=0; x < (numChunks*2)+1; x++)
//		{
//			for (int y=0; y < (numChunks*2)+1; y++)
//			{
//				
//				GridCell cell = chunkGrid[x,y];
//				Vector3 pos = new Vector3(cell.m_xPos, 20, cell.m_yPos);
//				if (cell.m_isOccupied)
//				{
//					Instantiate(m_temp01, pos, m_temp01.transform.rotation);
//				} else {
//					Instantiate(m_temp02, pos, m_temp02.transform.rotation);
//				}
//			}
//		}
		
		//we now have a grid of where each chunk will be placed
		//for each occupied grid cell, build one chunk
		int chunksPlaced = 0;

		while (occupiedCells.Count > 0)
		{
			chunksPlaced ++;
			
			GridCell thisCell = occupiedCells[0];
			occupiedCells.RemoveAt(0);
			
			//get a random chunk
			
			int chunkX = thisCell.m_xPos;
			int chunkY = thisCell.m_yPos;
			
			int i=0;
			int[] thisChunk = null;

			if (numChunks == 1 && (difficulty+1) % 10 != 0)
			{
				//if (adjustedDiff < 4)
				if (chunkXSize == 5)
				{
					thisChunk = GetChunk(ChunkType.Single);
				} //else if (adjustedDiff <= 6) {
				else if (chunkXSize == 7)
				{
					thisChunk = GetChunk(ChunkType.SingleMed);
				} //else if (adjustedDiff <= 9) {
				else if (chunkXSize == 9)
				{
					thisChunk = GetChunk(ChunkType.SingleLarge);
				}
			} else {

				if (difficulty < 0)
				{
					thisChunk = GetChunk(ChunkType.Tutorial);
				}
				else if ((difficulty+1) % 10 == 0)
				{
					thisChunk = GetChunk(ChunkType.Boss);
				} else if (chunksPlaced == 1)
				{
					thisChunk = GetChunk(ChunkType.Entrance);
				} else if (chunksPlaced == numChunks)
				{
					thisChunk = GetChunk(ChunkType.Exit);
				} else {
					thisChunk = GetChunk(ChunkType.Standard);
				}
			}

				
			for (int x=0; x<chunkXSize; x++)
			{
				for (int y=0; y<chunkYSize; y++)
				{
					float chunkXOffset = ((chunkX-centerCell) * chunkXSize) * cardWidthOffset;
					float chunkYOffset = ((chunkY+centerCell) * chunkYSize) * cardHeightOffset;
					
					if (thisChunk[i] != 0)
					{
						Vector3 pos = new Vector3(y * cardWidthOffset,0,x * cardHeightOffset * -1);
						pos.x += chunkXOffset - (chunkXSize/2);
						pos.z += chunkYOffset - (chunkYSize/2);

						int col = y + (chunkXSize * chunkX);
						int row = x + (chunkYSize * (chunkY*-1));

						GameManager.m_gameManager.numTiles ++;
						
						//skew placement slightly
						Vector3 rot = m_card.transform.eulerAngles;
						rot.y += Random.Range(-2.5f, 2.5f);
						
						Card newCard = (Card)((GameObject)Instantiate(m_cardTypes[0], pos, Quaternion.Euler(rot))).transform.GetComponent("Card");	
						newCard.gameObject.transform.parent = newMap.m_root;
						newMap.m_cards.Add(newCard);

						if (thisChunk[i] == 1)
						{
					
							newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Normal);
							
							// Debug
//							newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Trap_Razorvine);
//							newCard.type = Card.CardType.Trap_Razorvine;
						} else if (thisChunk[i] == 2)
						{
							newCard.doFlipTrap = true;
							newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Normal);
							
						} else if (thisChunk[i] == 3)
						{
							newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Normal);
							
							if (doSpawnEnemies)
							{
								GameObject newEnemy = GetEnemy();
								Vector3 cardPos = newCard.m_actorBase.transform.position;
								Vector3 cardRot = newCard.transform.eulerAngles;
								cardRot.z = 180;
								Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
								newCard.enemy = enemy;
								enemy.Initialize(Enemy.EnemyState.Inactive, newCard);
//								enemy.gameObject.transform.parent = newCard.cardMesh.transform;
								newMap.m_enemies.Add(enemy);
							}
							
						} else if (thisChunk[i] == 4)
						{
							newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Normal);
							
							Vector3 cardPos = newCard.m_actorBase.position;
							Vector3 cardRot = newCard.transform.eulerAngles;
							cardRot.z = 180;

							bool trapChest = false;
							float trapChestChance = 0;
							if (difficulty > 12)
							{
								trapChestChance = 0.1f;
							} else if (difficulty > 20)
							{
								trapChestChance = 0.15f;
							}

							if (Random.Range(0.0f, 1.0f) <= trapChestChance)
							{
								trapChest = true;
							}

							Chest newChest = (Chest)((GameObject)Instantiate(m_chest, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Chest");
							newChest.currentCard = newCard;
							if (trapChest)
							{
								newChest.Initialize(GetEnemy(), lootLevel);
							} else {
								newChest.Initialize(false, lootLevel);
							}
							newChest.m_chestMesh.renderer.enabled = false;
							newCard.chest = newChest;
							newChest.gameObject.transform.parent = newCard.m_actorBase.transform;
							newMap.m_chests.Add(newChest);
						} else if (thisChunk[i] == 5)
						{
							List<Card.CardType> poiTypes = new List<Card.CardType>();

							if (difficulty < 0)
							{
								poiTypes.Add(Card.CardType.Fort);
							} else {

								poiTypes.Add(Card.CardType.HighGround);
								poiTypes.Add(Card.CardType.Trap_Razorvine);
								poiTypes.Add(Card.CardType.Quicksand);
								poiTypes.Add(Card.CardType.Tower);
								poiTypes.Add(Card.CardType.Fort);
								//poiTypes.Add(Card.CardType.Warren);
								poiTypes.Add(Card.CardType.Trap_Razorvine);
								poiTypes.Add(Card.CardType.Quicksand);
								poiTypes.Add(Card.CardType.HighGround);
							}
						
							Card.CardType type = poiTypes[Random.Range(0, poiTypes.Count)];
							
							// chance to remove POI on beginning levels
							if (difficulty >= 0 && difficulty < 3)
							{
								type = Card.CardType.Normal;	
							} else if (difficulty >= 0 && difficulty < 6)
							{
								float rand = Random.Range(0.0f, 1.0f);
								if (rand >= 0.5f)
								{
									type = Card.CardType.Normal;	
								}
							} else if (difficulty >= 0 && difficulty < 10)
							{
								float rand = Random.Range(0.0f, 1.0f);
								if (rand >= 0.75f)
								{
									type = Card.CardType.Normal;	
								}
							}
							
							newCard.Initialize(id, col, row, Card.CardState.Hidden, type);
							newCard.type = type; // trap card properties are set here, should fix this to be cleaner (done in initialization) later.
						} else if (thisChunk[i] == 7)
						{
							newCard.doFlipTrap = true;
							newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Normal);
							
							if (doSpawnEnemies)
							{
								GameObject newEnemy = GetEnemy();
								Vector3 cardPos = newCard.m_actorBase.transform.position;
								Vector3 cardRot = newCard.transform.eulerAngles;
								cardRot.z = 180;
								Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
								newCard.enemy = enemy;
								enemy.Initialize(Enemy.EnemyState.Inactive, newCard);
								//enemy.gameObject.transform.parent = newCard.cardMesh.transform;
								newMap.m_enemies.Add(enemy);
							}
						} else if (thisChunk[i] == 8)
						{
							newCard.Initialize(id, col, row, Card.CardState.Normal, Card.CardType.Entrance);
							newMap.m_entrance = newCard;
						} else if (thisChunk[i] == 9)
						{
							if (doGate)
							{
								newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Gate);
							} else {
								newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Exit);
							}
							newMap.m_exit = newCard;
						} else if (thisChunk[i] == 10)
						{
							newCard.Initialize(id, col, row, Card.CardState.Hidden, Card.CardType.Normal);
							
							if (doSpawnEnemies)
							{
								//spawn boss enemy
								if (m_bBank != null)
								{
									string boss = m_bBank[0];
									foreach (GameObject go in m_bosses)
									{
										if (boss == go.name)
										{
											Debug.Log("SPAWNING BOSS: " + go.name);
											GameObject newEnemy = go;
											Vector3 cardPos = newCard.m_actorBase.transform.position;
											Vector3 cardRot = newCard.transform.eulerAngles;
											cardRot.z = 180;
											Enemy enemy = (Enemy)((GameObject)Instantiate(newEnemy, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Enemy");
											newCard.enemy = enemy;
											enemy.Initialize(Enemy.EnemyState.Inactive, newCard);
											//enemy.gameObject.transform.parent = newCard.cardMesh.transform;
											newMap.m_enemies.Add(enemy);

											break;
										}
									}
								} else { Debug.Log("BOSS BANK NOT FOUND"); }


							}
						}
					}
						i++;
						id++;
				}
			}
		}
		
		// place key in chest
		if (doGate && newMap.m_chests.Count > 0)
		{
			Chest keyChest = newMap.m_chests[Random.Range(0, newMap.m_chests.Count)];
			keyChest.Initialize(true, 0);
		} else if (doGate && newMap.m_chests.Count == 0) {
			newMap.m_exit.Initialize(newMap.m_exit.id, newMap.m_exit.column, newMap.m_exit.row, Card.CardState.Hidden, Card.CardType.Exit);
			Debug.Log("NO CHESTS TO PLACE KEY IN");
		}
		
		
		
		//set up linked cards
		List<Card> deadEnds = new List<Card>();
		
		foreach (Card thisCard in newMap.m_cards)
		{
			Card[] linkedCards = new Card[4];
			foreach (Card tempCard in newMap.m_cards)
			{
				if (tempCard.column == thisCard.column+1 && tempCard.row == thisCard.row)
				{
					linkedCards[2] = tempCard;	
				}else if  (tempCard.column == thisCard.column-1 && tempCard.row == thisCard.row)
				{
					linkedCards[3] = tempCard;	
				}else if (tempCard.row == thisCard.row-1 && tempCard.column == thisCard.column)
				{
					linkedCards[0] = tempCard;	
				}else if (tempCard.row == thisCard.row+1 && tempCard.column == thisCard.column)
				{
					linkedCards[1] = tempCard;	
				}
			}	
			
			thisCard.SetLinkedCards(linkedCards);
			
			//sort cards by number of linked cards
			if (thisCard.numLinked == 1 && thisCard.type == Card.CardType.Normal && !thisCard.isOccupied)
			{
				deadEnds.Add(thisCard);	
			}
		}
			
				
		//place follower if level is a multiple of 3

		//remove followers that don't meet minimum difficulty or are already in party
		List<GameObject> availableFollowers = new List<GameObject>();

		if ((difficulty+1) % 4 == 0 && difficulty >= 0)
		//if ((difficulty % 1) == 0)
		{

			//Debug.Log(difficulty % 3);
			foreach (GameObject bank in GameManager.m_gameManager.m_followerBank)
			{
				Follower bankF = (Follower)bank.GetComponent("Follower");
				bool validFollower = true;
				if (validFollower)
				{
					availableFollowers.Add(bank);	
				}
			}

		if (availableFollowers.Count > 0 && m_currentFollower < GameManager.m_gameManager.m_followerBank.Length)
		{
			Debug.Log("LOOKING UP FOLLOWER");
			bool followerPlaced = false;
			while (!followerPlaced)
			{

				GameObject randF = (GameObject)GameManager.m_gameManager.m_followerBank[m_currentFollower];
				Follower randFScript = (Follower)randF.GetComponent("Follower");

				
				if (!GameManager.m_gameManager.CheckLockState(randFScript))
				//if (GameManager.m_gameManager.CheckLockState(randFScript)) //Debug
				{
						Debug.Log("ABANDONING FOLLOWER PLACEMENT");
						m_currentFollower ++;


					break;
				}
				
				if (deadEnds.Count > 0)
				{
					Debug.Log("SPAWNING FOLLOWER");
					int rand = Random.Range(0, deadEnds.Count);
					Card randCard = deadEnds[rand];
					deadEnds.RemoveAt(rand);
					
					m_currentFollower ++;
					Vector3 pos = randCard.transform.position;
					Vector3 rot = randF.transform.eulerAngles;
					rot.z = 180;
					GameObject newFollower = (GameObject)(Instantiate(randF, pos, Quaternion.Euler(rot)));
					randCard.follower = newFollower;
					Follower fScript = (Follower)newFollower.transform.GetComponent("Follower");
					newMap.m_follower = fScript;
					BoxCollider bc = (BoxCollider) newFollower.GetComponent("BoxCollider");
					bc.enabled = false;

					fScript.currentCard = randCard;
					newFollower.gameObject.transform.parent = randCard.cardMesh.transform;
					newFollower.SetActive(false);
					followerPlaced = true;	
					
					//set progress values
					foreach (GameState.ProgressState thisState in GameManager.m_gameManager.gameProgress)
					{
						if (thisState.m_followerType == fScript.m_followerType)
						{
							fScript.isLocked = thisState.m_isLocked;
							fScript.currentLevel = thisState.m_level;
							fScript.currentXP = thisState.m_XP;
							break;
						}
					}
					
				} else
				{
					//no empty dead ends, place follower on random card
					Card randCard = newMap.m_cards[Random.Range(0, newMap.m_cards.Count)];
					
					if (!randCard.isOccupied)
					{
						Debug.Log("SPAWNING FOLLOWER");
						
						m_currentFollower ++;
						Vector3 pos = randCard.transform.position;
						Vector3 rot = randF.transform.eulerAngles;
						rot.z = 180;
						GameObject newFollower = (GameObject)(Instantiate(randF, pos, Quaternion.Euler(rot)));
						randCard.follower = newFollower;
						Follower fScript = (Follower)newFollower.transform.GetComponent("Follower");
						newMap.m_follower = fScript;	
						BoxCollider bc = (BoxCollider) newFollower.GetComponent("BoxCollider");
						bc.enabled = false;
	
						fScript.currentCard = randCard;
						newFollower.gameObject.transform.parent = randCard.cardMesh.transform;
						newFollower.SetActive(false);
						followerPlaced = true;	
						
						//set progress values
						foreach (GameState.ProgressState thisState in GameManager.m_gameManager.gameProgress)
						{
							if (thisState.m_followerType == fScript.m_followerType)
							{
								fScript.isLocked = thisState.m_isLocked;
								fScript.currentLevel = thisState.m_level;
								fScript.currentXP = thisState.m_XP;
								break;
							}
						}
					}
				}
					
					if (followerPlaced)
					{
						DHeart.m_dHeart.StartHeartBeat();	
					}
				
			}
		}
	}
				
		//place storage chest if level is a multiple of 8
		//if ((difficulty+1) % 8 == 0 )
		if ((difficulty+1) == m_chapter1Shop || (difficulty+1) == m_chapter2Shop)
		{
//			if (deadEnds.Count > 0)
//			{
//				Debug.Log("SPAWNING STORAGE CHEST");
//				int rand = Random.Range(0, deadEnds.Count);
//				Card randCard = deadEnds[rand];
//				deadEnds.RemoveAt(rand);
//				
//				Vector3 pos = randCard.transform.position;
//				Vector3 rot = randCard.transform.eulerAngles;
//				rot.z = 180;
//				Chest newChest = (Chest)((GameObject)Instantiate(m_storageChest, pos, Quaternion.Euler(rot))).transform.GetComponent("Chest");
//				newChest.currentCard = randCard;
//				newChest.Initialize(false, 0);
//				newChest.m_chestMesh.renderer.enabled = false;
//				randCard.chest = newChest;
//				newChest.gameObject.transform.parent = randCard.cardMesh.transform;
//			} else 
//			{
//				bool chestPlaced = false;
//				
//			}



			Card shopCard = null;

			if (deadEnds.Count > 0)
			{
				int rand = Random.Range(0, deadEnds.Count);
				shopCard = deadEnds[rand];
				deadEnds.RemoveAt(rand);
			} else {

				//no empty dead ends, place shop on random card
				bool freeCardFound = false;
				while (!freeCardFound)
				{
					Card randCard = newMap.m_cards[Random.Range(0, newMap.m_cards.Count)];
					if (!randCard.isOccupied)
					{
						shopCard = randCard;
						freeCardFound = true;
					}
				}
			}

			if (shopCard != null)
			{
				Debug.Log("SPAWNING SHOP");
				Vector3 pos = shopCard.transform.position;
				Vector3 rot = shopCard.transform.eulerAngles;
				rot.z = 180;
				GameObject shop = (GameObject)Instantiate(AssetManager.m_assetManager.m_props[0], pos, Quaternion.Euler(rot));
				Shop s = (Shop)shop.transform.GetComponent("Shop");
				s.Initialize(difficulty);
				
				foreach (MeshRenderer mr in s.m_shopMesh)
				{
					mr.renderer.enabled = false;
				}
				shop.transform.parent = shopCard.cardMesh.transform;
				shopCard.shop = s;
				
				BoxCollider bc = (BoxCollider) shop.GetComponent("BoxCollider");
				bc.enabled = false;
			}
			
		}
		
		
		
		
		
		//create sub biomes
		if (Random.Range(0.0f, 1.0f) >= 0.75f && adjustedDiff > 3)
//		if (difficulty > 0)
		{
			int numBiomes = Random.Range(1, 4);
			
			if (adjustedDiff < 9)
			{
				numBiomes = 1;	
			} else if (adjustedDiff < 17 && numBiomes > 2)
			{
				numBiomes = 2;	
			}
			for (int i=0; i < numBiomes; i++)
			{
				PlaceBiome(newMap);
			}
		}
		
		//Debug
//		int numB = 3;
//		for (int i=0; i < numB; i++)
//		{
//			PlaceBiome(newMap);
//		}


		// place debug shop
//		bool shopPlaced = false;
//		while (!shopPlaced)
//		{
//			Card c = (Card)newMap.m_cards[Random.Range(0, newMap.m_cards.Count)];
//
//			if (!c.isOccupied)
//			{
//				Vector3 pos = c.transform.position;
//				Vector3 rot = c.transform.eulerAngles;
//				rot.z = 180;
//				GameObject shop = (GameObject)Instantiate(AssetManager.m_assetManager.m_props[0], pos, Quaternion.Euler(rot));
//				Shop s = (Shop)shop.transform.GetComponent("Shop");
//
//				foreach (MeshRenderer mr in s.m_shopMesh)
//				{
//					mr.renderer.enabled = false;
//				}
//				shop.transform.parent = c.cardMesh.transform;
//				c.shop = s;
//
//				BoxCollider bc = (BoxCollider) shop.GetComponent("BoxCollider");
//				bc.enabled = false;
//
//				shopPlaced = true;
//			}
//		}

		// manage any informational props used during the tutorial
		if (m_tempProps.Count > 0)
		{
			foreach (GameObject g in m_tempProps)
			{
				Destroy(g);
			}
			m_tempProps.Clear();
		}

		if (difficulty < 0)
		{


			Vector3 pos = newMap.m_entrance.transform.position;
			switch (difficulty)
			{
			case -6:
				GameObject prop1 = AssetManager.m_assetManager.m_props[1];
				m_tempProps.Add((GameObject)Instantiate(prop1, pos, prop1.transform.rotation));
				break;
			case -5:
				GameObject prop2 = AssetManager.m_assetManager.m_props[2];
				m_tempProps.Add((GameObject)Instantiate(prop2, pos, prop2.transform.rotation));
				break;
			case -4:
				GameObject prop3 = AssetManager.m_assetManager.m_props[3];
				m_tempProps.Add((GameObject)Instantiate(prop3, pos, prop3.transform.rotation));
				break;
			case -3:
				GameObject prop4 = AssetManager.m_assetManager.m_props[9];
				m_tempProps.Add((GameObject)Instantiate(prop4, pos, prop4.transform.rotation));
				break;
			case -2:
				GameObject prop5 = AssetManager.m_assetManager.m_props[4];
				m_tempProps.Add((GameObject)Instantiate(prop5, pos, prop5.transform.rotation));
				break;
			case -1:
				GameObject prop6 = AssetManager.m_assetManager.m_props[5];
				m_tempProps.Add((GameObject)Instantiate(prop6, pos, prop6.transform.rotation));
				break;
			}
		}

		DrawMap(newMap.m_cards, newMap);
		
		return newMap;
	}
	
	private GameObject GetBiomeEnemy (int type)
	{
		GameObject[] m_enemies = null;
		//List<Enemy> enemies = new List<Enemy>();
		
		//get enemy list based on biome type and difficulty level
		EnemyList[] enemyList = m_subBiomeEnemies;
//		if (difficulty == 0)
//		{
//			enemyList = m_plainsEnemies;	
//		} else
//		{
//			enemyList = m_dungeonEnemies;
//		}
		if (enemyList != null)
		{
			EnemyList eList = null;
			if (type < enemyList.Length)
			{
				eList = enemyList[type];
			} else {
				eList = enemyList[enemyList.Length-1];	
			}
			
			if (eList != null)
			{
				m_enemies = eList.m_enemyList;	
			}
		}
		
		GameObject thisEnemy = m_enemies[Random.Range(0, m_enemies.Length)];

		return thisEnemy;
	}
	
	
	
	private GameObject GetEnemy ()
	{
//		return m_enemies [25];

		if (m_eBank != null)
		{
			if (m_eBank.Count > 0)
			{
				string eString = m_eBank[Random.Range(0, m_eBank.Count)];
				Debug.Log("SPAWNING: " + eString);
				foreach (GameObject e in m_enemies)
				{
					if (e.name == eString)
					{
						return e;
					}
				}

			} else {
				Debug.Log("BANK IS EMPTY");
			}
		} else {
			Debug.Log("BANK IS NULL");
		}

		return null;
	}
	
	public void SpawnPlayer (Map thisMap, Follower charType)
	{
		Card entranceCard = thisMap.m_entrance;
		if (entranceCard != null)
		{
			Player player = (Player)((GameObject)Instantiate(m_player, entranceCard.m_actorBase.position, m_player.transform.rotation)).transform.GetComponent("Player");
			player.Initialize(entranceCard, charType);
			entranceCard.player = player;
			FollowCamera.m_followCamera.Initialize(player.m_cameraTarget);
			SetDistanceToPlayer();
			if (SettingsManager.m_settingsManager.difficultyLevel < 0)
			{
				Player.m_player.ChangeFacing(GameManager.Direction.North);
			} else {Player.m_player.ChangeFacing(GameManager.Direction.South); }

			if (SettingsManager.m_settingsManager.difficultyLevel == 0 && !SettingsManager.m_settingsManager.trial)
			{
				FirstMap(thisMap);
			}
		} else {
			Debug.Log("No valid entrance");	
		}	
	}
	
	public IEnumerator NextLevel (int difficultyIncrease)
	{
		//delete current map and create next map
		SettingsManager.m_settingsManager.levelsTravelled ++;
		GameManager.m_gameManager.numTilesFlipped = 0;
		GameManager.m_gameManager.numTiles = 0;
		SettingsManager.m_settingsManager.EndLevel(difficultyIncrease);
		Map thisMap = GameManager.m_gameManager.currentMap;
		Destroy(thisMap.m_root.gameObject);
		GameManager.m_gameManager.currentMap = null;
		StartCoroutine (UIManager.m_uiManager.UpdateStack ());
		GameManager.m_gameManager.currentMap = BuildLevel(SettingsManager.m_settingsManager.difficultyLevel);
		
		//place player
		Card startCard = GameManager.m_gameManager.currentMap.m_entrance;
		FollowCamera.m_followCamera.m_moveTransform.parent = Player.m_player.transform;
		Player.m_player.transform.position = startCard.m_actorBase.transform.position;
		Player.m_player.currentCard = startCard;
		startCard.player = Player.m_player;
		FollowCamera.m_followCamera.m_moveTransform.parent = null;
		SetDistanceToPlayer();		

		DHeart.m_dHeart.SetLevel(SettingsManager.m_settingsManager.difficultyLevel);
		GameManager.m_gameManager.levelsCompletedBonus ++;
//		yield return StartCoroutine( GameManager.m_gameManager.FillHand());
//		UIManager.m_uiManager.RefreshInventoryMenu ();
//		Player.m_player.animation.Stop();
//		Player.m_player.animation.Play("PlayerDrop01");
//		yield return new WaitForSeconds(2);

		yield return new WaitForSeconds(0.5f);
		//AssetManager.m_assetManager.m_uiSprites [0].gameObject.SetActive (true);
		float time = 0.0f;
		while (time < 1.5f)
		{
			time += Time.deltaTime;
			float a = Mathf.Lerp(1.0f, 0.0f, time / 1.5f);
			Color c = AssetManager.m_assetManager.m_uiSprites[0].color;
			c.a = a;
			AssetManager.m_assetManager.m_uiSprites[0].color = c;
			yield return null;
		}
		AssetManager.m_assetManager.m_uiSprites [0].gameObject.SetActive (false);
		AssetManager.m_assetManager.m_uiSprites[0].color = Color.white;
		//AssetManager.m_assetManager.m_uiSprites [0].transform.localPosition = Vector3.one * 100000;
		
		yield return StartCoroutine( GameManager.m_gameManager.FillHand());
		GameManager.m_gameManager.acceptInput = true;
		yield return null;
	}
	
	private void FirstMap (Map thisMap)
	{

		//remove features close to the entrance in the first level
		foreach (Card c in thisMap.m_cards)
		{
			if (c.distanceToPlayer == 1 || c.distanceToPlayer == 2)
			{
				if (c.doFlipTrap)
				{
					c.doFlipTrap = false;
				}
				
				if (c.enemy != null)
				{
					Enemy e = (Enemy)c.enemy;
					for (int i=0; i < thisMap.m_enemies.Count; i++)
					{
						Enemy thisEnemy = (Enemy)thisMap.m_enemies[i];
						
						if (e == thisEnemy)
						{
							thisMap.m_enemies.RemoveAt(i);
							c.enemy = null;
							Destroy(thisEnemy);
							i = 999;
						}
					}
				}
			}
		}
		
	}
	
	private int[] GetChunk (ChunkType type)
	{
		Debug.Log ("GETTING CHUNK TYPE: " + type.ToString ());
		//tile types:
		// 0: Empty
		// 1: Normal tile
		// 2: Flip Trap
		// 3: Enemy
		// 4: Chest
		// 5: Environment Tile
		// 6: Gate
		// 7: Flip Trap w Enemy
		// 8: Entrance
		// 9: Exit
		// 10: Boss
		
		List<int[]> chunkArray = new List<int[]>();

		if (type == ChunkType.Tutorial)
		{
			//6x6 chunk size

			switch (SettingsManager.m_settingsManager.difficultyLevel )
			{
			case -6:
				chunkArray.Add( new int[] {0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,3,0,0,0,0,1,1,0,0,0,0,1,0,0,0,0,0,8,0,0,0});
				break;
			case -5:
				chunkArray.Add( new int[] {0,0,9,0,0,0,0,0,1,0,0,0,0,3,1,3,0,0,0,1,2,1,0,0,0,0,1,0,0,0,0,0,8,0,0,0});
				break;
			case -4:
				chunkArray.Add( new int[] {0,0,0,0,0,0,0,0,4,0,0,0,0,0,3,0,0,0,0,2,2,1,9,0,0,1,0,0,0,0,0,8,0,0,0,0});
				break;
			case -3:
				chunkArray.Add( new int[] {0,1,9,1,0,0,0,1,1,1,0,0,0,2,2,2,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,8,0,0,0});
				break;
			case -2:
				chunkArray.Add( new int[] {0,0,0,0,0,0,0,0,1,9,0,0,0,0,1,0,0,0,0,7,2,2,0,0,0,2,1,2,0,0,0,0,8,0,0,0});
				break;
			case -1:
				chunkArray.Add( new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,5,2,0,9,0,7,1,7,4,1,0,0,8,0,0,0});
				break;
			}

			//m_currentTutorial ++;
		}
		else if (type == ChunkType.Entrance)
		{
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,3,1,2,0,0,1,1,2,8,2,1,1,0,0,2,1,3,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {3,1,1,1,1,1,3,2,0,0,3,0,0,2,1,0,3,1,1,0,1,1,0,1,2,3,0,1,1,0,0,8,0,0,1,1,0,0,0,0,0,1,3,1,1,2,1,1,3});
//			chunkArray.Add( new int[] {0,1,1,1,3,2,0,2,1,2,0,1,1,0,3,3,0,0,0,1,0,1,0,0,0,8,2,1,1,0,0,0,2,3,0,1,0,4,1,1,0,0,0,0,0,1,3,0,0});
//			chunkArray.Add( new int[] {3,1,1,1,1,1,3,3,2,0,4,0,2,3,2,1,0,0,0,1,2,1,0,0,0,0,0,1,1,0,8,0,1,0,1,3,2,1,2,3,1,1,1,1,0,1,0,1,2});
//			chunkArray.Add( new int[] {1,0,0,1,0,0,4,1,0,3,1,3,0,1,1,1,2,1,2,1,3,3,0,0,0,0,0,1,2,1,1,3,1,2,1,0,8,0,1,0,3,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {4,3,0,1,1,1,1,3,2,0,0,0,1,0,0,1,0,7,1,2,1,1,1,0,4,1,8,3,0,1,0,7,1,2,1,3,1,0,0,0,5,0,2,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,2,8,0,0,0,1,1,1,0,0,0,1,3,0,1,1,2,1,0,0,0,0,0,3,1,0,4,3,0,0,2,3,0,1,3,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,1,1,0,1,4,1,1,3,0,0,1,0,1,0,2,1,1,3,0,1,0,3,0,2,1,1,0,0,1,0,8,0,1,0,0,1,1,0,0,3,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,4,2,1,3,1,3,1,1,1,0,1,0,1,0,1,1,0,2,8,2,0,1,0,1,1,0,1,1,0,4,1,3,0,3,1,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {1,1,0,1,1,1,0,1,8,0,0,0,1,0,1,0,0,0,0,2,0,1,0,0,5,3,1,1,2,1,1,1,2,3,0,0,1,0,3,3,1,4,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,3,0,0,0,0,0,0,1,0,0,0,8,0,0,1,3,0,1,1,1,1,2,2,1,0,0,0,0,1,3,0,1,1,2,1,3,0,0,4,3,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,2,1,2,0,0,0,3,1,0,1,3,0,1,2,3,0,3,2,1,1,1,0,8,0,1,1,0,1,0,1,0,1,0,0,3,1,1,1,3,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,4,0,0,3,1,1,7,1,0,0,1,3,0,1,3,0,1,1,2,8,2,1,1,0,3,1,0,3,1,0,0,1,7,1,1,3,0,0,4,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,0,1,1,1,0,1,1,1,3,2,3,1,1,1,0,1,1,1,0,1,1,0,2,8,2,0,1,0,0,1,1,1,0,0,0,4,3,2,3,1,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,1,1,3,0,3,0,4,2,0,2,1,2,0,3,1,0,1,8,1,1,2,1,0,2,1,2,0,3,1,1,3,0,3,0,1,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,2,1,1,2,1,3,1,4,0,1,0,3,2,0,0,0,1,0,0,8,0,0,1,0,0,0,2,0,0,1,0,3,1,7,1,3,1,1,1,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,3,1,1,1,3,0,1,3,2,1,2,3,1,1,0,0,1,0,0,1,2,0,1,8,1,0,2,1,0,0,1,0,0,1,1,3,2,1,2,3,1,0,1,3,1,3,1,0});
//			chunkArray.Add( new int[] {0,0,1,3,1,0,0,0,0,1,0,1,0,0,0,0,2,8,2,0,0,1,1,1,0,1,1,3,1,1,3,2,3,1,1,4,0,1,1,1,0,1,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,1,1,0,1,1,0,0,1,3,0,3,1,0,1,2,2,0,2,2,1,0,3,1,8,1,3,0,0,0,1,1,1,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,3,0,1,0,3,1,1,2,0,1,0,2,3,0,1,1,1,1,1,0,1,3,1,0,1,3,1,0,0,1,8,1,0,0,0,4,7,2,7,4,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,1,1,1,0,0,4,0,2,3,2,0,1,1,0,3,0,1,0,1,2,1,1,8,1,1,2,1,0,1,1,3,0,1,2,1,3,1,1,1,2});
//			chunkArray.Add( new int[] {0,3,0,1,0,3,0,0,2,1,1,1,2,0,4,1,0,8,0,1,1,1,1,0,0,0,1,1,1,3,0,0,0,3,1,1,3,0,0,0,3,1,2,1,1,1,1,1,2});
//			chunkArray.Add( new int[] {0,0,0,1,1,1,1,0,0,0,0,3,1,3,0,3,0,3,2,0,2,1,1,1,1,1,8,1,0,3,0,3,2,0,2,0,0,0,0,3,1,3,0,0,0,1,1,1,1});
//			chunkArray.Add( new int[] {4,3,1,1,0,0,1,3,2,1,0,0,0,1,0,3,2,1,1,2,3,1,1,0,8,1,0,1,0,3,2,1,1,2,3,3,2,1,0,0,0,1,4,3,1,1,0,0,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,2,0,0,1,0,0,1,1,0,7,7,7,0,3,1,0,1,8,1,0,1,1,0,4,1,1,0,1,1,0,0,0,0,0,2,3,1,2,1,1,1,1});
//			chunkArray.Add( new int[] {2,1,1,1,1,1,1,1,0,3,0,3,0,1,1,3,2,1,2,3,1,1,0,4,8,1,0,1,1,3,2,1,2,3,1,1,0,3,0,1,0,1,1,1,1,1,1,1,2});
//			chunkArray.Add( new int[] {1,1,1,1,1,2,1,3,0,0,0,0,0,1,1,2,1,8,1,2,1,1,3,1,2,1,3,1,2,0,4,0,1,0,3,1,0,0,0,0,0,2,1,2,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,2,1,3,2,0,3,0,0,0,1,1,0,1,3,0,0,1,1,0,2,1,4,0,1,1,0,8,2,1,3,1,1,0,0,0,0,0,2,3,1,2,1,3,1,1});
//			chunkArray.Add( new int[] {1,3,1,1,1,2,1,1,0,0,0,0,0,1,1,0,1,8,4,0,3,2,3,1,2,1,1,2,0,0,0,1,0,0,0,0,0,1,7,1,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,3,1,1,2,1,1,3,0,0,0,0,0,1,1,0,3,1,0,0,1,1,0,2,1,7,0,2,2,0,8,0,1,1,1,1,0,0,0,1,2,3,1,1,3,2,1,3,1});
//			chunkArray.Add( new int[] {1,2,1,1,1,2,1,3,0,0,0,0,0,1,1,1,3,2,3,1,1,1,0,1,2,1,0,1,1,0,0,8,0,0,1,2,0,0,0,0,0,2,3,1,1,1,1,1,3});
//			chunkArray.Add( new int[] {1,2,1,1,1,1,1,1,0,3,0,3,0,1,1,0,2,2,2,0,1,1,0,1,8,1,0,1,3,1,2,0,2,1,3,1,0,0,0,0,0,1,1,1,1,1,1,2,1});
//			chunkArray.Add( new int[] {2,1,1,3,1,1,1,1,0,1,1,1,0,2,1,0,0,1,0,0,1,1,0,3,2,4,0,1,1,0,0,8,0,0,1,3,2,0,0,0,2,3,1,3,1,1,1,3,1});
//			chunkArray.Add( new int[] {0,0,0,1,1,3,0,0,4,0,0,0,2,0,0,3,0,0,0,1,0,1,1,1,8,0,1,1,0,0,0,1,2,3,0,0,0,0,1,3,1,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,0,0,0,0,0,2,3,1,1,0,8,0,0,0,0,1,1,2,1,3,1,1,3,1,0,0,0,0,0,0,1,2,1,3,0,0,0,0,0,1,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,1,1,1,3,2,0,0,3,0,0,0,1,0,1,1,0,2,1,1,1,0,0,0,3,0,8,0,3,2,1,1,0,0,0,1,3,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,2,1,1,3,1,1,3,1,8,0,0,1,0,0,0,0,0,0,1,0,4,0,0,3,1,3,0,1,1,1,2,1,2});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,3,1,2,1,0,0,4,1,0,1,0,0,0,3,1,0,3,0,0,0,1,0,0,1,1,0,0,1,0,0,0,3,1,2,1,0,0,0,1,1,2,8});
//			chunkArray.Add( new int[] {4,1,0,1,0,0,0,1,3,2,1,1,3,0,0,2,8,0,0,1,0,1,1,0,0,0,1,1,1,0,0,0,1,1,0,0,1,0,3,1,2,3,0,1,1,1,0,1,4});
//			chunkArray.Add( new int[] {0,8,0,1,1,1,0,1,2,1,0,0,1,0,1,3,1,0,0,3,0,1,0,1,0,0,1,1,2,0,1,3,1,2,0,1,0,0,0,0,4,0,1,1,1,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,3,1,1,1,2,8,0,1,2,0,1,0,0,0,1,0,0,1,2,0,1,0,0,0,0,1,1,1,0,0,0,0,3,0,0,0,1,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,2,8,2,0,0,0,0,1,0,1,0,0,1,1,3,0,3,1,1,0,1,0,0,0,1,0,3,1,0,1,0,2,3,1,2,1,1,0,1,4});
//			chunkArray.Add( new int[] {2,1,1,1,1,3,1,1,0,0,0,0,0,1,1,1,1,1,3,1,1,3,1,2,8,2,1,3,1,4,1,1,1,1,1,1,0,0,0,0,0,1,1,1,3,1,1,1,2});
//			chunkArray.Add( new int[] {1,7,1,1,1,7,1,1,0,1,2,3,0,1,1,0,1,1,1,0,1,2,0,1,8,1,0,2,1,0,1,1,1,0,1,1,0,3,2,1,0,1,1,7,1,1,1,7,1});
//			chunkArray.Add( new int[] {0,0,0,1,2,1,0,0,0,3,1,1,0,0,1,2,5,1,3,2,1,1,1,1,8,0,1,1,1,2,5,1,3,2,1,0,0,3,1,1,0,0,0,0,0,1,2,1,0});
//			chunkArray.Add( new int[] {1,1,2,1,1,1,3,1,1,3,0,3,5,1,2,3,1,0,4,1,2,1,0,0,0,0,0,1,1,1,4,0,1,1,1,1,5,3,0,1,8,1,3,1,2,1,2,1,1});
//			chunkArray.Add( new int[] {0,1,1,3,0,1,0,1,5,1,5,0,1,3,2,3,4,1,0,2,1,1,5,1,3,0,8,1,1,1,5,1,0,1,1,2,3,1,2,1,1,1,0,1,1,1,0,3,0});
//			chunkArray.Add( new int[] {3,1,2,1,1,1,1,1,0,0,0,0,0,2,1,0,8,1,1,1,1,1,0,1,1,3,2,1,1,0,1,3,1,1,1,2,0,2,1,0,0,1,1,3,1,1,1,1,3});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,2,0,1,3,4,0,2,1,0,1,2,1,0,1,1,2,3,8,1,2,1,1,2,1,2,3,2,1,2,0,0,0,0,0,2,3,1,1,1,1,1,3});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,1,1,1,0,0,1,3,2,4,2,3,1,1,1,1,0,1,1,1,1,3,2,1,2,3,1,0,0,1,8,1,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,1,1,1,1,1,0,3,5,2,1,2,1,1,1,1,7,0,7,1,1,1,1,0,0,0,1,1,1,1,7,0,7,5,1,1,1,2,8,2,1,3,0,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,2,1,3,3,3,1,2,1,1,0,1,0,1,1,1,1,1,0,8,1,1,5,1,0,4,0,1,1,3,5,3,3,3,1,1,1,2,2,1,2,2,1});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,1,1,1,0,0,1,1,2,1,2,1,1,1,3,2,8,2,3,1,1,1,3,0,3,1,1,1,0,0,0,0,0,1,1,2,1,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,1,1,3,0,0,3,1,2,0,1,1,1,2,1,1,0,1,2,3,1,4,3,0,8,1,1,2,1,1,0,1,2,1,3,1,2,0,1,3,1,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,2,1,1,0,0,0,4,3,3,0,0,0,0,0,1,1,2,0,1,3,0,0,8,1,1,1,1,0,2,1,3,0,3,1,2,1,3,0,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,0,0,0,0,3,1,1,1,1,0,1,2,3,1,2,3,0,1,1,0,0,3,2,1,1,0,0,0,1,1,1,0,0,0,1,3,2,1,0,8,1,1,1,1,0});
//			chunkArray.Add( new int[] {1,1,2,1,1,1,3,1,0,0,0,0,0,1,2,1,3,0,3,2,1,1,8,1,0,4,3,2,2,1,1,0,3,2,1,1,0,0,0,0,0,1,1,2,1,1,3,2,1});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,2,1,1,0,0,0,1,3,1,3,2,0,1,1,1,8,1,1,1,0,2,3,1,3,1,0,0,0,1,1,2,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {2,1,1,3,0,0,0,3,1,3,0,0,0,0,0,1,1,2,0,0,0,1,0,1,8,1,0,1,1,3,1,2,1,1,1,0,1,0,0,3,2,3,0,1,1,1,0,3,2});
//			chunkArray.Add( new int[] {1,1,0,1,0,0,0,1,3,1,1,1,1,0,0,2,7,1,1,3,1,1,1,0,0,0,1,1,3,1,2,1,3,2,0,0,1,1,1,1,1,1,0,0,0,1,0,1,8});
//			chunkArray.Add( new int[] {3,1,1,1,1,2,1,2,2,0,0,1,2,3,2,3,0,0,0,3,1,1,3,0,0,0,0,1,0,0,0,3,1,1,1,0,0,1,2,8,1,3,0,1,1,1,1,1,2});
//			chunkArray.Add( new int[] {1,2,3,1,1,1,1,8,2,1,3,1,1,1,0,0,0,1,0,0,0,1,0,2,1,1,0,1,1,1,1,0,1,1,1,2,2,1,1,1,2,2,3,3,0,1,0,3,3});


			// 5x5
			chunkArray.Add( new int[] {0,0,3,0,0,0,8,1,2,0,3,5,2,3,1,0,2,3,1,0,0,0,1,0,0});
			chunkArray.Add( new int[] {1,5,3,0,0,1,2,1,0,0,5,8,2,5,1,3,1,1,0,0,4,5,7,0,0});
			chunkArray.Add( new int[] {1,5,3,0,0,1,2,1,0,0,5,8,2,5,1,3,1,1,0,0,4,5,7,0,0});
			chunkArray.Add( new int[] {0,1,1,1,0,0,2,8,2,0,3,1,1,1,3,0,3,0,1,0,0,1,1,3,0});
			chunkArray.Add( new int[] {3,7,1,1,3,1,4,0,1,1,1,0,0,0,2,1,3,0,8,1,3,2,1,1,1});
			chunkArray.Add( new int[] {3,7,1,1,3,1,4,0,1,1,1,0,0,0,2,1,3,0,8,1,3,2,1,1,1});
			chunkArray.Add( new int[] {3,2,1,1,1,1,1,8,2,3,1,2,0,1,1,1,3,0,1,1,1,1,1,3,1});
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,1,8,1,3,3,2,1,1,0,0,3,0,0,0,0,3,0,0});
			chunkArray.Add( new int[] {0,0,3,0,0,0,2,1,0,0,3,1,8,0,1,0,1,1,0,1,0,0,1,1,1});
			chunkArray.Add( new int[] {1,1,1,3,2,1,0,0,0,1,1,0,8,1,1,1,0,0,0,3,3,5,1,5,1});
			chunkArray.Add( new int[] {0,0,1,1,4,0,0,0,3,0,1,0,0,1,1,3,1,0,2,8,7,1,1,1,1});
			chunkArray.Add( new int[] {0,0,1,1,4,0,0,0,3,0,1,0,0,1,1,3,1,0,2,8,7,1,1,1,1});
			chunkArray.Add( new int[] {3,1,1,2,3,1,2,0,1,1,1,3,0,8,1,1,2,0,1,1,3,1,1,2,3});
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,8,1,3,0,0,0,1,1,4,0,0,2,7,3,3,1,1});
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,8,1,3,0,0,0,1,1,4,0,0,2,7,3,3,1,1});
			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,1,3,8,1,3,0,0,1,0,0,0,0,3,0,0});
			chunkArray.Add( new int[] {0,1,1,0,0,0,2,0,8,0,1,1,0,1,1,0,1,1,2,3,0,0,3,1,3});
			chunkArray.Add( new int[] {0,0,1,3,2,0,4,0,0,3,3,1,1,1,1,0,8,0,0,2,0,0,3,1,1});
			chunkArray.Add( new int[] {0,0,1,3,2,0,4,0,0,3,3,1,1,1,1,0,8,0,0,2,0,0,3,1,1});
			chunkArray.Add( new int[] {0,1,1,1,0,0,0,3,0,0,1,0,8,0,1,1,1,7,1,1,0,1,1,1,0});
			chunkArray.Add( new int[] {0,0,1,2,1,0,0,0,1,8,3,1,1,1,1,7,2,0,0,0,1,3,1,0,0});
			chunkArray.Add( new int[] {0,3,1,3,0,3,0,1,0,3,1,1,8,1,5,7,0,1,0,1,0,2,1,3,0});
			chunkArray.Add( new int[] {0,3,1,0,0,1,2,1,3,0,1,1,8,1,3,0,3,1,2,1,0,0,1,1,0});
			chunkArray.Add( new int[] {0,0,1,0,3,0,3,1,1,2,1,1,8,1,1,3,2,1,1,0,1,0,1,0,0});
			chunkArray.Add( new int[] {0,2,1,1,0,1,1,8,1,1,1,0,0,0,3,2,1,1,3,1,0,3,1,1,0});
			chunkArray.Add( new int[] {3,0,1,1,1,1,0,1,8,1,1,0,2,0,2,1,1,1,0,1,2,0,3,0,3});
			chunkArray.Add( new int[] {2,1,1,1,1,0,3,0,0,0,3,2,1,1,1,0,0,1,8,0,1,2,1,1,1});
			chunkArray.Add( new int[] {0,0,1,2,1,0,3,1,8,2,1,1,1,1,1,0,2,3,0,1,0,0,1,0,3});
			chunkArray.Add( new int[] {0,0,1,1,8,2,0,3,1,1,1,1,0,0,1,1,5,3,1,2,0,1,1,3,0});
//			chunkArray.Add( new int[] {});

		} else if (type == ChunkType.Exit)
		{
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,3,1,2,0,0,1,1,2,9,2,1,1,0,0,2,1,3,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {3,1,1,1,1,1,3,2,0,0,3,0,0,2,1,0,3,1,1,0,1,1,0,1,2,3,0,1,1,0,0,9,0,0,1,1,0,0,0,0,0,1,3,1,1,2,1,1,3});
//			chunkArray.Add( new int[] {0,1,1,1,3,2,0,2,1,2,0,1,1,0,3,3,0,0,0,1,0,1,0,0,0,9,2,1,1,0,0,0,2,3,0,1,0,4,1,1,0,0,0,0,0,1,3,0,0});
//			chunkArray.Add( new int[] {3,1,1,1,1,1,3,3,2,0,4,0,2,3,2,1,0,0,0,1,2,1,0,0,0,0,0,1,1,0,9,0,1,0,1,3,2,1,2,3,1,1,1,1,0,1,0,1,2});
//			chunkArray.Add( new int[] {1,0,0,1,0,0,4,1,0,3,1,3,0,1,1,1,2,1,2,1,3,3,0,0,0,0,0,1,2,1,1,3,1,2,1,0,9,0,1,0,3,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {4,3,0,1,1,1,1,3,2,0,0,0,1,0,0,1,0,7,1,2,1,1,1,0,4,1,9,3,0,1,0,7,1,2,1,3,1,0,0,0,5,0,2,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,2,9,0,0,0,1,1,1,0,0,0,1,3,0,1,1,2,1,0,0,0,0,0,3,1,0,4,3,0,0,2,3,0,1,3,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,1,1,0,1,4,1,1,3,0,0,1,0,1,0,2,1,1,3,0,1,0,3,0,2,1,1,0,0,1,0,9,0,1,0,0,1,1,0,0,3,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,4,2,1,3,1,3,1,1,1,0,1,0,1,0,1,1,0,2,9,2,0,1,0,1,1,0,1,1,0,4,1,3,0,3,1,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {1,1,0,1,1,1,0,1,9,0,0,0,1,0,1,0,0,0,0,2,0,1,0,0,5,3,1,1,2,1,1,1,2,3,0,0,1,0,3,3,1,4,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,3,0,0,0,0,0,0,1,0,0,0,9,0,0,1,3,0,1,1,1,1,2,2,1,0,0,0,0,1,3,0,1,1,2,1,3,0,0,4,3,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,2,1,2,0,0,0,3,1,0,1,3,0,1,2,3,0,3,2,1,1,1,0,9,0,1,1,0,1,0,1,0,1,0,0,3,1,1,1,3,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,4,0,0,3,1,1,7,1,0,0,1,3,0,1,3,0,1,1,2,9,2,1,1,0,3,1,0,3,1,0,0,1,7,1,1,3,0,0,4,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,0,1,1,1,0,1,1,1,3,2,3,1,1,1,0,1,1,1,0,1,1,0,2,9,2,0,1,0,0,1,1,1,0,0,0,4,3,2,3,1,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,1,1,3,0,3,0,4,2,0,2,1,2,0,3,1,0,1,9,1,1,2,1,0,2,1,2,0,3,1,1,3,0,3,0,1,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,2,1,1,2,1,3,1,4,0,1,0,3,2,0,0,0,1,0,0,9,0,0,1,0,0,0,2,0,0,1,0,3,1,7,1,3,1,1,1,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,3,1,1,1,3,0,1,3,2,1,2,3,1,1,0,0,1,0,0,1,2,0,1,9,1,0,2,1,0,0,1,0,0,1,1,3,2,1,2,3,1,0,1,3,1,3,1,0});
//			chunkArray.Add( new int[] {0,0,1,3,1,0,0,0,0,1,0,1,0,0,0,0,2,9,2,0,0,1,1,1,0,1,1,3,1,1,3,2,3,1,1,4,0,1,1,1,0,1,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,1,1,0,1,1,0,0,1,3,0,3,1,0,1,2,2,0,2,2,1,0,3,1,9,1,3,0,0,0,1,1,1,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,3,0,1,0,3,1,1,2,0,1,0,2,3,0,1,1,1,1,1,0,1,3,1,0,1,3,1,0,0,1,9,1,0,0,0,4,7,2,7,4,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,1,1,1,0,0,4,0,2,3,2,0,1,1,0,3,0,1,0,1,2,1,1,9,1,1,2,1,0,1,1,3,0,1,2,1,3,1,1,1,2});
//			chunkArray.Add( new int[] {0,3,0,1,0,3,0,0,2,1,1,1,2,0,4,1,0,9,0,1,1,1,1,0,0,0,1,1,1,3,0,0,0,3,1,1,3,0,0,0,3,1,2,1,1,1,1,1,2});
//			chunkArray.Add( new int[] {0,0,0,1,1,1,1,0,0,0,0,3,1,3,0,3,0,3,2,0,2,1,1,1,1,1,9,1,0,3,0,3,2,0,2,0,0,0,0,3,1,3,0,0,0,1,1,1,1});
//			chunkArray.Add( new int[] {4,3,1,1,0,0,1,3,2,1,0,0,0,1,0,3,2,1,1,2,3,1,1,0,9,1,0,1,0,3,2,1,1,2,3,3,2,1,0,0,0,1,4,3,1,1,0,0,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,2,0,0,1,0,0,1,1,0,7,7,7,0,3,1,0,1,9,1,0,1,1,0,4,1,1,0,1,1,0,0,0,0,0,2,3,1,2,1,1,1,1});
//			chunkArray.Add( new int[] {2,1,1,1,1,1,1,1,0,3,0,3,0,1,1,3,2,1,2,3,1,1,0,4,9,1,0,1,1,3,2,1,2,3,1,1,0,3,0,1,0,1,1,1,1,1,1,1,2});
//			chunkArray.Add( new int[] {1,1,1,1,1,2,1,3,0,0,0,0,0,1,1,2,1,9,1,2,1,1,3,1,2,1,3,1,2,0,4,0,1,0,3,1,0,0,0,0,0,2,1,2,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,2,1,3,2,0,3,0,0,0,1,1,0,1,3,0,0,1,1,0,2,1,4,0,1,1,0,9,2,1,3,1,1,0,0,0,0,0,2,3,1,2,1,3,1,1});
//			chunkArray.Add( new int[] {1,3,1,1,1,2,1,1,0,0,0,0,0,1,1,0,1,9,4,0,3,2,3,1,2,1,1,2,0,0,0,1,0,0,0,0,0,1,7,1,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,3,1,1,2,1,1,3,0,0,0,0,0,1,1,0,3,1,0,0,1,1,0,2,1,7,0,2,2,0,9,0,1,1,1,1,0,0,0,1,2,3,1,1,3,2,1,3,1});
//			chunkArray.Add( new int[] {1,2,1,1,1,2,1,3,0,0,0,0,0,1,1,1,3,2,3,1,1,1,0,1,2,1,0,1,1,0,0,9,0,0,1,2,0,0,0,0,0,2,3,1,1,1,1,1,3});
//			chunkArray.Add( new int[] {1,2,1,1,1,1,1,1,0,3,0,3,0,1,1,0,2,2,2,0,1,1,0,1,9,1,0,1,3,1,2,0,2,1,3,1,0,0,0,0,0,1,1,1,1,1,1,2,1});
//			chunkArray.Add( new int[] {2,1,1,3,1,1,1,1,0,1,1,1,0,2,1,0,0,1,0,0,1,1,0,3,2,4,0,1,1,0,0,9,0,0,1,3,2,0,0,0,2,3,1,3,1,1,1,3,1});
//			chunkArray.Add( new int[] {0,0,0,1,1,3,0,0,4,0,0,0,2,0,0,3,0,0,0,1,0,1,1,1,9,0,1,1,0,0,0,1,2,3,0,0,0,0,1,3,1,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,0,0,0,0,0,2,3,1,1,0,9,0,0,0,0,1,1,2,1,3,1,1,3,1,0,0,0,0,0,0,1,2,1,3,0,0,0,0,0,1,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,1,1,1,3,2,0,0,3,0,0,0,1,0,1,1,0,2,1,1,1,0,0,0,3,0,9,0,3,2,1,1,0,0,0,1,3,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,2,1,1,3,1,1,3,1,9,0,0,1,0,0,0,0,0,0,1,0,4,0,0,3,1,3,0,1,1,1,2,1,2});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,3,1,2,1,0,0,4,1,0,1,0,0,0,3,1,0,3,0,0,0,1,0,0,1,1,0,0,1,0,0,0,3,1,2,1,0,0,0,1,1,2,9});
//			chunkArray.Add( new int[] {4,1,0,1,0,0,0,1,3,2,1,1,3,0,0,2,9,0,0,1,0,1,1,0,0,0,1,1,1,0,0,0,1,1,0,0,1,0,3,1,2,3,0,1,1,1,0,1,4});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,3,1,1,1,2,9,0,1,2,0,1,0,0,0,1,0,0,1,2,0,1,0,0,0,0,1,1,1,0,0,0,0,3,0,0,0,1,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,2,9,2,0,0,0,0,1,0,1,0,0,1,1,3,0,3,1,1,0,1,0,0,0,1,0,3,1,0,1,0,2,3,1,2,1,1,0,1,4});
//			chunkArray.Add( new int[] {2,1,1,1,1,3,1,1,0,0,0,0,0,1,1,1,1,1,3,1,1,3,1,2,9,2,1,3,1,4,1,1,1,1,1,1,0,0,0,0,0,1,1,1,3,1,1,1,2});
//			chunkArray.Add( new int[] {1,7,1,1,1,7,1,1,0,1,2,3,0,1,1,0,1,1,1,0,1,2,0,1,9,1,0,2,1,0,1,1,1,0,1,1,0,3,2,1,0,1,1,7,1,1,1,7,1});
//			chunkArray.Add( new int[] {0,0,0,1,2,1,0,0,0,3,1,1,0,0,1,2,5,1,3,2,1,1,1,1,9,0,1,1,1,2,5,1,3,2,1,0,0,3,1,1,0,0,0,0,0,1,2,1,0});
//			chunkArray.Add( new int[] {1,1,2,1,1,1,3,1,1,3,0,3,5,1,2,3,1,0,4,1,2,1,0,0,0,0,0,1,1,1,4,0,1,1,1,1,5,3,0,1,9,1,3,1,2,1,2,1,1});
//			chunkArray.Add( new int[] {0,1,1,3,0,1,0,1,5,1,5,0,1,3,2,3,4,1,0,2,1,1,5,1,3,0,9,1,1,1,5,1,0,1,1,2,3,1,2,1,1,1,0,1,1,1,0,3,0});
//			chunkArray.Add( new int[] {3,1,2,1,1,1,1,1,0,0,0,0,0,2,1,0,9,1,1,1,1,1,0,1,1,3,2,1,1,0,1,3,1,1,1,2,0,2,1,0,0,1,1,3,1,1,1,1,3});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,2,0,1,3,4,0,2,1,0,1,2,1,0,1,1,2,3,9,1,2,1,1,2,1,2,3,2,1,2,0,0,0,0,0,2,3,1,1,1,1,1,3});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,1,1,1,0,0,1,3,2,4,2,3,1,1,1,1,0,1,1,1,1,3,2,1,2,3,1,0,0,1,9,1,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,1,1,1,1,1,0,3,5,2,1,2,1,1,1,1,7,0,7,1,1,1,1,0,0,0,1,1,1,1,7,0,7,5,1,1,1,2,9,2,1,3,0,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,2,1,3,3,3,1,2,1,1,0,1,0,1,1,1,1,1,0,9,1,1,5,1,0,4,0,1,1,3,5,3,3,3,1,1,1,2,2,1,2,2,1});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,1,1,1,0,0,1,1,2,1,2,1,1,1,3,2,9,2,3,1,1,1,3,0,3,1,1,1,0,0,0,0,0,1,1,2,1,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,1,1,3,0,0,3,1,2,0,1,1,1,2,1,1,0,1,2,3,1,4,3,0,9,1,1,2,1,1,0,1,2,1,3,1,2,0,1,3,1,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,2,1,1,0,0,0,4,3,3,0,0,0,0,0,1,1,2,0,1,3,0,0,9,1,1,1,1,0,2,1,3,0,3,1,2,1,3,0,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,0,0,0,0,3,1,1,1,1,0,1,2,3,1,2,3,0,1,1,0,0,3,2,1,1,0,0,0,1,1,1,0,0,0,1,3,2,1,0,9,1,1,1,1,0});
//			chunkArray.Add( new int[] {1,1,2,1,1,1,3,1,0,0,0,0,0,1,2,1,3,0,3,2,1,1,9,1,0,4,3,2,2,1,1,0,3,2,1,1,0,0,0,0,0,1,1,2,1,1,3,2,1});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,2,1,1,0,0,0,1,3,1,3,2,0,1,1,1,9,1,1,1,0,2,3,1,3,1,0,0,0,1,1,2,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {2,1,1,3,0,0,0,3,1,3,0,0,0,0,0,1,1,2,0,0,0,1,0,1,9,1,0,1,1,3,1,2,1,1,1,0,1,0,0,3,2,3,0,1,1,1,0,3,2});
//			chunkArray.Add( new int[] {1,1,0,1,0,0,0,1,3,1,1,1,1,0,0,2,7,1,1,3,1,1,1,0,0,0,1,1,3,1,2,1,3,2,0,0,1,1,1,1,1,1,0,0,0,1,0,1,9});
//			chunkArray.Add( new int[] {3,1,1,1,1,2,1,2,2,0,0,1,2,3,2,3,0,0,0,3,1,1,3,0,0,0,0,1,0,0,0,3,1,1,1,0,0,1,2,9,1,3,0,1,1,1,1,1,2});
//			chunkArray.Add( new int[] {1,2,3,1,1,1,1,9,2,1,3,1,1,1,0,0,0,1,0,0,0,1,0,2,1,1,0,1,1,1,1,0,1,1,1,2,2,1,1,1,2,2,3,3,0,1,0,3,3});

			// 5x5
			chunkArray.Add( new int[] {0,0,3,0,0,0,9,1,2,0,3,5,2,3,1,0,2,3,1,0,0,0,1,0,0});
			chunkArray.Add( new int[] {1,5,3,0,0,1,2,1,0,0,5,9,2,5,1,3,1,1,0,0,4,5,7,0,0});
			chunkArray.Add( new int[] {1,5,3,0,0,1,2,1,0,0,5,9,2,5,1,3,1,1,0,0,4,5,7,0,0});
			chunkArray.Add( new int[] {0,1,1,1,0,0,2,9,2,0,3,1,1,1,3,0,3,0,1,0,0,1,1,3,0});
			chunkArray.Add( new int[] {3,7,1,1,3,1,4,0,1,1,1,0,0,0,2,1,3,0,9,1,3,2,1,1,1});
			chunkArray.Add( new int[] {3,7,1,1,3,1,4,0,1,1,1,0,0,0,2,1,3,0,9,1,3,2,1,1,1});
			chunkArray.Add( new int[] {3,2,1,1,1,1,1,9,2,3,1,2,0,1,1,1,3,0,1,1,1,1,1,3,1});
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,1,9,1,3,3,2,1,1,0,0,3,0,0,0,0,3,0,0});
			chunkArray.Add( new int[] {0,0,3,0,0,0,2,1,0,0,3,1,9,0,1,0,1,1,0,1,0,0,1,1,1});
			chunkArray.Add( new int[] {1,1,1,3,2,1,0,0,0,1,1,0,9,1,1,1,0,0,0,3,3,5,1,5,1});
			chunkArray.Add( new int[] {0,0,1,1,4,0,0,0,3,0,1,0,0,1,1,3,1,0,2,9,7,1,1,1,1});
			chunkArray.Add( new int[] {0,0,1,1,4,0,0,0,3,0,1,0,0,1,1,3,1,0,2,9,7,1,1,1,1});
			chunkArray.Add( new int[] {3,1,1,2,3,1,2,0,1,1,1,3,0,9,1,1,2,0,1,1,3,1,1,2,3});
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,9,1,3,0,0,0,1,1,4,0,0,2,7,3,3,1,1});
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,9,1,3,0,0,0,1,1,4,0,0,2,7,3,3,1,1});
			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,1,3,9,1,3,0,0,1,0,0,0,0,3,0,0});
			chunkArray.Add( new int[] {0,1,1,0,0,0,2,0,9,0,1,1,0,1,1,0,1,1,2,3,0,0,3,1,3});
			chunkArray.Add( new int[] {0,0,1,3,2,0,4,0,0,3,3,1,1,1,1,0,9,0,0,2,0,0,3,1,1});
			chunkArray.Add( new int[] {0,0,1,3,2,0,4,0,0,3,3,1,1,1,1,0,9,0,0,2,0,0,3,1,1});
			chunkArray.Add( new int[] {0,1,1,1,0,0,0,3,0,0,1,0,9,0,1,1,1,7,1,1,0,1,1,1,0});
			chunkArray.Add( new int[] {0,0,1,2,1,0,0,0,1,9,3,1,1,1,1,7,2,0,0,0,1,3,1,0,0});
			chunkArray.Add( new int[] {0,3,1,3,0,3,0,1,0,3,1,1,9,1,5,7,0,1,0,1,0,2,1,3,0});
			chunkArray.Add( new int[] {0,3,1,0,0,1,2,1,3,0,1,1,9,1,3,0,3,1,2,1,0,0,1,1,0});
			chunkArray.Add( new int[] {0,0,1,0,3,0,3,1,1,2,1,1,9,1,1,3,2,1,1,0,1,0,1,0,0});
			chunkArray.Add( new int[] {0,2,1,1,0,1,1,9,1,1,1,0,0,0,3,2,1,1,3,1,0,3,1,1,0});
			chunkArray.Add( new int[] {3,0,1,1,1,1,0,1,9,1,1,0,2,0,2,1,1,1,0,1,2,0,3,0,3});
			chunkArray.Add( new int[] {2,1,1,1,1,0,3,0,0,0,3,2,1,1,1,0,0,1,9,0,1,2,1,1,1});
			chunkArray.Add( new int[] {0,0,1,2,1,0,3,1,9,2,1,1,1,1,1,0,2,3,0,1,0,0,1,0,3});
			chunkArray.Add( new int[] {0,0,1,1,9,2,0,3,1,1,1,1,0,0,1,1,5,3,1,2,0,1,1,3,0});

		} else if (type == ChunkType.Boss)
		{
//			chunkArray.Add( new int[] {0,0,0,9,1,9,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,1,1,0,0,0,4,0,1,3,10,3,1,0,4,2,1,3,1,2,1,3,1,2,4,0,1,1,1,1,1,0,4,0,0,0,0,2,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,8,1,0,0,0});		
//			chunkArray.Add( new int[] {0,3,1,9,1,9,1,3,0,0,1,0,0,1,0,0,1,0,0,1,0,1,1,1,0,1,0,4,1,1,3,10,3,1,1,4,2,1,7,3,2,3,7,1,2,4,3,1,1,0,1,1,3,4,0,0,2,0,0,0,2,0,0,0,1,1,1,1,1,1,1,0,0,0,0,1,8,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,9,1,9,0,0,0,0,0,4,1,10,3,1,0,0,0,0,1,0,1,0,1,0,0,4,0,3,1,2,1,3,0,1,1,1,2,1,8,1,2,1,1,1,0,3,1,2,1,3,0,4,0,0,1,0,1,0,1,0,0,0,0,1,3,1,1,4,0,0,0,0,0,9,1,9,0,0,0});

			//exits removed
			chunkArray.Add( new int[] {0,0,0,1,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,1,1,0,0,0,4,0,1,3,10,3,1,0,4,2,1,3,1,2,1,3,1,2,4,0,1,1,1,1,1,0,4,0,0,0,0,2,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,8,1,0,0,0});		
			chunkArray.Add( new int[] {0,3,1,1,1,1,1,3,0,0,1,0,0,1,0,0,1,0,0,1,0,1,1,1,0,1,0,4,1,1,3,10,3,1,1,4,2,1,7,3,2,3,7,1,2,4,3,1,1,0,1,1,3,4,0,0,2,0,0,0,2,0,0,0,1,1,1,1,1,1,1,0,0,0,0,1,8,1,0,0,0});
			chunkArray.Add( new int[] {0,0,0,1,1,1,0,0,0,0,0,4,1,10,3,1,0,0,0,0,1,0,1,0,1,0,0,4,0,3,1,2,1,3,0,1,1,1,2,1,8,1,2,1,1,1,0,3,1,2,1,3,0,4,0,0,1,0,1,0,1,0,0,0,0,1,3,1,1,4,0,0,0,0,0,1,1,1,0,0,0});
		}  else if (type == ChunkType.Single)
		{
			// 5x5 chunks
//			chunkArray.Add( new int[] {0,0,3,0,0,0,9,1,2,0,3,5,2,3,1,0,1,1,4,0,0,0,8,0,0});
			chunkArray.Add( new int[] {0,0,3,0,0,0,9,1,2,0,3,5,2,3,1,0,2,3,1,0,0,0,8,0,0});
			chunkArray.Add( new int[] {8,5,3,0,0,1,2,1,0,0,5,1,2,5,1,3,1,9,0,0,1,5,7,0,0});
			chunkArray.Add( new int[] {0,0,1,2,1,0,3,1,9,2,1,1,1,1,1,0,2,3,0,1,0,0,8,0,3});
			chunkArray.Add( new int[] {0,0,3,1,9,0,0,0,1,0,8,0,0,1,3,1,1,0,1,1,3,1,2,3,2});
			chunkArray.Add( new int[] {0,0,1,1,8,0,0,0,1,0,9,0,0,1,1,3,1,0,2,2,7,1,1,3,3});
			chunkArray.Add( new int[] {0,0,3,1,2,0,0,0,1,0,9,0,0,3,7,1,1,0,1,1,1,3,1,2,8});
			chunkArray.Add( new int[] {0,0,1,1,9,0,0,0,7,0,3,0,0,1,1,1,1,0,1,1,7,1,1,3,8});
			chunkArray.Add( new int[] {0,0,1,1,9,0,1,7,3,1,3,1,2,1,3,0,1,1,0,1,0,0,8,0,1});
			chunkArray.Add( new int[] {0,0,1,1,1,0,7,1,3,1,9,1,1,1,2,0,3,7,0,1,0,0,1,0,8});
			chunkArray.Add( new int[] {0,0,1,1,3,0,1,2,1,1,1,1,1,2,1,0,8,1,0,1,0,0,3,0,9});
			chunkArray.Add( new int[] {0,0,1,0,0,0,3,1,2,0,1,7,1,1,8,0,3,1,1,0,0,0,9,0,0});
			chunkArray.Add( new int[] {0,0,1,0,0,0,1,3,1,0,2,1,8,1,3,0,1,7,1,0,0,0,9,0,0});
			chunkArray.Add( new int[] {0,0,1,0,0,0,2,3,3,0,9,1,1,1,1,0,3,1,2,0,0,0,8,0,0});
			chunkArray.Add( new int[] {1,1,1,3,1,1,9,0,1,1,1,0,0,0,2,1,3,0,1,1,3,1,2,1,8});
			chunkArray.Add( new int[] {1,1,1,2,3,1,1,0,3,2,1,0,0,0,1,1,8,0,9,3,3,1,7,1,1});
			chunkArray.Add( new int[] {9,7,1,1,1,7,2,0,3,1,1,0,0,0,1,1,3,0,2,1,1,1,8,1,1});
			chunkArray.Add( new int[] {0,9,1,1,0,0,3,1,3,0,1,2,1,2,3,0,1,0,1,0,0,1,8,1,0});
			chunkArray.Add( new int[] {0,9,1,3,0,0,1,3,2,0,1,1,3,2,1,0,1,0,1,0,0,1,8,1,0});
			chunkArray.Add( new int[] {0,2,1,8,0,0,1,1,1,0,9,1,3,1,1,0,1,0,1,0,0,3,1,2,0});
			chunkArray.Add( new int[] {0,7,3,3,0,0,1,1,1,0,8,1,1,2,1,0,1,0,1,0,0,1,1,9,0});
			chunkArray.Add( new int[] {0,0,9,3,1,0,0,0,2,1,1,3,1,1,1,1,7,0,0,0,1,1,8,0,0});
			chunkArray.Add( new int[] {0,0,1,1,9,0,0,0,7,1,1,2,3,1,1,1,1,0,0,0,1,1,8,0,0});
			chunkArray.Add( new int[] {0,0,9,1,1,0,0,0,7,3,1,1,1,1,2,1,1,0,0,0,8,1,3,0,0});
			chunkArray.Add( new int[] {0,1,9,1,0,0,7,1,7,0,0,1,0,1,0,0,1,1,1,0,0,1,8,1,0});
			chunkArray.Add( new int[] {0,1,9,1,0,0,1,7,1,0,0,2,0,3,0,0,1,1,1,0,0,1,8,1,0});
			chunkArray.Add( new int[] {0,0,9,0,0,0,3,1,2,0,1,2,1,1,3,0,1,1,1,0,0,0,8,0,0});
			chunkArray.Add( new int[] {0,0,9,0,0,0,2,1,1,0,7,1,1,1,7,0,1,1,1,0,0,0,8,0,0});
			chunkArray.Add( new int[] {0,0,1,0,0,0,2,3,1,0,1,1,1,3,9,0,1,1,2,0,0,0,8,0,0});
			chunkArray.Add( new int[] {0,0,9,0,0,0,3,1,3,0,1,1,1,1,1,0,2,2,2,0,0,0,8,0,0});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
		}
		else if (type == ChunkType.SingleMed)
		{
			// 7x7 chunks
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,3,1,2,0,0,1,1,2,8,2,1,1,0,0,2,1,3,0,0,0,0,0,1,0,0,0,0,0,0,9,0,0,0});
//			chunkArray.Add( new int[] {3,1,9,1,1,1,3,2,0,0,3,0,0,2,1,0,3,1,1,0,1,1,0,1,2,3,0,1,1,0,0,8,0,0,1,1,0,0,0,0,0,1,3,1,1,2,1,1,3});
//			chunkArray.Add( new int[] {0,9,1,1,3,2,0,2,1,2,0,1,1,0,3,3,0,0,0,1,0,1,0,0,0,8,2,1,1,0,0,0,2,3,0,1,0,4,1,1,0,0,0,0,0,1,3,0,0});
			chunkArray.Add( new int[] {0,9,1,3,0,0,0,0,0,1,2,1,0,0,0,2,3,1,1,3,0,0,1,1,0,3,1,0,0,1,2,1,2,1,0,0,0,1,1,1,0,0,0,0,0,1,1,8,0});
			chunkArray.Add( new int[] {0,1,1,1,0,0,0,0,0,2,1,1,0,0,0,9,1,1,3,1,0,0,1,1,0,1,1,0,0,7,5,1,2,3,0,0,0,1,1,1,0,0,0,0,0,1,1,8,0});
			chunkArray.Add( new int[] {0,1,3,1,0,0,0,0,0,1,1,9,0,0,0,1,2,1,3,1,0,0,1,1,0,1,1,0,0,1,1,1,1,1,0,0,0,8,1,2,0,0,0,0,0,1,3,1,0});
			chunkArray.Add( new int[] {9,1,3,0,1,1,0,0,3,2,1,1,3,1,0,0,0,0,3,7,1,0,0,0,0,0,1,1,0,0,0,1,1,1,2,0,1,1,7,1,0,0,0,8,1,0,0,0,0});
			chunkArray.Add( new int[] {1,3,9,0,1,4,0,0,1,1,1,7,5,1,0,0,0,0,1,3,1,0,0,0,0,0,3,1,0,0,0,1,1,2,1,0,1,1,1,1,0,0,0,8,1,0,0,0,0});
			chunkArray.Add( new int[] {1,1,3,1,1,1,9,1,0,1,1,3,0,1,3,1,2,1,1,1,3,1,0,1,0,2,0,1,1,0,1,1,1,0,1,0,0,1,1,1,0,0,0,0,0,8,0,0,0});
			chunkArray.Add( new int[] {2,1,3,9,3,1,1,1,0,1,1,1,0,1,1,1,1,1,1,1,1,1,0,7,0,7,0,1,4,0,1,1,1,0,3,0,0,1,1,1,0,0,0,0,0,8,0,0,0});
			chunkArray.Add( new int[] {3,1,1,1,1,1,1,1,0,1,1,1,0,1,1,1,1,7,1,1,3,7,0,5,0,5,0,1,9,0,1,2,1,0,1,0,0,1,1,1,0,0,0,0,0,8,0,0,0});
			chunkArray.Add( new int[] {0,0,1,1,0,0,0,0,1,3,2,1,1,0,7,1,1,1,3,1,0,5,1,0,0,2,1,3,3,0,0,0,1,1,1,9,0,0,2,1,1,2,0,8,1,1,1,3,0});
			chunkArray.Add( new int[] {0,0,1,1,0,0,0,0,1,3,1,2,4,0,1,1,1,3,2,1,0,1,2,0,0,1,3,1,1,0,0,0,1,1,1,9,0,0,1,1,1,1,0,8,1,1,1,2,0});
			chunkArray.Add( new int[] {0,0,1,1,0,0,0,0,1,3,2,1,1,0,1,1,1,1,1,3,0,2,1,0,0,5,1,1,1,0,0,0,1,1,2,9,0,0,1,3,1,1,0,8,1,1,2,1,0});
			chunkArray.Add( new int[] {0,0,1,1,9,0,0,0,1,2,3,0,0,0,0,0,1,1,1,0,4,3,0,0,3,1,1,1,1,0,1,2,1,0,3,1,1,1,1,0,0,0,0,0,1,1,8,0,0});
			chunkArray.Add( new int[] {0,0,1,3,1,0,0,0,1,1,1,0,0,0,0,0,1,1,1,0,1,8,0,0,7,2,1,3,1,0,1,1,7,0,9,1,1,1,1,0,0,0,0,0,1,1,1,0,0});
			chunkArray.Add( new int[] {0,0,3,1,9,0,0,0,1,2,1,0,0,0,0,0,1,1,1,0,1,8,0,0,3,1,1,2,1,0,1,1,1,0,1,1,1,2,1,0,0,0,0,0,1,1,3,0,0});
			chunkArray.Add( new int[] {0,0,1,1,9,0,0,0,1,1,3,0,0,0,0,0,1,5,1,0,1,8,0,0,7,1,1,3,1,0,1,1,1,0,1,1,1,1,1,0,0,0,0,0,2,1,1,0,0});
		}
		else if (type == ChunkType.SingleLarge)
		{
			// 9x9 chunks
			chunkArray.Add( new int[] {0,0,0,9,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,1,1,0,0,0,4,0,1,3,10,3,1,0,4,2,1,3,1,2,1,3,1,2,4,0,1,1,1,1,1,0,4,0,0,0,0,2,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,8,1,0,0,0});		
			chunkArray.Add( new int[] {0,3,1,9,1,1,1,3,0,0,1,0,0,1,0,0,1,0,0,1,0,1,1,1,0,1,0,4,1,1,3,10,3,1,1,4,2,1,7,3,2,3,7,1,2,4,3,1,1,0,1,1,3,4,0,0,2,0,0,0,2,0,0,0,1,1,1,1,1,1,1,0,0,0,0,1,8,1,0,0,0});
			chunkArray.Add( new int[] {0,0,0,9,1,1,0,0,0,0,0,4,1,10,3,1,0,0,0,0,1,0,1,0,1,0,0,4,0,3,1,2,1,3,0,1,1,1,2,1,8,1,2,1,1,1,0,3,1,2,1,3,0,4,0,0,1,0,1,0,1,0,0,0,0,1,3,1,1,4,0,0,0,0,0,1,1,1,0,0,0});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});
//			chunkArray.Add( new int[] {});

		}
		else 
		{
			// 000
//			chunkArray.Add( new int[] {1,2,1,1,1,2,1,1,0,0,3,0,0,1,2,0,7,1,7,0,1,1,0,2,7,2,0,1,1,0,0,4,0,0,2,3,0,0,0,0,0,3,1,2,1,1,1,2,1});
//			chunkArray.Add( new int[] {1,1,3,1,3,1,1,2,0,0,1,0,0,2,1,0,3,5,3,0,1,1,0,4,1,4,0,1,1,0,0,3,0,0,1,3,0,0,0,0,0,3,1,1,1,3,1,1,1});
//			chunkArray.Add( new int[] {1,1,2,1,2,1,1,3,0,0,1,0,0,3,1,0,5,1,5,0,1,1,0,7,2,7,0,1,1,0,0,3,0,0,1,3,0,0,0,0,0,3,1,1,2,1,2,1,1});
//			
//			// 001
//			chunkArray.Add( new int[] {0,1,1,1,3,4,0,3,1,1,0,2,3,0,1,1,0,0,0,1,0,1,0,0,0,1,2,1,2,0,0,0,1,1,0,4,0,2,1,1,0,0,0,0,0,1,1,0,0});
//			chunkArray.Add( new int[] {0,2,3,1,1,1,0,3,3,1,0,1,3,0,2,2,0,0,0,5,0,1,0,0,0,2,1,1,1,0,0,0,1,1,0,3,0,4,1,3,0,0,0,0,0,1,3,0,0});
//			chunkArray.Add( new int[] {0,1,3,1,5,1,0,2,3,4,0,1,1,0,1,1,0,0,0,2,0,1,0,0,0,3,1,1,1,0,0,0,2,3,0,2,0,1,3,1,0,0,0,0,0,1,1,0,0});
//			
//			// 002
//			chunkArray.Add( new int[] {1,2,3,2,3,1,1,3,5,0,4,0,2,5,3,1,0,0,0,3,3,1,0,0,0,0,0,1,1,0,4,0,3,0,1,2,1,3,1,2,3,1,1,1,0,1,0,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,2,3,7,2,0,1,0,3,4,1,3,0,0,0,2,3,1,0,0,0,0,0,1,2,0,3,0,3,0,1,1,1,1,1,1,1,1,4,1,0,1,0,1,3});
//			chunkArray.Add( new int[] {1,3,2,1,1,3,1,1,3,0,1,0,3,1,5,1,0,0,0,2,1,1,0,0,0,0,0,1,2,0,1,0,1,0,1,1,7,1,1,1,7,2,4,1,0,1,0,1,5});
//			
//			// 003
//			chunkArray.Add( new int[] {4,0,0,1,0,0,1,5,0,3,1,3,0,2,1,1,2,2,2,1,1,1,0,0,0,0,0,3,1,1,1,1,2,1,1,0,1,0,1,0,4,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {4,0,0,1,0,0,3,1,0,1,1,1,0,1,3,2,1,3,1,2,3,1,0,0,0,0,0,1,1,1,2,1,2,1,1,0,1,0,1,0,3,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {3,0,0,1,0,0,3,1,0,3,1,4,0,1,1,1,2,5,2,1,1,2,0,0,0,0,0,2,1,1,1,1,1,1,1,0,4,0,1,0,1,0,0,0,0,1,0,0,0});
//			
//			// 004
//			chunkArray.Add( new int[] {0,0,1,1,2,1,0,0,0,0,0,3,2,0,1,0,0,0,0,3,1,1,1,0,0,0,0,1,3,2,0,0,0,0,1,0,2,1,3,0,1,1,0,0,3,1,1,1,0});
//			chunkArray.Add( new int[] {0,0,1,1,2,3,0,0,0,0,0,5,3,0,3,0,0,0,0,1,1,1,5,0,0,0,0,1,1,7,0,0,0,0,1,0,1,1,4,0,2,2,0,0,1,1,1,1,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,2,0,0,0,0,0,1,1,0,2,0,0,0,0,3,1,1,1,0,0,0,0,1,1,1,0,0,0,0,1,0,1,1,3,0,1,3,0,0,2,3,1,2,0});
//			
//			// 005
//			chunkArray.Add( new int[] {1,3,0,4,1,1,1,2,1,0,0,0,1,0,0,1,0,1,3,2,1,1,1,0,3,5,2,1,0,1,0,1,1,2,3,3,2,0,0,0,1,0,1,3,1,1,1,1,0});
//			chunkArray.Add( new int[] {4,2,0,1,1,1,1,1,3,0,0,0,3,0,0,3,0,1,1,1,1,1,2,0,1,2,2,1,0,3,0,3,1,1,1,3,2,0,0,0,1,0,1,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {3,2,0,1,1,3,1,1,1,0,0,0,1,0,0,1,0,4,3,2,1,1,1,0,1,1,1,5,0,1,0,4,3,2,1,1,1,0,0,0,1,0,2,1,1,3,1,1,0});
//			
//			// 006
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,4,0,0,0,2,1,1,0,0,0,1,1,0,1,1,2,7,0,0,0,0,0,5,3,0,4,1,0,0,3,5,0,3,1,1,1,2,2});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,3,1,0,0,0,1,3,2,0,0,0,1,1,0,3,1,1,1,0,0,0,0,0,2,1,0,2,1,0,0,1,1,0,1,1,1,1,3,1});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,3,0,0,0,2,1,1,0,0,0,1,1,0,1,1,1,2,0,0,0,0,0,5,4,0,7,3,0,0,1,3,0,1,1,1,1,3,2});
//			
//			// 007
//			chunkArray.Add( new int[] {0,0,3,1,0,1,3,2,1,1,0,0,1,0,1,0,1,2,5,3,0,1,0,1,0,3,2,1,0,0,1,0,4,0,1,0,0,1,2,0,0,1,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,0,1,4,2,1,3,0,0,1,0,1,0,1,1,7,3,0,1,0,1,0,1,1,1,0,0,1,0,2,0,1,0,0,2,1,0,0,3,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,0,1,1,2,1,3,0,0,1,0,1,0,5,3,1,2,0,1,0,3,0,1,1,1,0,0,1,0,3,0,1,0,0,2,1,0,0,4,0,0,0,1,0,0,0});
//			
//			// 008
//			chunkArray.Add( new int[] {0,0,0,1,0,0,1,1,1,1,7,1,1,2,1,0,1,0,1,0,1,1,0,3,4,3,0,1,0,5,1,0,1,3,0,1,3,2,0,2,3,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,4,3,1,1,1,1,1,3,1,0,1,0,1,0,1,1,0,2,2,1,0,1,0,1,1,0,1,3,0,4,1,1,0,3,2,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,1,2,3,1,1,1,3,1,1,0,1,0,1,0,1,1,0,1,3,1,0,1,0,1,7,0,7,1,0,1,1,7,0,7,1,0,0,0,1,1,5,0,0});
//			
//			// 009
//			chunkArray.Add( new int[] {3,4,0,1,1,1,0,2,3,0,0,0,1,0,1,0,0,0,0,1,0,1,0,0,7,1,2,1,1,1,1,3,4,1,0,0,1,0,2,3,1,3,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,1,0,1,1,1,0,3,1,0,0,0,2,0,1,0,0,0,0,1,0,1,0,0,3,1,1,3,2,1,1,5,2,5,0,0,1,0,1,3,1,1,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,1,0,1,1,2,0,1,4,0,0,0,1,0,1,0,0,0,0,3,0,1,0,0,3,1,1,1,2,3,1,3,1,2,0,0,1,0,1,3,1,1,0,0,0,1,0,0,0});
//			
//			// 010
//			chunkArray.Add( new int[] {2,1,1,1,0,3,2,1,4,0,0,3,1,1,1,0,1,1,2,0,2,1,0,1,0,1,0,1,1,0,1,1,1,0,2,3,1,3,0,0,5,3,3,1,0,1,1,3,2});
//			chunkArray.Add( new int[] {3,1,1,1,0,1,4,1,2,0,0,1,1,1,1,0,7,2,1,0,1,1,0,3,0,3,0,1,1,0,1,2,7,0,1,1,2,1,0,0,3,1,4,3,0,1,1,3,2});
//			chunkArray.Add( new int[] {1,3,1,1,0,1,1,3,5,0,0,2,1,1,1,0,1,1,1,0,1,1,0,3,0,3,0,1,1,0,1,1,1,0,1,2,1,2,0,0,3,1,1,1,0,1,1,1,3});
//			
//			// 011
//			chunkArray.Add( new int[] {0,0,0,1,1,0,0,0,0,0,0,1,0,0,0,4,0,0,2,3,0,1,1,1,2,3,5,1,0,0,0,0,2,3,0,3,2,1,1,1,0,0,5,7,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,0,0,0,0,0,0,1,0,0,0,1,0,0,3,3,0,1,1,1,1,1,1,1,0,0,0,0,7,2,0,1,1,3,1,2,0,0,1,3,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,0,0,0,0,0,0,1,0,0,0,1,0,0,3,4,0,1,1,1,1,7,1,1,0,0,0,0,3,4,0,2,1,1,1,1,0,0,7,3,0,1,0,0,0});
//			
//			// 012
//			chunkArray.Add( new int[] {0,0,0,1,1,1,1,0,0,1,3,0,0,1,3,1,2,1,0,3,3,1,1,0,0,0,2,2,1,1,0,1,1,1,3,0,0,0,1,2,0,0,0,4,1,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,1,1,0,0,3,3,0,0,1,3,1,1,1,0,3,2,2,2,0,0,0,5,3,3,1,0,4,1,2,1,0,0,0,1,1,0,0,0,3,1,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,1,2,0,0,3,2,0,0,1,1,2,1,7,0,3,1,1,1,0,0,0,1,1,1,3,0,3,3,1,1,0,0,0,1,2,0,0,0,3,1,1,0,0,0});
//			
//			// 013
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,1,1,1,0,0,0,7,3,0,3,7,0,3,5,1,0,1,5,3,1,1,0,4,0,1,1,0,2,0,1,0,2,0,0,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,3,2,3,0,0,0,1,1,0,1,1,0,2,1,4,0,4,1,2,3,1,0,3,0,1,3,0,3,0,1,0,3,0,0,1,1,2,1,1,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,2,1,1,0,0,0,4,1,0,1,3,0,3,2,1,0,2,1,7,1,1,0,1,0,1,3,0,3,0,3,0,1,0,0,1,1,1,1,1,0});
//			
//			// 014
//			chunkArray.Add( new int[] {0,0,0,1,0,3,0,0,5,1,7,1,1,0,0,1,1,0,1,1,0,1,1,3,4,3,1,1,0,1,1,0,5,1,0,0,3,2,1,2,3,0,0,1,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,1,0,0,2,1,1,7,1,0,0,3,3,0,1,2,0,1,1,1,3,1,1,1,0,1,3,0,2,3,0,0,2,1,1,1,4,0,0,1,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,1,0,0,1,3,1,2,3,0,0,1,1,0,1,1,0,1,3,1,2,1,1,1,0,1,1,0,5,3,0,0,1,2,1,3,1,0,0,4,0,1,0,0,0});
//			
//			// 015
//			chunkArray.Add( new int[] {1,0,2,1,2,0,4,1,1,3,1,1,1,3,1,0,1,1,1,0,1,3,0,1,1,3,0,1,0,0,3,1,1,0,0,0,1,2,2,7,1,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {1,0,2,1,1,0,1,1,3,1,5,2,3,1,1,0,1,3,1,0,1,1,0,1,3,1,0,1,0,0,1,1,1,0,0,0,4,3,5,3,4,0,0,0,2,1,2,0,0});
//			chunkArray.Add( new int[] {3,0,1,1,1,0,7,1,1,1,2,1,1,1,1,0,1,1,1,0,1,7,0,1,7,1,0,3,0,0,1,1,1,0,0,0,3,1,2,1,3,0,0,0,1,1,1,0,0});
//			
//			// 016
//			chunkArray.Add( new int[] {0,0,1,1,3,0,0,0,0,1,4,1,0,0,1,0,3,1,2,0,1,1,1,1,0,1,1,1,3,0,2,1,3,0,1,0,0,1,1,1,0,0,0,0,3,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,3,1,3,0,0,0,0,3,1,3,0,0,1,0,1,1,1,0,4,1,1,2,0,2,1,1,4,0,1,1,1,0,1,0,0,3,1,3,0,0,0,0,7,1,7,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,3,1,1,0,0,1,0,1,1,3,0,1,1,1,2,0,2,1,1,3,0,1,1,1,0,1,0,0,3,7,3,0,0,0,0,1,1,1,0,0});
//			
//			// 017
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,1,1,3,0,3,0,1,1,0,1,1,1,0,1,1,0,7,4,2,1,1,1,0,1,1,1,0,1,1,1,3,0,3,0,1,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,3,1,0,0,2,1,1,0,1,0,1,1,0,3,1,3,0,1,1,0,1,5,1,1,1,1,0,3,1,2,0,1,2,1,1,0,1,0,1,0,0,1,3,1,0,0});
//			chunkArray.Add( new int[] {0,0,3,1,1,0,0,2,3,2,0,1,0,1,1,0,1,1,1,0,1,1,0,3,1,1,2,2,1,0,1,1,3,0,1,2,3,2,0,1,0,1,0,0,3,1,1,0,0});
//			
//			// 018
//			chunkArray.Add( new int[] {0,0,1,1,1,3,1,1,1,2,1,3,1,0,1,0,3,1,0,0,0,1,0,0,1,0,0,1,0,0,0,1,0,0,1,0,3,1,1,3,2,1,3,2,1,1,3,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,3,4,1,3,1,7,1,1,0,1,0,3,1,0,0,0,1,0,0,2,0,0,1,0,0,0,1,0,0,1,0,3,1,7,1,3,1,4,1,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,2,1,1,3,1,1,1,3,1,1,1,0,1,0,2,3,0,0,0,1,0,0,1,0,0,1,0,0,0,1,0,0,1,0,1,2,3,2,1,1,1,3,3,1,1,0,0});
//			
//			// 019
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,3,1,3,0,0,0,3,2,0,2,1,0,1,1,3,0,3,1,1,0,1,2,0,2,3,0,0,0,3,1,3,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,3,1,1,0,0,0,0,1,1,2,0,0,0,2,3,0,3,1,0,1,1,5,0,4,2,1,0,3,1,0,1,3,0,0,0,2,1,1,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,1,1,7,0,0,0,4,1,0,1,1,0,1,3,1,0,1,3,1,0,1,1,0,1,2,0,0,0,7,1,1,0,0,0,0,1,1,1,0,0});
//			
//			// 020
//			chunkArray.Add( new int[] {0,1,1,1,1,1,0,2,3,1,3,1,1,2,1,0,0,1,0,0,1,3,0,4,1,3,0,3,1,0,0,1,0,0,1,2,1,1,3,1,3,2,0,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {0,7,1,1,1,7,0,1,3,1,1,1,1,1,2,0,0,1,0,0,1,1,0,1,3,4,0,1,1,0,0,1,0,0,1,1,1,1,1,1,3,1,0,7,1,1,1,7,0});
//			chunkArray.Add( new int[] {0,2,5,1,1,1,0,1,2,3,1,3,1,1,1,0,0,1,0,0,3,1,0,3,1,2,0,1,3,0,0,1,0,0,1,1,1,2,3,5,1,1,0,1,1,1,1,1,0});
//			
//			// 021
//			chunkArray.Add( new int[] {0,0,1,3,1,0,0,0,0,2,0,2,0,0,0,0,3,4,3,0,0,1,1,1,0,1,1,1,3,3,2,1,2,3,3,1,0,1,1,1,0,1,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,3,0,1,0,0,0,0,2,3,1,0,0,1,3,1,0,3,3,1,3,2,1,5,1,2,3,4,0,1,1,1,0,4,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,1,0,1,0,0,0,0,1,3,2,0,0,3,2,1,0,1,2,3,1,3,1,4,1,3,1,1,0,1,1,1,0,1,0,0,0,1,0,0,0});
//			
//			// 022
//			chunkArray.Add( new int[] {0,0,1,1,2,0,0,0,3,1,0,1,3,0,0,1,1,0,1,1,0,1,7,1,0,1,7,1,0,1,3,5,3,1,0,0,0,2,1,1,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,2,3,0,3,2,0,0,1,1,0,1,1,0,1,1,3,0,3,1,1,0,7,1,4,1,7,0,0,0,1,1,1,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,1,1,0,1,1,0,0,2,2,0,3,3,0,1,1,1,0,1,1,1,0,3,3,1,2,2,0,0,0,1,1,1,0,0,0,0,0,1,0,0,0});
//			
//			// 023
//			chunkArray.Add( new int[] {4,3,0,1,0,3,4,3,5,0,1,0,5,3,0,2,1,1,1,2,0,1,1,1,0,1,1,1,0,0,3,1,3,0,0,0,1,2,1,2,1,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,2,0,1,0,1,2,3,1,0,1,0,1,3,0,3,2,1,2,3,0,1,1,3,0,3,1,1,0,0,2,1,2,0,0,0,1,1,1,1,1,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,1,0,1,0,1,1,1,3,0,3,0,3,1,0,2,1,1,1,2,0,1,1,1,0,1,1,1,0,0,1,1,3,0,0,0,3,2,1,2,3,0,0,0,0,1,0,0,0});
//			
//			// 024
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,1,1,1,0,0,1,0,1,1,3,0,1,1,0,3,0,1,0,1,3,1,2,5,2,1,3,1,0,1,1,3,0,2,2,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,3,1,2,0,0,1,0,1,3,1,0,4,1,0,1,0,1,0,1,3,1,1,1,1,3,2,2,0,1,1,2,0,3,3,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,2,1,2,0,0,2,0,1,1,3,0,2,3,0,3,0,1,0,3,1,1,1,1,3,1,1,1,0,3,1,1,0,1,1,1,2,1,2,1,1});
//			
//			// 025
//			chunkArray.Add( new int[] {0,1,1,1,1,1,0,1,3,1,0,1,3,1,0,0,2,0,2,0,0,1,1,3,0,3,1,1,0,1,4,0,1,1,0,0,3,2,0,2,3,0,0,2,1,1,1,2,0});
//			chunkArray.Add( new int[] {0,2,1,1,1,2,0,1,7,1,0,1,7,1,0,0,1,0,1,0,0,1,1,3,0,1,3,1,0,3,1,0,1,1,0,0,5,3,0,3,3,0,0,2,1,1,1,2,0});
//			chunkArray.Add( new int[] {0,3,1,1,1,1,0,4,3,2,0,2,2,3,0,0,2,0,3,0,0,1,1,1,0,1,1,1,0,1,3,0,1,2,0,0,1,3,0,1,3,0,0,1,1,1,1,1,0});
//			
//			// 026
//			chunkArray.Add( new int[] {0,1,0,1,0,1,0,0,1,1,2,1,1,0,1,3,0,3,0,3,1,1,1,0,0,0,1,1,2,2,0,0,0,2,2,1,1,0,0,0,1,1,3,1,1,1,1,1,3});
//			chunkArray.Add( new int[] {0,1,0,1,0,1,0,0,1,1,3,1,1,0,2,3,0,1,0,3,2,1,5,0,0,0,5,1,1,1,0,0,0,1,1,3,1,0,0,0,1,3,7,1,1,1,1,1,7});
//			chunkArray.Add( new int[] {0,4,0,1,0,4,0,0,1,2,1,2,1,0,1,3,0,1,0,3,1,2,1,0,0,0,1,2,1,3,0,0,0,3,1,3,1,0,0,0,1,3,1,1,1,1,1,1,1});
//			
//			// 027
//			chunkArray.Add( new int[] {0,0,0,1,2,1,2,0,0,0,0,1,1,1,0,3,0,1,1,0,3,1,1,1,1,3,5,1,0,4,0,1,1,0,3,0,0,0,0,3,1,1,0,0,0,1,1,1,2});
//			chunkArray.Add( new int[] {0,0,0,1,1,1,3,0,0,0,0,1,4,1,0,3,0,1,3,0,2,1,2,1,1,2,3,1,0,3,0,1,3,0,2,0,0,0,0,1,4,1,0,0,0,1,1,1,3});
//			chunkArray.Add( new int[] {0,0,0,1,1,2,3,0,0,0,0,3,7,1,0,1,0,2,1,0,1,1,1,1,3,1,3,1,0,1,0,2,1,0,1,0,0,0,0,1,7,3,0,0,0,1,1,2,3});
//			
//			// 028
//			chunkArray.Add( new int[] {3,2,1,4,0,0,3,1,1,1,0,0,0,2,0,3,1,2,1,1,1,1,1,0,7,3,0,1,0,3,1,2,1,1,1,1,1,1,0,0,0,2,1,2,1,3,0,0,3});
//			chunkArray.Add( new int[] {3,2,2,3,0,0,2,3,1,1,0,0,0,1,0,1,1,1,3,1,1,1,1,0,2,2,0,1,0,1,1,3,1,1,1,3,1,1,0,0,0,1,3,2,2,3,0,0,1});
//			chunkArray.Add( new int[] {1,1,1,2,0,0,1,1,3,1,0,0,0,3,0,1,1,1,1,2,1,1,1,0,3,1,0,1,0,2,2,1,1,1,1,4,5,3,0,0,0,1,4,3,3,1,0,0,1});
//			
//			// 029
//			chunkArray.Add( new int[] {1,3,0,1,1,1,3,1,1,0,0,0,2,1,1,2,0,0,4,3,2,1,0,0,0,0,0,3,2,3,4,0,0,1,1,3,2,0,0,0,3,1,3,1,1,1,1,1,3});
//			chunkArray.Add( new int[] {7,2,0,1,1,1,2,1,3,0,0,0,1,1,1,3,0,0,2,1,3,1,0,0,0,0,0,1,1,1,1,0,0,2,2,3,2,0,0,0,5,3,3,1,1,1,1,3,1});
//			chunkArray.Add( new int[] {1,4,0,1,1,1,3,7,7,0,0,0,1,3,1,3,0,0,1,2,1,1,0,0,0,0,0,1,1,1,1,0,0,3,1,1,3,0,0,0,7,7,3,1,1,1,1,1,1});
//			
//			// 030
//			chunkArray.Add( new int[] {0,0,0,1,1,1,0,0,3,0,0,0,3,0,0,1,0,0,0,1,0,1,2,1,1,0,1,1,0,0,0,3,1,7,0,0,0,0,1,5,3,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,1,0,0,3,0,0,0,2,0,0,3,0,0,0,1,0,1,1,1,1,0,1,1,0,0,0,2,1,3,0,0,0,0,1,3,4,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,1,0,0,4,0,0,0,1,0,0,1,0,0,0,3,0,1,1,1,2,0,1,1,0,0,0,1,1,1,0,0,0,0,7,1,7,0,0,0,0,1,0,0,0});
//			
//			// 031
//			chunkArray.Add( new int[] {0,0,0,1,1,0,0,0,0,0,3,1,1,1,0,4,0,0,0,0,1,1,2,3,1,3,2,1,1,0,0,0,0,1,0,1,1,3,1,0,0,0,0,0,2,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,0,0,0,0,0,2,3,1,1,0,1,0,0,0,0,1,1,1,2,1,3,1,3,1,0,0,0,0,4,0,1,1,7,3,0,0,0,0,0,5,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,1,0,0,0,0,0,2,1,3,1,0,1,0,0,0,0,1,1,1,1,3,1,7,1,1,0,0,0,0,1,0,2,1,3,1,0,0,0,0,0,1,1,0,0,0});
//			
//			// 032
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,2,1,1,1,3,0,0,1,0,0,0,1,0,1,1,0,3,1,2,1,0,0,0,1,0,4,0,7,1,2,1,0,0,0,3,1,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,1,2,1,1,1,0,0,1,0,0,0,1,0,1,3,0,2,1,3,1,0,0,0,1,0,1,0,3,5,1,3,0,0,0,4,1,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,2,1,1,1,1,0,0,1,0,0,0,2,0,1,3,0,1,1,3,1,0,0,0,1,0,4,0,4,1,1,2,0,0,0,1,3,0,1,0,0,0});
//			
//			// 033
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,2,1,3,1,1,1,1,3,1,0,0,1,0,0,0,0,0,0,3,0,4,0,0,2,3,2,0,3,1,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,2,3,1,1,1,1,1,2,3,0,0,1,0,0,0,0,0,0,2,0,3,0,0,2,3,1,0,1,1,1,2,3,1});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,2,3,1,1,1,1,1,3,5,0,0,1,0,0,0,0,0,0,1,0,4,0,0,1,1,2,0,2,1,1,3,5,3});
//			
//			// 034
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,3,1,2,0,0,0,0,1,0,1,0,0,1,1,1,0,1,3,1,0,1,0,0,0,2,0,4,2,4,0,0,1,0,0,3,0,1,1,1,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,1,3,2,0,0,0,0,1,0,1,0,0,1,1,1,0,2,7,1,0,1,0,0,0,1,0,4,5,3,0,0,3,0,0,3,0,1,1,1,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,1,3,2,0,0,0,0,1,0,1,0,0,1,3,1,0,1,3,1,0,2,0,0,0,1,0,1,1,1,0,0,1,0,0,3,0,1,2,1,0});
//			
//			// 035
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,3,1,1,1,0,0,1,1,0,2,0,0,0,1,1,0,1,0,0,0,1,0,0,3,1,0,0,7,0,0,0,1,1,5,1,0,0,0,7,1,3,4});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,1,1,2,3,0,0,1,1,0,1,0,0,0,1,3,0,1,0,0,0,1,0,0,1,1,0,0,2,0,0,0,2,1,4,3,0,0,0,1,1,3,1});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,1,2,1,1,0,0,3,1,0,1,0,0,0,1,1,0,1,0,0,0,1,0,0,3,1,0,0,1,0,0,0,1,3,2,1,0,0,0,1,1,1,2});
//			
//			// 036
//			chunkArray.Add( new int[] {3,4,0,1,0,0,0,2,5,7,1,1,1,0,0,1,3,0,0,2,0,1,1,0,0,0,1,1,1,0,0,0,1,1,0,0,3,0,1,2,1,3,0,1,1,1,0,3,1});
//			chunkArray.Add( new int[] {1,3,0,1,0,0,0,1,1,1,1,2,1,0,0,2,1,0,0,1,0,1,1,0,0,0,1,1,1,0,0,0,3,2,0,0,3,0,1,1,1,1,0,1,1,1,0,3,1});
//			chunkArray.Add( new int[] {1,1,0,1,0,0,0,3,2,1,3,1,1,0,0,1,1,0,0,3,0,1,1,0,0,0,1,1,1,0,0,0,1,2,0,0,1,0,1,1,1,1,0,1,3,1,0,3,4});
//			
//			// 037
//			chunkArray.Add( new int[] {4,3,0,1,0,3,1,3,2,1,1,1,2,3,0,1,0,0,0,1,0,1,1,1,0,3,1,0,0,1,0,0,0,1,0,3,2,1,1,1,2,3,1,3,0,1,0,3,4});
//			chunkArray.Add( new int[] {1,2,0,1,0,7,3,1,1,2,1,1,1,1,0,1,0,0,0,1,0,1,1,3,0,1,1,0,0,1,0,0,0,1,0,5,1,1,1,2,1,1,4,7,0,1,0,2,1});
//			chunkArray.Add( new int[] {1,3,0,1,0,1,3,2,3,1,1,1,3,2,0,1,0,0,0,1,0,1,1,1,0,4,3,0,0,1,0,0,0,1,0,7,3,1,1,1,3,7,1,1,0,1,0,1,1});
//			
//			// 038
//			chunkArray.Add( new int[] {3,2,1,1,1,0,0,1,1,0,0,1,1,0,1,0,0,0,4,1,3,1,0,0,0,0,0,1,0,0,0,0,0,8,2,0,0,0,0,0,0,1,0,0,0,1,1,1,3});
//			chunkArray.Add( new int[] {3,1,1,1,1,0,0,1,2,0,0,1,2,0,1,0,0,0,3,1,1,1,0,0,0,0,0,1,0,0,0,0,0,4,3,0,0,0,0,0,0,1,0,0,0,1,1,2,1});
//			chunkArray.Add( new int[] {7,1,1,1,3,0,0,1,5,0,0,1,1,0,1,0,0,0,7,1,3,1,0,0,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,0,2,0,0,0,1,1,1,1});
//			
//			// 039
//			chunkArray.Add( new int[] {0,4,0,1,1,1,0,7,1,7,0,0,1,0,2,5,2,0,0,3,0,1,0,1,0,0,1,1,1,0,3,1,1,1,0,1,0,0,0,0,3,0,1,3,1,1,0,0,0});
//			chunkArray.Add( new int[] {0,3,0,1,1,3,0,1,3,1,0,0,1,0,1,2,1,0,0,1,0,1,0,1,0,0,1,1,1,0,3,1,1,2,0,2,0,0,0,0,3,0,1,1,1,1,0,0,0});
//			chunkArray.Add( new int[] {0,1,0,1,1,3,0,1,1,1,0,0,1,0,3,1,7,0,0,2,0,1,0,1,0,0,1,1,1,0,1,1,1,1,0,1,0,0,0,0,3,0,2,1,1,1,0,0,0});
//			
//			// 040
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,2,7,1,1,1,3,0,1,4,0,1,0,0,0,1,0,0,1,1,0,1,0,0,0,0,2,1,1,0,0,0,0,1,0,0,0,3,2,1,3,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,1,2,1,3,1,4,0,5,1,0,1,0,0,0,1,0,0,1,2,0,1,0,0,0,0,1,1,1,0,0,0,0,3,0,0,0,3,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,3,1,1,1,1,3,0,2,3,0,1,0,0,0,1,0,0,1,1,0,1,0,0,0,0,3,2,1,0,0,0,0,1,0,0,0,2,1,1,1,0,0});
//			
//			// 041
//			chunkArray.Add( new int[] {2,3,1,1,1,1,2,1,0,0,0,0,0,3,1,0,4,1,4,0,1,1,3,2,7,2,3,1,0,0,0,1,0,0,0,0,0,3,1,1,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {1,2,1,1,1,3,1,1,0,0,0,0,0,2,1,0,3,1,3,0,1,1,2,1,5,1,2,1,0,0,0,1,0,0,0,0,0,1,3,1,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {3,1,1,1,1,1,3,1,0,0,0,0,0,1,1,0,1,4,1,0,1,2,1,7,1,7,1,2,0,0,0,1,0,0,0,0,0,3,2,3,0,0,0,0,0,1,0,0,0});
//			
//			// 042
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,1,3,1,0,0,0,0,2,1,0,0,0,0,1,1,0,0,0,1,1,0,1,3,1,2,3,0,0,0,0,1,2,4,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,1,3,1,0,0,0,0,3,2,0,0,0,0,1,1,0,0,0,1,1,0,2,1,3,5,1,0,0,0,0,1,1,7,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,4,2,1,0,0,0,0,3,7,0,0,0,0,1,1,0,0,0,1,1,0,1,1,3,2,1,0,0,0,0,1,1,3,0,0,0,0,1,0,0,0});
//			
//			// 043
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,7,3,1,0,0,0,0,1,0,1,0,0,1,1,1,0,1,1,1,0,1,0,0,0,2,0,3,2,0,1,0,5,1,3,1,1,1,0,3,4});
//			chunkArray.Add( new int[] {0,0,3,1,3,0,0,0,0,1,1,1,0,0,0,0,2,0,2,0,0,1,1,3,0,3,1,1,0,1,0,0,0,1,0,1,7,0,1,0,1,3,1,1,1,1,0,7,1});
//			chunkArray.Add( new int[] {0,0,1,1,2,0,0,0,0,1,3,1,0,0,0,0,1,0,1,0,0,1,2,1,0,3,1,1,0,1,0,0,0,2,0,1,1,0,4,0,5,1,1,2,1,1,0,1,3});
//			
//			// 044
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,4,1,0,0,0,1,1,3,7,3,0,1,0,0,0,1,1,2,1,0,0,0,1,0,0,0,0,0,3,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,3,2,0,0,0,1,3,1,5,4,0,1,0,0,0,2,3,1,1,0,0,0,1,0,0,0,0,0,3,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,2,0,0,0,0,0,0,1,0,0,0,0,0,1,3,0,0,0,2,1,3,2,1,0,1,0,0,0,1,1,3,1,0,0,0,3,0,0,0,0,0,1,2,0,0,0});
//			
//			// 045
//			chunkArray.Add( new int[] {1,1,1,1,1,1,3,2,0,0,1,0,0,2,1,0,5,1,5,0,1,1,0,3,2,3,0,1,3,0,4,3,4,0,1,1,0,0,0,0,0,1,1,1,1,2,1,1,3});
//			chunkArray.Add( new int[] {7,1,1,1,1,1,7,1,0,0,1,0,0,1,1,0,4,1,3,0,1,3,0,1,1,3,0,1,1,0,1,2,3,0,1,1,0,0,0,0,0,1,7,1,1,1,1,1,7});
//			chunkArray.Add( new int[] {2,1,1,3,1,1,1,1,0,0,1,0,0,1,1,0,3,1,1,0,1,3,0,2,5,2,0,3,1,0,3,1,3,0,1,1,0,0,0,0,0,2,1,3,1,1,3,1,1});
//			
//			// 046
//			chunkArray.Add( new int[] {1,1,1,1,2,1,1,2,0,0,0,0,0,1,1,0,3,1,0,0,2,1,0,3,5,2,0,1,1,0,4,0,1,3,1,3,0,0,0,1,2,1,1,2,1,1,1,3,1});
//			chunkArray.Add( new int[] {3,2,1,1,3,1,2,2,0,0,0,0,0,1,1,0,3,1,0,0,1,1,0,2,3,1,0,1,1,0,3,0,1,1,1,1,0,0,0,1,4,3,2,1,1,1,3,3,2});
//			chunkArray.Add( new int[] {1,1,1,3,1,1,1,3,0,0,0,0,0,3,1,0,4,4,0,0,2,1,0,2,1,1,0,1,1,0,3,0,3,1,1,2,0,0,0,2,1,2,1,1,1,1,1,1,3});
//			
//			// 047
//			chunkArray.Add( new int[] {1,1,3,1,1,1,2,1,0,0,0,0,0,3,1,4,0,0,0,0,5,1,0,0,0,0,0,2,1,0,0,0,0,3,1,3,0,0,0,0,0,1,1,2,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,2,2,0,0,0,0,0,1,1,3,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0,0,0,5,1,1,0,0,0,0,0,2,2,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,3,3,1,1,2,0,0,0,0,0,1,1,1,0,0,0,0,3,3,0,0,0,0,0,1,1,0,0,0,0,1,1,1,0,0,0,0,0,2,1,1,3,3,2,1,1});
//			
//			// 048
//			chunkArray.Add( new int[] {1,1,1,1,2,1,3,2,0,0,0,0,0,1,1,0,0,2,3,2,1,1,3,1,2,3,0,1,1,0,0,0,0,0,2,3,0,3,1,3,0,1,1,1,2,1,2,1,1});
//			chunkArray.Add( new int[] {2,1,1,1,1,3,1,1,0,0,0,0,0,1,1,0,0,3,2,1,1,1,2,1,1,3,0,2,3,0,0,0,0,0,1,1,0,2,1,3,0,1,1,1,7,5,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,3,1,1,2,0,0,0,0,0,2,1,0,0,2,1,3,1,1,3,1,1,4,0,1,1,0,0,0,0,0,1,3,0,3,2,3,0,1,1,1,1,2,1,1,1});
//			
//			// 049
//			chunkArray.Add( new int[] {1,3,1,1,7,1,1,1,0,0,0,0,0,1,2,1,2,1,2,1,1,1,0,3,1,3,0,1,1,0,0,4,0,0,2,1,0,0,0,0,0,1,1,1,7,1,1,1,3});
//			chunkArray.Add( new int[] {1,2,1,1,1,1,3,1,0,0,0,0,0,1,1,1,2,5,3,1,1,1,0,3,1,2,0,2,3,0,0,3,0,0,1,2,0,0,0,0,0,1,1,2,1,1,1,1,3});
//			chunkArray.Add( new int[] {3,2,1,1,1,2,1,1,0,0,0,0,0,1,2,1,1,4,1,1,2,1,0,3,1,3,0,3,1,0,0,1,0,0,1,3,0,0,0,0,0,1,1,2,1,1,1,2,3});
//			
//			// 050
//			chunkArray.Add( new int[] {1,1,3,1,3,1,2,1,0,0,0,0,0,1,3,2,3,5,3,2,3,1,2,1,3,1,2,1,1,0,1,0,4,0,1,1,0,0,0,0,0,1,2,1,1,3,1,1,1});
//			chunkArray.Add( new int[] {3,1,1,1,1,1,3,1,0,0,0,0,0,1,2,1,2,4,2,1,2,3,1,1,1,1,1,3,1,0,3,0,3,0,1,1,0,0,0,0,0,1,1,2,1,3,1,2,1});
//			chunkArray.Add( new int[] {2,1,1,1,1,1,2,1,0,0,0,0,0,1,7,1,1,3,1,1,7,1,1,5,1,1,1,1,3,0,4,0,4,0,3,1,0,0,0,0,0,1,2,1,1,3,1,1,2});
//			
//			// 051
//			chunkArray.Add( new int[] {2,1,1,1,1,1,1,1,0,1,0,1,0,1,1,0,3,4,1,0,1,3,0,1,5,3,0,1,1,1,2,0,2,1,3,1,0,0,0,0,0,2,1,1,3,1,1,2,1});
//			chunkArray.Add( new int[] {3,1,2,1,2,1,1,1,0,1,0,1,0,3,2,0,1,2,1,0,1,1,0,1,7,1,0,1,1,1,3,0,1,1,1,1,0,0,0,0,0,1,1,1,3,1,1,1,2});
//			chunkArray.Add( new int[] {1,1,1,1,1,2,1,1,0,1,0,1,0,1,3,0,3,1,3,0,1,1,0,1,1,1,0,1,2,1,7,0,7,1,2,1,0,0,0,0,0,1,1,2,1,1,1,1,3});
//			
//			// 052
//			chunkArray.Add( new int[] {3,1,1,1,1,1,3,1,2,3,0,0,0,2,1,0,1,0,4,1,1,2,0,2,3,1,2,1,1,0,3,1,0,0,1,1,0,0,1,0,0,1,1,1,1,2,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,2,1,3,1,0,0,0,1,1,0,1,0,3,1,1,1,0,7,1,3,2,1,2,0,3,1,0,0,1,1,0,0,1,0,0,3,1,1,1,2,1,1,3});
//			chunkArray.Add( new int[] {4,7,1,3,1,1,1,7,2,1,0,0,0,1,1,0,3,0,1,1,2,3,0,2,2,2,2,1,1,0,4,3,0,0,1,1,0,0,1,0,0,1,1,2,1,1,1,1,2});
//			
//			// 053
//			chunkArray.Add( new int[] {1,3,1,1,2,1,1,1,0,2,0,0,0,3,2,0,1,1,0,0,1,1,0,1,3,1,0,1,3,0,2,1,3,2,1,1,0,0,0,0,0,1,1,1,3,3,2,1,2});
//			chunkArray.Add( new int[] {1,1,1,2,1,1,1,3,0,1,0,0,0,3,1,0,1,3,0,0,1,1,0,2,5,3,0,1,2,0,4,2,1,1,1,1,0,0,0,0,0,2,1,1,2,1,1,3,1});
//			chunkArray.Add( new int[] {1,1,2,3,1,1,1,1,0,1,0,0,0,1,1,0,3,3,0,0,1,3,0,1,4,3,0,1,1,0,2,1,3,1,2,2,0,0,0,0,0,3,1,2,1,1,3,1,1});
//			
//			// 054
//			chunkArray.Add( new int[] {3,1,1,3,1,1,1,1,3,0,1,0,0,1,2,0,0,2,1,3,1,1,0,0,0,0,0,2,1,2,3,2,0,1,1,1,0,4,3,0,0,1,1,3,1,2,1,1,3});
//			chunkArray.Add( new int[] {3,1,1,1,1,1,2,1,7,0,3,0,0,1,1,0,0,1,2,1,3,2,0,0,0,0,0,1,1,1,3,1,0,4,1,1,0,5,3,0,0,1,3,1,2,2,1,1,1});
//			chunkArray.Add( new int[] {7,2,1,3,1,1,1,2,7,0,1,0,0,1,3,0,0,3,1,1,3,1,0,0,0,0,0,1,1,1,3,2,0,1,2,1,0,2,1,0,0,1,1,2,1,1,1,3,1});
//			
//			// 055
//			chunkArray.Add( new int[] {3,1,1,1,2,1,1,1,0,0,2,0,0,1,2,0,3,3,0,0,3,1,0,4,2,1,2,1,1,0,0,3,1,0,1,3,0,0,0,0,0,2,1,1,2,1,1,1,1});
//			chunkArray.Add( new int[] {2,1,1,1,1,1,3,1,0,0,2,0,0,3,1,0,1,3,0,0,1,3,0,1,5,3,2,1,1,0,0,1,1,0,1,2,0,0,0,0,0,2,1,1,3,1,2,1,1});
//			chunkArray.Add( new int[] {1,1,1,2,1,3,1,2,0,0,1,0,0,2,3,0,1,1,0,0,1,1,0,7,1,7,1,3,3,0,0,4,1,0,1,2,0,0,0,0,0,2,1,2,1,1,1,2,1});
//			
//			// 056
//			chunkArray.Add( new int[] {1,1,1,1,3,1,1,1,0,1,0,2,0,2,3,2,1,3,1,1,1,1,0,3,4,1,0,1,1,1,1,5,3,2,3,2,0,2,0,1,0,1,1,1,3,1,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,2,1,3,3,0,1,0,1,0,1,1,1,1,2,1,3,1,1,0,1,7,1,0,1,1,3,1,2,1,1,1,2,0,1,0,1,0,2,3,1,2,1,1,3,1});
//			chunkArray.Add( new int[] {1,1,1,3,1,1,2,1,0,1,0,1,0,1,3,1,2,3,2,1,1,1,0,3,5,4,0,1,1,1,2,3,2,1,3,1,0,1,0,1,0,1,2,1,1,3,1,1,1});
//			
//			// 057
//			chunkArray.Add( new int[] {1,2,1,3,1,2,1,1,0,1,1,1,0,1,1,0,0,1,0,0,1,1,0,3,2,3,0,1,1,0,0,4,0,0,1,1,3,0,0,0,3,1,1,2,1,1,1,2,1});
//			chunkArray.Add( new int[] {1,2,1,1,1,1,2,3,0,3,1,3,0,1,1,0,0,1,0,0,1,2,0,4,5,4,0,3,1,0,0,3,0,0,1,1,2,0,0,0,1,2,3,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,3,1,1,1,1,1,2,0,2,1,2,0,3,1,0,0,1,0,0,1,1,0,1,7,1,0,1,1,0,0,1,0,0,1,1,3,0,0,0,3,1,3,2,1,1,1,2,3});
//			
//			// 058
//			chunkArray.Add( new int[] {1,1,2,1,1,1,3,1,0,0,0,0,0,2,3,0,4,2,1,1,1,1,0,0,3,3,0,1,2,0,4,2,1,1,1,1,0,0,0,0,0,1,1,1,2,1,3,3,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,3,2,0,0,0,0,0,1,1,0,4,7,1,1,2,1,0,0,3,1,0,1,1,0,4,1,7,1,2,2,0,0,0,0,0,1,1,3,1,1,1,1,3});
//			chunkArray.Add( new int[] {1,3,1,1,1,2,1,3,0,0,0,0,0,1,1,0,2,1,2,1,1,1,0,0,5,3,0,3,1,0,2,1,2,1,1,2,0,0,0,0,0,1,1,1,3,1,1,2,1});
//			
//			// 059
//			chunkArray.Add( new int[] {1,1,2,1,1,1,1,1,0,0,0,0,0,1,2,1,3,1,1,3,2,1,0,0,0,2,0,1,1,4,0,1,3,2,1,1,0,0,3,1,0,1,1,3,2,1,3,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,2,1,1,1,0,0,0,0,0,1,3,1,1,1,2,1,1,1,0,0,0,1,0,1,2,3,0,4,3,1,2,1,0,0,3,1,0,1,1,1,1,1,1,3,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,2,1,3,0,0,0,0,0,3,2,3,1,1,1,2,1,1,0,0,0,3,0,1,1,1,0,1,2,1,1,1,0,0,4,1,0,2,1,1,1,3,7,1,1});
//
//			// 060
//			chunkArray.Add( new int[] {1,1,5,5,5,1,1,3,0,0,0,0,0,1,1,1,3,1,3,1,2,1,1,5,4,1,1,1,2,1,3,1,3,1,1,1,0,0,0,0,0,3,1,1,5,5,5,1,1});
//			chunkArray.Add( new int[] {1,1,2,1,1,3,1,1,0,0,0,0,0,1,1,3,1,1,1,2,1,1,1,1,3,2,1,1,1,4,1,2,1,1,3,5,0,0,0,0,0,1,5,5,3,1,1,1,2});
//			chunkArray.Add( new int[] {1,1,2,1,1,1,3,1,0,0,0,0,0,1,1,5,3,4,5,1,1,1,3,2,5,2,5,1,1,1,5,4,3,1,1,1,0,0,0,0,0,1,3,1,1,1,1,2,1});
//
//			// 061
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,1,0,2,1,1,0,1,2,0,1,4,5,0,1,1,0,3,5,3,0,1,1,0,5,4,1,0,2,1,0,1,1,2,0,1,1,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,7,1,1,1,7,1,1,0,1,2,3,0,1,1,0,1,1,1,0,1,2,0,1,4,1,0,2,1,0,1,1,1,0,1,1,0,3,2,1,0,1,1,7,1,1,1,7,1});
//			chunkArray.Add( new int[] {1,2,1,1,1,5,3,1,0,1,1,1,0,5,1,0,4,5,4,0,1,1,0,3,2,3,0,1,1,0,4,5,4,0,1,2,0,1,1,1,0,1,1,1,1,1,1,2,1});
//
//			// 062
//			chunkArray.Add( new int[] {0,0,0,3,1,4,0,0,0,1,1,1,0,0,1,1,3,1,2,1,1,1,2,1,1,0,1,3,1,1,3,1,2,1,1,0,0,1,1,1,0,0,0,0,0,3,1,4,0});
//			chunkArray.Add( new int[] {0,0,0,1,2,1,0,0,0,3,1,1,0,0,1,2,5,1,3,1,1,1,1,1,4,0,1,1,1,2,5,1,3,1,1,0,0,3,1,1,0,0,0,0,0,1,2,1,0});
//			chunkArray.Add( new int[] {0,0,0,1,5,3,0,0,0,4,1,5,0,0,1,2,1,1,3,1,1,1,3,1,2,0,3,1,1,2,1,1,3,1,1,0,0,4,1,5,0,0,0,0,0,1,5,3,0});
//
//			// 063
//			chunkArray.Add( new int[] {1,1,2,1,1,1,3,1,1,3,0,3,5,1,2,3,1,0,4,1,2,1,0,0,0,0,0,1,1,1,4,0,1,1,1,1,5,3,0,1,3,1,3,1,2,1,2,1,1});
//			chunkArray.Add( new int[] {2,1,1,1,1,1,1,1,4,1,0,1,3,7,7,1,3,0,1,2,1,1,0,0,0,0,0,1,1,2,1,0,3,1,1,1,3,1,0,1,4,1,1,1,1,1,7,1,2});
//			chunkArray.Add( new int[] {1,1,1,1,2,1,3,1,3,1,0,1,4,1,2,1,1,0,1,1,1,1,0,0,0,0,0,1,1,1,3,0,1,1,1,1,5,1,0,1,3,1,3,1,1,1,2,1,1});
//
//			// 064
//			chunkArray.Add( new int[] {0,1,1,3,0,1,0,1,5,1,5,0,1,3,2,3,4,1,0,1,1,1,5,1,3,0,2,1,1,1,5,1,0,1,1,2,3,1,2,1,1,1,0,1,1,1,0,3,0});
//			chunkArray.Add( new int[] {0,3,2,4,0,7,0,1,1,3,1,0,1,2,1,1,1,3,0,1,1,1,1,2,1,0,1,1,1,1,1,1,0,1,1,1,1,2,1,2,1,1,0,1,3,1,0,7,0});
//			chunkArray.Add( new int[] {0,3,1,1,0,1,0,1,1,1,1,0,1,1,1,2,3,5,0,2,2,1,2,5,4,0,1,1,1,2,3,5,0,1,1,1,1,1,1,5,3,3,0,3,1,1,0,1,0});
//
//			// 065
//			chunkArray.Add( new int[] {1,1,1,1,2,3,3,1,0,0,0,0,0,1,2,0,5,1,7,1,2,1,0,2,5,2,3,1,3,0,3,3,5,1,1,1,0,2,1,0,0,1,2,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {3,1,2,1,1,1,1,1,0,0,0,0,0,2,1,0,4,1,1,1,1,1,0,1,1,3,2,1,1,0,1,3,1,1,1,2,0,2,1,0,0,1,1,3,1,1,1,1,3});
//			chunkArray.Add( new int[] {1,2,1,1,1,1,1,2,0,0,0,0,0,1,1,0,2,4,5,1,2,1,0,4,2,7,1,1,3,0,5,7,1,3,1,1,0,1,1,0,0,1,1,1,2,1,1,1,1});
//
//			// 066
//			chunkArray.Add( new int[] {1,3,2,1,2,3,1,1,0,1,1,1,0,2,2,0,1,5,1,0,1,1,5,7,2,7,5,1,1,3,4,2,4,3,1,1,0,0,0,0,0,2,1,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,2,0,1,3,1,0,2,1,0,1,1,1,0,1,1,2,3,4,1,2,1,1,2,1,2,3,2,1,2,0,0,0,0,0,2,3,1,1,1,1,1,3});
//			chunkArray.Add( new int[] {1,3,1,1,1,3,1,2,0,1,1,1,0,2,1,0,2,1,2,0,1,1,3,1,1,1,3,1,1,7,1,2,1,7,1,3,0,0,0,0,0,3,1,2,1,1,1,2,1});
//
//			// 067
//			chunkArray.Add( new int[] {0,0,3,1,1,0,0,0,0,1,1,1,0,0,1,3,2,4,2,3,1,1,1,5,0,5,1,1,1,3,2,1,2,3,1,0,0,1,1,1,0,0,0,0,1,1,3,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,3,0,0,0,0,2,1,1,0,0,1,1,1,3,2,1,4,1,1,3,0,3,1,1,3,1,1,3,2,1,1,0,0,2,1,1,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,2,1,1,0,0,0,0,1,3,1,0,0,2,3,5,2,5,3,2,1,4,1,0,1,4,1,1,3,5,2,5,3,1,0,0,1,3,1,0,0,0,0,1,1,2,0,0});
//
//			// 068
//			chunkArray.Add( new int[] {0,1,1,1,1,2,0,2,3,1,1,3,1,2,1,1,1,0,1,3,1,1,1,0,0,0,1,1,1,3,4,0,1,1,1,2,1,3,1,1,3,2,0,2,1,1,1,1,0});
//			chunkArray.Add( new int[] {0,1,1,1,1,1,0,3,5,2,1,2,1,1,1,1,7,0,7,1,1,1,1,0,0,0,1,1,1,1,7,0,7,5,1,1,1,2,1,2,1,3,0,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {0,1,1,1,1,1,0,1,3,1,1,2,3,1,1,1,4,0,3,2,1,1,1,0,0,0,1,1,1,2,3,0,1,1,1,1,3,2,1,1,3,1,0,1,1,1,1,1,0});
//
//			// 069
//			chunkArray.Add( new int[] {1,1,1,1,1,1,3,3,2,1,3,1,1,1,1,1,0,4,0,2,1,1,1,1,0,1,1,1,1,3,0,3,0,1,2,1,2,1,1,1,3,1,1,1,1,1,1,1,3});
//			chunkArray.Add( new int[] {1,2,1,1,1,3,4,1,3,1,1,1,2,3,1,1,0,1,0,2,1,1,1,3,0,3,1,1,1,2,0,1,0,1,1,3,2,1,1,1,3,1,4,3,1,1,1,2,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,1,2,1,3,3,3,1,2,1,1,0,1,0,1,1,1,1,1,0,1,1,1,5,1,0,4,0,1,1,3,5,3,3,3,1,1,1,2,2,1,2,2,1});
//
//			// 070
//			chunkArray.Add( new int[] {0,0,1,1,3,0,0,0,0,3,1,1,0,0,2,1,1,1,1,1,2,1,2,3,2,3,2,1,1,3,4,0,4,3,1,1,0,0,0,0,0,1,1,3,2,1,1,3,1});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,0,7,1,7,0,0,4,3,1,1,1,3,1,3,1,1,1,1,1,3,2,2,1,0,1,2,2,1,0,0,0,0,0,1,1,1,1,1,1,1,1});
//			chunkArray.Add( new int[] {0,0,3,1,1,0,0,0,0,1,1,1,0,0,3,1,2,1,2,1,3,5,3,2,1,2,3,1,1,1,1,0,1,1,1,1,0,0,0,0,0,1,1,2,1,1,1,1,1});
//
//			// 071
//			chunkArray.Add( new int[] {0,0,1,1,3,0,0,3,1,2,0,1,1,1,2,1,1,0,1,2,1,1,4,3,0,3,1,1,2,1,1,0,1,2,1,3,1,2,0,1,1,1,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,1,1,1,0,3,1,2,3,1,2,0,1,4,1,1,5,1,0,3,1,1,2,4,2,0,2,1,3,3,1,3,0,1,1,1,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,3,1,1,0,0,1,3,5,0,2,1,1,1,5,1,0,1,3,1,1,1,1,0,1,7,1,2,1,2,0,1,3,1,1,3,1,0,2,1,1,0,0,1,1,1,0,0});
//
//			// 072
//			chunkArray.Add( new int[] {0,0,2,1,1,0,0,0,4,3,3,0,0,0,0,0,1,1,2,0,1,3,0,0,1,1,1,1,1,0,2,1,3,0,3,1,2,1,3,0,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,3,5,1,0,0,0,0,0,2,3,2,0,1,1,0,0,5,2,1,1,1,0,2,1,3,0,1,2,1,7,5,0,0,0,0,0,1,1,1,0,0});
//			chunkArray.Add( new int[] {0,0,3,1,2,0,0,0,1,1,1,0,0,0,0,0,7,1,7,0,1,1,0,0,1,1,1,1,1,0,7,1,7,0,1,1,2,1,1,0,0,0,0,0,3,1,1,0,0});
//
//			// 073
//			chunkArray.Add( new int[] {0,0,1,1,0,0,0,0,3,1,1,1,1,0,1,2,3,1,2,3,0,1,1,0,0,3,2,1,1,0,0,0,1,1,1,0,0,0,1,3,2,1,0,1,1,1,1,1,0});
//			chunkArray.Add( new int[] {0,0,1,1,0,0,0,0,1,1,1,1,1,0,1,2,1,1,3,1,0,1,3,0,0,5,7,2,1,0,0,0,1,4,1,0,0,0,1,3,5,3,0,1,1,1,1,2,0});
//			chunkArray.Add( new int[] {0,0,1,1,0,0,0,0,1,2,2,3,2,0,1,1,1,1,1,3,0,1,3,0,0,1,2,1,1,0,0,0,1,3,1,0,0,0,1,1,2,1,0,1,1,1,1,3,0});
//
//			// 074
//			chunkArray.Add( new int[] {1,1,2,1,1,1,3,1,0,0,0,0,0,1,1,2,3,0,3,2,1,1,1,1,0,4,3,2,1,3,1,0,3,2,1,1,0,0,0,0,0,1,1,2,1,1,3,2,1});
//			chunkArray.Add( new int[] {1,1,1,1,2,1,1,1,0,0,0,0,0,1,2,3,2,0,2,3,2,1,5,3,0,3,5,1,2,3,2,0,2,3,2,1,0,0,0,0,0,1,1,1,2,1,1,1,1});
//			chunkArray.Add( new int[] {2,1,1,1,1,1,1,1,0,0,0,0,0,1,1,3,1,0,1,1,2,1,2,4,0,1,3,1,1,3,1,0,1,1,2,1,0,0,0,0,0,1,1,1,1,1,1,1,2});
//
//			// 075
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,3,2,3,0,0,0,3,4,5,4,3,0,1,1,5,7,1,1,1,0,7,4,2,4,7,0,0,0,3,1,3,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,2,1,1,0,0,0,1,3,1,2,3,0,1,1,1,4,1,1,1,0,1,2,1,3,1,0,0,0,3,1,2,0,0,0,0,0,1,0,0,0});
//			chunkArray.Add( new int[] {0,0,0,1,0,0,0,0,0,2,1,1,0,0,0,1,3,1,3,2,0,1,1,1,5,1,1,1,0,2,3,1,3,1,0,0,0,1,1,2,0,0,0,0,0,1,0,0,0});
//
//			// 076
//			chunkArray.Add( new int[] {1,1,1,1,1,3,1,2,3,0,0,0,2,3,1,1,0,0,0,1,1,1,1,0,0,0,1,2,3,2,1,3,1,1,3,1,3,1,1,1,3,1,1,2,1,1,2,1,1});
//			chunkArray.Add( new int[] {1,1,1,1,1,1,3,2,3,0,0,0,1,3,1,1,0,0,0,1,2,1,1,0,0,0,1,1,1,3,1,4,5,3,1,1,2,3,2,3,2,1,1,5,1,1,1,1,1});
//			chunkArray.Add( new int[] {1,3,1,1,2,3,1,7,1,0,0,0,1,7,1,1,0,0,0,1,1,1,1,0,0,0,2,1,1,2,1,1,1,3,1,3,1,1,1,3,1,1,1,3,1,1,2,1,1});
//
//			// 077
//			chunkArray.Add( new int[] {2,1,1,3,0,0,0,3,1,3,0,0,0,0,0,1,1,1,0,0,0,1,0,1,2,1,0,1,1,3,1,2,1,1,1,0,1,0,0,3,2,3,0,1,1,1,0,3,2});
//			chunkArray.Add( new int[] {1,1,1,1,0,0,0,1,2,1,0,0,0,0,0,1,3,7,0,0,0,1,0,3,7,1,0,1,1,1,1,1,1,1,1,0,1,0,0,2,3,1,0,3,1,1,0,1,1});
//			chunkArray.Add( new int[] {7,1,1,2,0,0,0,3,5,1,0,0,0,0,0,1,3,2,0,0,0,1,0,2,4,2,0,1,1,1,3,2,3,1,1,0,1,0,0,5,2,2,0,1,1,1,0,3,3});
//
//			// 078
//			chunkArray.Add( new int[] {3,1,0,1,0,0,0,1,1,2,1,1,3,0,0,2,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,2,2,0,0,3,1,1,2,5,3,0,0,0,1,0,3,4});
//			chunkArray.Add( new int[] {1,1,0,1,0,0,0,1,3,1,1,1,1,0,0,2,7,1,1,3,1,1,1,0,0,0,1,1,3,1,2,1,3,7,0,0,1,1,1,1,1,1,0,0,0,1,0,1,1});
//			chunkArray.Add( new int[] {3,1,0,1,0,0,0,1,1,2,1,1,1,0,0,1,2,1,3,5,2,3,1,0,0,0,3,1,2,5,1,1,2,1,0,0,1,3,1,2,1,1,0,0,0,1,0,1,1});
//
//			// 079
//			chunkArray.Add( new int[] {2,1,1,1,2,3,1,7,3,0,0,2,1,3,1,1,0,0,0,1,1,1,4,0,0,0,0,1,0,0,0,1,3,1,1,0,0,2,1,2,1,2,0,1,3,1,1,1,3});
//			chunkArray.Add( new int[] {3,1,1,1,1,2,1,2,2,0,0,1,2,3,2,3,0,0,0,3,1,1,3,0,0,0,0,1,0,0,0,3,1,1,1,0,0,1,2,5,1,3,0,1,1,1,1,1,2});
//			chunkArray.Add( new int[] {1,1,1,1,1,2,3,1,2,0,0,3,5,1,1,1,0,0,0,1,1,1,1,0,0,0,0,1,0,0,0,1,1,3,1,0,0,3,1,4,2,3,0,1,7,1,1,2,1});
//
//			// 080
//			chunkArray.Add( new int[] {3,2,1,1,1,2,3,3,2,1,1,1,2,3,0,0,0,1,0,0,0,2,0,2,1,2,0,2,1,3,5,0,1,3,1,3,1,1,2,1,5,3,4,1,0,1,0,1,4});
//			chunkArray.Add( new int[] {1,3,1,1,1,3,1,1,2,3,2,3,2,1,0,0,0,1,0,0,0,1,0,1,1,1,0,1,1,1,1,0,1,1,1,2,2,1,1,1,2,2,3,3,0,1,0,3,3});
//			chunkArray.Add( new int[] {2,3,5,1,2,5,3,3,5,2,1,5,3,2,0,0,0,1,0,0,0,1,0,2,1,1,0,1,1,3,1,0,1,2,1,2,1,3,1,1,1,1,4,1,0,1,0,3,1});

//			chunkArray.Add( new int[] {});


			// 5x5 chunks

			//000
			chunkArray.Add( new int[] {0,0,1,0,0,0,2,3,2,0,1,3,1,1,1,0,2,1,4,0,0,0,1,0,0});
			chunkArray.Add( new int[] {0,0,1,0,0,0,4,5,7,0,1,5,1,2,1,0,3,1,7,0,0,0,1,0,0});
			chunkArray.Add( new int[] {0,0,1,0,0,0,1,3,1,0,1,3,5,1,1,0,2,1,7,0,0,0,1,0,0});

			//001
			chunkArray.Add( new int[] {4,3,2,0,0,3,2,1,0,0,1,1,1,1,1,1,2,3,0,0,1,3,2,0,0});
			chunkArray.Add( new int[] {3,1,1,0,0,2,4,7,0,0,1,5,1,1,1,7,4,2,0,0,1,1,3,0,0});
			chunkArray.Add( new int[] {2,3,3,0,0,1,1,1,0,0,1,1,1,1,1,1,2,2,0,0,3,1,1,0,0});

			//002
			chunkArray.Add( new int[] {0,3,1,3,0,0,3,2,3,0,1,1,1,1,1,0,2,0,2,0,0,1,1,1,0});
			chunkArray.Add( new int[] {0,3,1,3,0,0,5,4,5,0,1,1,3,1,1,0,7,0,7,0,0,1,1,1,0});
			chunkArray.Add( new int[] {0,3,1,3,0,0,5,4,5,0,1,1,3,1,1,0,7,0,7,0,0,1,1,1,0});
			chunkArray.Add( new int[] {0,1,1,3,0,0,3,5,1,0,1,2,3,2,1,0,1,0,3,0,0,1,1,1,0});

			//003
			chunkArray.Add( new int[] {4,3,1,1,1,1,7,0,2,1,1,0,0,0,1,3,5,0,2,7,5,3,1,3,4});
			chunkArray.Add( new int[] {4,3,1,1,1,1,7,0,2,1,1,0,0,0,1,3,5,0,2,7,5,3,1,3,4});
			chunkArray.Add( new int[] {1,1,1,2,3,2,3,0,3,1,1,0,0,0,1,1,3,0,3,2,3,2,1,1,1});
			chunkArray.Add( new int[] {3,1,1,1,3,1,5,0,2,1,1,0,0,0,1,2,1,0,1,2,3,1,1,1,3});

			//004
			chunkArray.Add( new int[] {1,1,1,3,4,5,2,1,1,3,1,1,0,2,1,1,1,0,2,7,2,1,1,3,1});
			chunkArray.Add( new int[] {3,2,1,2,3,1,3,1,3,1,1,1,0,1,1,2,1,0,1,2,1,3,1,1,1});
			chunkArray.Add( new int[] {1,2,3,2,1,5,3,4,3,1,1,2,0,2,1,1,1,0,5,1,1,1,1,1,1});

			//005
			chunkArray.Add( new int[] {0,0,1,7,2,0,0,1,4,3,1,3,1,3,1,0,0,1,0,0,0,0,1,0,0});
			chunkArray.Add( new int[] {0,0,1,1,2,0,0,3,1,1,1,2,5,3,1,0,0,2,0,0,0,0,1,0,0});
			chunkArray.Add( new int[] {0,0,1,2,4,0,0,1,3,2,1,1,1,3,1,0,0,1,0,0,0,0,5,0,0});

			//006
			chunkArray.Add( new int[] {0,0,1,0,0,0,2,3,0,0,1,3,1,0,3,0,2,1,0,1,0,0,3,1,2});
			chunkArray.Add( new int[] {0,0,1,0,0,0,4,1,0,0,1,7,3,0,1,0,3,7,0,1,0,0,1,1,1});
			chunkArray.Add( new int[] {0,0,1,0,0,0,4,1,0,0,1,7,3,0,1,0,3,7,0,1,0,0,1,1,1});
			chunkArray.Add( new int[] {0,0,1,0,0,0,3,1,0,0,1,5,3,0,3,0,2,1,0,1,0,0,1,1,2});

			//007
			chunkArray.Add( new int[] {3,1,1,2,1,1,0,0,0,7,1,0,4,3,5,1,0,0,0,7,3,1,1,2,1});
			chunkArray.Add( new int[] {3,3,2,1,3,2,0,0,0,1,1,0,4,5,1,1,0,0,0,1,3,1,2,3,3});
			chunkArray.Add( new int[] {1,2,1,1,1,1,0,0,3,1,1,0,3,2,3,1,0,0,2,1,1,1,1,1,2});

			//008
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,2,0,3,0,0,3,4,1,1,0,2,3,2,3,1,1,3});
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,2,0,3,0,0,3,4,1,1,0,2,3,2,3,1,1,3});
			chunkArray.Add( new int[] {0,0,1,1,1,0,0,0,1,0,1,0,0,5,3,1,1,0,7,2,7,1,1,5,3});
			chunkArray.Add( new int[] {0,0,1,1,3,0,0,0,1,0,7,0,0,2,1,1,1,0,1,7,7,1,1,1,3});

			//009
			chunkArray.Add( new int[] {3,1,1,2,1,2,7,0,4,7,1,5,0,3,1,1,4,0,2,1,1,3,1,1,3});
			chunkArray.Add( new int[] {3,3,1,1,1,1,1,0,2,2,1,1,0,1,1,2,2,0,3,1,1,3,1,1,3});
			chunkArray.Add( new int[] {3,1,2,1,4,3,1,0,1,1,1,1,0,1,1,2,3,0,1,3,1,5,2,1,3});

			//010
			chunkArray.Add( new int[] {0,0,2,1,3,0,0,0,3,1,4,0,0,0,1,1,1,0,0,2,7,3,1,1,1});
			chunkArray.Add( new int[] {0,0,2,1,3,0,0,0,3,1,4,0,0,0,1,1,1,0,0,2,7,3,1,1,1});
			chunkArray.Add( new int[] {0,0,1,3,3,0,0,0,1,2,3,0,0,0,1,1,5,0,0,1,3,2,1,1,3});
			chunkArray.Add( new int[] {0,0,2,1,3,0,0,0,1,1,2,0,0,0,1,1,1,0,0,7,3,1,1,7,1});
			
			//011
			chunkArray.Add( new int[] {0,0,1,0,0,0,0,3,0,0,3,2,1,1,3,0,0,2,0,0,0,0,3,0,0});
			chunkArray.Add( new int[] {0,0,1,0,0,0,0,1,0,0,3,3,1,3,3,0,0,1,0,0,0,0,1,0,0});
			chunkArray.Add( new int[] {0,0,1,0,0,0,0,3,0,0,1,3,5,3,1,0,0,3,0,0,0,0,1,0,0});

			//012
			chunkArray.Add( new int[] {0,1,1,0,0,0,2,0,4,0,1,1,0,7,3,0,3,1,1,1,0,0,1,3,2});
			chunkArray.Add( new int[] {0,2,1,0,0,0,1,0,7,0,1,1,0,5,1,0,3,5,7,1,0,0,1,1,3});
			chunkArray.Add( new int[] {0,1,1,0,0,0,1,0,3,0,1,1,0,1,1,0,3,3,2,1,0,0,1,2,4});
			
			//013
			chunkArray.Add( new int[] {0,0,1,1,1,0,2,0,0,2,1,1,1,1,1,0,3,0,0,1,0,0,1,3,3});
			chunkArray.Add( new int[] {0,0,1,1,7,0,4,0,0,1,1,2,5,1,1,0,3,0,0,7,0,0,1,1,1});
			chunkArray.Add( new int[] {0,0,1,1,7,0,4,0,0,1,1,2,5,1,1,0,3,0,0,7,0,0,1,1,1});
			chunkArray.Add( new int[] {0,0,3,1,1,0,7,0,0,1,1,1,1,1,1,0,7,0,0,2,0,0,1,1,1});

			//014
			chunkArray.Add( new int[] {0,3,1,1,0,0,0,1,0,0,1,0,1,0,3,1,2,5,3,2,0,1,3,4,0});
			chunkArray.Add( new int[] {0,4,1,1,0,0,0,1,0,0,1,0,1,0,1,1,7,1,7,1,0,2,3,2,0});
			chunkArray.Add( new int[] {0,2,5,1,0,0,0,3,0,0,1,0,3,0,1,1,1,5,1,1,0,2,1,2,0});

			//015
			chunkArray.Add( new int[] {0,0,1,2,3,0,0,0,3,4,1,1,5,2,3,1,1,0,0,0,7,1,3,0,0});
			chunkArray.Add( new int[] {0,0,1,2,2,0,0,0,3,3,1,1,1,1,1,3,3,0,0,0,2,2,1,0,0});
			chunkArray.Add( new int[] {0,0,3,1,2,0,0,0,3,5,1,3,1,1,3,3,2,0,0,0,1,1,4,0,0});

			//016
			chunkArray.Add( new int[] {0,4,1,3,0,2,0,3,0,3,1,3,1,1,1,3,0,7,0,7,0,7,1,4,0});
			chunkArray.Add( new int[] {0,1,3,1,0,1,0,1,0,2,3,1,5,3,1,1,0,3,0,1,0,2,1,1,0});
			chunkArray.Add( new int[] {0,1,1,2,0,1,0,1,0,1,1,3,1,3,1,2,0,3,0,1,0,7,1,3,0});
			
			//017
			chunkArray.Add( new int[] {0,3,1,0,0,1,2,1,3,0,1,3,1,2,1,0,1,5,1,3,0,0,1,2,0});
			chunkArray.Add( new int[] {0,3,1,0,0,7,1,1,2,0,1,1,4,3,1,0,1,7,1,1,0,0,1,1,0});
			chunkArray.Add( new int[] {0,2,1,0,0,2,3,1,4,0,1,1,5,1,3,0,1,7,1,1,0,0,1,1,0});
			
			//018
			chunkArray.Add( new int[] {0,0,1,0,1,0,2,3,2,1,3,1,4,1,3,2,3,1,1,0,1,0,1,0,0});
			chunkArray.Add( new int[] {0,0,1,0,1,0,7,1,5,2,1,1,3,3,1,2,5,1,4,0,1,0,3,0,0});
			chunkArray.Add( new int[] {0,0,1,0,1,0,2,1,3,1,3,1,1,1,1,1,3,1,2,0,2,0,1,0,0});

			//019
			chunkArray.Add( new int[] {0,1,1,2,0,3,1,1,1,2,2,0,0,0,3,1,1,4,1,1,0,7,3,3,0});
			chunkArray.Add( new int[] {0,1,1,2,0,3,1,1,1,2,2,0,0,0,3,1,1,4,1,1,0,7,3,3,0});
			chunkArray.Add( new int[] {0,7,1,1,0,1,2,3,3,1,1,0,0,0,2,3,2,1,1,1,0,1,1,3,0});
			chunkArray.Add( new int[] {0,1,1,3,0,7,1,5,1,7,1,0,0,0,1,7,5,1,1,7,0,1,1,5,0});
			
			//020
			chunkArray.Add( new int[] {1,0,1,2,1,2,0,1,3,1,1,0,1,0,1,1,7,1,0,3,1,0,1,0,1});
			chunkArray.Add( new int[] {2,0,3,4,1,1,0,2,3,3,1,0,1,0,1,1,1,1,0,1,1,0,1,0,2});
			chunkArray.Add( new int[] {2,0,3,4,1,1,0,2,3,3,1,0,1,0,1,1,1,1,0,1,1,0,1,0,2});
			chunkArray.Add( new int[] {1,0,5,3,1,1,0,1,5,1,1,0,1,0,1,1,1,2,0,1,2,0,1,0,1});
			
			//021
			chunkArray.Add( new int[] {3,1,7,3,4,0,1,0,0,0,1,1,3,1,1,0,0,2,1,0,1,1,1,1,3});
			chunkArray.Add( new int[] {1,1,1,1,3,0,2,0,0,0,1,3,5,1,1,0,0,4,3,0,1,1,3,2,1});
			chunkArray.Add( new int[] {3,1,1,2,1,0,1,0,0,0,1,1,1,1,2,0,0,1,3,0,1,2,1,1,1});
			
			//022
			chunkArray.Add( new int[] {0,0,1,3,1,0,3,5,4,2,1,1,2,1,3,0,1,1,0,1,0,0,1,0,1});
			chunkArray.Add( new int[] {0,0,1,3,1,0,3,5,4,2,1,1,2,1,3,0,1,1,0,1,0,0,1,0,1});
			chunkArray.Add( new int[] {0,0,1,7,3,0,1,1,2,3,1,1,1,1,1,0,2,1,0,1,0,0,1,0,1});
			chunkArray.Add( new int[] {0,0,1,2,2,0,2,1,3,1,1,1,1,1,1,0,3,3,0,2,0,0,1,0,1});

			//023
			chunkArray.Add( new int[] {0,0,1,3,4,3,0,1,2,3,1,4,0,0,1,2,2,3,1,1,0,3,5,1,0});
			chunkArray.Add( new int[] {0,0,1,1,3,1,0,1,7,1,1,1,0,0,1,3,1,1,1,2,0,3,1,2,0});
			chunkArray.Add( new int[] {0,0,1,7,4,1,0,3,2,1,1,2,0,0,1,1,1,3,2,1,0,2,1,3,0});
		}
		
		
		int[] randChunk = chunkArray[Random.Range(0, chunkArray.Count)];
		
		return(randChunk);
	}

	private Trial GetTrial (int trialNum)
	{
		Trial trial = new Trial ();
		Debug.Log ("trial num " + trialNum);
		switch (trialNum)
		{
		case 0:
			trial.m_columns = 7;
			trial.m_rows = 7;
			trial.m_levels = 1;
			trial.m_xpPrize = 20;
			trial.m_gpPrize = 0;
			trial.m_cardMap = new int[] {0,0,0,9,0,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,0,1,0,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,0,8,0,0,0};
			trial.m_enemyMap = new int[] {0,0,0,0,0,0,0,0,0,1,0,20,0,0,0,0,2,0,4,0,0,0,0,0,11,0,0,0,0,0,1,0,25,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			trial.m_trapMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0};
			trial.m_lootMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			break;
		case 1:
			trial.m_columns = 7;
			trial.m_rows = 7;
			trial.m_levels = 1;
			trial.m_xpPrize = 20;
			trial.m_gpPrize = 0;
			trial.m_cardMap = new int[] {0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,1,1,1,0,0,0,1,1,1,1,1,0,0,0,1,1,1,0,0,0,0,0,8,0,0,0,0,0,0,0,0,0,0};
			trial.m_enemyMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,25,20,25,0,0,0,1,0,0,0,1,0,0,0,2,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			trial.m_trapMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			trial.m_lootMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			break;
		case 2:
			trial.m_columns = 7;
			trial.m_rows = 7;
			trial.m_levels = 1;
			trial.m_xpPrize = 30;
			trial.m_gpPrize = 0;
			trial.m_cardMap = new int[] {0,0,0,9,0,0,0,0,0,0,1,0,0,0,0,0,1,1,1,0,0,9,1,1,8,1,1,9,0,0,1,1,1,0,0,0,0,0,1,0,0,0,0,0,0,9,0,0,0};
			trial.m_enemyMap = new int[] {0,0,0,0,0,0,0,0,0,0,25,0,0,0,0,0,4,0,1,0,0,0,25,0,0,0,25,0,0,0,1,0,4,0,0,0,0,0,25,0,0,0,0,0,0,0,0,0,0};
			trial.m_trapMap = new int[] {0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,1,3,1,0,0,0,2,3,0,3,2,0,0,0,1,3,1,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0};
			trial.m_lootMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			break;
		case 3:
			trial.m_columns = 7;
			trial.m_rows = 7;
			trial.m_levels = 1;
			trial.m_xpPrize = 40;
			trial.m_gpPrize = 0;
			trial.m_cardMap = new int[] {0,0,0,9,0,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,0,8,0,0,0,0,0,0,0,0,0,0};
			trial.m_enemyMap = new int[] {0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,34,0,34,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			trial.m_trapMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			trial.m_lootMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			break;
		case 4:
			trial.m_columns = 7;
			trial.m_rows = 7;
			trial.m_levels = 1;
			trial.m_xpPrize = 50;
			trial.m_gpPrize = 0;
			trial.m_cardMap = new int[] {0,0,0,9,0,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,0,8,0,0,0};
			trial.m_enemyMap = new int[] {0,0,0,0,0,0,0,0,0,0,29,0,0,0,0,0,20,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			trial.m_trapMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0};
			trial.m_lootMap = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			break;
		}

		return trial;
	}
}
