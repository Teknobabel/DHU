using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	
	public static InputManager
		m_inputManager;
	
	private GameObject
		m_selectedObject = null;
	
	private List<GameObject>
		m_deactivatedGOs = new List<GameObject>();
	
	private Vector3
		m_lastMousePos = Vector3.zero; 

	private bool
		m_shiftHeld = false,
		m_cardsMoving = false;

	private UICard
		m_itemForEquipping = null;

	void Awake ()
	{
		m_inputManager = this;
	}

	public IEnumerator DoUpdate ()
	{
		if (Input.GetKeyUp(KeyCode.M))
		{
			if (AudioListener.volume == 0)
			{
				AudioListener.volume = 1;	
			} else if (AudioListener.volume == 1)
			{
				AudioListener.volume = 0;	
			}
		}

//		if (Input.GetKeyUp(KeyCode.P) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
//		{
//			CineCam.m_cineCam.ActivateCineCam(Player.m_player.m_playerMesh.gameObject.transform);
//		}

		if (Input.GetKeyUp(KeyCode.Escape) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
		{
			yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Pause));
		}
		if (Input.GetKeyUp (KeyCode.P) && !GameManager.m_gameManager.acceptInput && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
		{
			GameManager.m_gameManager.acceptInput = true;
		}
		if (Input.GetKeyUp(KeyCode.Space) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None
			&& GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.playerState == Player.PlayerState.Idle &&
			Player.m_player.currentActionPoints > 0 && GameManager.m_gameManager.acceptInput)
		{
			//Debug
//			GameManager.m_gameManager.accruedXP += 100;
//			UIManager.m_uiManager.UpdateXP(GameManager.m_gameManager.accruedXP);

			string newString = GameManager.m_gameManager.currentFollower.m_nameText + " passes ";
			UIManager.m_uiManager.UpdateActions (newString);

			Player.m_player.GainActionPoints((Player.m_player.currentActionPoints - 1) * -1);
			Player.m_player.UseActionPoint();	


			
			// Debug
//			foreach (Follower f in GameManager.m_gameManager.followers)
//			{
//				f.currentXP = 99;	
//			}
			
//			Player.m_player.GainActionPoints(99);
		}
		
//		if (Input.GetKeyUp(KeyCode.Q))
//		{
//			if (UIManager.m_uiManager.currentReactMode == UIManager.ReactMode.ForceActive)
//			{
//				StartCoroutine(UIManager.m_uiManager.ChangeReactMode(UIManager.ReactMode.MouseProx));	
//			}else if (UIManager.m_uiManager.currentReactMode == UIManager.ReactMode.MouseProx)
//			{
//				StartCoroutine(UIManager.m_uiManager.ChangeReactMode(UIManager.ReactMode.ForceActive));	
//			}
//		}
		
		if (Input.GetKeyUp(KeyCode.E) && GameManager.m_gameManager.acceptInput)
		{
			if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
			{
				//StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Inventory));
				yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Limbo));
			
			}
		} 
