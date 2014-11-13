using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMenu : MonoBehaviour {

	public GameObject
		m_card,
		m_settingsManager;

	public GameObject[]
		m_cardTypes;

	void Awake () {
		if (SettingsManager.m_settingsManager == null)
		{
			Instantiate(m_settingsManager, Vector3.zero, Quaternion.identity);	
			SettingsManager.m_settingsManager.gameState = new GameState();
			SettingsManager.m_settingsManager.gameProgress = SettingsManager.m_settingsManager.gameState.loadState();
		}
	}

	// Use this for initialization
	void Start () {
	
		// build grid of cards
		int[] grid = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,1,1,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,1,0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,3,1,1,0,1,1,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,2,0,3,2,2,0,2,2,5,0,5,0,0,6,0,0,0,0,0,0,0,0,0,0,4,4,4,2,2,3,2,2,2,2,2,2,2,2,6,6,6,0,0,0,0,0,0,0,0,0,0,0,3,3,4,3,3,0,0,2,0,0,5,5,6,5,5,0,0,0,0,0,0,0,0,0,0,4,0,4,4,4,3,3,0,10,2,11,0,5,5,6,6,6,0,6,0,0,0,0,0,0,4,4,4,4,4,0,0,0,3,3,3,9,5,5,5,0,0,0,6,6,6,6,6,0,0,0,0,0,0,4,0,4,4,4,3,3,0,13,7,12,0,5,5,6,6,6,0,6,0,0,0,0,0,0,0,0,0,0,3,3,4,3,3,0,0,7,0,0,5,5,6,5,5,0,0,0,0,0,0,0,0,0,0,0,4,4,4,7,7,7,7,7,7,7,7,5,7,7,6,6,6,0,0,0,0,0,0,0,0,0,0,4,0,0,3,0,3,7,7,0,7,7,5,0,7,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,8,8,0,8,8,5,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,7,8,0,8,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,7,8,8,8,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,0,0,8,0,0,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
		int[] gridCardType = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,2,0,0,2,0,0,2,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,2,1,2,5,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,3,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,3,10,3,3,0,2,3,3,2,11,0,0,0,0,0,0,0,0,0,0,0,0,0,13,0,0,2,0,2,2,3,0,3,2,2,0,3,0,0,19,0,0,0,0,0,0,0,0,0,0,2,3,2,3,2,2,3,2,3,2,3,2,2,4,2,3,2,0,0,0,0,0,0,0,0,0,0,0,12,3,3,2,3,0,0,2,0,0,3,2,3,2,3,0,0,0,0,0,0,0,0,0,0,3,0,2,3,2,3,2,0,0,1,0,0,2,3,2,3,2,0,3,0,0,0,0,0,0,14,3,2,2,1,0,0,0,3,2,1,0,1,2,3,0,0,0,1,2,2,3,16,0,0,0,0,0,0,3,0,2,3,2,3,2,0,0,1,0,0,2,3,2,3,2,0,3,0,0,0,0,0,0,0,0,0,0,3,2,3,2,3,0,0,2,0,0,3,2,3,2,17,0,0,0,0,0,0,0,0,0,0,0,2,3,2,21,2,2,3,2,3,2,3,2,2,3,2,3,2,0,0,0,0,0,0,0,0,0,0,15,0,0,3,0,2,2,3,0,3,2,2,0,2,0,0,18,0,0,0,0,0,0,0,0,0,0,0,0,0,27,2,3,3,2,0,2,3,20,3,24,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,3,0,3,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,25,2,1,2,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,26,2,0,0,2,0,0,2,23,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,22,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

		List<Card> gridCards = new List<Card> ();

		int chunkXSize = 27;
		int chunkYSize = 27;
		float cardWidthOffset = 1.2f;
		float cardHeightOffset = 1.55f;
		int numChunks = 1;
		int chunkX = 0;
		int chunkY = 0;
		int centerCell = ((numChunks*2)+1)/2;
		int i=0;

		for (int x=0; x<chunkXSize; x++) {
			for (int y=0; y<chunkYSize; y++) {

				if (grid[i] != 0)
				{

					float chunkXOffset = ((float)chunkXSize) / 2.0f;
					float chunkYOffset = (((float)chunkYSize) / 2.0f) + (cardHeightOffset / 2.0f);

					int col = y;
					int row = x;

					Vector3 pos = new Vector3(y * cardWidthOffset,0,x * cardHeightOffset * -1);
					pos.x -= chunkXOffset;
					pos.z += chunkYOffset;

					//skew placement slightly
					Vector3 rot = m_card.transform.eulerAngles;
					rot.y += Random.Range(-2.5f, 2.5f);

					// spawn card
					Card newCard = (Card)((GameObject)Instantiate(m_card, pos, Quaternion.Euler(rot))).transform.GetComponent("Card");	
					newCard.Initialize(0, col, row, Card.CardState.Hidden, Card.CardType.Normal);

					//Initialize highlight color
					Color newColor = Color.black;

					switch (grid[i])
					{
					case 1:

						break;
					case 2:
						newColor = Color.red;
						if (IsUnlocked(Follower.FollowerType.Brand))
						{
							newCard.m_cardMesh.material = ((Card)m_cardTypes[2].transform.GetComponent("Card")).cardMesh.sharedMaterial;
							newCard.m_displayName = ((Card)m_cardTypes[2].transform.GetComponent("Card")).m_displayName;
							newCard.m_abilityText = ((Card)m_cardTypes[2].transform.GetComponent("Card")).m_abilityText;
							newCard.m_portraitSpriteName = ((Card)m_cardTypes[2].transform.GetComponent("Card")).m_portraitSpriteName;
							newCard.Initialize(i, col, row, Card.CardState.Normal, Card.CardType.Normal);
						}
						break;
					case 3:
						newColor = Color.yellow;
						if (IsUnlocked(Follower.FollowerType.Jin))
						{

						}
						break;
					case 4:
						newColor = Color.grey;
						break;
					case 5:
						newColor = Color.green;
						if (IsUnlocked(Follower.FollowerType.Telina))
						{

						}
						break;
					case 6:
						newColor = Color.cyan;
						break;
					case 7:
						newColor = Color.blue;
						if (IsUnlocked(Follower.FollowerType.August))
						{

						}
						break;
					case 8:
						newColor = Color.magenta;
						break;
					case 9:
						newColor = Color.white;

						newCard.m_cardMesh.material = ((Card)m_cardTypes[1].transform.GetComponent("Card")).cardMesh.sharedMaterial;
						newCard.m_displayName = ((Card)m_cardTypes[1].transform.GetComponent("Card")).m_displayName;
						newCard.m_abilityText = ((Card)m_cardTypes[1].transform.GetComponent("Card")).m_abilityText;
						newCard.m_portraitSpriteName = ((Card)m_cardTypes[1].transform.GetComponent("Card")).m_portraitSpriteName;
						newCard.Initialize(i, col, row, Card.CardState.Normal, Card.CardType.Exit);
						break;
					case 10:
						newColor = Color.white;
						break;
					case 11:
						newColor = Color.white;
						break;
					case 12:
						newColor = Color.white;
						break;
					case 13:
						newColor = Color.white;
						break;
					}

					newCard.SetColor(newColor);
					newCard.m_highlightMesh.material.color = newColor;
					gridCards.Add(newCard);
				}

				i++;
			}
		}

		foreach (Card thisCard in gridCards)
		{
			Card[] linkedCards = new Card[4];
			foreach (Card tempCard in gridCards)
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

		FollowCamera.m_followCamera.SetZoomDistance (0.25f);
	}

	private bool IsUnlocked (Follower.FollowerType fType)
	{
		if (SettingsManager.m_settingsManager.gameProgress.Count > 0) {
			foreach (GameState.ProgressState pS in SettingsManager.m_settingsManager.gameProgress)
			{
				if (pS.m_followerType == fType)
				{
					if (!pS.m_isLocked)
					{
						return true;
					}  else {return false;}
				}
			}
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () {
	

		// move camera based on keyboard input
		Vector3 newPos = Vector3.zero;
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			newPos.z += 4 * Time.deltaTime;
		} 
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			newPos.x -= 4 * Time.deltaTime;
		} 
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			newPos.z -= 4 * Time.deltaTime;
		} 
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			newPos.x += 4 * Time.deltaTime;
		}

		Vector3 mousePos = Input.mousePosition;
		float edgeBuffer = 200;
		float cornerBuffer = 300;
		if (mousePos.x < cornerBuffer && mousePos.y > Screen.height - cornerBuffer) {
			newPos.x -= 4 * Time.deltaTime;
			newPos.z += 4 * Time.deltaTime;
		} else if (mousePos.x < cornerBuffer && mousePos.y < cornerBuffer) {
			newPos.x -= 4 * Time.deltaTime;
			newPos.z -= 4 * Time.deltaTime;
		} else if (mousePos.x > Screen.width - cornerBuffer && mousePos.y < cornerBuffer) {
			newPos.x += 4 * Time.deltaTime;
			newPos.z -= 4 * Time.deltaTime;
		} else if (mousePos.x > Screen.width - cornerBuffer && mousePos.y > Screen.height - cornerBuffer) {
			newPos.x += 4 * Time.deltaTime;
			newPos.z += 4 * Time.deltaTime;
		}
		else if (mousePos.x < edgeBuffer) {
			newPos.x -= 4 * Time.deltaTime;
		} else if (mousePos.x > Screen.width - edgeBuffer) {
			newPos.x += 4 * Time.deltaTime;
		}
		else if (mousePos.y < edgeBuffer) {
			newPos.z -= 4 * Time.deltaTime;
		}
		else if (mousePos.y > Screen.height - edgeBuffer) {
			newPos.z += 4 * Time.deltaTime;
		}

		if (newPos != Vector3.zero)
		{
			FollowCamera.m_followCamera.MoveCamera(newPos);	
		}


		//zoom camera in / out
		if (Input.GetKey (KeyCode.LeftBracket)) {
			FollowCamera.m_followCamera.ChangeZoomDistance (-0.01f);
		} else if (Input.GetKey (KeyCode.RightBracket)) {
			FollowCamera.m_followCamera.ChangeZoomDistance (0.01f);
		} else if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			float zoomDist = Input.GetAxis ("Mouse ScrollWheel") * 0.05f;
			FollowCamera.m_followCamera.ChangeZoomDistance (zoomDist);
		}
	}
}
