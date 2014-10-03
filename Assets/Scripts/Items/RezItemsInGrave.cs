using UnityEngine;
using System.Collections;

public class RezItemsInGrave : Item {

	public override IEnumerator Activate ()
	{
		string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());


		foreach (GameManager.GraveSlot gs in GameManager.m_gameManager.grave)
		{
			if (gs.type == GameManager.GraveSlot.ObjectType.Item && gs.item != null && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP )
			{
				Item rezzedItem = gs.item;
				// set cost based on current leader
				if (rezzedItem.m_class == GameManager.m_gameManager.currentFollower.m_followerClass)
				{
					rezzedItem.adjustedEnergyCost = 0;
					rezzedItem.adjustedHealthCost = 0;
				} else {
					rezzedItem.adjustedEnergyCost = rezzedItem.m_energyCost;
					rezzedItem.adjustedHealthCost = rezzedItem.m_healthCost;
				}
				GameManager.m_gameManager.inventory.Add(rezzedItem);
				gs.item = null;
				gs.type = GameManager.GraveSlot.ObjectType.None;

				newString = rezzedItem.m_name + " is removed from The Grave";
				UIManager.m_uiManager.UpdateActions (newString);
				newString = rezzedItem.m_name + " is added to " + GameManager.m_gameManager.currentFollower.m_nameText + "'s Hand";
				UIManager.m_uiManager.UpdateActions (newString);
			}
		}

		// remove item
		for (int i=0; i < GameManager.m_gameManager.inventory.Count; i++) {
			Item item = (Item)GameManager.m_gameManager.inventory[i];
			if (item.id == this.id)
			{
				GameManager.m_gameManager.inventory.RemoveAt(i);
				break;
			}
		}

		UICard c = null;
		for (int i=0; i < UIManager.m_uiManager.cardList.Count; i++) {
			c = (UICard)UIManager.m_uiManager.cardList[i].GetComponent("UICard");
			if (c.itemData.id == this.id)
			{
				UIManager.m_uiManager.cardList.RemoveAt(i);
				break;
			}
		}

		UIManager.m_uiManager.RefreshInventoryMenu();

		// add item back
		GameManager.m_gameManager.inventory.Add (this);
		UIManager.m_uiManager.cardList.Add (c.gameObject);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