//		else if (Input.GetKeyUp(KeyCode.R) && GameManager.m_gameManager.acceptInput)
//		{
//			if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
//			{
//				yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Crafting));	
//			} 
//		}

		if (Input.GetKeyUp(KeyCode.Q) && GameManager.m_gameManager != null)
		{
			if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
			{
				GameManager.m_gameManager.ToggleStatBars();
			}
		}

		if (Input.GetKeyUp(KeyCode.Return) && Player.m_player.currentCard.type == Card.CardType.Exit && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.playerState == Player.PlayerState.Idle && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None  )
		{
			Debug.Log("DESCEND BUTTON CLICKED");
			UIManager.m_uiManager.m_exitButton.SetActive(false);
			
			foreach (Follower thisFollower in GameManager.m_gameManager.followers)
			{
				if (thisFollower.followerState == Follower.FollowerState.Spent)
				{
					yield return StartCoroutine(thisFollower.ChangeState(Follower.FollowerState.Normal));	
				}
			}
			//GameManager.m_gameManager.acceptInput = true;
			GameManager.m_gameManager.acceptInput = false;
			
			if (SettingsManager.m_settingsManager.difficultyLevel < 0)
			{
				if (Input.GetKey(KeyCode.P))
				{
					yield return StartCoroutine(MapManager.m_mapManager.NextLevel(GameManager.m_gameManager.currentMap.m_difficulty * -1));
				} else {
					yield return StartCoroutine(MapManager.m_mapManager.NextLevel(1));
				}
			} else {
				yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.EndLevel));
			}
			yield break;
		}

		if (Player.m_player.playerState == Player.PlayerState.Idle && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && !Player.m_player.cardsFlipping && GameManager.m_gameManager.acceptInput)
		{

			if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.North));
			}
			else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.West));
			}
			else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.South));
			}
			else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.East));
			}
			else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && Player.m_player.IsAdjacentOpen(GameManager.Direction.North) && Player.m_player.canContinuousMove && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.North));
			}
			else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && Player.m_player.IsAdjacentOpen(GameManager.Direction.West) && Player.m_player.canContinuousMove && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.West));
			}
			else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && Player.m_player.IsAdjacentOpen(GameManager.Direction.South) && Player.m_player.canContinuousMove && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.South));
			}
			else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && Player.m_player.IsAdjacentOpen(GameManager.Direction.East) && Player.m_player.canContinuousMove && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.East));
			}
			
			Vector3 newPos = Vector3.zero;
			if (((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
				&& FollowCamera.m_followCamera.transform.position.z < Player.m_player.transform.position.z + 0)
			{
				newPos.z += 4 * Time.deltaTime;
			} 
			if (((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
				&& FollowCamera.m_followCamera.transform.position.x > Player.m_player.transform.position.x - 3)
			{
				newPos.x -= 4 * Time.deltaTime;
			} 
			if (((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
				&& FollowCamera.m_followCamera.transform.position.z > Player.m_player.transform.position.z - 7)
			{
				newPos.z -= 4 * Time.deltaTime;
			} 
			if (((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
				&& FollowCamera.m_followCamera.transform.position.x < Player.m_player.transform.position.x + 3)
			{
				newPos.x += 4 * Time.deltaTime;
			}
			
			if (newPos != Vector3.zero)
			{
				FollowCamera.m_followCamera.MoveCamera(newPos);	
			}
			

			if (Input.GetKeyUp(KeyCode.Tab) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player)
			{
				// move camera through enemies based on distance to player
				GameManager.m_gameManager.ShiftFocus();
			}

			if (Input.GetKey(KeyCode.LeftBracket))
		   	{
				FollowCamera.m_followCamera.ChangeZoomDistance(-0.01f);
//		   		if (Camera.main.fieldOfView<=80)
//		   			Camera.main.fieldOfView +=2;
		   	} else if (Input.GetKey(KeyCode.RightBracket))
	    	{
				FollowCamera.m_followCamera.ChangeZoomDistance(0.01f);
//	    		if (Camera.main.fieldOfView>20)
//	    			Camera.main.fieldOfView -=2;
	    	}

			if (Input.GetKeyUp(KeyCode.Alpha0) && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP && 
			    GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.currentEnergy >= GameManager.m_gameManager.drawCost )
			{
				Player.m_player.GainEnergy(GameManager.m_gameManager.drawCost * -1);
				yield return StartCoroutine( GameManager.m_gameManager.FillHand());
//				UIManager.m_uiManager.RefreshInventoryMenu();
			}


			if (Input.GetKeyUp(KeyCode.Alpha1) && GameManager.m_gameManager.followers.Count > 0 && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.currentEnergy > 0 && GameManager.m_gameManager.acceptInput )
			{
				
				Follower thisF = GameManager.m_gameManager.followers[0];

				if (thisF != GameManager.m_gameManager.currentFollower)
				{
					GameManager.m_gameManager.ShowFollower(thisF, true);

//					UICard fCard = (UICard) UIManager.m_uiManager.m_followerCards[0].GetComponent("UICard");
//				 	yield return StartCoroutine(GameManager.m_gameManager.ActivateFollower(fCard.m_followerData));
				}
			}
			if (Input.GetKeyUp(KeyCode.Alpha2) && GameManager.m_gameManager.followers.Count > 1 && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.currentEnergy > 0 && GameManager.m_gameManager.acceptInput )
			{
				Follower thisF = GameManager.m_gameManager.followers[1];

				if (thisF != GameManager.m_gameManager.currentFollower)
				{
					
					GameManager.m_gameManager.ShowFollower(thisF, true);

//					UICard fCard = (UICard) UIManager.m_uiManager.m_followerCards[1].GetComponent("UICard");
//					yield return StartCoroutine(GameManager.m_gameManager.ActivateFollower(fCard.m_followerData));
				}
			}
			if (Input.GetKeyUp(KeyCode.Alpha3) && GameManager.m_gameManager.followers.Count > 2 && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.currentEnergy > 0 && GameManager.m_gameManager.acceptInput )
			{
				Follower thisF = GameManager.m_gameManager.followers[2];

				if (thisF != GameManager.m_gameManager.currentFollower)
				{
					
					GameManager.m_gameManager.ShowFollower(thisF, true);
//					UICard fCard = (UICard) UIManager.m_uiManager.m_followerCards[2].GetComponent("UICard");
//					yield return StartCoroutine(GameManager.m_gameManager.ActivateFollower(fCard.m_followerData));
				}
			}
			if (Input.GetKeyUp(KeyCode.Alpha4) && GameManager.m_gameManager.followers.Count > 3 && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.currentEnergy > 0  && GameManager.m_gameManager.acceptInput )
			{
				
				Follower thisF = GameManager.m_gameManager.followers[3];

				if (thisF != GameManager.m_gameManager.currentFollower)
				{
					
					GameManager.m_gameManager.ShowFollower(thisF, true);

//					UICard fCard = (UICard) UIManager.m_uiManager.m_followerCards[3].GetComponent("UICard");
//					yield return StartCoroutine(GameManager.m_gameManager.ActivateFollower(fCard.m_followerData));
				}
			}
			
//			Item it = null;
//			if (Input.GetKeyUp(KeyCode.Z) && EquipCards.m_equipCards.m_items[0].m_itemData != null && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player
//				&& Player.m_player.playerState == Player.PlayerState.Idle && GameManager.m_gameManager.acceptInput)
//			{
//				it = EquipCards.m_equipCards.m_items[0].m_itemData;
//			}
//			if (Input.GetKeyUp(KeyCode.X) && EquipCards.m_equipCards.m_items[1].m_itemData != null && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player
//				&& Player.m_player.playerState == Player.PlayerState.Idle && GameManager.m_gameManager.acceptInput)
//			{
//				it = EquipCards.m_equipCards.m_items[1].m_itemData;
//				
//			}
//			if (Input.GetKeyUp(KeyCode.C) && EquipCards.m_equipCards.m_items[2].m_itemData != null && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player
//				&& Player.m_player.playerState == Player.PlayerState.Idle && GameManager.m_gameManager.acceptInput)
//			{
//				it = EquipCards.m_equipCards.m_items[2].m_itemData;
//				
//			}
//			
//			if (it != null)
//			{
//				if (it.itemState == Item.ItemState.Normal && it.m_energyCost <= Player.m_player.currentEnergy)
//				{
//					Player.m_player.GainEnergy(it.m_energyCost * -1);
//					yield return StartCoroutine(it.ActivateItem());
//					
//					//check for single use item
//					if (it.HasKeyword(Item.Keyword.Consumeable))
//					{
//						//delete item
//						EquipCards.m_equipCards.RemoveItem(it);
//						
//						//remove item from equipped items list
//						for (int i=0; i<GameManager.m_gameManager.equippedItems.Count; i++)
//						{
//							Item thisItem = GameManager.m_gameManager.equippedItems[i];
//							if (it == thisItem)
//							{
//								GameManager.m_gameManager.equippedItems.RemoveAt(i);
//								break;
//							}
//						}
//						
//						//Destroy(item.gameObject);
//						it.gameObject.SetActive(false);
//					}
//				}	
//			}
			
			
			
//			if (Input.GetKeyUp(KeyCode.Alpha0) && UIManager.m_uiManager.numEquippedItems > 0)
//			{
//				UICard card = UIManager.m_uiManager.m_equipSlots[0];
//				Item equippedItem = card.m_itemData;
//				if (Player.m_player.currentEnergy >= equippedItem.m_energyCost && equippedItem.itemState == Item.ItemState.Normal)
//				{
//					Player.m_player.GainEnergy(equippedItem.m_energyCost * -1);
//					yield return StartCoroutine(equippedItem.ActivateItem());
//					
//					//check for single use item
//					if (equippedItem.HasKeyword(Item.Keyword.Consumeable))
//					{
//						//delete item
//						card.Deactivate();	
//						card.itemData = null;
//						card.gameObject.SetActive(false);
//						
//						//remove item from equipped items list
//						for (int i=0; i<GameManager.m_gameManager.equippedItems.Count; i++)
//						{
//							Item thisItem = GameManager.m_gameManager.equippedItems[i];
//							if (equippedItem == thisItem)
//							{
//								GameManager.m_gameManager.equippedItems.RemoveAt(i);
//								break;
//							}
//						}
//						
//						Destroy(equippedItem.gameObject);
//					}
//				}
//			}
//			if (Input.GetKeyUp(KeyCode.Alpha9) && UIManager.m_uiManager.numEquippedItems > 1)
//			{
//				UICard card = UIManager.m_uiManager.m_equipSlots[1];
//				Item equippedItem = card.m_itemData;
//				if (Player.m_player.currentEnergy >= equippedItem.m_energyCost && equippedItem.itemState == Item.ItemState.Normal)
//				{
//					Player.m_player.GainEnergy(equippedItem.m_energyCost * -1);
//					yield return StartCoroutine(equippedItem.ActivateItem());
//					
//					//check for single use item
//					if (equippedItem.HasKeyword(Item.Keyword.Consumeable))
//					{
//						//delete item
//						card.Deactivate();	
//						card.itemData = null;
//						card.gameObject.SetActive(false);
//						
//						//remove item from equipped items list
//						for (int i=0; i<GameManager.m_gameManager.equippedItems.Count; i++)
//						{
//							Item thisItem = GameManager.m_gameManager.equippedItems[i];
//							if (equippedItem == thisItem)
//							{
//								GameManager.m_gameManager.equippedItems.RemoveAt(i);
//								break;
//							}
//						}
//						
//						Destroy(equippedItem.gameObject);
//					}
//				}
//			}
//			if (Input.GetKeyUp(KeyCode.Alpha8) && UIManager.m_uiManager.numEquippedItems > 2)
//			{
//				UICard card = UIManager.m_uiManager.m_equipSlots[2];
//				Item equippedItem = card.m_itemData;
//				if (Player.m_player.currentEnergy >= equippedItem.m_energyCost && equippedItem.itemState == Item.ItemState.Normal)
//				{
//					Player.m_player.GainEnergy(equippedItem.m_energyCost * -1);
//					yield return StartCoroutine(equippedItem.ActivateItem());
//					
//					//check for single use item
//					if (equippedItem.HasKeyword(Item.Keyword.Consumeable))
//					{
//						//delete item
//						card.Deactivate();	
//						card.itemData = null;
//						card.gameObject.SetActive(false);
//						
//						//remove item from equipped items list
//						for (int i=0; i<GameManager.m_gameManager.equippedItems.Count; i++)
//						{
//							Item thisItem = GameManager.m_gameManager.equippedItems[i];
//							if (equippedItem == thisItem)
//							{
//								GameManager.m_gameManager.equippedItems.RemoveAt(i);
//								break;
//							}
//						}
//						
//						Destroy(equippedItem.gameObject);
//					}
//				}
//			}
			
			if (Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				float zoomDist = Input.GetAxis("Mouse ScrollWheel") * 0.05f;

				FollowCamera.m_followCamera.ChangeZoomDistance(zoomDist);

//				float zoomDist = Input.GetAxis("Mouse ScrollWheel") * 10;
//				
//				if (Camera.main.fieldOfView<=80 && zoomDist > 0)
//		   		{
//		   			Camera.main.fieldOfView +=zoomDist;
//			   	} else if (Camera.main.fieldOfView>20 && zoomDist < 0)
//		    	{
//		    		Camera.main.fieldOfView +=zoomDist;
//		    	}
			}
			
//			if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
//			{
//				//rotate camera view
//				Vector3 currentMousePos = Input.mousePosition;
//				if (m_lastMousePos == Vector3.zero)
//				{
//					m_lastMousePos = currentMousePos;	
//				}
//				float moveDist = (Vector3.Distance (currentMousePos, m_lastMousePos)) * 10;
//				if (currentMousePos.x < m_lastMousePos.x)
//				{
//					moveDist *= -1;	
//				}
//				FollowCamera.m_followCamera.RotateCamera(moveDist);
//				
//				m_lastMousePos = Input.mousePosition;
//			} else if (Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1))
//			{
//				m_lastMousePos = Vector3.zero;	
//			}



			if (Input.GetMouseButtonUp(1))
			{
				bool HudItemClicked = false;
				
				Ray cardTouchRay = UIManager.m_uiManager.m_uiCamera.camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(cardTouchRay, out hit))
				{
					if (hit.transform.gameObject.tag == "InvCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						UICard card = (UICard)hit.transform.GetComponent("UICard");
						Item thisItem = (Item)card.m_itemData;
						if (thisItem != null)
						{
							// Delete Inventory item
							HudItemClicked = true;
							Debug.Log("DELETE ITEM");
							
							List<Item> items = GameManager.m_gameManager.inventory;
							Item oldItem = null;
							for (int i=0; i < items.Count; i++)
							{
								Item invItem = items[i];
								if (invItem == thisItem)
								{
									items.RemoveAt(i);
									oldItem = invItem;
									//card.Deactivate();
									card.m_itemData = null;
									

									i=99;
								}
							}
							if (oldItem != null)
							{
								//Destroy(oldItem);

//								if (oldItem.m_graveBomb)
//								{
//									yield return new WaitForSeconds(0.25f);
//									yield return StartCoroutine(Player.m_player.TakeDirectDamage(1));
//									UIManager.m_uiManager.SpawnFloatingText("-1", UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
//									yield return new WaitForSeconds(0.5f);
//								}

								string newString = GameManager.m_gameManager.currentFollower.m_nameText + " sends " + oldItem.m_name + " to The Grave";
								UIManager.m_uiManager.UpdateActions (newString);

								// move card to grave
								float t = 0;
								float time = 0.5f;
								Vector3 startPos = card.transform.position;
								Vector3 startScale = card.transform.localScale;
								m_cardsMoving = true;
								while (t < time)
								{
									t += Time.deltaTime;;
									Vector3 nPos = Vector3.Lerp(startPos, UIManager.m_uiManager.m_backpackButton.transform.position , t / time);
									Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
									card.transform.position = nPos;
									card.transform.localScale = newScale;
									yield return null;
								}
								m_cardsMoving = false;
								card.transform.position = startPos;
								card.transform.localScale = startScale;

								//GameManager.m_gameManager.AddLimboCard(oldItem);
								GameManager.m_gameManager.numDiscardedThisTurn += 1;
								GameManager.m_gameManager.SendToGrave(oldItem);

								// activate any stat bonuses
								foreach (Item.GraveBonus gb in oldItem.m_graveBonus)
								{
									if (gb == Item.GraveBonus.Attack)
									{
										Player.m_player.turnDamage += 1;	
										StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage + Player.m_player.permDamage + Player.m_player.currentCard.siteDamageBonus));
									} else if (gb == Item.GraveBonus.Health && Player.m_player.currentHealth < Player.m_player.maxHealth)
									{
										Player.m_player.GainHealth(1);
									} else if (gb == Item.GraveBonus.Energy && Player.m_player.currentEnergy < Player.m_player.maxEnergy)
									{
										Player.m_player.GainEnergy(1);
									} else if (gb == Item.GraveBonus.Armor)
									{
										int armor = Player.m_player.turnArmor;
										armor += 1;
										Player.m_player.turnArmor = armor;
										StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor + Player.m_player.permArmor + Player.m_player.currentCard.siteArmorBonus ));
									} else if (gb == Item.GraveBonus.Actions)
									{
										Player.m_player.GainActionPoints(1);
									}
								}
							}

							if (UIManager.m_uiManager.invHoveredCard != null)
							{
								UIManager.m_uiManager.ClearInvSelection();	
							}
							
							UIManager.m_uiManager.RefreshInventoryMenu();

							// check for held keys
//							if (Input.GetKey(KeyCode.Z))
//							{
//								Player.m_player.turnDamage += 1;	
//								StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage));
//							} else if (Input.GetKey(KeyCode.X) && Player.m_player.currentHealth < Player.m_player.maxHealth)
//							{
//								Player.m_player.GainHealth(1);
//							} else if (Input.GetKey(KeyCode.C) && Player.m_player.currentEnergy < Player.m_player.maxEnergy)
//							{
//								Player.m_player.GainEnergy(1);
//							} else if (Input.GetKey(KeyCode.V))
//							{
//								int armor = Player.m_player.turnArmor;
//								armor += 1;
//								Player.m_player.turnArmor = armor;
//								StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor));f);
//							} else if (Input.GetKey(KeyCode.B))
//							{
//								Player.m_player.GainActionPoints(1);
//							}
						}

					} else if (hit.transform.gameObject.tag == "SkillCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP )
					{
						UICard card = (UICard)hit.transform.GetComponent("UICard");
						Item thisItem = (Item)card.m_itemData;
						if (thisItem != null)
						{
							// Remove Skill/Item and place in inventory
							HudItemClicked = true;
							Debug.Log("REMOVE ITEM/SKILL");

							//Item thisItem = thisCard.m_itemData;
							
//							if (thisItem.itemState == Item.ItemState.Normal)
//							{
//								//EquipCards.m_equipCards.RemoveItem(thisItem);
//								thisItem.attachedFollower.currentSkills --;
//								PartyCards.m_partyCards.RemoveSkill(thisItem);
//								GameManager.m_gameManager.inventory.Add(thisItem);
//								UIManager.m_uiManager.RefreshInventoryMenu();
//							}
						}
					}
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				bool HudItemClicked = false;
				
				Ray cardTouchRay = UIManager.m_uiManager.m_uiCamera.camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(cardTouchRay, out hit))
				{
					if (hit.transform.gameObject.tag == "DrawDeck" && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP && 
					    GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.currentEnergy >= GameManager.m_gameManager.drawCost )
					{
						string newString = GameManager.m_gameManager.currentFollower.m_nameText + "'s Hand is refilled";
						UIManager.m_uiManager.UpdateActions (newString);

						Player.m_player.GainEnergy(GameManager.m_gameManager.drawCost * -1);
						yield return StartCoroutine( GameManager.m_gameManager.FillHand());
//						UIManager.m_uiManager.RefreshInventoryMenu();
					}
					else if (hit.transform.gameObject.tag == "InvCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
				    {
						// Item card in backpack clicked
						HudItemClicked = true;

						UICard card = (UICard)hit.transform.GetComponent("UICard");
						Item thisItem = (Item)card.m_itemData;
						if (thisItem != null)
						{

							if (thisItem.adjustedEnergyCost <= Player.m_player.currentEnergy && !thisItem.HasKeyword(Item.Keyword.WhileInHand) && Player.m_player.currentCard.type != Card.CardType.Darkness)
							{
								yield return StartCoroutine(thisItem.Activate());
							}

//							if (thisItem.HasKeyword(Item.Keyword.Skill) && PartyCards.m_partyCards.CanEquipSkill(thisItem) && !thisItem.HasKeyword(Item.Keyword.Consumeable))
//							{
//								int numHeroes = PartyCards.m_partyCards.NumHeroesEligible(thisItem);
//								if ( numHeroes == 1)
//								{
//									Debug.Log("EQUIPPING SKILL");
//
////									//move card to hero
////									float t = 0;
////									float time = 0.3f;
////									Vector3 startPos = card.transform.position;
////									Vector3 startScale = card.transform.localScale;
////									while (t < time)
////									{
////										t += Time.deltaTime;;
////										Vector3 nPos = Vector3.Lerp(startPos, UIManager.m_uiManager.m_backpackButton.transform.position , t / time);
////										Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
////										card.transform.position = nPos;
////										card.transform.localScale = newScale;
////										yield return null;
////									}
////									
////									card.transform.position = startPos;
////									card.transform.localScale = startScale;
//
//
//									yield return StartCoroutine(PartyCards.m_partyCards.EquipSkill(thisItem, card));
//									
//									//remove item from inventory
//									List<Item> items = GameManager.m_gameManager.inventory;
//									GameObject oldItem = null;
//									
//									for (int i=0; i < items.Count; i++)
//									{
//										Item invItem = items[i];
//										if (invItem == thisItem)
//										{
//											items.RemoveAt(i);
//											oldItem = invItem.gameObject;
//											break;
//										}
//									}
//									
//									if (oldItem != null)
//									{
//										card.Deactivate();
//										card.m_itemData = null;
//									}
//									
//									if (UIManager.m_uiManager.invHoveredCard == card)
//									{
//										UIManager.m_uiManager.ClearInvSelection();	
//									}
//									
//									UIManager.m_uiManager.RefreshInventoryMenu();
//								} else if (numHeroes > 1)
//								{
//									Debug.Log("CHOSE HERO TO TAKE ITEM/SKILL");
//
//									yield return StartCoroutine(GetHeroSelection(thisItem, card));
//								}
//							} else if (thisItem.HasKeyword(Item.Keyword.Pet))
//							{
//								// check for open adjacent site
//								int adjacentOpen = 0;
//								foreach (Card thisCard in Player.m_player.currentCard.linkedCards)
//								{
//									if (thisCard != null)
//									{
//										if (!thisCard.isOccupied && thisCard.cardState == Card.CardState.Normal)
//										{
//											adjacentOpen ++;
//										}
//									}
//								}
//
//								if (adjacentOpen > 0)
//								{
//
//									yield return StartCoroutine(thisItem.ActivateItem());
//
//									//Debug.Log("SELECTED CARD: " + GameManager.m_gameManager.selectedCard);
//
//									if (GameManager.m_gameManager.selectedCard != null)
//									{
//										List<Item> items = GameManager.m_gameManager.inventory;
//										Item oldItem = null;
//										for (int i=0; i < items.Count; i++)
//										{
//											Item invItem = items[i];
//											if (invItem == thisItem)
//											{
//												items.RemoveAt(i);
//												oldItem = invItem;
//												//card.Deactivate();
//												card.m_itemData = null;
//												
//												
//												i=99;
//											}
//										}
//										if (oldItem != null)
//										{
//											//Destroy(oldItem);
//											// move card to limbo
//											float t = 0;
//											float time = 0.5f;
//											Vector3 startPos = card.transform.position;
//											Vector3 startScale = card.transform.localScale;
//											m_cardsMoving = true;
//											while (t < time)
//											{
//												t += Time.deltaTime;;
//												Vector3 nPos = Vector3.Lerp(startPos, UIManager.m_uiManager.m_backpackButton.transform.position , t / time);
//												Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
//												card.transform.position = nPos;
//												card.transform.localScale = newScale;
//												yield return null;
//											}
//											m_cardsMoving = false;
//											card.transform.position = startPos;
//											card.transform.localScale = startScale;
//											
//											//GameManager.m_gameManager.AddLimboCard(oldItem);
//											GameManager.m_gameManager.SendToGrave(oldItem);
//										}
//										
//										if (UIManager.m_uiManager.invHoveredCard != null)
//										{
//											UIManager.m_uiManager.ClearInvSelection();	
//										}
//										
//										UIManager.m_uiManager.RefreshInventoryMenu();
//
//										GameManager.m_gameManager.selectedCard = null;
//									}
//								}
//							} else if (thisItem.HasKeyword(Item.Keyword.Skill) && thisItem.HasKeyword(Item.Keyword.Consumeable) && PartyCards.m_partyCards.CanEquipSkill(thisItem))
//							{
//								// ACTIVATE SINGLE USE ITEM
//
//								if (thisItem.itemState == Item.ItemState.Normal && thisItem.m_energyCost <= Player.m_player.currentEnergy)
//								{
//									yield return StartCoroutine(thisItem.ActivateItem());
//								}
//							}
						}

					}
					else if ((hit.transform.gameObject.tag == "FollowerCard" || hit.transform.gameObject.tag == "LeaderCard") && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						HudItemClicked = true;
						UICard fCard = (UICard) hit.transform.gameObject.GetComponent("UICard");
						if (fCard.m_followerData != null)
						{
							Follower thisF = fCard.m_followerData;
							if (thisF.followerState == Follower.FollowerState.Normal && GameManager.m_gameManager.currentFollower != thisF)
							{
								GameManager.m_gameManager.ShowFollower(thisF, true);
					 			//yield return StartCoroutine(GameManager.m_gameManager.ActivateFollower(fCard.m_followerData));
							}
						}
					} else if ((hit.transform.gameObject.tag == "EquipCard" || hit.transform.gameObject.tag == "SkillCard") && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						HudItemClicked = true;
						
						UICard eCard = (UICard) hit.transform.gameObject.GetComponent("UICard");
						if (eCard.m_itemData != null)
						{
							Item item = eCard.m_itemData;
							
							if (item.m_energyCost <= Player.m_player.currentEnergy)
							{
								Player.m_player.GainEnergy(item.m_energyCost * -1);
								if (item.m_healthCost > 0)
								{
									yield return StartCoroutine(Player.m_player.TakeDirectDamage(item.m_healthCost));
								}

//								yield return StartCoroutine(item.ActivateItem());
								
								//check for single use item
//								if (item.HasKeyword(Item.Keyword.Consumeable))
//								{
//									if (item.HasKeyword(Item.Keyword.Skill))
//									{
//										PartyCards.m_partyCards.RemoveSkill(item);
//										item.gameObject.SetActive(false);
//									}
//									else if (item.HasKeyword(Item.Keyword.Equippable))
//									{
//										//delete item
//										EquipCards.m_equipCards.RemoveItem(item);
//										
//										//remove item from equipped items list
//										for (int i=0; i<GameManager.m_gameManager.equippedItems.Count; i++)
//										{
//											Item thisItem = GameManager.m_gameManager.equippedItems[i];
//											if (item == thisItem)
//											{
//												GameManager.m_gameManager.equippedItems.RemoveAt(i);
//												break;
//											}
//										}
//										
//										//Destroy(item.gameObject);
//										item.gameObject.SetActive(false);
//									}
//								}
							}
						}
					} else if (hit.transform.gameObject.tag == "DescendButton" && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.playerState == Player.PlayerState.Idle)
					{
						Debug.Log("DESCEND BUTTON CLICKED");
						UIManager.m_uiManager.m_exitButton.SetActive(false);
						
						foreach (Follower thisFollower in GameManager.m_gameManager.followers)
						{
							if (thisFollower.followerState == Follower.FollowerState.Spent)
							{
								yield return StartCoroutine(thisFollower.ChangeState(Follower.FollowerState.Normal));	
							}
						}
						//GameManager.m_gameManager.acceptInput = true;
						GameManager.m_gameManager.acceptInput = false;

						if (SettingsManager.m_settingsManager.difficultyLevel < 0)
						{
							if (Input.GetKey(KeyCode.P))
							{
								yield return StartCoroutine(MapManager.m_mapManager.NextLevel(GameManager.m_gameManager.currentMap.m_difficulty * -1));
							} else {
								yield return StartCoroutine(MapManager.m_mapManager.NextLevel(1));
							}
						} else {
							yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.EndLevel));
						}
						yield break;
					} 
					else if (hit.transform.gameObject.tag == "PauseButton")
					{
						Debug.Log("PAUSE BUTTON CLICKED");
						yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Pause));
					}
					else if (hit.transform.gameObject.tag == "LimboButton")
					{
						Debug.Log("LIMBO BUTTON CLICKED");
						yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Limbo));
					}
					else if (hit.transform.gameObject.tag == "SkipButton"&& GameManager.m_gameManager.currentTurn == GameManager.Turn.Player && Player.m_player.playerState == Player.PlayerState.Idle
					&& GameManager.m_gameManager.acceptInput && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						Debug.Log("SKIP BUTTON CLICKED");
						Player.m_player.GainActionPoints((Player.m_player.currentActionPoints - 1) * -1);
						Player.m_player.UseActionPoint();	
					}
					else if (hit.transform.gameObject.tag == "FuseButton" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						Debug.Log("FUSE BUTTON CLICKED");
						FuseButton fb = (FuseButton)hit.transform.GetComponent("FuseButton");
						
						// remove used resources from inventory
						foreach (UICard c in fb.cardList)
						{
							Item invItem = c.itemData;
							for (int i=0; i < GameManager.m_gameManager.inventory.Count; i++)
							{
								if (invItem.gameObject == GameManager.m_gameManager.inventory[i].gameObject)
								{
									Item oldItem = GameManager.m_gameManager.inventory[i];
									GameManager.m_gameManager.inventory.RemoveAt(i);
									Destroy(oldItem);
									i = 99;
								}
							}
						}
						
						// add new item to inventory
						GameObject item = (GameObject)Instantiate(fb.craftingResult.gameObject, Vector3.zero, fb.craftingResult.transform.rotation);
						GameManager.m_gameManager.inventory.Add((Item)(item.GetComponent("Item")));
						
						// refresh inventory	
						UIManager.m_uiManager.RefreshInventoryMenu();
					} 
				} 
				
				if (!HudItemClicked && !GameManager.m_gameManager.selectMode)
				{ 
					
					//no HUD items clicked, check for clicks in game view
					Ray r = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
					RaycastHit h;
					if(Physics.Raycast(r, out h))
					{
						Card c = null;
						
						if (h.transform.gameObject.tag == "Card")
						{
							c = (Card)h.transform.GetComponent("Card");
						}
						else if (h.transform.gameObject.tag == "Enemy")
						{
							Enemy e = (Enemy)h.transform.GetComponent("Enemy");
							c = (Card)e.currentCard;
						} else if (h.transform.gameObject.tag == "Chest")
						{
							Chest ch = (Chest)h.transform.GetComponent("Chest");
							c = (Card)ch.currentCard;
						} else if (h.transform.gameObject.tag == "Follower")
						{
							Follower f = (Follower)h.transform.GetComponent("Follower");
							c = (Card)f.currentCard;
						}
						
						if (c != null)
						{
							if (c.distanceToPlayer == 1)
							{
								//get direction from player to call move to
								for (int i=0; i < c.linkedCards.Length; i++)
								{
									Card lc = c.linkedCards[i];
									
									if (lc != null)
									{
										if (lc.player != null)
										{
											//player present on adjacent card
											if (i == 0)
											{
												yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.South));	
												i = 99;
											} else if (i == 1)
											{
												yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.North));
												i = 99;
											}
											 else if (i == 2)
											{
												yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.West));
												i = 99;
											}
											 else if (i == 3)
											{
												yield return StartCoroutine(Player.m_player.MovePlayer(GameManager.Direction.East));
												i = 99;
											}
										}
									}
								}
							}
						}
					}
				}
			} else {

				//MOUSE HOVER
				
				Ray worldTouchRay = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				bool objectTouched = false;
				if(Physics.Raycast(worldTouchRay, out hitInfo))
				{
					if (hitInfo.transform.gameObject.tag == "Enemy")
					{
						objectTouched = true;
						if (m_selectedObject != hitInfo.transform.gameObject)
						{
							m_selectedObject = hitInfo.transform.gameObject;
							Enemy thisEnemy = (Enemy)hitInfo.transform.GetComponent("Enemy");
							yield return StartCoroutine( UIManager.m_uiManager.DisplayTargetEnemy(thisEnemy));
						}
					} else if (hitInfo.transform.gameObject.tag == "Follower")
					{
						objectTouched = true;
						if (m_selectedObject != hitInfo.transform.gameObject)
						{
							m_selectedObject = hitInfo.transform.gameObject;
							Follower thisFollower = (Follower)hitInfo.transform.GetComponent("Follower");
							UIManager.m_uiManager.DisplayTargetCard(thisFollower);
						}
					}  else if (hitInfo.transform.gameObject.tag == "Card")
					{
						Card thisCard = (Card)hitInfo.transform.GetComponent("Card");
						if ((thisCard.type == Card.CardType.Tower || thisCard.type == Card.CardType.Fort || thisCard.type == Card.CardType.Gate || thisCard.type == Card.CardType.HighGround
							 || thisCard.type == Card.CardType.Quicksand || thisCard.type == Card.CardType.Trap_Razorvine || thisCard.type == Card.CardType.Warren || thisCard.type == Card.CardType.Fire
							|| thisCard.type == Card.CardType.Stalactite || thisCard.type == Card.CardType.Spore || thisCard.type == Card.CardType.Spikes || thisCard.type == Card.CardType.Unholy
							|| thisCard.type == Card.CardType.AshCloud || thisCard.type == Card.CardType.Magma || thisCard.type == Card.CardType.Mine
							|| thisCard.type == Card.CardType.Exit || thisCard.type == Card.CardType.Darkness || thisCard.type == Card.CardType.ClawingHands || thisCard.type == Card.CardType.Whispers
							|| thisCard.type == Card.CardType.FrostSnap || thisCard.type == Card.CardType.RazorGlade || thisCard.type == Card.CardType.ManaBurn || thisCard.type == Card.CardType.BrokenGround) && thisCard.cardState == Card.CardState.Normal)
						{
							
							objectTouched = true;
							if (m_selectedObject != hitInfo.transform.gameObject)
							{
								m_selectedObject = hitInfo.transform.gameObject;
								yield return StartCoroutine( UIManager.m_uiManager.DisplayTargetCard(thisCard, UIManager.m_uiManager.m_followerCards[4]));
							}
						}
					}
					  else if (hitInfo.transform.gameObject.tag == "Chest")
					{
						objectTouched = true;
						if (m_selectedObject != hitInfo.transform.gameObject)
						{
							m_selectedObject = hitInfo.transform.gameObject;
							Chest thisChest = (Chest)hitInfo.transform.GetComponent("Chest");
							yield return StartCoroutine( UIManager.m_uiManager.DisplayTargetCard(thisChest));
						}
					} else if (hitInfo.transform.gameObject.tag == "Shop")
					{
						objectTouched = true;
						if (m_selectedObject != hitInfo.transform.gameObject)
						{
							m_selectedObject = hitInfo.transform.gameObject;
							Shop s = (Shop)hitInfo.transform.GetComponent("Shop");
							yield return StartCoroutine( UIManager.m_uiManager.DisplayTargetCard(s));
						}
					}
				}
				if (!objectTouched && UIManager.m_uiManager.targetDisplayed && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None && m_selectedObject != null)
				{
					m_selectedObject = null;
					yield return StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
				}
				
				DoHoveredCards();
				
//				Ray cardTouchRay = UIManager.m_uiManager.m_uiCamera.camera.ScreenPointToRay(Input.mousePosition);
//				RaycastHit hit;
//				if(Physics.Raycast(cardTouchRay, out hit))
//				{



//					if ((hit.transform.gameObject.tag == "FollowerCard" || hit.transform.gameObject.tag == "LeaderCard") && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
//					{
//						PartyCards.m_partyCards.CardHovered(hit.transform.gameObject);
//					} else if (PartyCards.m_partyCards.hoveredCard != null)
//					{
//						PartyCards.m_partyCards.ClearSelection();	
//					}
//					
//					if (hit.transform.gameObject.tag == "EquipCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
//					{
//						EquipCards.m_equipCards.CardHovered(hit.transform.gameObject);
//					} else if (EquipCards.m_equipCards.hoveredCard != null)
//					{
//						EquipCards.m_equipCards.ClearSelection();	
//					}
//				} 
//					else
//				{
//					if (PartyCards.m_partyCards.hoveredCard != null)
//					{
//						PartyCards.m_partyCards.ClearSelection();	
//					}
//					
//					if (EquipCards.m_equipCards.hoveredCard != null)
//					{
//						EquipCards.m_equipCards.ClearSelection();	
//					}
//				}
			}
		}
		
		if (!Player.m_player.canContinuousMove)
		{
			if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
			{
				Player.m_player.canContinuousMove = true;	
			}
			if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
			{
				Player.m_player.canContinuousMove = true;	
			}
			if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
			{
				Player.m_player.canContinuousMove = true;	
			}
			if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				Player.m_player.canContinuousMove = true;	
			}
		}
	}
	
	public IEnumerator GetMouseSelection (List<Card> validCards)
	{		
		bool cardSelected = false;
		Vector3 clickStartPos = Vector3.one * 100;
		
		while (!cardSelected)
		{
			DoHoveredCards();
			if (Input.GetKeyDown(KeyCode.Return) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.EndLevel)
			{
				foreach (Follower thisFollower in GameManager.m_gameManager.followers)
				{
					//update progressState
					for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
					{
						GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[i];
						if (progress.m_followerType == thisFollower.m_followerType && !thisFollower.isLocked)
						{
							progress.m_level = thisFollower.currentLevel;
							progress.m_XP = thisFollower.currentXP;
							GameManager.m_gameManager.gameProgress[i] = progress;
							i=99;
						}
					}
				}

				if (SettingsManager.m_settingsManager.trial)
				{
					UIManager.m_uiManager.leaveDungeon = true;
				}
				
				GameManager.m_gameManager.gameState.saveState();
				
				yield break;
			}

			if (!m_shiftHeld && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			{
				m_shiftHeld = true;
				UIManager.m_uiManager.HighlightConsumables(true);

			} else if (m_shiftHeld && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory)
			{
				m_shiftHeld = false;
				UIManager.m_uiManager.HighlightConsumables(false);
			}
			
			if (Input.GetKeyUp(KeyCode.Escape))
			{
//				if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory || UIManager.m_uiManager.menuMode == UIManager.MenuMode.Crafting || UIManager.m_uiManager.menuMode == UIManager.MenuMode.FollowerSwap || UIManager.m_uiManager.menuMode == UIManager.MenuMode.Chest
//					|| UIManager.m_uiManager.menuMode == UIManager.MenuMode.Storage|| UIManager.m_uiManager.menuMode == UIManager.MenuMode.Pause || GameManager.m_gameManager.selectMode || UIManager.m_uiManager.menuMode == UIManager.MenuMode.Shop)
				if (UIManager.m_uiManager.menuMode != UIManager.MenuMode.None || (UIManager.m_uiManager.menuMode == UIManager.MenuMode.None && GameManager.m_gameManager.selectMode ))
				{
					return true;
				}
			} else if (Input.GetKeyUp(KeyCode.E) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Limbo)
			{
				return true;
			} 
//			else if (Input.GetKeyUp(KeyCode.Return) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Chest && UIManager.m_uiManager.chestCard != null)
//			{
//				// take card
//				UICard card = UIManager.m_uiManager.chestCard;
//				UIManager.m_uiManager.chestCard = null;
//				Item itemRef = card.m_itemData;
//				card.m_itemData = null;
//				List<Item> inventory = GameManager.m_gameManager.inventory;
//				inventory.Add(itemRef);
//				GameManager.m_gameManager.inventory = inventory;
//				
//				//animated card moving
//				float t = 0;
//				float time = 0.3f;
//				Vector3 startPos = card.transform.position;
//				Vector3 startScale = card.transform.localScale;
//				m_cardsMoving = true;
//				while (t < time)
//				{
//					t += Time.deltaTime;
//					//Vector3 newPos = Vector3.Lerp(startPos, UIManager.m_uiManager.m_backpackButton.transform.position, t / time);
//					Vector3 newPos = Vector3.Lerp(startPos, AssetManager.m_assetManager.m_props[7].transform.position , t / time);
//					Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
//					card.transform.position = newPos;
//					card.transform.localScale = newScale;
//					yield return null;
//				}
//				m_cardsMoving = false;
//				UIManager.m_uiManager.RefreshInventoryMenu();
//				
//				yield return new WaitForSeconds(0.3f);
//				
//				yield break;
//			}


			if (Input.GetMouseButtonDown(0))
			{
				clickStartPos = Input.mousePosition;	
			}
			
			//right clicking is generally used for removing items
			
			if (Input.GetMouseButtonUp(1))
			{
				Camera inputCam = Camera.mainCamera;
				if (UIManager.m_uiManager.menuMode != UIManager.MenuMode.None)
				{
					inputCam = UIManager.m_uiManager.m_uiCamera.camera;
				}
				Ray worldTouchRay = inputCam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if(Physics.Raycast(worldTouchRay, out hitInfo))
				{
					if (hitInfo.transform.gameObject.tag == "UICard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory)
					{
						//delete touched item
						Debug.Log("REMOVING ITEM");	
						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
						Item thisItem = (Item)card.m_itemData;
						
						List<Item> items = GameManager.m_gameManager.inventory;
						GameObject oldItem = null;
						for (int i=0; i < items.Count; i++)
						{
							Item invItem = items[i];
							if (invItem == thisItem)
							{
								items.RemoveAt(i);
								oldItem = invItem.gameObject;
								card.Deactivate();
								card.m_itemData = null;
								
								if (UIManager.m_uiManager.invHoveredCard != null)
								{
									UIManager.m_uiManager.ClearInvSelection();	
								}
								i=99;
							}
						}
						if (oldItem != null)
						{
							Destroy(oldItem);
						}
						
						UIManager.m_uiManager.RefreshInventoryMenu();
						
					} 
//					else if (hitInfo.transform.gameObject.tag == "InvCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Shop)
//					{
//						// destroy item and increase gold
//
//						//delete touched item
//						Debug.Log("REMOVING ITEM");	
//						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
//						Item thisItem = (Item)card.m_itemData;
//						
//						List<Item> items = GameManager.m_gameManager.inventory;
//						GameObject oldItem = null;
//						for (int i=0; i < items.Count; i++)
//						{
//							Item invItem = items[i];
//							if (invItem == thisItem)
//							{
//								items.RemoveAt(i);
//								oldItem = invItem.gameObject;
//								//card.Deactivate();
//								card.m_itemData = null;
//								
//								if (UIManager.m_uiManager.invHoveredCard != null)
//								{
//									UIManager.m_uiManager.ClearInvSelection();	
//								}
//								i=99;
//							}
//						}
//						if (oldItem != null)
//						{
//							Destroy(oldItem);
//						}
//						
//						UIManager.m_uiManager.RefreshInventoryMenu();
//
//						SettingsManager.m_settingsManager.gold += 2;
//						UIManager.m_uiManager.UpdateGoldUI();
//					}
					else if (hitInfo.transform.gameObject.tag == "FollowerCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory)
					{
						
						//delete touched follower
						UICard followerCard = (UICard)hitInfo.transform.GetComponent("UICard");	
						
						if (followerCard.m_followerData != null)
						{
							PartyCards.m_partyCards.RemoveFollower(followerCard.m_followerData);
						}
						
						
						
						
						
//						Debug.Log("REMOVING FOLLOWER");	
//						UICard followerCard = (UICard)hitInfo.transform.GetComponent("UICard");
//						
//						//remove follower's passive bonuses if any
//						List<Follower> oldfollowerList = new List<Follower>();
//						Follower oldFollower = (Follower)followerCard.m_followerData;
//						oldfollowerList.Add(oldFollower);
//						Player.m_player.RemovePassiveFollowerBonuses(oldfollowerList);
//						
//						//remove follower from list
//						List<Follower> fList = GameManager.m_gameManager.followers;
//						for (int i=0; i < fList.Count; i++)
//						{
//							Follower thisF = fList[i];
//							if (thisF == oldFollower)
//							{
//								fList.RemoveAt(i);
//								
//								//Destroy(oldFollower);
//								oldFollower.gameObject.SetActive(false);
//								oldFollower = null;
//								i=99;
//							}
//						}
//						
//						//refresh follower cards
//						UIManager.m_uiManager.SetFollowers(GameManager.m_gameManager.followers);
					} 
//					else if (hitInfo.transform.gameObject.tag == "EquipCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory && GameManager.m_gameManager.inventory.Count < 10)
//					{
//						//return equipped item to inventory					
//						UICard thisCard = (UICard)hitInfo.transform.GetComponent("UICard");
//						if (thisCard.m_portrait.spriteName != "Card_Back03")
//						{
//							Item thisItem = thisCard.m_itemData;
//							
//							if (thisItem.itemState == Item.ItemState.Normal)
//							{
//								foreach (UICard equipCard in UIManager.m_uiManager.m_equipSlots)
//								{
//									if (equipCard.m_itemData == thisItem)
//									{
//										GameManager.m_gameManager.inventory.Add(thisItem);
//										equipCard.Deactivate();	
//										equipCard.itemData = null;
//										//equipCard.gameObject.SetActive(false);
//										
//										//remove item from equipped items list
//										for (int i=0; i<GameManager.m_gameManager.equippedItems.Count; i++)
//										{
//											Item item = GameManager.m_gameManager.equippedItems[i];
//											if (item == thisItem)
//											{
//												GameManager.m_gameManager.equippedItems.RemoveAt(i);
//												break;
//											}
//										}
//										break;
//									}
//								}
//								
//								//activate deactivated card
//								for (int i=0; i < UIManager.m_uiManager.cardList.Count; i++)
//								{
//									UICard invCard = (UICard)UIManager.m_uiManager.cardList[i].GetComponent("UICard");
//									if (invCard.m_itemData == null)
//									{
//										invCard.m_cardSprite.spriteName = "Card_Front01";
//										invCard.m_nameUI.gameObject.SetActive(true);
//										invCard.m_nameUI.text = thisItem.m_name;
//										invCard.m_abilityUI.gameObject.SetActive(true);
//										invCard.m_abilityUI.text = thisItem.m_description;
//										invCard.m_portrait.gameObject.SetActive(true);
//										invCard.m_portrait.spriteName = thisItem.m_portraitSpriteName;
//										invCard.itemData = thisItem;
//										break;
//									}
//								}
//							}
//						}
//					} 
					else if (hitInfo.transform.gameObject.tag == "FollowerCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.CharSelect)
					{
						UICard thisCard = (UICard)hitInfo.transform.GetComponent("UICard");
						if (thisCard.m_nameUI.text != "Name")
						{
							thisCard.Deactivate();	
							List<Follower> pFollowers = new List<Follower>();
							//resort cards
							foreach (UICard pCard in UIManager.m_uiManager.m_charSelectSlots)
							{
								if (pCard.m_nameUI.text != "Name")
								{
									pFollowers.Add(pCard.m_followerData);
									pCard.Deactivate();
								}
							}
							
							foreach (GameObject cardGO in UIManager.m_uiManager.cardList)
							{
								UICard listCard = (UICard)cardGO.GetComponent("UICard");
								if (listCard.m_followerData != null)
								{
									if (listCard.m_portrait.spriteName == "Card_Back03" && listCard.m_followerData.m_followerType == thisCard.m_followerData.m_followerType)
									{
										listCard.m_portrait.spriteName = listCard.m_followerData.m_portraitSpriteName;
										listCard.m_nameUI.text = listCard.m_followerData.m_nameText;
										listCard.m_nameUI.gameObject.SetActive(true);
										listCard.m_abilityUI.text = listCard.m_followerData.m_abilityText;
										listCard.m_abilityUI.gameObject.SetActive(true);
										break;
									}
								}
							}
							
							if (pFollowers.Count > 0)
							{
								for (int i=0; i < pFollowers.Count; i++)
								{
									Follower thisF = (Follower)pFollowers[i];
									UICard fSlot = (UICard)UIManager.m_uiManager.m_charSelectSlots[i];
									
									fSlot.m_nameUI.gameObject.SetActive(true);
									fSlot.m_nameUI.text = thisF.m_nameText;
									fSlot.m_abilityUI.gameObject.SetActive(true);
									fSlot.m_abilityUI.text = thisF.m_abilityText;
									fSlot.m_portrait.spriteName = thisF.m_portraitSpriteName;
									fSlot.m_followerData = thisF;
								}
								pFollowers.Clear();
							}
						}
					}
				}
			}
			
			
			
			
			
			
			if (Input.GetMouseButtonUp(0))
			{
				Vector3 clickEndPos = Input.mousePosition;
				float clickDist = Vector3.Distance(clickStartPos, clickEndPos);
				
				Camera inputCam = Camera.mainCamera;
				if (UIManager.m_uiManager.menuMode != UIManager.MenuMode.None)
				{
					inputCam = UIManager.m_uiManager.m_uiCamera.camera;
				}
				Ray worldTouchRay = inputCam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if(Physics.Raycast(worldTouchRay, out hitInfo) && clickDist < 6)
				{
					if (hitInfo.transform.gameObject.tag == "BackButton")
					{
						return true;
					}
					else if (hitInfo.transform.gameObject.tag == "YesButton" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Shop)
					{
						//apply effect
						Shop.m_shop.ApplyEffect();

						return true;
					}
					else if (hitInfo.transform.gameObject.tag == "NoButton" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Shop)
					{
						return true;
					}
					else if (hitInfo.transform.gameObject.tag == "Card" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						Card selectedCard = (Card)hitInfo.transform.GetComponent("Card");
						//check if it is a valid card
						foreach (Card vc in validCards)
						{
							if (vc == selectedCard)
							{
								cardSelected = true;
								GameManager.m_gameManager.selectedCard = vc;
								return true;
							}
						}
					} else if (hitInfo.transform.gameObject.tag == "Enemy" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						Enemy selectedEnemy = (Enemy)hitInfo.transform.GetComponent("Enemy");
						Card selectedCard = (Card)selectedEnemy.currentCard;
						//check if it is a valid card
						foreach (Card vc in validCards)
						{
							if (vc == selectedCard)
							{
								cardSelected = true;
								GameManager.m_gameManager.selectedCard = vc;
								return true;
							}
						}
					} else if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.CharSelect)
					{
						if (hitInfo.transform.gameObject.tag == "UICard")
						{
							//Debug.Log("CHARACTER SELECTED");
							UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
							bool alreadySelected = false;
							if (card.m_portrait.spriteName != "Card_Back03")
							{
//								foreach (UICard thisCard in UIManager.m_uiManager.m_charSelectSlots)
//								{
//									if (thisCard.m_portrait.spriteName != "Card_Back03")
//									{
//										if (thisCard.m_followerData.m_followerType == card.m_followerData.m_followerType)
//										{
//											alreadySelected = true;
//											break;
//										}
//									}
//								}
							}
							if (card.m_followerData != null && !alreadySelected && card.m_portrait.spriteName != "Card_Back03")
							{
								Follower selectedFollower = card.m_followerData;
								if (selectedFollower != null)
								{
									//activate char in party UI
//									UIManager.m_uiManager.SetCharSelectSlot(card);
									PartyCards.m_partyCards.AddFollower(selectedFollower);
									
									//update stats
									int level = 0;
									foreach (GameState.ProgressState progressState in GameManager.m_gameManager.gameProgress)
									{
										if (selectedFollower.m_followerType == progressState.m_followerType)
										{
											level = progressState.m_level;
											break;
										}
										
									}
										Follower.Level l = selectedFollower.m_levelTable[level];
										if (l != null)
										{
//										SettingsManager.m_settingsManager.startingHealth += l.m_healthMod;
//										SettingsManager.m_settingsManager.startingEnergy += l.m_energyMod;
//										SettingsManager.m_settingsManager.startingDamage += l.m_damageMod;
//										SettingsManager.m_settingsManager.startingArmor += l.m_armorMod;
//										
//										UIManager.m_uiManager.UpdateHealth(SettingsManager.m_settingsManager.startingHealth);	
//										UIManager.m_uiManager.UpdateEnergy(SettingsManager.m_settingsManager.startingEnergy);
//										StartCoroutine(UIManager.m_uiManager.UpdateDamage(SettingsManager.m_settingsManager.startingDamage));
//										StartCoroutine(UIManager.m_uiManager.UpdateArmor(SettingsManager.m_settingsManager.startingArmor));
									}
									
									card.Deactivate();
									//GameManager.m_gameManager.playerFollower = selectedFollower;
								}
							}
						} else if (hitInfo.transform.gameObject.tag == "CraftButton")
						{
							//add any selected followers to party
							List<GameObject> party = new List<GameObject>();

							foreach (UICard thisCard in PartyCards.m_partyCards.m_party)
							{
								if (thisCard.m_followerData != null)
								{
									foreach (GameObject bFollower in GameManager.m_gameManager.m_followerBank)
									{
										Follower bF = (Follower)bFollower.GetComponent("Follower");
										if (bF.m_followerType == thisCard.m_followerData.m_followerType)
										{
											party.Add(bFollower);
										}
									}
								}
							}
							
							if (party.Count > 0)
							{
								GameManager.m_gameManager.playerFollower = (Follower)party[0].GetComponent("Follower");
								party.RemoveAt(0);
								
								foreach (GameObject thisF in party)
								{
									Vector3 pos = Vector3.one * 1000;
									Follower thisFollower = (Follower)((GameObject)Instantiate(thisF, pos, thisF.transform.rotation)).GetComponent("Follower");
									GameManager.m_gameManager.followers.Add(thisFollower);	
									
									//GameState.ProgressState charProgress = GameManager.m_gameManager.gameProgress[0];
									foreach (GameState.ProgressState thisCharState in GameManager.m_gameManager.gameProgress)
									{
										if (thisCharState.m_followerType == thisFollower.m_followerType)
										{
											thisFollower.isLocked = thisCharState.m_isLocked;
											break;
										}
									}
								}
								
								
								cardSelected = true;
							}
						} else if (hitInfo.transform.gameObject.tag == "ResetButton")
						{
//							foreach (UICard card in UIManager.m_uiManager.m_charSelectSlots)
//							{
//								card.Deactivate();	
//							}
							if (!SettingsManager.m_settingsManager.trial)
							{
								foreach (UICard card in PartyCards.m_partyCards.m_party)
								{
									if (card.m_followerData != null)
									{
										// Remove follower
										int level = 0;
										foreach (GameState.ProgressState progressState in GameManager.m_gameManager.gameProgress)
										{
											if (card.m_followerData.m_followerType == progressState.m_followerType)
											{
												level = progressState.m_level;
												break;
											}
											
										}
										
										Follower.Level l = card.m_followerData.m_levelTable[level];
										if (l != null)
										{
//											SettingsManager.m_settingsManager.startingHealth -= l.m_healthMod;
//											SettingsManager.m_settingsManager.startingEnergy -= l.m_energyMod;
//											SettingsManager.m_settingsManager.startingDamage -= l.m_damageMod;
//											SettingsManager.m_settingsManager.startingArmor -= l.m_armorMod;
//											
//											UIManager.m_uiManager.UpdateHealth(SettingsManager.m_settingsManager.startingHealth);	
//											UIManager.m_uiManager.UpdateEnergy(SettingsManager.m_settingsManager.startingEnergy);
//											StartCoroutine(UIManager.m_uiManager.UpdateDamage(SettingsManager.m_settingsManager.startingDamage));
//											StartCoroutine(UIManager.m_uiManager.UpdateArmor(SettingsManager.m_settingsManager.startingArmor));
										}
										PartyCards.m_partyCards.RemoveFollower(card.m_followerData);
									}
								}
								
								foreach (GameObject cardGO in UIManager.m_uiManager.cardList)
								{
									UICard listCard = (UICard)cardGO.GetComponent("UICard");
									
									//if (listCard.m_portrait.spriteName == "Card_Back03")
									if (listCard.m_followerData != null)
									{
										listCard.m_portrait.spriteName = listCard.m_followerData.m_portraitSpriteName;
										listCard.m_nameUI.text = listCard.m_followerData.m_nameText;
										listCard.m_nameUI.gameObject.SetActive(true);
										listCard.m_abilityUI.text = listCard.m_followerData.m_abilityText;
										listCard.m_abilityUI.gameObject.SetActive(true);
										listCard.m_passive01UI.gameObject.SetActive(true);
										listCard.m_passive02UI.gameObject.SetActive(true);
										listCard.m_rankUI.gameObject.SetActive(true);
									}
								}
							} else 
							{
								UIManager.m_uiManager.leaveDungeon = true;
								yield break;
							}
						}
						
					} else if (hitInfo.transform.gameObject.tag == "BuyPortal" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Shop && SettingsManager.m_settingsManager.gold >= 20)
					{
						SettingsManager.m_settingsManager.gold -= 20;
						UIManager.m_uiManager.doPortal = true;
						cardSelected = true;

					} else if (hitInfo.transform.gameObject.tag == "BuyRandCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Shop && SettingsManager.m_settingsManager.gold >= 10 && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP ) 
					{
						SettingsManager.m_settingsManager.gold -= 10;
						UIManager.m_uiManager.UpdateGoldUI();

						//deactivate card
						//hitInfo.transform.gameObject.SetActive(false);

						// choose random item
						int lootvalue = 0;
						GameObject[] table = GameManager.m_gameManager.m_lootTable[lootvalue].m_lootTable;
						GameObject randItem = (GameObject)Instantiate((GameObject)table[Random.Range(0, table.Length)], Vector3.zero, Quaternion.identity);
						Item thisItem = (Item)randItem.GetComponent("Item");

						GameManager.m_gameManager.inventory.Add(thisItem);

						// show card
						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
						card.SetCard(thisItem, false);
						yield return new WaitForSeconds(0.5f);

						// move card
						float t = 0;
						float time = 0.5f;
						Vector3 startPos = hitInfo.transform.position;
						//Vector3 startScale = card.transform.localScale;
						while (t < time)
						{
							t += Time.deltaTime;;
							Vector3 nPos = Vector3.Lerp(startPos, AssetManager.m_assetManager.m_props[7].transform.position , t / time);
							//Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
							hitInfo.transform.position = nPos;
							//card.transform.localScale = newScale;
							yield return null;
						}
						hitInfo.transform.position = startPos;
						//card.transform.localScale = startScale;

						//deactivate card
						hitInfo.transform.gameObject.SetActive(false);
						
						UIManager.m_uiManager.RefreshInventoryMenu();
					}
					else if (hitInfo.transform.gameObject.tag == "UICard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
//						bool shiftHeld = false;
//						if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
//						{
//							shiftHeld = true;
//						}

						Debug.Log("ITEM TOUCHED");
						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
						Item thisItem = (Item)card.m_itemData;
						//if (thisItem != null && ((thisItem.HasKeyword(Item.Keyword.Consumeable) && !thisItem.HasKeyword(Item.Keyword.Equippable)) || (thisItem.HasKeyword(Item.Keyword.Equippable) && EquipCards.m_equipCards.freeEquipSlots > 0)))
						if (thisItem != null)
						{
//							if (thisItem.HasKeyword(Item.Keyword.Skill) && PartyCards.m_partyCards.CanEquipSkill(thisItem))
//							{
//								Debug.Log("EQUIPPING SKILL");
//								yield return StartCoroutine(PartyCards.m_partyCards.EquipSkill(thisItem, card));
//
//								//remove item from inventory
//								List<Item> items = GameManager.m_gameManager.inventory;
//								GameObject oldItem = null;
//								
//								for (int i=0; i < items.Count; i++)
//								{
//									Item invItem = items[i];
//									if (invItem == thisItem)
//									{
//										items.RemoveAt(i);
//										oldItem = invItem.gameObject;
//										break;
//									}
//								}
//								
//								if (oldItem != null)
//								{
//									card.Deactivate();
//									card.m_itemData = null;
//								}
//
//								if (UIManager.m_uiManager.invHoveredCard == card)
//								{
//									UIManager.m_uiManager.ClearInvSelection();	
//								}
//								
//								UIManager.m_uiManager.RefreshInventoryMenu();
//
//
//
//							}
//							else if (thisItem.HasKeyword(Item.Keyword.Equippable) && !m_shiftHeld && EquipCards.m_equipCards.freeEquipSlots > 0)
//							{
//								yield return StartCoroutine(thisItem.ActivateItem());
//								List<Item> items = GameManager.m_gameManager.inventory;
//								//remove item from inventory
//								GameObject oldItem = null;
//								
//								for (int i=0; i < items.Count; i++)
//								{
//									Item invItem = items[i];
//									if (invItem == thisItem)
//									{
//										items.RemoveAt(i);
//										if (thisItem.HasKeyword(Item.Keyword.Equippable))
//										{
//											GameManager.m_gameManager.equippedItems.Add(thisItem);
//										} else if (thisItem.HasKeyword(Item.Keyword.Consumeable) && !thisItem.HasKeyword(Item.Keyword.Equippable))
//										{
//											oldItem = invItem.gameObject;
//
//										}
//										break;
//									}
//								}
//								
//								if (oldItem != null)
//								{
//									Destroy(oldItem);
//								}
//								
//								card.Deactivate();
//								card.m_itemData = null;
//								
//								if (UIManager.m_uiManager.invHoveredCard == card)
//								{
//									UIManager.m_uiManager.ClearInvSelection();	
//								}
//								
//								UIManager.m_uiManager.RefreshInventoryMenu();
//							} 
//							else if (m_shiftHeld && thisItem.HasKeyword(Item.Keyword.UseFromInv))
//							{
//								yield return StartCoroutine(thisItem.ActivateItem());
//								List<Item> items = GameManager.m_gameManager.inventory;
//								//remove item from inventory
//								GameObject oldItem = null;
//								
//								for (int i=0; i < items.Count; i++)
//								{
//									Item invItem = items[i];
//									if (invItem == thisItem)
//									{
//										items.RemoveAt(i);
//										oldItem = invItem.gameObject;
//										break;
//									}
//								}
//								
//								if (oldItem != null)
//								{
//									Destroy(oldItem);
//								}
//								
//								card.Deactivate();
//								card.m_itemData = null;
//								
//								if (UIManager.m_uiManager.invHoveredCard == card)
//								{
//									UIManager.m_uiManager.ClearInvSelection();	
//								}
//								
//								UIManager.m_uiManager.RefreshInventoryMenu();
//							}
							
						} 
//						else if (thisItem != null && thisItem.HasKeyword(Item.Keyword.Pet) && GameManager.m_gameManager.followers.Count < 4)
//						{
//							Debug.Log("ADDING PET");
//							
//							//get follower from card
//							GameObject follower = card.m_itemData.m_craftResult;
//							
//							//add follower
//							Vector3 pos = Vector3.one * 1000;
//							Follower thisFollower = (Follower)((GameObject)Instantiate(follower, pos, follower.transform.rotation)).GetComponent("Follower");
//							List<Follower> newFollower = new List<Follower>();
//							newFollower.Add(thisFollower);
//							GameManager.m_gameManager.followers.Add(thisFollower);
//							
//							Player.m_player.SetPassiveFollowerBonuses(newFollower);
//							//UIManager.m_uiManager.SetFollowers(GameManager.m_gameManager.followers);
//							PartyCards.m_partyCards.AddFollower(thisFollower);
//							
//							//remove item from inventory
//							List<Item> items = GameManager.m_gameManager.inventory;
//							for (int i=0; i < items.Count; i++)
//							{
//								Item invItem = items[i];
//								if (invItem == thisItem)
//								{
//									items.RemoveAt(i);
//									//Destroy(invItem.gameObject);
//									invItem.gameObject.SetActive(false);
//									invItem = null;
//									
//									//card.m_cardSprite.spriteName = "Card_Back02";
////									card.m_nameUI.gameObject.SetActive(false);
////									card.m_abilityUI.gameObject.SetActive(false);
////									card.m_portrait.spriteName = "Card_Back04";
//									card.Deactivate();
//									card.m_itemData = null;
//									i=99;
//								}
//							}
//
//						}
					} else if (hitInfo.transform.gameObject.tag == "UICard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Chest)
					{
						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
						Item itemRef = card.m_itemData;
						card.m_itemData = null;
						List<Item> inventory = GameManager.m_gameManager.inventory;
						inventory.Add(itemRef);
						GameManager.m_gameManager.inventory = inventory;

						//animated card moving
						float t = 0;
						float time = 0.3f;
						Vector3 startPos = card.transform.position;
						Vector3 startScale = card.transform.localScale;
						m_cardsMoving = true;
						while (t < time)
						{
							t += Time.deltaTime;
							Vector3 newPos = Vector3.Lerp(startPos, AssetManager.m_assetManager.m_props[7].transform.position, t / time);
							Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
							card.transform.position = newPos;
							card.transform.localScale = newScale;
							yield return null;
						}
						m_cardsMoving = false;
						UIManager.m_uiManager.RefreshInventoryMenu();
						//yield return new WaitForSeconds(0.3f);

						yield break;
					} 
//					else if (hitInfo.transform.gameObject.tag == "UICard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Crafting && UIManager.m_uiManager.numCraftingItems < 3)
//					{
//						
//						//add card to crafting slot
//						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
//						
//						//make sure item hasn't already been added
//						if (card.m_portrait.spriteName != "Card_Back03")
//						{
//							Debug.Log("CRAFTING ITEM TOUCHED");
//							UIManager.m_uiManager.SetCraftingSlot(card);
//							
//							//remove card from resource list
//							card.Deactivate();
//							m_deactivatedGOs.Add(hitInfo.transform.gameObject);
//						}
//					} 
//					else if (hitInfo.transform.gameObject.tag == "CraftButton" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Crafting)
//					{
//						Debug.Log("START CRAFTING");
//						bool wasValid = GameManager.m_gameManager.StartCrafting();
//						
//						if (wasValid)
//						{
//							m_deactivatedGOs.Clear();	
//						}
//					} 
//					else if (hitInfo.transform.gameObject.tag == "ResetButton" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Crafting)
//					{
//						foreach (UICard thisCard in UIManager.m_uiManager.m_craftingSlots)
//						{
//							thisCard.Deactivate();
//						}
//						
//						//turn on used resources on inventory row
//						foreach (GameObject thisGO in m_deactivatedGOs)
//						{
//							UICard card = (UICard)thisGO.transform.GetComponent("UICard");
//							//card.m_cardSprite.spriteName = "Card_Front01";
//							card.m_nameUI.gameObject.SetActive(true);
//							card.m_nameUI.text = card.m_itemData.m_name;
//							card.m_abilityUI.gameObject.SetActive(true);
//							card.m_portrait.spriteName = card.m_itemData.m_portraitSpriteName;
//							card.itemData = card.m_itemData;
//						}
//						m_deactivatedGOs.Clear();
//					}
					else if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.EndLevel)
					{

						if (hitInfo.transform.gameObject.tag == "ResetButton")
						{

							foreach (Follower thisFollower in GameManager.m_gameManager.followers)
							{
								//update progressState
								for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
								{
									GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[i];
									if (progress.m_followerType == thisFollower.m_followerType && !thisFollower.isLocked)
									{
										progress.m_level = thisFollower.currentLevel;
										progress.m_XP = thisFollower.currentXP;
										GameManager.m_gameManager.gameProgress[i] = progress;
										i=99;
									}
								}
							}
							
							GameManager.m_gameManager.gameState.saveState();

							if (SettingsManager.m_settingsManager.trial)
							{
								UIManager.m_uiManager.leaveDungeon = true;
							}

							yield break;
						}
						else if (hitInfo.transform.gameObject.tag == "CraftButton")
						{

//							foreach (Follower thisFollower in GameManager.m_gameManager.lostSouls)
//							{
//								//update progressState
//								for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
//								{
//									GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[i];
//									if (progress.m_followerType == thisFollower.m_followerType)
//									{
//										progress.m_isLocked = false;
//										progress.m_level = thisFollower.currentLevel;
//										progress.m_XP = thisFollower.currentXP;
//										GameManager.m_gameManager.gameProgress[i] = progress;
//										i=99;
//									}
//								}
//							}
							
//							foreach (Follower thisFollower in GameManager.m_gameManager.followers)
//							{
//								//update progressState
//								for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
//								{
//									GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[i];
//									if (progress.m_followerType == thisFollower.m_followerType)
//									{
//										progress.m_isLocked = false;
//										progress.m_level = thisFollower.currentLevel;
//										progress.m_XP = thisFollower.currentXP;
//										GameManager.m_gameManager.gameProgress[i] = progress;
//										i=99;
//									}
//								}
//							}

							UIManager.m_uiManager.leaveDungeon = true;
							yield break;
//							SettingsManager.m_settingsManager.difficultyLevel = 0;
//							Application.LoadLevel("PartySelect01");
						}
					} else if (hitInfo.transform.gameObject.tag == "ResetButton" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.GameOver)
					{
						SettingsManager.m_settingsManager.difficultyLevel = 0;
						//Application.LoadLevel("PartySelect01");
						Application.LoadLevel("MainMenu01");
					}  else if (hitInfo.transform.gameObject.tag == "SkipButton" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.GameOver)
					{
						SettingsManager.m_settingsManager.difficultyLevel = 0;
						Application.LoadLevel("GameScene01");
					}
					else if (hitInfo.transform.gameObject.tag == "FollowerCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.FollowerSwap
						&& UIManager.m_uiManager.cardList.Count > 0)
					{
						cardSelected = true;
						UICard followerCard = (UICard)hitInfo.transform.GetComponent("UICard");
						
						//remove follower's passive bonuses if any
//						List<Follower> oldfollowerList = new List<Follower>();
						Follower oldFollower = (Follower)followerCard.m_followerData;
//						oldfollowerList.Add(oldFollower);
//						Player.m_player.RemovePassiveFollowerBonuses(oldfollowerList);

						//add any equipped skills/items to the backpack, rest go to Limbo
						UICard[] skills = PartyCards.m_partyCards.GetSkillCards(oldFollower);
						if (skills.Length > 0)
						{
							foreach (UICard c in skills)
							{
								if (c.itemData != null)
								{
									Item item = c.itemData;

									EffectsPanel.m_effectsPanel.RemoveEffect(item);
//									if (item.itemState == Item.ItemState.Spent)
//									{
//										item.ChangeState(Item.ItemState.Normal);
//									}

//									if (GameManager.m_gameManager.inventory.Count < 10)
//									{
//										item.attachedFollower.currentSkills --;
//										PartyCards.m_partyCards.UnequipSkill(item);
//										GameManager.m_gameManager.inventory.Add(item);
//									} else {
//										item.attachedFollower.currentSkills --;
//										PartyCards.m_partyCards.RemoveSkill(item);
//									}
								}
							}

							UIManager.m_uiManager.RefreshInventoryMenu();
						}
						
						//remove follower from list
						List<Follower> fList = GameManager.m_gameManager.followers;
						for (int i=0; i < fList.Count; i++)
						{
							Follower thisF = fList[i];
							if (thisF == oldFollower)
							{
								fList.RemoveAt(i);
								//Destroy(oldFollower);
								oldFollower.gameObject.SetActive(false);
								oldFollower = null;
								i=99;
							}
						}
						//GameManager.m_gameManager.followers = fList;
						
						//replace follower card 
						Follower newFollower = (Follower)m_selectedObject.transform.GetComponent("Follower");
						newFollower.SetLevel();
						followerCard.m_nameUI.text = newFollower.m_nameText;
						followerCard.m_portrait.spriteName = newFollower.m_portraitSpriteName;
						followerCard.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(newFollower);
						followerCard.m_shortCutUI.text = PartyCards.m_partyCards.UpdateShortAbilityText(newFollower);
						followerCard.m_rankUI.text =  "Level " + (newFollower.currentLevel+1).ToString() + " " + newFollower.m_followerClass.ToString();
						PartyCards.m_partyCards.UpdatePassiveText(followerCard, newFollower, newFollower.currentLevel);
						followerCard.m_followerData = newFollower;
						
						// update follower list
						GameManager.m_gameManager.followers.Add(newFollower);
							
						//add new follower passive bonuses if any
						List<Follower> newfollowerList = new List<Follower>();
						//newfollowerList.Add(newFollower);
						foreach (UICard c in PartyCards.m_partyCards.m_party)
						{
							if (c.m_followerData != null)
							{
								newfollowerList.Add(c.m_followerData);
							}
						}
						Player.m_player.SetPassiveFollowerBonuses(newfollowerList);

						
					}
					else if (hitInfo.transform.gameObject.tag == "EquipCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Storage && GameManager.m_gameManager.numStorageCards > 0
						&& GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
					{
						Debug.Log("ADDING CARD TO INVENTORY");
//						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
//						
//						foreach (GameObject cardGO in UIManager.m_uiManager.cardList)
//						{
//							UICard listCard = (UICard)cardGO.GetComponent("UICard");	
//							
//							if (listCard.itemData == null)
//							{
//								//add card to inventory
//								GameManager.m_gameManager.inventory.Add(card.m_itemData);
//				
//								//remove item from storage
//								for (int i=0; i < GameManager.m_gameManager.storageItems.Count; i++)
//								{
//									Item sItem = (Item)GameManager.m_gameManager.storageItems[i];
//									if (sItem.m_name == card.m_itemData.m_name)
//									{
//										GameManager.m_gameManager.storageItems.RemoveAt(i);
//										break;
//									}
//								}
//
//								float t = 0;
//								float time = 0.2f;
//								Vector3 startPos = card.transform.position;
//								Vector3 startScale = card.transform.localScale;
//
//								while (t < time)
//								{
//									t += Time.deltaTime;
//									Vector3 nPos = Vector3.Lerp(startPos, listCard.transform.position , t / time);
//									Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.7f, t / time);
//									card.transform.position = nPos;
//									card.transform.localScale = newScale;
//									yield return null;
//								}
//								card.transform.position = startPos;
//								card.transform.localScale = startScale;
//								
//								//activate card in inventory list
//								UIManager.m_uiManager.RefreshInventoryMenu();
//
//								//remove card from storage list
//								card.Deactivate();
//								card.itemData = null;
//								
//								break;
//							}
//						}

						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
						if (card.itemData != null)
						{
							Item itemRef = card.m_itemData;
							card.m_itemData = null;
							GameManager.m_gameManager.inventory.Add(itemRef);

							//remove item from storage
							for (int i=0; i < GameManager.m_gameManager.storageItems.Count; i++)
							{
								Item sItem = (Item)GameManager.m_gameManager.storageItems[i];
								if (sItem.m_name == itemRef.m_name)
								{
									GameManager.m_gameManager.storageItems.RemoveAt(i);
									break;
								}
							}
							
							//animated card moving
							float t = 0;
							float time = 0.3f;
							Vector3 startPos = card.transform.position;
							Vector3 startScale = card.transform.localScale;
							m_cardsMoving = true;
							while (t < time)
							{
								t += Time.deltaTime;
								Vector3 newPos = Vector3.Lerp(startPos, AssetManager.m_assetManager.m_props[7].transform.position, t / time);
								Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
								card.transform.position = newPos;
								card.transform.localScale = newScale;
								yield return null;
							}
							m_cardsMoving = false;
							card.Deactivate();
							card.transform.localScale = startScale;
							card.transform.position = startPos;
							UIManager.m_uiManager.RefreshInventoryMenu();
						}

					}else if (hitInfo.transform.gameObject.tag == "InvCard" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Storage && GameManager.m_gameManager.numStorageCards < GameManager.m_gameManager.maxStorage )
					{
						UICard card = (UICard)hitInfo.transform.GetComponent("UICard");
						UICard craftSlot = null;
						foreach (UICard craftingSlot in UIManager.m_uiManager.m_storageSlots)
						{
							if (craftingSlot.itemData == null && craftingSlot.gameObject.activeSelf)
							{
								craftSlot = craftingSlot;
							}
						}

						Item thisItem = (Item)card.m_itemData;
						

						float t = 0;
						float time = 0.2f;
						Vector3 startPos = card.transform.position;

						while (t < time)
						{
							t += Time.deltaTime;;
							Vector3 nPos = Vector3.Lerp(startPos, craftSlot.transform.position , t / time);
							card.transform.position = nPos;
							yield return null;
						}
						card.transform.position = startPos;


						//add card to empty storage slot
						GameManager.m_gameManager.storageItems.Add(thisItem);
						craftSlot.SetCard(thisItem, false);

						
						//remove card from inventory
						List<Item> items = GameManager.m_gameManager.inventory;
						for (int i=0; i < items.Count; i++)
						{
							Item invItem = items[i];
							if (invItem == thisItem)
							{
								items.RemoveAt(i);
								break;
							}
						}

						card.m_itemData = null;
						UIManager.m_uiManager.ClearInvSelection();
						UIManager.m_uiManager.RefreshInventoryMenu();

					} else if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.Pause)
					{
						//Debug.Log(hitInfo.transform.gameObject.name);
						if (hitInfo.transform.gameObject.tag == "CraftButton")
						{
							yield break;
						}else if (hitInfo.transform.gameObject.tag == "ResetButton")
						{
							Time.timeScale = 1.0f;
							Application.LoadLevel("MainMenu01");
						}
					} 
					else if (hitInfo.transform.gameObject.tag == "FuseButton" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						Debug.Log(";LSKD;LAKSDJFLSKJFDLSKDJF;LKSDJF;LSKJF");
						FuseButton fb = (FuseButton)hitInfo.transform.GetComponent("FuseButton");
						
						// remove used resources from inventory
						foreach (UICard c in fb.cardList)
						{
							Item invItem = c.itemData;
							for (int i=0; i < GameManager.m_gameManager.inventory.Count; i++)
							{
								if (invItem.gameObject == GameManager.m_gameManager.inventory[i].gameObject)
								{
									Item oldItem = GameManager.m_gameManager.inventory[i];
									GameManager.m_gameManager.inventory.RemoveAt(i);
									Destroy(oldItem);
									i = 99;
								}
							}
						}
						
						// add new item to inventory
						GameObject item = (GameObject)Instantiate(fb.craftingResult.gameObject, Vector3.zero, fb.craftingResult.transform.rotation);
						GameManager.m_gameManager.inventory.Add((Item)(item.GetComponent("Item")));
						
						// refresh inventory	
						UIManager.m_uiManager.RefreshInventoryMenu();
					} 
					else if ((hitInfo.transform.gameObject.tag == "LeaderCard" || hitInfo.transform.gameObject.tag == "FollowerCard") && UIManager.m_uiManager.menuMode == UIManager.MenuMode.SelectHero)
					{
						Item item = m_itemForEquipping.itemData;
						UICard c = (UICard)hitInfo.transform.GetComponent("UICard");
						if (c.m_followerData != null)
						{
							if (PartyCards.m_partyCards.CanEquipSkill(c.m_followerData, item))
							{
								Debug.Log("VALID HERO, EQUIPPING SKILL");
							
								float t = 0;
								float time = 0.3f;
								Vector3 startPos = m_itemForEquipping.transform.position;
								Vector3 startScale = m_itemForEquipping.transform.localScale;
								//m_cardsMoving = true;
								while (t < time)
								{
									t += Time.deltaTime;;
									Vector3 nPos = Vector3.Lerp(startPos, c.transform.position , t / time);
									//Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
									m_itemForEquipping.transform.position = nPos;
									//m_itemForEquipping.transform.localScale = newScale;
									yield return null;
								}
								//m_cardsMoving = false;
								m_itemForEquipping.transform.position = startPos;
								m_itemForEquipping.transform.localScale = startScale;


								PartyCards.m_partyCards.EquipSkill(c.m_followerData, item);

								//remove item from inventory
								List<Item> items = GameManager.m_gameManager.inventory;
								GameObject oldItem = null;
								
								for (int i=0; i < items.Count; i++)
								{
									Item invItem = items[i];
									if (invItem == item)
									{
										items.RemoveAt(i);
										oldItem = invItem.gameObject;
										break;
									}
								}
								
								if (oldItem != null)
								{
									m_itemForEquipping.Deactivate();
									m_itemForEquipping.m_itemData = null;
								}
								
								if (UIManager.m_uiManager.invHoveredCard == m_itemForEquipping)
								{
									UIManager.m_uiManager.ClearInvSelection();	
								}
								
								UIManager.m_uiManager.RefreshInventoryMenu();

								cardSelected = true;
							}
						}
					}
				}	
			} else {
				//no button held down
				//if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory || UIManager.m_uiManager.menuMode == UIManager.MenuMode.CharSelect)
				if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.None || UIManager.m_uiManager.menuMode == UIManager.MenuMode.CharSelect)
				{
					DoHoveredCards();	
				}

				Ray worldTouchRay = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				bool objectTouched = false;
				if(Physics.Raycast(worldTouchRay, out hitInfo))
				{

					if (hitInfo.transform.gameObject.tag == "Enemy" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						objectTouched = true;
						if (m_selectedObject != hitInfo.transform.gameObject)
						{
							m_selectedObject = hitInfo.transform.gameObject;
							Enemy thisEnemy = (Enemy)hitInfo.transform.GetComponent("Enemy");
							StartCoroutine( UIManager.m_uiManager.DisplayTargetEnemy(thisEnemy));
						}
					} else if (hitInfo.transform.gameObject.tag == "Follower" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						objectTouched = true;
						if (m_selectedObject != hitInfo.transform.gameObject)
						{
							m_selectedObject = hitInfo.transform.gameObject;
							Follower thisFollower = (Follower)hitInfo.transform.GetComponent("Follower");
							UIManager.m_uiManager.DisplayTargetCard(thisFollower);
						}
					}  else if (hitInfo.transform.gameObject.tag == "Card" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						Card thisCard = (Card)hitInfo.transform.GetComponent("Card");
						if ((thisCard.type == Card.CardType.Tower || thisCard.type == Card.CardType.Fort || thisCard.type == Card.CardType.Gate || thisCard.type == Card.CardType.HighGround
						     || thisCard.type == Card.CardType.Quicksand || thisCard.type == Card.CardType.Trap_Razorvine || thisCard.type == Card.CardType.Warren || thisCard.type == Card.CardType.Fire
						     || thisCard.type == Card.CardType.Stalactite || thisCard.type == Card.CardType.Spore || thisCard.type == Card.CardType.Spikes || thisCard.type == Card.CardType.Unholy
						     || thisCard.type == Card.CardType.AshCloud || thisCard.type == Card.CardType.Magma || thisCard.type == Card.CardType.Mine
						     || thisCard.type == Card.CardType.Exit || thisCard.type == Card.CardType.Darkness || thisCard.type == Card.CardType.ClawingHands || thisCard.type == Card.CardType.Whispers
						     || thisCard.type == Card.CardType.FrostSnap || thisCard.type == Card.CardType.RazorGlade || thisCard.type == Card.CardType.ManaBurn || thisCard.type == Card.CardType.BrokenGround) && thisCard.cardState == Card.CardState.Normal)
						{
							
							objectTouched = true;
							if (m_selectedObject != hitInfo.transform.gameObject)
							{
								m_selectedObject = hitInfo.transform.gameObject;
								yield return StartCoroutine( UIManager.m_uiManager.DisplayTargetCard(thisCard, UIManager.m_uiManager.m_followerCards[4]));
							}
						}
					}
					else if (hitInfo.transform.gameObject.tag == "Chest" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						objectTouched = true;
						if (m_selectedObject != hitInfo.transform.gameObject)
						{
							m_selectedObject = hitInfo.transform.gameObject;
							Chest thisChest = (Chest)hitInfo.transform.GetComponent("Chest");
							yield return StartCoroutine( UIManager.m_uiManager.DisplayTargetCard(thisChest));
						}
					}else if (hitInfo.transform.gameObject.tag == "Shop" && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						Debug.Log("SHOP OBJECT HOVERED");
						objectTouched = true;
						if (m_selectedObject != hitInfo.transform.gameObject)
						{
							Shop s = (Shop)hitInfo.transform.GetComponent("Shop");
							yield return StartCoroutine( UIManager.m_uiManager.DisplayTargetCard(s));
						}
					}

					if (!objectTouched && UIManager.m_uiManager.targetDisplayed && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
					{
						m_selectedObject = null;
						yield return StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
					}
				}
			}

			yield return false;
		}

		yield break;
	}

	private GameObject m_hoveredStackObject = null;

	private void DoHoveredCards()
	{

		Ray cardTouchRay = UIManager.m_uiManager.m_uiCamera.camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(cardTouchRay, out hit))
		{
			//Debug.Log(hit.transform.gameObject.name);
//			if (hit.transform.gameObject.tag == "SkillCard")
//		    {
//				Debug.Log("SKILL CARD HOVERED");
//				PartyCards.m_partyCards.SkillCardHovered((UICard)hit.transform.GetComponent("UICard"));
//			}else if (PartyCards.m_partyCards.hoveredSkill != null)
//			{
//				PartyCards.m_partyCards.ClearSkillSelection();
//			}

//			if (hit.transform.gameObject.tag == "FollowerCard" || hit.transform.gameObject.tag == "LeaderCard")
//			{
//				UICard c = (UICard) hit.transform.GetComponent("UICard");
//				if (c.selectState != UICard.SelectState.Locked)
//				{
//					PartyCards.m_partyCards.CardHovered(hit.transform.gameObject);
//				}
//			} 


//			else if (PartyCards.m_partyCards.hoveredCard != null)
//			{
//				PartyCards.m_partyCards.ClearSelection();	
//			}
			
//			if (hit.transform.gameObject.tag == "EquipCard" && UIManager.m_uiManager.menuMode != UIManager.MenuMode.Storage)
//			{
//				EquipCards.m_equipCards.CardHovered(hit.transform.gameObject);
//			} 
//			else if (EquipCards.m_equipCards.hoveredCard != null)
//			{
//				EquipCards.m_equipCards.ClearSelection();	
//			}

			//if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))


			if (hit.transform.gameObject.tag == "StackObject")
			{
				StackObject so = (StackObject) hit.transform.gameObject.GetComponent("StackObject");
				if ( FollowCamera.m_followCamera.target != so.m_target.gameObject)
				{
					m_hoveredStackObject = so.gameObject;
					FollowCamera.m_followCamera.SetTarget(so.m_target.gameObject);
				}
			}

			
			if (hit.transform.gameObject.tag == "EffectIcon")
			{

				if (EffectsPanel.m_effectsPanel.displayedSpriteTip == null)
				{
					UISprite s = (UISprite)hit.transform.GetComponent("UISprite");	
					EffectsPanel.m_effectsPanel.DisplayTip(s);
				}
				else if (EffectsPanel.m_effectsPanel.displayedSpriteTip.gameObject != hit.transform.gameObject)
				{
					UISprite s = (UISprite)hit.transform.GetComponent("UISprite");	
					EffectsPanel.m_effectsPanel.DisplayTip(s);
				}
			}

			if (hit.transform.gameObject.tag == "CurrentTurnIcon" && !CurrentTurnIcon.m_currentTurnIcon.tipDisplayed)
			{
				CurrentTurnIcon.m_currentTurnIcon.DisplayTip(true);
			}
			
			//if (hit.transform.gameObject.tag == "UICard" && (UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory || UIManager.m_uiManager.menuMode == UIManager.MenuMode.Storage || UIManager.m_uiManager.menuMode == UIManager.MenuMode.Shop))
			if (hit.transform.gameObject.tag == "InvCard" && !m_cardsMoving && UIManager.m_uiManager.menuMode != UIManager.MenuMode.SelectHero)
			{
				UICard c = (UICard)hit.transform.GetComponent("UICard");

//				if (c.itemData != null)
//				{
//					if ((m_shiftHeld && c.itemData.HasKeyword(Item.Keyword.UseFromInv)) || !m_shiftHeld)
//					{
//						if (PartyCards.m_partyCards.hoveredCard != null)
//						{
//							PartyCards.m_partyCards.ClearSelection();
//						}
//						UIManager.m_uiManager.InventoryCardHovered(hit.transform.gameObject);
//					}
//				} else if (c.m_followerData != null)
//				{
//					if (UIManager.m_uiManager.invHoveredCard != null)
//					{
//						UIManager.m_uiManager.ClearInvSelection();
//					}
//					PartyCards.m_partyCards.CardHovered(hit.transform.gameObject);
//				}
//				else
//				{
//					if (PartyCards.m_partyCards.hoveredCard != null)
//					{
//						PartyCards.m_partyCards.ClearSelection();
//					}
					UIManager.m_uiManager.InventoryCardHovered(hit.transform.gameObject);
//				}
			}
			
//			if (hit.transform.gameObject.tag == "FuseButton")
//			{
//				foreach (FuseButton fb in UIManager.m_uiManager.fuseButtons)
//				{
//					if (fb.gameObject == hit.transform.gameObject && !fb.isHovered)
//					{
//						fb.Hover();
//						break;
//					}
//				}
//			} 

		} else
		{
//			if (CurrentTurnIcon.m_currentTurnIcon.tipDisplayed)
//			{
//				CurrentTurnIcon.m_currentTurnIcon.DisplayTip(false);
//			}

			if (PartyCards.m_partyCards.hoveredCard != null)
			{
				PartyCards.m_partyCards.ClearSelection();	
			}

			if (PartyCards.m_partyCards.hoveredSkill != null)
			{
				PartyCards.m_partyCards.ClearSkillSelection();
			}

			if (m_hoveredStackObject != null)
			{
				m_hoveredStackObject = null;
				FollowCamera.m_followCamera.SetTarget(Player.m_player.gameObject);
			}
			
//			if (EquipCards.m_equipCards.hoveredCard != null)
//			{
//				EquipCards.m_equipCards.ClearSelection();	
//			}
			
			if (EffectsPanel.m_effectsPanel.displayedSpriteTip != null)
			{
				EffectsPanel.m_effectsPanel.DeactivateTip();	
			}
			
			if (UIManager.m_uiManager.invHoveredCard != null && UIManager.m_uiManager.menuMode != UIManager.MenuMode.SelectHero && !m_cardsMoving)
			{
				UIManager.m_uiManager.ClearInvSelection();	
			}
			
			foreach (FuseButton fb in UIManager.m_uiManager.fuseButtons)
			{
				if (fb.isHovered)
				{
					fb.ClearSelection();
				}
			}
		}
	}

	public IEnumerator GetHeroSelection (Item item, UICard card)
	{
//		bool heroSelected = false;

		// darken ineligible heroes
		foreach (UICard c in PartyCards.m_partyCards.m_party)
		{
			if (c.m_followerData != null)
			{
//				if ((c.m_followerData.m_followerClass == item.m_class || item.m_class == Follower.FollowerClass.None) && c.m_followerData.currentLevel+1 >= item.m_itemLevel  && c.m_followerData.currentSkills < c.m_followerData.maxSkills )
//				{
//					c.selectState = UICard.SelectState.Unselected;
//				} else 
//				{
//					c.SetDark(true);
//				}
			}
		}
		m_itemForEquipping = card;
		yield return StartCoroutine( UIManager.m_uiManager.ChangeMenuMode (UIManager.MenuMode.SelectHero));
		m_itemForEquipping = null;
//		while (!heroSelected)
//		{
//			if (Input.GetMouseButtonUp(0))
//			{
//				Ray cardTouchRay = UIManager.m_uiManager.m_uiCamera.camera.ScreenPointToRay(Input.mousePosition);
//				RaycastHit hit;
//				if(Physics.Raycast(cardTouchRay, out hit))
//				{
//					if (hit.transform.gameObject.tag == "LeaderCard" || hit.transform.gameObject.tag == "FollowerCard" )
//					{
//						UICard c = (UICard)hit.transform.GetComponent("UICard");
//						if (c.m_followerData != null)
//						{
//							if (PartyCards.m_partyCards.CanEquipSkill(c.m_followerData, item))
//							{
//								Debug.Log("VALID HERO, EQUIPPING SKILL");
//								heroSelected = true;
//
//								float t = 0;
//								float time = 0.3f;
//								Vector3 startPos = card.transform.position;
//								Vector3 startScale = card.transform.localScale;
//								m_cardsMoving = true;
//								while (t < time)
//								{
//									t += Time.deltaTime;;
//									Vector3 nPos = Vector3.Lerp(startPos, c.transform.position , t / time);
//									Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
//									card.transform.position = nPos;
//									card.transform.localScale = newScale;
//									yield return null;
//								}
//								m_cardsMoving = false;
//								card.transform.position = startPos;
//								card.transform.localScale = startScale;
//
//
//								PartyCards.m_partyCards.EquipSkill(c.m_followerData, item);
//
//								//remove item from inventory
//								List<Item> items = GameManager.m_gameManager.inventory;
//								GameObject oldItem = null;
//								
//								for (int i=0; i < items.Count; i++)
//								{
//									Item invItem = items[i];
//									if (invItem == item)
//									{
//										items.RemoveAt(i);
//										oldItem = invItem.gameObject;
//										break;
//									}
//								}
//								
//								if (oldItem != null)
//								{
//									card.Deactivate();
//									card.m_itemData = null;
//								}
//								
//								if (UIManager.m_uiManager.invHoveredCard == card)
//								{
//									UIManager.m_uiManager.ClearInvSelection();	
//								}
//								
//								UIManager.m_uiManager.RefreshInventoryMenu();
//							}
//						}
//					}
//				}
//			}
//			yield return false;
//		}

		foreach (UICard c in PartyCards.m_partyCards.m_party)
		{
			if (c.m_followerData != null)
			{
//				if ((c.m_followerData.m_followerClass == item.m_class || item.m_class == Follower.FollowerClass.None) && c.m_followerData.currentLevel+1 >= item.m_itemLevel  && c.m_followerData.currentSkills < c.m_followerData.maxSkills )
//				{
//					
//				} else 
//				{
//					c.SetDark(false);
//				}
			}
		}

		yield return StartCoroutine( UIManager.m_uiManager.ChangeMenuMode (UIManager.MenuMode.None));

		yield break;
	}
	
	public GameObject selectedObject {get{return m_selectedObject;}set{m_selectedObject = value;}}
	public bool shiftHeld {get{return m_shiftHeld;}}
	public bool cardsMoving {get{return m_cardsMoving;}set{m_cardsMoving = value;}}
	
//	public struct ThisTouch
//	{
//		public enum Phase{
//			Start,
//			Held,
//			Released,
//			Inactive,
//		}
//		
//		public Vector3 currentPosition;
//		public Vector3 startPosition;
//		public Phase touchPhase;
//		
//	}
//
//	
//	private ThisTouch								m_currentTouch;
//	

//	
//	void Start ()
//	{
//		ThisTouch newTouch = new ThisTouch();
//		newTouch.touchPhase = ThisTouch.Phase.Inactive;
//		m_currentTouch = newTouch;
//	}
//
//	public ThisTouch GetInput () {
//
//		ThisTouch newTouch = new ThisTouch();
//		
//		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
//		{
//			if (Input.GetMouseButton(0)) //left mouse button is down
//			{
//				
//				//determine touch phase state
//				if (m_currentTouch.touchPhase == null)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Start;	
//				} else if (m_currentTouch.touchPhase == ThisTouch.Phase.Inactive || m_currentTouch.touchPhase == ThisTouch.Phase.Released)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Start;	
//				} else if (m_currentTouch.touchPhase == ThisTouch.Phase.Held || m_currentTouch.touchPhase == ThisTouch.Phase.Start)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Held;
//				}
//				
//				//determine touch position
//				if (newTouch.touchPhase == ThisTouch.Phase.Start)
//				{
//					newTouch.startPosition = Input.mousePosition;
//					newTouch.currentPosition = Input.mousePosition;
//				} else if (newTouch.touchPhase == ThisTouch.Phase.Held || newTouch.touchPhase == ThisTouch.Phase.Released)
//				{
//					if (m_currentTouch.touchPhase != null)
//					{
//						newTouch.startPosition = m_currentTouch.startPosition;
//					}
//					
//					newTouch.currentPosition = Input.mousePosition;
//				}
//				
//			} else if (!Input.GetMouseButton(0)) //left mouse button is NOT pressed
//			{
//				if (m_currentTouch.touchPhase == null)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Inactive;	
//				} else if (m_currentTouch.touchPhase == ThisTouch.Phase.Held || m_currentTouch.touchPhase == ThisTouch.Phase.Start)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Released;	
//				} else if (m_currentTouch.touchPhase == ThisTouch.Phase.Released || m_currentTouch.touchPhase == ThisTouch.Phase.Inactive)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Inactive;	
//				}
//				
//				//determine touch position
//				if (newTouch.touchPhase == ThisTouch.Phase.Released)
//				{
//					if (m_currentTouch.touchPhase != null)
//					{
//						newTouch.startPosition = m_currentTouch.startPosition;
//					}
//					
//					newTouch.currentPosition = Input.mousePosition;
//				}
//			}
//		} else if (Application.platform == RuntimePlatform.IPhonePlayer)
//		{
//			int numTouches = Input.touchCount;
//			
//			if (numTouches > 0)
//			{
//				Touch touch = Input.GetTouch(0);
//				//inputPos = touch.position;
//				
//				if (touch.phase == TouchPhase.Began)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Start;
//				} else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Held;	
//				} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Released;	
//				}
//				
//				//determine touch position
//				if (newTouch.touchPhase == ThisTouch.Phase.Start)
//				{
//					newTouch.startPosition = touch.position;
//					newTouch.currentPosition = touch.position;
//				} else if (newTouch.touchPhase == ThisTouch.Phase.Held || newTouch.touchPhase == ThisTouch.Phase.Released)
//				{
//					if (m_currentTouch.touchPhase != null)
//					{
//						newTouch.startPosition = m_currentTouch.startPosition;
//					}
//					
//					newTouch.currentPosition = touch.position;
//				}
//			} else { //no touches
//				
//				if (m_currentTouch.touchPhase == null)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Inactive;	
//				} else if (m_currentTouch.touchPhase == ThisTouch.Phase.Held || m_currentTouch.touchPhase == ThisTouch.Phase.Start)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Released;	
//				} else if (m_currentTouch.touchPhase == ThisTouch.Phase.Released || m_currentTouch.touchPhase == ThisTouch.Phase.Inactive)
//				{
//					newTouch.touchPhase = ThisTouch.Phase.Inactive;	
//				}
//				
//				//determine touch position
//				if (newTouch.touchPhase == ThisTouch.Phase.Released)
//				{
//					if (m_currentTouch.touchPhase != null)
//					{
//						newTouch.startPosition = m_currentTouch.startPosition;
//					}
//					
//					newTouch.currentPosition = m_currentTouch.currentPosition;
//				}
//			}
//		}
//		
//		m_currentTouch = newTouch;
//		return newTouch;
//	
//	}
}

