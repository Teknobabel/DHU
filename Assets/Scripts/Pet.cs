using UnityEngine;
using System.Collections;

public class Pet : Enemy {

	// Use this for initialization
	void Start () {
	
	}

	public IEnumerator MovePet (Card thisCard)
	{
		Card nextCard = thisCard;
		base.m_moveTimer = 0;
		base.m_moveTime = animation["EnemyJump01"].length;
		base.m_moveStart = m_currentCard.transform.position;
		base.m_moveEnd = nextCard.transform.position;
		base.m_currentCard.enemy = null;
		base.m_currentCard = nextCard;
		base.m_currentCard.enemy = this;
		yield return StartCoroutine(base.Move());
		
		yield return null;
	}

	private IEnumerator PetAttack (Enemy e)
	{
		Debug.Log ("PET ATTACK");
		yield return StartCoroutine (e.TakeDamage (base.m_damage));
		yield return null;
	}
	
	public override IEnumerator DoTurn () {

		// check for adjacent enemy
		Enemy enemy = null;
		for (int i=0; i < m_currentCard.linkedCards.Length; i++)
		{
			Card linkedCard = m_currentCard.linkedCards[i];
			if (linkedCard != null)
			{
				if (linkedCard.enemy != null)
				{
					if (linkedCard.enemy.m_enemyType != EnemyType.Pet && linkedCard.cardState == Card.CardState.Normal)
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
						enemy = linkedCard.enemy;
						i = 99;
					}
				}
			}
		}

		if (enemy != null)
		{
			yield return new WaitForSeconds(0.5f);
			animation.Play("EnemyJump01");
			yield return StartCoroutine(PetAttack(enemy));
			yield return new WaitForSeconds(0.5f);
			animation.Play("EnemyIdle01");
		} else {

			//move toward player
			int minDistance = 999;
			Card nextCard = null;
			//foreach (Card linkedCard in m_currentCard.linkedCards)
			for (int i=0; i < m_currentCard.linkedCards.Length; i++)
			{
				Card linkedCard = m_currentCard.linkedCards[i];
				if (linkedCard != null)
				{
					if (linkedCard.distanceToPlayer < minDistance && linkedCard != Player.m_player.currentCard && linkedCard.cardState == Card.CardState.Normal && !linkedCard.isOccupied
					    && linkedCard.distanceToPlayer < m_currentCard.distanceToPlayer)
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

			if (nextCard != null)
			{
				Debug.Log("PET MOVE");
				m_moveTimer = 0;
				m_moveTime = animation["EnemyJump01"].length;
				m_moveStart = m_currentCard.transform.position;
				m_moveEnd = nextCard.transform.position;
				m_currentCard.enemy = null;
				m_currentCard = nextCard;
				m_currentCard.enemy = this;
				yield return StartCoroutine(Move());

			}
		}

		yield return null;
	}
}
