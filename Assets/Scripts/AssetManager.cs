using UnityEngine;
using System.Collections;

public class AssetManager : MonoBehaviour {
	
	public enum SFXType {
		PlayerMove,
		PlayerMeleeAttack,
		CardFlip,
		Flipper,
		ChestReveal,
		EnemyReveal,
		EnemyDefeated,
		EnemyHit,
		SkillUsed,
		ChestOpened,
	}
	
	public AudioSource
		m_audioSource;

	public GameObject[]
		m_particleFX,
		m_props,
		m_UIelements;

	public AudioClip[]
		m_sfx,
		m_sfxMove,
		m_sfxCardFlip;

	public Material[]
		m_materials;

	public UILabel[]
		m_labels;

	public UISprite[]
		m_uiSprites;

	public TypogenicText[]
		m_typogenicText,
		m_oldActions;
	
	public static AssetManager
		m_assetManager;

	void Awake () {
		m_assetManager = this;
	}
	
	public void PlaySFX (SFXType type)
	{
		if (type == SFXType.PlayerMove)
		{
			AudioClip clip = (AudioClip) m_sfxMove[Random.Range(0, m_sfxMove.Length)];
			m_audioSource.PlayOneShot(clip, 0.05f);
		} else if (type == SFXType.CardFlip)
		{
			AudioClip clip = (AudioClip) m_sfxCardFlip[Random.Range(0, m_sfxCardFlip.Length)];
			m_audioSource.PlayOneShot(clip, 0.5f);
		} else if (type == SFXType.Flipper)
		{
			AudioClip clip = (AudioClip) m_sfx[0];
			m_audioSource.PlayOneShot(clip);
		} else if (type == SFXType.ChestReveal)
		{
			AudioClip clip = (AudioClip) m_sfx[1];
			m_audioSource.PlayOneShot(clip);
		} else if (type == SFXType.EnemyReveal)
		{
			AudioClip clip = (AudioClip) m_sfx[2];
			m_audioSource.PlayOneShot(clip, 0.5f);
		} else if (type == SFXType.EnemyDefeated)
		{
			AudioClip clip = (AudioClip) m_sfx[4];
			m_audioSource.PlayOneShot(clip);
		} else if (type == SFXType.EnemyHit)
		{
			AudioClip clip = (AudioClip) m_sfx[3];
			m_audioSource.PlayOneShot(clip);
		} else if (type == SFXType.SkillUsed)
		{
			AudioClip clip = (AudioClip) m_sfx[6];
			m_audioSource.PlayOneShot(clip);
		} else if (type == SFXType.ChestOpened)
		{
			AudioClip clip = (AudioClip) m_sfx[5];
			m_audioSource.PlayOneShot(clip);
		}
	}
}
