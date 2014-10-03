using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {

	[System.Serializable]
	public struct ProgressState
	{
		public Follower.FollowerType m_followerType;
		public bool m_isLocked;	
		public int m_level;
		public int m_XP;
		public float m_XPBonus;
		public int m_badgeLevel1;
		public int m_badgeLevel2;
		public int m_badgeLevel3;
		public int m_badgeLevel4;
		public int m_badgeLevel5;
		public int m_badgeLevel6;
	}
	
	private const string Version = "Version";
	private const string Gold = "Gold";
	private const string XP = "XP";

	private const string Badge_DungeonHeart = "Badge_DungeonHeart";

	private const string Badge_DamageBoost_01 = "Badge_DamageBoost_01";
	private const string Badge_DamageBoost_02 = "Badge_DamageBoost_02";
	private const string Badge_DamageBoost_03 = "Badge_DamageBoost_03";
	private const string Badge_DamageBoost_04 = "Badge_DamageBoost_04";

	private const string Badge_HealthBoost_01 = "Badge_HealthBoost_01";
	private const string Badge_HealthBoost_02 = "Badge_HealthBoost_02";
	private const string Badge_HealthBoost_03 = "Badge_HealthBoost_03";
	private const string Badge_HealthBoost_04 = "Badge_HealthBoost_04";

	private const string Badge_EnergyBoost_01 = "Badge_EnergyBoost_01";
	private const string Badge_EnergyBoost_02 = "Badge_EnergyBoost_02";
	private const string Badge_EnergyBoost_03 = "Badge_EnergyBoost_03";
	private const string Badge_EnergyBoost_04 = "Badge_EnergyBoost_04";

	private const string Badge_HandBoost_01 = "Badge_HandBoost_01";
	private const string Badge_HandBoost_02 = "Badge_HandBoost_02";

	private const string Badge_HealthRecover_01 = "Badge_HealthRecover_01";
	private const string Badge_EnergyRecover_01 = "Badge_EnergyRecover_01";
	private const string Badge_EnergyRecover_02 = "Badge_EnergyRecover_02";

	private const string Badge_BadgeBoost_01 = "Badge_BadgeBoost_01";
	private const string Badge_BadgeBoost_02 = "Badge_BadgeBoost_02";
	private const string Badge_BadgeBoost_03 = "Badge_BadgeBoost_03";
	private const string Badge_BadgeBoost_04 = "Badge_BadgeBoost_04";
	private const string Badge_BadgeBoost_05 = "Badge_BadgeBoost_05";
	
	private const string BrandLockState = "BrandLockState";
	private const string BrandLevel = "BrandLevel";
	private const string BrandXP = "BrandXP";
	private const string BrandXPBonus = "BrandXPBonus";
	private const string Brand_Badge01 = "Brand_Badge01";
	private const string Brand_Badge02 = "Brand_Badge02";
	private const string Brand_Badge03 = "Brand_Badge03";
	private const string Brand_Badge04 = "Brand_Badge04";
	private const string Brand_Badge05 = "Brand_Badge05";
	private const string Brand_Badge06 = "Brand_Badge06";
	
	private const string AugustLockState = "AugustLockState";
	private const string AugustLevel = "AugustLevel";
	private const string AugustXP = "AugustXP";
	private const string AugustXPBonus = "AugustXPBonus";
	private const string August_Badge01 = "August_Badge01";
	private const string August_Badge02 = "August_Badge02";
	private const string August_Badge03 = "August_Badge03";
	private const string August_Badge04 = "August_Badge04";
	private const string August_Badge05 = "August_Badge05";
	private const string August_Badge06 = "August_Badge06";
	
	private const string JinLockState = "JinLockState";
	private const string JinLevel = "JinLevel";
	private const string JinXP = "JinXP";
	private const string JinXPBonus = "JinXPBonus";
	private const string Jin_Badge01 = "Jin_Badge01";
	private const string Jin_Badge02 = "Jin_Badge02";
	private const string Jin_Badge03 = "Jin_Badge03";
	private const string Jin_Badge04 = "Jin_Badge04";
	private const string Jin_Badge05 = "Jin_Badge05";
	private const string Jin_Badge06 = "Jin_Badge06";
	
	private const string TelinaLockState = "TelinaLockState";
	private const string TelinaLevel = "TelinaLevel";
	private const string TelinaXP = "TelinaXP";
	private const string TelinaXPBonus = "TelinaXPBonus";
	private const string Telina_Badge01 = "Telina_Badge01";
	private const string Telina_Badge02 = "Telina_Badge02";
	private const string Telina_Badge03 = "Telina_Badge03";
	private const string Telina_Badge04 = "Telina_Badge04";
	private const string Telina_Badge05 = "Telina_Badge05";
	private const string Telina_Badge06 = "Telina_Badge06";

	private const string KnightLockState = "KnightLockState";
	private const string KnightLevel = "KnightLevel";
	private const string KnightXP = "KnightXP";
	private const string KnightXPBonus = "KnightXPBonus";
	private const string Knight_Badge01 = "Knight_Badge01";
	private const string Knight_Badge02 = "Knight_Badge02";
	private const string Knight_Badge03 = "Knight_Badge03";
	private const string Knight_Badge04 = "Knight_Badge04";
	private const string Knight_Badge05 = "Knight_Badge05";
	private const string Knight_Badge06 = "Knight_Badge06";

	private const string SamuraiLockState = "SamuraiLockState";
	private const string SamuraiLevel = "SamuraiLevel";
	private const string SamuraiXP = "SamuraiXP";
	private const string SamuraiXPBonus = "SamuraiXPBonus";
	private const string Samurai_Badge01 = "Samurai_Badge01";
	private const string Samurai_Badge02 = "Samurai_Badge02";
	private const string Samurai_Badge03 = "Samurai_Badge03";
	private const string Samurai_Badge04 = "Samurai_Badge04";
	private const string Samurai_Badge05 = "Samurai_Badge05";
	private const string Samurai_Badge06 = "Samurai_Badge06";

	private const string RangerLockState = "RangerLockState";
	private const string RangerLevel = "RangerLevel";
	private const string RangerXP = "RangerXP";
	private const string RangerXPBonus = "RangerXPBonus";
	private const string Ranger_Badge01 = "Ranger_Badge01";
	private const string Ranger_Badge02 = "Ranger_Badge02";
	private const string Ranger_Badge03 = "Ranger_Badge03";
	private const string Ranger_Badge04 = "Ranger_Badge04";
	private const string Ranger_Badge05 = "Ranger_Badge05";
	private const string Ranger_Badge06 = "Ranger_Badge06";

	private const string SeerLockState = "SeerLockState";
	private const string SeerLevel = "SeerLevel";
	private const string SeerXP = "SeerXP";
	private const string SeerXPBonus = "SeerXPBonus";
	private const string Seer_Badge01 = "Seer_Badge01";
	private const string Seer_Badge02 = "Seer_Badge02";
	private const string Seer_Badge03 = "Seer_Badge03";
	private const string Seer_Badge04 = "Seer_Badge04";
	private const string Seer_Badge05 = "Seer_Badge05";
	private const string Seer_Badge06 = "Seer_Badge06";

	private const string MysticLockState = "MysticLockState";
	private const string MysticLevel = "MysticLevel";
	private const string MysticXP = "MysticXP";
	private const string MysticXPBonus = "MysticXPBonus";
	private const string Mystic_Badge01 = "Mystic_Badge01";
	private const string Mystic_Badge02 = "Mystic_Badge02";
	private const string Mystic_Badge03 = "Mystic_Badge03";
	private const string Mystic_Badge04 = "Mystic_Badge04";
	private const string Mystic_Badge05 = "Mystic_Badge05";
	private const string Mystic_Badge06 = "Mystic_Badge06";

	private const string ElfLockState = "ElfLockState";
	private const string ElfLevel = "ElfLevel";
	private const string ElfXP = "ElfXP";
	private const string ElfXPBonus = "ElfXPBonus";
	private const string Elf_Badge01 = "Elf_Badge01";
	private const string Elf_Badge02 = "Elf_Badge02";
	private const string Elf_Badge03 = "Elf_Badge03";
	private const string Elf_Badge04 = "Elf_Badge04";
	private const string Elf_Badge05 = "Elf_Badge05";
	private const string Elf_Badge06 = "Elf_Badge06";

	private const string BarbarianLockState = "BarbarianLockState";
	private const string BarbarianLevel = "BarbarianLevel";
	private const string BarbarianXP = "BarbarianXP";
	private const string BarbarianXPBonus = "BarbarianXPBonus";
	private const string Barbarian_Badge01 = "Barbarian_Badge01";
	private const string Barbarian_Badge02 = "Barbarian_Badge02";
	private const string Barbarian_Badge03 = "Barbarian_Badge03";
	private const string Barbarian_Badge04 = "Barbarian_Badge04";
	private const string Barbarian_Badge05 = "Barbarian_Badge05";
	private const string Barbarian_Badge06 = "Barbarian_Badge06";

	private const string BerserkerLockState = "BerserkerLockState";
	private const string BerserkerLevel = "BerserkerLevel";
	private const string BerserkerXP = "BerserkerXP";
	private const string BerserkerXPBonus = "BerserkerXPBonus";
	private const string Berserker_Badge01 = "Berserker_Badge01";
	private const string Berserker_Badge02 = "Berserker_Badge02";
	private const string Berserker_Badge03 = "Berserker_Badge03";
	private const string Berserker_Badge04 = "Berserker_Badge04";
	private const string Berserker_Badge05 = "Berserker_Badge05";
	private const string Berserker_Badge06 = "Berserker_Badge06";

	private const string PyromageLockState = "PyromageLockState";
	private const string PyromageLevel = "PyromageLevel";
	private const string PyromageXP = "PyromageXP";
	private const string PyromageXPBonus = "PyromageXPBonus";
	private const string Pyromage_Badge01 = "Pyromage_Badge01";
	private const string Pyromage_Badge02 = "Pyromage_Badge02";
	private const string Pyromage_Badge03 = "Pyromage_Badge03";
	private const string Pyromage_Badge04 = "Pyromage_Badge04";
	private const string Pyromage_Badge05 = "Pyromage_Badge05";
	private const string Pyromage_Badge06 = "Pyromage_Badge06";

	private const string DragoonLockState = "DragoonLockState";
	private const string DragoonLevel = "DragoonLevel";
	private const string DragoonXP = "DragoonXP";
	private const string DragoonXPBonus = "DragoonXPBonus";
	private const string Dragoon_Badge01 = "Dragoon_Badge01";
	private const string Dragoon_Badge02 = "Dragoon_Badge02";
	private const string Dragoon_Badge03 = "Dragoon_Badge03";
	private const string Dragoon_Badge04 = "Dragoon_Badge04";
	private const string Dragoon_Badge05 = "Dragoon_Badge05";
	private const string Dragoon_Badge06 = "Dragoon_Badge06";

	private const string FencerLockState = "FencerLockState";
	private const string FencerLevel = "FencerLevel";
	private const string FencerXP = "FencerXP";
	private const string FencerXPBonus = "FencerXPBonus";
	private const string Fencer_Badge01 = "Fencer_Badge01";
	private const string Fencer_Badge02 = "Fencer_Badge02";
	private const string Fencer_Badge03 = "Fencer_Badge03";
	private const string Fencer_Badge04 = "Fencer_Badge04";
	private const string Fencer_Badge05 = "Fencer_Badge05";
	private const string Fencer_Badge06 = "Fencer_Badge06";

	private const string SuccubusLockState = "SuccubusLockState";
	private const string SuccubusLevel = "SuccubusLevel";
	private const string SuccubusXP = "SuccubusXP";
	private const string SuccubusXPBonus = "SuccubusXPBonus";
	private const string Succubus_Badge01 = "Succubus_Badge01";
	private const string Succubus_Badge02 = "Succubus_Badge02";
	private const string Succubus_Badge03 = "Succubus_Badge03";
	private const string Succubus_Badge04 = "Succubus_Badge04";
	private const string Succubus_Badge05 = "Succubus_Badge05";
	private const string Succubus_Badge06 = "Succubus_Badge06";

	private const string DancerLockState = "DancerLockState";
	private const string DancerLevel = "DancerLevel";
	private const string DancerXP = "DancerXP";
	private const string DancerXPBonus = "DancerXPBonus";
	private const string Dancer_Badge01 = "Dancer_Badge01";
	private const string Dancer_Badge02 = "Dancer_Badge02";
	private const string Dancer_Badge03 = "Dancer_Badge03";
	private const string Dancer_Badge04 = "Dancer_Badge04";
	private const string Dancer_Badge05 = "Dancer_Badge05";
	private const string Dancer_Badge06 = "Dancer_Badge06";

	private const string WrestlerLockState = "WrestlerLockState";
	private const string WrestlerLevel = "WrestlerLevel";
	private const string WrestlerXP = "WrestlerXP";
	private const string WrestlerXPBonus = "WrestlerXPBonus";
	private const string Wrestler_Badge01 = "Wrestler_Badge01";
	private const string Wrestler_Badge02 = "Wrestler_Badge02";
	private const string Wrestler_Badge03 = "Wrestler_Badge03";
	private const string Wrestler_Badge04 = "Wrestler_Badge04";
	private const string Wrestler_Badge05 = "Wrestler_Badge05";
	private const string Wrestler_Badge06 = "Wrestler_Badge06";

	private const string PsychicLockState = "PsychicLockState";
	private const string PsychicLevel = "PsychicLevel";
	private const string PsychicXP = "PsychicXP";
	private const string PsychicXPBonus = "PsychicXPBonus";
	private const string Psychic_Badge01 = "Psychic_Badge01";
	private const string Psychic_Badge02 = "Psychic_Badge02";
	private const string Psychic_Badge03 = "Psychic_Badge03";
	private const string Psychic_Badge04 = "Psychic_Badge04";
	private const string Psychic_Badge05 = "Psychic_Badge05";
	private const string Psychic_Badge06 = "Psychic_Badge06";

	private const string MonkLockState = "MonkLockState";
	private const string MonkLevel = "MonkLevel";
	private const string MonkXP = "MonkXP";
	private const string MonkXPBonus = "MonkXPBonus";
	private const string Monk_Badge01 = "Monk_Badge01";
	private const string Monk_Badge02 = "Monk_Badge02";
	private const string Monk_Badge03 = "Monk_Badge03";
	private const string Monk_Badge04 = "Monk_Badge04";
	private const string Monk_Badge05 = "Monk_Badge05";
	private const string Monk_Badge06 = "Monk_Badge06";
	
	private const string Storage01 = "Storage01";
	private const string Storage02 = "Storage02";
	private const string Storage03 = "Storage03";

	private const string Shortcut01 = "Shortcut01";
	private const string Shortcut02 = "Shortcut02";
	private const string Shortcut03 = "Shortcut03";
	private const string Shortcut04 = "Shortcut04";
	private const string Shortcut05 = "Shortcut05";

	private const string Trial01State = "Trial01State";
	private const string Trial02State = "Trial02State";
	private const string Trial03State = "Trial03State";
	private const string Trial04State = "Trial04State";
	private const string Trial05State = "Trial05State";
	private const string Trial06State = "Trial06State";
	private const string Trial07State = "Trial07State";
	private const string Trial08State = "Trial08State";
	private const string Trial09State = "Trial09State";
	private const string Trial10State = "Trial10State";
	
	// Save file key
	public const string SaveFileName = "__DUNGEON_HEARTS_SAVE__";


	public bool saveStateExists()
	{
		return GameSerializer.Instance.SerializedDataExists(SaveFileName);	
	}
	
	public void InitializeSaveState()
	{
		Debug.Log("INITIALIZING SAVE STATE");
		GameSerializer serializer = GameSerializer.Instance;
		serializer.ClearSerializedData();
		//serializer.CompressData = false;
		
		bool lockHero = true;
//		if (GameManager.m_gameManager.m_unlockHeroes)
//		{
//			lockHero = false;	
//		}
		
		// serializer.SaveInt; SaveFloat; etc.
		serializer.SaveString(Version, SettingsManager.m_settingsManager.version);
		serializer.SaveInt(Gold, 10);
		serializer.SaveInt (XP, 0);
		PlayerPrefs.SetInt("DoTutorial", 0);
		PlayerPrefs.Save();

		serializer.SaveBool (BrandLockState, false);
		serializer.SaveInt (BrandLevel, 1);
		serializer.SaveInt (BrandXP, 0);
		serializer.SaveFloat (BrandXPBonus, 1.0f);
		serializer.SaveInt (Brand_Badge01, 2);
		serializer.SaveInt (Brand_Badge02, 2);
		serializer.SaveInt (Brand_Badge03, 2);
		serializer.SaveInt (Brand_Badge04, 2);
		serializer.SaveInt (Brand_Badge05, 2);
		serializer.SaveInt (Brand_Badge06, 2);

		serializer.SaveBool (AugustLockState, lockHero);
		serializer.SaveInt (AugustLevel, 1);
		serializer.SaveInt (AugustXP, 0);
		serializer.SaveFloat (AugustXPBonus, 1.0f);
		serializer.SaveInt (August_Badge01, 2);
		serializer.SaveInt (August_Badge02, 2);
		serializer.SaveInt (August_Badge03, 2);
		serializer.SaveInt (August_Badge04, 2);
		serializer.SaveInt (August_Badge05, 2);
		serializer.SaveInt (August_Badge06, 2);

		serializer.SaveBool (TelinaLockState, lockHero);
		serializer.SaveInt (TelinaLevel, 1);
		serializer.SaveInt (TelinaXP, 0);
		serializer.SaveFloat (TelinaXPBonus, 1.0f);
		serializer.SaveInt (Telina_Badge01, 2);
		serializer.SaveInt (Telina_Badge02, 2);
		serializer.SaveInt (Telina_Badge03, 2);
		serializer.SaveInt (Telina_Badge04, 2);
		serializer.SaveInt (Telina_Badge05, 2);
		serializer.SaveInt (Telina_Badge06, 2);

		serializer.SaveBool (JinLockState, lockHero);
		serializer.SaveInt (JinLevel, 1);
		serializer.SaveInt (JinXP, 0);
		serializer.SaveFloat (JinXPBonus, 1.0f);
		serializer.SaveInt (Jin_Badge01, 2);
		serializer.SaveInt (Jin_Badge02, 2);
		serializer.SaveInt (Jin_Badge03, 2);
		serializer.SaveInt (Jin_Badge04, 2);
		serializer.SaveInt (Jin_Badge05, 2);
		serializer.SaveInt (Jin_Badge06, 2);

		serializer.SaveBool (KnightLockState, lockHero);
		serializer.SaveInt (KnightLevel, 1);
		serializer.SaveInt (KnightXP, 0);
		serializer.SaveFloat (KnightXPBonus, 1.0f);
		serializer.SaveInt (Knight_Badge01, 2);
		serializer.SaveInt (Knight_Badge02, 2);
		serializer.SaveInt (Knight_Badge03, 2);
		serializer.SaveInt (Knight_Badge04, 2);
		serializer.SaveInt (Knight_Badge05, 2);
		serializer.SaveInt (Knight_Badge06, 2);

		serializer.SaveBool (DragoonLockState, lockHero);
		serializer.SaveInt (DragoonLevel, 1);
		serializer.SaveInt (DragoonXP, 0);
		serializer.SaveFloat (DragoonXPBonus, 1.0f);
		serializer.SaveInt (Dragoon_Badge01, 2);
		serializer.SaveInt (Dragoon_Badge02, 2);
		serializer.SaveInt (Dragoon_Badge03, 2);
		serializer.SaveInt (Dragoon_Badge04, 2);
		serializer.SaveInt (Dragoon_Badge05, 2);
		serializer.SaveInt (Dragoon_Badge06, 2);

		serializer.SaveBool (ElfLockState, lockHero);
		serializer.SaveInt (ElfLevel, 1);
		serializer.SaveInt (ElfXP, 0);
		serializer.SaveFloat (ElfXPBonus, 1.0f);
		serializer.SaveInt (Elf_Badge01, 2);
		serializer.SaveInt (Elf_Badge02, 2);
		serializer.SaveInt (Elf_Badge03, 2);
		serializer.SaveInt (Elf_Badge04, 2);
		serializer.SaveInt (Elf_Badge05, 2);
		serializer.SaveInt (Elf_Badge06, 2);

		serializer.SaveBool (MysticLockState, lockHero);
		serializer.SaveInt (MysticLevel, 1);
		serializer.SaveInt (MysticXP, 0);
		serializer.SaveFloat (MysticXPBonus, 1.0f);
		serializer.SaveInt (Mystic_Badge01, 2);
		serializer.SaveInt (Mystic_Badge02, 2);
		serializer.SaveInt (Mystic_Badge03, 2);
		serializer.SaveInt (Mystic_Badge04, 2);
		serializer.SaveInt (Mystic_Badge05, 2);
		serializer.SaveInt (Mystic_Badge06, 2);

		serializer.SaveBool (PyromageLockState, lockHero);
		serializer.SaveInt (PyromageLevel, 1);
		serializer.SaveInt (PyromageXP, 0);
		serializer.SaveFloat (PyromageXPBonus, 1.0f);
		serializer.SaveInt (Pyromage_Badge01, 2);
		serializer.SaveInt (Pyromage_Badge02, 2);
		serializer.SaveInt (Pyromage_Badge03, 2);
		serializer.SaveInt (Pyromage_Badge04, 2);
		serializer.SaveInt (Pyromage_Badge05, 2);
		serializer.SaveInt (Pyromage_Badge06, 2);

		serializer.SaveBool (RangerLockState, lockHero);
		serializer.SaveInt (RangerLevel, 1);
		serializer.SaveInt (RangerXP, 0);
		serializer.SaveFloat (RangerXPBonus, 1.0f);
		serializer.SaveInt (Ranger_Badge01, 2);
		serializer.SaveInt (Ranger_Badge02, 2);
		serializer.SaveInt (Ranger_Badge03, 2);
		serializer.SaveInt (Ranger_Badge04, 2);
		serializer.SaveInt (Ranger_Badge05, 2);
		serializer.SaveInt (Ranger_Badge06, 2);

		serializer.SaveBool (SamuraiLockState, lockHero);
		serializer.SaveInt (SamuraiLevel, 1);
		serializer.SaveInt (SamuraiXP, 0);
		serializer.SaveFloat (SamuraiXPBonus, 1.0f);
		serializer.SaveInt (Samurai_Badge01, 2);
		serializer.SaveInt (Samurai_Badge02, 2);
		serializer.SaveInt (Samurai_Badge03, 2);
		serializer.SaveInt (Samurai_Badge04, 2);
		serializer.SaveInt (Samurai_Badge05, 2);
		serializer.SaveInt (Samurai_Badge06, 2);

		serializer.SaveBool (SeerLockState, lockHero);
		serializer.SaveInt (SeerLevel, 1);
		serializer.SaveInt (SeerXP, 0);
		serializer.SaveFloat (SeerXPBonus, 1.0f);
		serializer.SaveInt (Seer_Badge01, 2);
		serializer.SaveInt (Seer_Badge02, 2);
		serializer.SaveInt (Seer_Badge03, 2);
		serializer.SaveInt (Seer_Badge04, 2);
		serializer.SaveInt (Seer_Badge05, 2);
		serializer.SaveInt (Seer_Badge06, 2);

		serializer.SaveBool (BarbarianLockState, lockHero);
		serializer.SaveInt (BarbarianLevel, 1);
		serializer.SaveInt (BarbarianXP, 0);
		serializer.SaveFloat (BarbarianXPBonus, 1.0f);
		serializer.SaveInt (Barbarian_Badge01, 2);
		serializer.SaveInt (Barbarian_Badge02, 2);
		serializer.SaveInt (Barbarian_Badge03, 2);
		serializer.SaveInt (Barbarian_Badge04, 2);
		serializer.SaveInt (Barbarian_Badge05, 2);
		serializer.SaveInt (Barbarian_Badge06, 2);

		serializer.SaveBool (BerserkerLockState, lockHero);
		serializer.SaveInt (BerserkerLevel, 1);
		serializer.SaveInt (BerserkerXP, 0);
		serializer.SaveFloat (BerserkerXPBonus, 1.0f);
		serializer.SaveInt (Berserker_Badge01, 2);
		serializer.SaveInt (Berserker_Badge02, 2);
		serializer.SaveInt (Berserker_Badge03, 2);
		serializer.SaveInt (Berserker_Badge04, 2);
		serializer.SaveInt (Berserker_Badge05, 2);
		serializer.SaveInt (Berserker_Badge06, 2);

		serializer.SaveBool (FencerLockState, lockHero);
		serializer.SaveInt (FencerLevel, 1);
		serializer.SaveInt (FencerXP, 0);
		serializer.SaveFloat (FencerXPBonus, 1.0f);
		serializer.SaveInt (Fencer_Badge01, 2);
		serializer.SaveInt (Fencer_Badge02, 2);
		serializer.SaveInt (Fencer_Badge03, 2);
		serializer.SaveInt (Fencer_Badge04, 2);
		serializer.SaveInt (Fencer_Badge05, 2);
		serializer.SaveInt (Fencer_Badge06, 2);

		serializer.SaveBool (SuccubusLockState, lockHero);
		serializer.SaveInt (SuccubusLevel, 1);
		serializer.SaveInt (SuccubusXP, 0);
		serializer.SaveFloat (SuccubusXPBonus, 1.0f);
		serializer.SaveInt (Succubus_Badge01, 2);
		serializer.SaveInt (Succubus_Badge02, 2);
		serializer.SaveInt (Succubus_Badge03, 2);
		serializer.SaveInt (Succubus_Badge04, 2);
		serializer.SaveInt (Succubus_Badge05, 2);
		serializer.SaveInt (Succubus_Badge06, 2);

		serializer.SaveBool (DancerLockState, lockHero);
		serializer.SaveInt (DancerLevel, 1);
		serializer.SaveInt (DancerXP, 0);
		serializer.SaveFloat (DancerXPBonus, 1.0f);
		serializer.SaveInt (Dancer_Badge01, 2);
		serializer.SaveInt (Dancer_Badge02, 2);
		serializer.SaveInt (Dancer_Badge03, 2);
		serializer.SaveInt (Dancer_Badge04, 2);
		serializer.SaveInt (Dancer_Badge05, 2);
		serializer.SaveInt (Dancer_Badge06, 2);

		serializer.SaveBool (WrestlerLockState, lockHero);
		serializer.SaveInt (WrestlerLevel, 1);
		serializer.SaveInt (WrestlerXP, 0);
		serializer.SaveFloat (WrestlerXPBonus, 1.0f);
		serializer.SaveInt (Wrestler_Badge01, 2);
		serializer.SaveInt (Wrestler_Badge02, 2);
		serializer.SaveInt (Wrestler_Badge03, 2);
		serializer.SaveInt (Wrestler_Badge04, 2);
		serializer.SaveInt (Wrestler_Badge05, 2);
		serializer.SaveInt (Wrestler_Badge06, 2);

		serializer.SaveBool (PsychicLockState, lockHero);
		serializer.SaveInt (PsychicLevel, 1);
		serializer.SaveInt (PsychicXP, 0);
		serializer.SaveFloat (PsychicXPBonus, 1.0f);
		serializer.SaveInt (Psychic_Badge01, 2);
		serializer.SaveInt (Psychic_Badge02, 2);
		serializer.SaveInt (Psychic_Badge03, 2);
		serializer.SaveInt (Psychic_Badge04, 2);
		serializer.SaveInt (Psychic_Badge05, 2);
		serializer.SaveInt (Psychic_Badge06, 2);

		serializer.SaveBool (MonkLockState, lockHero);
		serializer.SaveInt (MonkLevel, 1);
		serializer.SaveInt (MonkXP, 0);
		serializer.SaveFloat (MonkXPBonus, 1.0f);
		serializer.SaveInt (Monk_Badge01, 2);
		serializer.SaveInt (Monk_Badge02, 2);
		serializer.SaveInt (Monk_Badge03, 2);
		serializer.SaveInt (Monk_Badge04, 2);
		serializer.SaveInt (Monk_Badge05, 2);
		serializer.SaveInt (Monk_Badge06, 2);

		serializer.SaveString(Storage01, "None");
		serializer.SaveString(Storage02, "None");
		serializer.SaveString(Storage03, "None");

		serializer.SaveInt (Badge_DungeonHeart, 1); // 0: Not Owned, 1: Owned, Not Equipped, 2: Equipped

		serializer.SaveInt (Badge_BadgeBoost_01, 0);
		serializer.SaveInt (Badge_BadgeBoost_02, 0);
		serializer.SaveInt (Badge_BadgeBoost_03, 0);
		serializer.SaveInt (Badge_BadgeBoost_04, 0);
		serializer.SaveInt (Badge_BadgeBoost_05, 0);

		serializer.SaveInt (Badge_DamageBoost_01, 0);
		serializer.SaveInt (Badge_DamageBoost_02, 0);
		serializer.SaveInt (Badge_DamageBoost_03, 0);
		serializer.SaveInt (Badge_DamageBoost_04, 0);
		serializer.SaveInt (Badge_HealthBoost_01, 0);
		serializer.SaveInt (Badge_HealthBoost_02, 0);
		serializer.SaveInt (Badge_HealthBoost_03, 0);
		serializer.SaveInt (Badge_HealthBoost_04, 0);
		serializer.SaveInt (Badge_EnergyBoost_01, 0);
		serializer.SaveInt (Badge_EnergyBoost_02, 0);
		serializer.SaveInt (Badge_EnergyBoost_03, 0);
		serializer.SaveInt (Badge_EnergyBoost_04, 0);
		serializer.SaveInt (Badge_HandBoost_01, 0);
		serializer.SaveInt (Badge_HandBoost_02, 0);
		serializer.SaveInt (Badge_EnergyRecover_01, 0);
		serializer.SaveInt (Badge_EnergyRecover_02, 0);
		serializer.SaveInt (Badge_HealthRecover_01, 0);

		serializer.SaveInt (Trial01State, 0);
		serializer.SaveInt (Trial02State, 0);
		serializer.SaveInt (Trial03State, 0);
		serializer.SaveInt (Trial04State, 0);
		serializer.SaveInt (Trial05State, 0);
		serializer.SaveInt (Trial06State, 0);
		serializer.SaveInt (Trial07State, 0);
		serializer.SaveInt (Trial08State, 0);
		serializer.SaveInt (Trial09State, 0);
		serializer.SaveInt (Trial10State, 0);

		serializer.SaveInt (Shortcut01, 1);
		serializer.SaveInt (Shortcut02, 1);
		serializer.SaveInt (Shortcut03, 0);
		serializer.SaveInt (Shortcut04, 0);
		serializer.SaveInt (Shortcut05, 0);
		
		serializer.SerializeDataToFile(SaveFileName);
		serializer.ClearSerializedData();
	}
	
	public void saveState()
	{
		Debug.Log("SAVING STATE");
		GameSerializer serializer = GameSerializer.Instance;
		serializer.ClearSerializedData();
		serializer.DeserializeDataFromFile(SaveFileName);
		//serializer.CompressData = false;
		
		// serializer.SaveInt; SaveFloat; etc.
		serializer.SaveString(Version, SettingsManager.m_settingsManager.version);
		serializer.SaveInt (Gold, SettingsManager.m_settingsManager.gold);
		serializer.SaveInt (XP, SettingsManager.m_settingsManager.xp);
		
		//foreach (ProgressState thisFollower in GameManager.m_gameManager.gameProgress)
		foreach (ProgressState thisFollower in SettingsManager.m_settingsManager.gameProgress)
		{
			switch(thisFollower.m_followerType)
			{
			case Follower.FollowerType.Brand:
				serializer.SaveBool(BrandLockState, false);
				serializer.SaveInt(BrandLevel, thisFollower.m_level);
				serializer.SaveInt(BrandXP, thisFollower.m_XP);
				serializer.SaveFloat(BrandXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt(Brand_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt(Brand_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt(Brand_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt(Brand_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt(Brand_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt(Brand_Badge06, 2);
				break;
			case Follower.FollowerType.August:
				//Debug.Log("August Lock: " + thisFollower.m_isLocked);
				serializer.SaveBool(AugustLockState, thisFollower.m_isLocked);
				serializer.SaveInt(AugustLevel, thisFollower.m_level);
				serializer.SaveInt(AugustXP, thisFollower.m_XP);
				serializer.SaveFloat(AugustXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (August_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (August_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (August_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (August_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (August_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (August_Badge06, 2);
				break;
			case Follower.FollowerType.Telina:
				//Debug.Log("Telina Lock: " + thisFollower.m_isLocked);
				serializer.SaveBool(TelinaLockState, thisFollower.m_isLocked);
				serializer.SaveInt(TelinaLevel, thisFollower.m_level);
				serializer.SaveInt(TelinaXP, thisFollower.m_XP);
				serializer.SaveFloat(TelinaXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Telina_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Telina_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Telina_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Telina_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Telina_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Telina_Badge06, 2);
				break;
			case Follower.FollowerType.Jin:
				serializer.SaveBool(JinLockState, thisFollower.m_isLocked);
				serializer.SaveInt(JinLevel, thisFollower.m_level);
				serializer.SaveInt(JinXP, thisFollower.m_XP);
				serializer.SaveFloat(JinXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Jin_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Jin_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Jin_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Jin_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Jin_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Jin_Badge06, 2);
				break;
			case Follower.FollowerType.Knight:
				serializer.SaveBool(KnightLockState, thisFollower.m_isLocked);
				serializer.SaveInt(KnightLevel, thisFollower.m_level);
				serializer.SaveInt(KnightXP, thisFollower.m_XP);
				serializer.SaveFloat(KnightXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Knight_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Knight_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Knight_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Knight_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Knight_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Knight_Badge06, 2);
				break;
			case Follower.FollowerType.Dragoon:
				serializer.SaveBool(DragoonLockState, thisFollower.m_isLocked);
				serializer.SaveInt(DragoonLevel, thisFollower.m_level);
				serializer.SaveInt(DragoonXP, thisFollower.m_XP);
				serializer.SaveFloat(DragoonXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Dragoon_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Dragoon_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Dragoon_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Dragoon_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Dragoon_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Dragoon_Badge06, 2);
				break;
			case Follower.FollowerType.Elf:
				serializer.SaveBool(ElfLockState, thisFollower.m_isLocked);
				serializer.SaveInt(ElfLevel, thisFollower.m_level);
				serializer.SaveInt(ElfXP, thisFollower.m_XP);
				serializer.SaveFloat(ElfXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Elf_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Elf_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Elf_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Elf_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Elf_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Elf_Badge06, 2);
				break;
			case Follower.FollowerType.Mystic:
				serializer.SaveBool(MysticLockState, thisFollower.m_isLocked);
				serializer.SaveInt(MysticLevel, thisFollower.m_level);
				serializer.SaveInt(MysticXP, thisFollower.m_XP);
				serializer.SaveFloat(MysticXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Mystic_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Mystic_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Mystic_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Mystic_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Mystic_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Mystic_Badge06, 2);
				break;
			case Follower.FollowerType.Pyromage:
				serializer.SaveBool(PyromageLockState, thisFollower.m_isLocked);
				serializer.SaveInt(PyromageLevel, thisFollower.m_level);
				serializer.SaveInt(PyromageXP, thisFollower.m_XP);
				serializer.SaveFloat(PyromageXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Pyromage_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Pyromage_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Pyromage_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Pyromage_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Pyromage_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Pyromage_Badge06, 2);
				break;
			case Follower.FollowerType.Ranger:
				serializer.SaveBool(RangerLockState, thisFollower.m_isLocked);
				serializer.SaveInt(RangerLevel, thisFollower.m_level);
				serializer.SaveInt(RangerXP, thisFollower.m_XP);
				serializer.SaveFloat(RangerXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Ranger_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Ranger_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Ranger_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Ranger_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Ranger_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Ranger_Badge06, 2);
				break;
			case Follower.FollowerType.Samurai:
				serializer.SaveBool(SamuraiLockState, thisFollower.m_isLocked);
				serializer.SaveInt(SamuraiLevel, thisFollower.m_level);
				serializer.SaveInt(SamuraiXP, thisFollower.m_XP);
				serializer.SaveFloat(SamuraiXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Samurai_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Samurai_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Samurai_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Samurai_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Samurai_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Samurai_Badge06, 2);
				break;
			case Follower.FollowerType.Seer:
				serializer.SaveBool(SeerLockState, thisFollower.m_isLocked);
				serializer.SaveInt(SeerLevel, thisFollower.m_level);
				serializer.SaveInt(SeerXP, thisFollower.m_XP);
				serializer.SaveFloat(SeerXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Seer_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Seer_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Seer_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Seer_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Seer_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Seer_Badge06, 2);
				break;
			case Follower.FollowerType.Barbarian:
				serializer.SaveBool(BarbarianLockState, thisFollower.m_isLocked);
				serializer.SaveInt(BarbarianLevel, thisFollower.m_level);
				serializer.SaveInt(BarbarianXP, thisFollower.m_XP);
				serializer.SaveFloat(BarbarianXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Barbarian_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Barbarian_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Barbarian_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Barbarian_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Barbarian_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Barbarian_Badge06, 2);
				break;
			case Follower.FollowerType.Berserker:
				serializer.SaveBool(BerserkerLockState, thisFollower.m_isLocked);
				serializer.SaveInt(BerserkerLevel, thisFollower.m_level);
				serializer.SaveInt(BerserkerXP, thisFollower.m_XP);
				serializer.SaveFloat(BerserkerXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Berserker_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Berserker_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Berserker_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Berserker_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Berserker_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Berserker_Badge06, 2);
				break;
			case Follower.FollowerType.Fencer:
				serializer.SaveBool(FencerLockState, thisFollower.m_isLocked);
				serializer.SaveInt(FencerLevel, thisFollower.m_level);
				serializer.SaveInt(FencerXP, thisFollower.m_XP);
				serializer.SaveFloat(FencerXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Fencer_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Fencer_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Fencer_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Fencer_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Fencer_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Fencer_Badge06, 2);
				break;
			case Follower.FollowerType.Succubus:
				serializer.SaveBool(SuccubusLockState, thisFollower.m_isLocked);
				serializer.SaveInt(SuccubusLevel, thisFollower.m_level);
				serializer.SaveInt(SuccubusXP, thisFollower.m_XP);
				serializer.SaveInt (Succubus_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Succubus_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Succubus_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Succubus_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Succubus_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Succubus_Badge06, 2);
				break;
			case Follower.FollowerType.Dancer:
				serializer.SaveBool(DancerLockState, thisFollower.m_isLocked);
				serializer.SaveInt(DancerLevel, thisFollower.m_level);
				serializer.SaveInt(DancerXP, thisFollower.m_XP);
				serializer.SaveFloat(DancerXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Dancer_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Dancer_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Dancer_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Dancer_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Dancer_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Dancer_Badge06, 2);
				break;
			case Follower.FollowerType.Wrestler:
				serializer.SaveBool(WrestlerLockState, thisFollower.m_isLocked);
				serializer.SaveInt(WrestlerLevel, thisFollower.m_level);
				serializer.SaveInt(WrestlerXP, thisFollower.m_XP);
				serializer.SaveFloat(WrestlerXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Wrestler_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Wrestler_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Wrestler_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Wrestler_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Wrestler_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Wrestler_Badge06, 2);
				break;
			case Follower.FollowerType.Psychic:
				serializer.SaveBool(PsychicLockState, thisFollower.m_isLocked);
				serializer.SaveInt(PsychicLevel, thisFollower.m_level);
				serializer.SaveInt(PsychicXP, thisFollower.m_XP);
				serializer.SaveFloat(PsychicXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Psychic_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Psychic_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Psychic_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Psychic_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Psychic_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Psychic_Badge06, 2);
				break;
			case Follower.FollowerType.Monk:
				serializer.SaveBool(MonkLockState, thisFollower.m_isLocked);
				serializer.SaveInt(MonkLevel, thisFollower.m_level);
				serializer.SaveInt(MonkXP, thisFollower.m_XP);
				serializer.SaveFloat(MonkXPBonus, thisFollower.m_XPBonus);
				serializer.SaveInt (Monk_Badge01, thisFollower.m_badgeLevel1);
				serializer.SaveInt (Monk_Badge02, thisFollower.m_badgeLevel2);
				serializer.SaveInt (Monk_Badge03, thisFollower.m_badgeLevel3);
				serializer.SaveInt (Monk_Badge04, thisFollower.m_badgeLevel4);
				serializer.SaveInt (Monk_Badge05, thisFollower.m_badgeLevel5);
				serializer.SaveInt (Monk_Badge06, 2);
				break;
			}
		}
			//Debug.Log("Saving storage items");
//		if (GameManager.m_gameManager != null)
//		{
//			if (GameManager.m_gameManager.storageItems.Count > 0)
//			{
//				Item thisItem = GameManager.m_gameManager.storageItems[0];
//				serializer.SaveString(Storage01, thisItem.m_storageName);
//				Debug.Log("saving item " + thisItem.m_storageName);
//			}
//			if (GameManager.m_gameManager.storageItems.Count > 1)
//			{
//				Item thisItem = GameManager.m_gameManager.storageItems[1];
//				serializer.SaveString(Storage02, thisItem.m_storageName);
//				Debug.Log("saving item " + thisItem.m_storageName);
//			}
//			if (GameManager.m_gameManager.storageItems.Count > 2)
//			{
//				Item thisItem = GameManager.m_gameManager.storageItems[2];
//				serializer.SaveString(Storage03, thisItem.m_storageName);
//				Debug.Log("saving item " + thisItem.m_storageName);
//			}
//		}
//		
//		string item1Name = serializer.LoadString(Storage01);
//		serializer.SaveString(Storage01, item1Name);
//		string item2Name = serializer.LoadString(Storage02);
//		serializer.SaveString(Storage02, item2Name);
//		string item3Name = serializer.LoadString(Storage03);
//		serializer.SaveString(Storage03, item3Name);

		//save badge state
		if (SettingsManager.m_settingsManager != null)
		{
			if (SettingsManager.m_settingsManager.badgeStates.Count > 0)
			{
				List<int> bs = SettingsManager.m_settingsManager.badgeStates;

				serializer.SaveInt (Badge_DungeonHeart, bs[0]);

				serializer.SaveInt (Badge_DamageBoost_01, bs[1]);
				serializer.SaveInt (Badge_DamageBoost_02, bs[2]);
				serializer.SaveInt (Badge_DamageBoost_03, bs[3]);
				serializer.SaveInt (Badge_DamageBoost_04, bs[4]);

				serializer.SaveInt (Badge_HealthBoost_01, bs[5]);
				serializer.SaveInt (Badge_HealthBoost_02, bs[6]);
				serializer.SaveInt (Badge_HealthBoost_03, bs[7]);
				serializer.SaveInt (Badge_HealthBoost_04, bs[8]);

				serializer.SaveInt (Badge_EnergyBoost_01, bs[9]);
				serializer.SaveInt (Badge_EnergyBoost_02, bs[10]);
				serializer.SaveInt (Badge_EnergyBoost_03, bs[11]);
				serializer.SaveInt (Badge_EnergyBoost_04, bs[12]);

				serializer.SaveInt (Badge_HandBoost_01, bs[13]);
				serializer.SaveInt (Badge_HandBoost_02, bs[14]);

				serializer.SaveInt (Badge_HealthRecover_01, bs[15]);

				serializer.SaveInt (Badge_EnergyRecover_01, bs[16]);
				serializer.SaveInt (Badge_EnergyRecover_02, bs[17]);

				serializer.SaveInt (Badge_BadgeBoost_01, bs[18]);
				serializer.SaveInt (Badge_BadgeBoost_02, bs[19]);
				serializer.SaveInt (Badge_BadgeBoost_03, bs[20]);
				serializer.SaveInt (Badge_BadgeBoost_04, bs[21]);
				serializer.SaveInt (Badge_BadgeBoost_05, bs[22]);

			}

			if (SettingsManager.m_settingsManager.trialStates.Count > 0)
			{
				List<int> ts = SettingsManager.m_settingsManager.trialStates;

				serializer.SaveInt (Trial01State, ts[0]);
				serializer.SaveInt (Trial02State, ts[1]);
				serializer.SaveInt (Trial03State, ts[2]);
				serializer.SaveInt (Trial04State, ts[3]);
				serializer.SaveInt (Trial05State, ts[4]);
				serializer.SaveInt (Trial06State, ts[5]);
				serializer.SaveInt (Trial07State, ts[6]);
				serializer.SaveInt (Trial08State, ts[7]);
				serializer.SaveInt (Trial09State, ts[8]);
				serializer.SaveInt (Trial10State, ts[9]);
			}

			if (SettingsManager.m_settingsManager.shortcutStates.Count > 0)
			{
				List<int> scs = SettingsManager.m_settingsManager.shortcutStates;
				
				serializer.SaveInt (Trial01State, scs[0]);
				serializer.SaveInt (Trial02State, scs[1]);
				serializer.SaveInt (Trial03State, scs[2]);
				serializer.SaveInt (Trial04State, scs[3]);
				serializer.SaveInt (Trial05State, scs[4]);
			}
		}
		
		serializer.SerializeDataToFile(SaveFileName);
		serializer.ClearSerializedData();
	}
	
	public void SaveStorageState ()
	{
//		Debug.Log("SAVING STATE WITH STORAGE");
//		GameSerializer serializer = GameSerializer.Instance;
//		serializer.ClearSerializedData();
//		serializer.DeserializeDataFromFile(SaveFileName);
//		
//		serializer.SaveString(Version, SettingsManager.m_settingsManager.version);
//		serializer.SaveInt (Gold, SettingsManager.m_settingsManager.gold);
//		serializer.SaveInt (XP, SettingsManager.m_settingsManager.xp);
//
//		List<ProgressState> progress = new List<ProgressState>();
//		
//		ProgressState brandProgress = new ProgressState();
//		brandProgress.m_followerType = Follower.FollowerType.Brand;
//		brandProgress.m_isLocked = serializer.LoadBool(BrandLockState);
//		brandProgress.m_level = serializer.LoadInt(BrandLevel);
//		brandProgress.m_XP = serializer.LoadInt(BrandXP);
//		brandProgress.m_XPBonus = serializer.LoadFloat(BrandXPBonus);
//		brandProgress.m_badgeLevel1 = serializer.LoadInt (Brand_Badge01);
//		brandProgress.m_badgeLevel2 = serializer.LoadInt (Brand_Badge02);
//		brandProgress.m_badgeLevel3 = serializer.LoadInt (Brand_Badge03);
//		brandProgress.m_badgeLevel4 = serializer.LoadInt (Brand_Badge04);
//		brandProgress.m_badgeLevel5 = serializer.LoadInt (Brand_Badge05);
//		brandProgress.m_badgeLevel6 = serializer.LoadInt (Brand_Badge06);
//		progress.Add(brandProgress);
//		
//		ProgressState augustProgress = new ProgressState();
//		augustProgress.m_followerType = Follower.FollowerType.August;
//		augustProgress.m_isLocked = serializer.LoadBool(AugustLockState);
//		augustProgress.m_level = serializer.LoadInt(AugustLevel);
//		augustProgress.m_XP = serializer.LoadInt(AugustXP);
//		augustProgress.m_XPBonus = serializer.LoadFloat(AugustXPBonus);
//		augustProgress.m_badgeLevel1 = serializer.LoadInt (August_Badge01);
//		augustProgress.m_badgeLevel2 = serializer.LoadInt (August_Badge02);
//		augustProgress.m_badgeLevel3 = serializer.LoadInt (August_Badge03);
//		augustProgress.m_badgeLevel4 = serializer.LoadInt (August_Badge04);
//		augustProgress.m_badgeLevel5 = serializer.LoadInt (August_Badge05);
//		augustProgress.m_badgeLevel6 = serializer.LoadInt (August_Badge06);
//		progress.Add(augustProgress);
//
//		ProgressState telinaProgress = new ProgressState();
//		telinaProgress.m_followerType = Follower.FollowerType.Telina;
//		telinaProgress.m_isLocked = serializer.LoadBool(TelinaLockState);
//		telinaProgress.m_level = serializer.LoadInt(TelinaLevel);
//		telinaProgress.m_XP = serializer.LoadInt(TelinaXP);
//		telinaProgress.m_XPBonus = serializer.LoadFloat(TelinaXPBonus);
//		telinaProgress.m_badgeLevel1 = serializer.LoadInt (Telina_Badge01);
//		telinaProgress.m_badgeLevel2 = serializer.LoadInt (Telina_Badge02);
//		telinaProgress.m_badgeLevel3 = serializer.LoadInt (Telina_Badge03);
//		telinaProgress.m_badgeLevel4 = serializer.LoadInt (Telina_Badge04);
//		telinaProgress.m_badgeLevel5 = serializer.LoadInt (Telina_Badge05);
//		telinaProgress.m_badgeLevel6 = serializer.LoadInt (Telina_Badge06);
//		progress.Add(telinaProgress);
//
//		ProgressState jinProgress = new ProgressState();
//		jinProgress.m_followerType = Follower.FollowerType.Jin;
//		jinProgress.m_isLocked = serializer.LoadBool(JinLockState);
//		jinProgress.m_level = serializer.LoadInt(JinLevel);
//		jinProgress.m_XP = serializer.LoadInt(JinXP);
//		jinProgress.m_XPBonus = serializer.LoadFloat(JinXPBonus);
//		jinProgress.m_badgeLevel1 = serializer.LoadInt (Jin_Badge01);
//		jinProgress.m_badgeLevel2 = serializer.LoadInt (Jin_Badge02);
//		jinProgress.m_badgeLevel3 = serializer.LoadInt (Jin_Badge03);
//		jinProgress.m_badgeLevel4 = serializer.LoadInt (Jin_Badge04);
//		jinProgress.m_badgeLevel5 = serializer.LoadInt (Jin_Badge05);
//		jinProgress.m_badgeLevel6 = serializer.LoadInt (Jin_Badge06);
//		progress.Add(jinProgress);
//		
//		ProgressState knightProgress = new ProgressState();
//		knightProgress.m_followerType = Follower.FollowerType.Knight;
//		knightProgress.m_isLocked = serializer.LoadBool(KnightLockState);
//		knightProgress.m_level = serializer.LoadInt(KnightLevel);
//		knightProgress.m_XP = serializer.LoadInt(KnightXP);
//		knightProgress.m_XPBonus = serializer.LoadFloat(KnightXPBonus);
//		knightProgress.m_badgeLevel1 = serializer.LoadInt (Knight_Badge01);
//		knightProgress.m_badgeLevel2 = serializer.LoadInt (Knight_Badge02);
//		knightProgress.m_badgeLevel3 = serializer.LoadInt (Knight_Badge03);
//		knightProgress.m_badgeLevel4 = serializer.LoadInt (Knight_Badge04);
//		knightProgress.m_badgeLevel5 = serializer.LoadInt (Knight_Badge05);
//		knightProgress.m_badgeLevel6 = serializer.LoadInt (Knight_Badge06);
//		progress.Add(knightProgress);
//
//		ProgressState dragoonProgress = new ProgressState();
//		dragoonProgress.m_followerType = Follower.FollowerType.Dragoon;
//		dragoonProgress.m_isLocked = serializer.LoadBool(DragoonLockState);
//		dragoonProgress.m_level = serializer.LoadInt(DragoonLevel);
//		dragoonProgress.m_XP = serializer.LoadInt(DragoonXP);
//		dragoonProgress.m_XPBonus = serializer.LoadFloat(DragoonXPBonus);
//		dragoonProgress.m_badgeLevel1 = serializer.LoadInt (Dragoon_Badge01);
//		dragoonProgress.m_badgeLevel2 = serializer.LoadInt (Dragoon_Badge02);
//		dragoonProgress.m_badgeLevel3 = serializer.LoadInt (Dragoon_Badge03);
//		dragoonProgress.m_badgeLevel4 = serializer.LoadInt (Dragoon_Badge04);
//		dragoonProgress.m_badgeLevel5 = serializer.LoadInt (Dragoon_Badge05);
//		dragoonProgress.m_badgeLevel6 = serializer.LoadInt (Dragoon_Badge06);
//		progress.Add(dragoonProgress);
//		
//		ProgressState elfProgress = new ProgressState();
//		elfProgress.m_followerType = Follower.FollowerType.Elf;
//		elfProgress.m_isLocked = serializer.LoadBool(ElfLockState);
//		elfProgress.m_level = serializer.LoadInt(ElfLevel);
//		elfProgress.m_XP = serializer.LoadInt(ElfXP);
//		elfProgress.m_XPBonus = serializer.LoadFloat(ElfXPBonus);
//		elfProgress.m_badgeLevel1 = serializer.LoadInt (Elf_Badge01);
//		elfProgress.m_badgeLevel2 = serializer.LoadInt (Elf_Badge02);
//		elfProgress.m_badgeLevel3 = serializer.LoadInt (Elf_Badge03);
//		elfProgress.m_badgeLevel4 = serializer.LoadInt (Elf_Badge04);
//		elfProgress.m_badgeLevel5 = serializer.LoadInt (Elf_Badge05);
//		elfProgress.m_badgeLevel6 = serializer.LoadInt (Elf_Badge06);
//		progress.Add(elfProgress);
//		
//		ProgressState mysticProgress = new ProgressState();
//		mysticProgress.m_followerType = Follower.FollowerType.Mystic;
//		mysticProgress.m_isLocked = serializer.LoadBool(MysticLockState);
//		mysticProgress.m_level = serializer.LoadInt(MysticLevel);
//		mysticProgress.m_XP = serializer.LoadInt(MysticXP);
//		mysticProgress.m_XPBonus = serializer.LoadFloat(MysticXPBonus);
//		mysticProgress.m_badgeLevel1 = serializer.LoadInt (Mystic_Badge01);
//		mysticProgress.m_badgeLevel2 = serializer.LoadInt (Mystic_Badge02);
//		mysticProgress.m_badgeLevel3 = serializer.LoadInt (Mystic_Badge03);
//		mysticProgress.m_badgeLevel4 = serializer.LoadInt (Mystic_Badge04);
//		mysticProgress.m_badgeLevel5 = serializer.LoadInt (Mystic_Badge05);
//		mysticProgress.m_badgeLevel6 = serializer.LoadInt (Mystic_Badge06);
//		progress.Add(mysticProgress);
//		
//		ProgressState pyromageProgress = new ProgressState();
//		pyromageProgress.m_followerType = Follower.FollowerType.Pyromage;
//		pyromageProgress.m_isLocked = serializer.LoadBool(PyromageLockState);
//		pyromageProgress.m_level = serializer.LoadInt(PyromageLevel);
//		pyromageProgress.m_XP = serializer.LoadInt(PyromageXP);
//		pyromageProgress.m_XPBonus = serializer.LoadFloat(PyromageXPBonus);
//		pyromageProgress.m_badgeLevel1 = serializer.LoadInt (Pyromage_Badge01);
//		pyromageProgress.m_badgeLevel2 = serializer.LoadInt (Pyromage_Badge02);
//		pyromageProgress.m_badgeLevel3 = serializer.LoadInt (Pyromage_Badge03);
//		pyromageProgress.m_badgeLevel4 = serializer.LoadInt (Pyromage_Badge04);
//		pyromageProgress.m_badgeLevel5 = serializer.LoadInt (Pyromage_Badge05);
//		pyromageProgress.m_badgeLevel6 = serializer.LoadInt (Pyromage_Badge06);
//		progress.Add(pyromageProgress);
//		
//		ProgressState rangerProgress = new ProgressState();
//		rangerProgress.m_followerType = Follower.FollowerType.Ranger;
//		rangerProgress.m_isLocked = serializer.LoadBool(RangerLockState);
//		rangerProgress.m_level = serializer.LoadInt(RangerLevel);
//		rangerProgress.m_XP = serializer.LoadInt(RangerXP);
//		rangerProgress.m_XPBonus = serializer.LoadFloat(RangerXPBonus);
//		rangerProgress.m_badgeLevel1 = serializer.LoadInt (Ranger_Badge01);
//		rangerProgress.m_badgeLevel2 = serializer.LoadInt (Ranger_Badge02);
//		rangerProgress.m_badgeLevel3 = serializer.LoadInt (Ranger_Badge03);
//		rangerProgress.m_badgeLevel4 = serializer.LoadInt (Ranger_Badge04);
//		rangerProgress.m_badgeLevel5 = serializer.LoadInt (Ranger_Badge05);
//		rangerProgress.m_badgeLevel6 = serializer.LoadInt (Ranger_Badge06);
//		progress.Add(rangerProgress);
//		
//		ProgressState samuraiProgress = new ProgressState();
//		samuraiProgress.m_followerType = Follower.FollowerType.Samurai;
//		samuraiProgress.m_isLocked = serializer.LoadBool(SamuraiLockState);
//		samuraiProgress.m_level = serializer.LoadInt(SamuraiLevel);
//		samuraiProgress.m_XP = serializer.LoadInt(SamuraiXP);
//		samuraiProgress.m_XPBonus = serializer.LoadFloat(SamuraiXPBonus);
//		samuraiProgress.m_badgeLevel1 = serializer.LoadInt (Samurai_Badge01);
//		samuraiProgress.m_badgeLevel2 = serializer.LoadInt (Samurai_Badge02);
//		samuraiProgress.m_badgeLevel3 = serializer.LoadInt (Samurai_Badge03);
//		samuraiProgress.m_badgeLevel4 = serializer.LoadInt (Samurai_Badge04);
//		samuraiProgress.m_badgeLevel5 = serializer.LoadInt (Samurai_Badge05);
//		samuraiProgress.m_badgeLevel6 = serializer.LoadInt (Samurai_Badge06);
//		progress.Add(samuraiProgress);
//
//		ProgressState seerProgress = new ProgressState();
//		seerProgress.m_followerType = Follower.FollowerType.Seer;
//		seerProgress.m_isLocked = serializer.LoadBool(SeerLockState);
//		seerProgress.m_level = serializer.LoadInt(SeerLevel);
//		seerProgress.m_XP = serializer.LoadInt(SeerXP);
//		seerProgress.m_XPBonus = serializer.LoadFloat(SeerXPBonus);
//		seerProgress.m_badgeLevel1 = serializer.LoadInt (Seer_Badge01);
//		seerProgress.m_badgeLevel2 = serializer.LoadInt (Seer_Badge02);
//		seerProgress.m_badgeLevel3 = serializer.LoadInt (Seer_Badge03);
//		seerProgress.m_badgeLevel4 = serializer.LoadInt (Seer_Badge04);
//		seerProgress.m_badgeLevel5 = serializer.LoadInt (Seer_Badge05);
//		seerProgress.m_badgeLevel6 = serializer.LoadInt (Seer_Badge06);
//		progress.Add(seerProgress);
//
//		ProgressState barbarianProgress = new ProgressState();
//		barbarianProgress.m_followerType = Follower.FollowerType.Barbarian;
//		barbarianProgress.m_isLocked = serializer.LoadBool(BarbarianLockState);
//		barbarianProgress.m_level = serializer.LoadInt(BarbarianLevel);
//		barbarianProgress.m_XP = serializer.LoadInt(BarbarianXP);
//		barbarianProgress.m_XPBonus = serializer.LoadFloat(BarbarianXPBonus);
//		barbarianProgress.m_badgeLevel1 = serializer.LoadInt (Barbarian_Badge01);
//		barbarianProgress.m_badgeLevel2 = serializer.LoadInt (Barbarian_Badge02);
//		barbarianProgress.m_badgeLevel3 = serializer.LoadInt (Barbarian_Badge03);
//		barbarianProgress.m_badgeLevel4 = serializer.LoadInt (Barbarian_Badge04);
//		barbarianProgress.m_badgeLevel5 = serializer.LoadInt (Barbarian_Badge05);
//		barbarianProgress.m_badgeLevel6 = serializer.LoadInt (Barbarian_Badge06);
//		progress.Add(barbarianProgress);
//
//		ProgressState berserkerProgress = new ProgressState();
//		berserkerProgress.m_followerType = Follower.FollowerType.Berserker;
//		berserkerProgress.m_isLocked = serializer.LoadBool(BerserkerLockState);
//		berserkerProgress.m_level = serializer.LoadInt(BerserkerLevel);
//		berserkerProgress.m_XP = serializer.LoadInt(BerserkerXP);
//		berserkerProgress.m_XPBonus = serializer.LoadFloat(BerserkerXPBonus);
//		berserkerProgress.m_badgeLevel1 = serializer.LoadInt (Berserker_Badge01);
//		berserkerProgress.m_badgeLevel2 = serializer.LoadInt (Berserker_Badge02);
//		berserkerProgress.m_badgeLevel3 = serializer.LoadInt (Berserker_Badge03);
//		berserkerProgress.m_badgeLevel4 = serializer.LoadInt (Berserker_Badge04);
//		berserkerProgress.m_badgeLevel5 = serializer.LoadInt (Berserker_Badge05);
//		berserkerProgress.m_badgeLevel6 = serializer.LoadInt (Berserker_Badge06);
//		progress.Add(berserkerProgress);
//		
//		ProgressState fencerProgress = new ProgressState();
//		fencerProgress.m_followerType = Follower.FollowerType.Fencer;
//		fencerProgress.m_isLocked = serializer.LoadBool(FencerLockState);
//		fencerProgress.m_level = serializer.LoadInt(FencerLevel);
//		fencerProgress.m_XP = serializer.LoadInt(FencerXP);
//		fencerProgress.m_XPBonus = serializer.LoadFloat(FencerXPBonus);
//		fencerProgress.m_badgeLevel1 = serializer.LoadInt (Fencer_Badge01);
//		fencerProgress.m_badgeLevel2 = serializer.LoadInt (Fencer_Badge02);
//		fencerProgress.m_badgeLevel3 = serializer.LoadInt (Fencer_Badge03);
//		fencerProgress.m_badgeLevel4 = serializer.LoadInt (Fencer_Badge04);
//		fencerProgress.m_badgeLevel5 = serializer.LoadInt (Fencer_Badge05);
//		fencerProgress.m_badgeLevel6 = serializer.LoadInt (Fencer_Badge06);
//		progress.Add(fencerProgress);
//		
//		ProgressState succubusProgress = new ProgressState();
//		succubusProgress.m_followerType = Follower.FollowerType.Succubus;
//		succubusProgress.m_isLocked = serializer.LoadBool(SuccubusLockState);
//		succubusProgress.m_level = serializer.LoadInt(SuccubusLevel);
//		succubusProgress.m_XP = serializer.LoadInt(SuccubusXP);
//		succubusProgress.m_XPBonus = serializer.LoadFloat(SuccubusXPBonus);
//		succubusProgress.m_badgeLevel1 = serializer.LoadInt (Succubus_Badge01);
//		succubusProgress.m_badgeLevel2 = serializer.LoadInt (Succubus_Badge02);
//		succubusProgress.m_badgeLevel3 = serializer.LoadInt (Succubus_Badge03);
//		succubusProgress.m_badgeLevel4 = serializer.LoadInt (Succubus_Badge04);
//		succubusProgress.m_badgeLevel5 = serializer.LoadInt (Succubus_Badge05);
//		succubusProgress.m_badgeLevel6 = serializer.LoadInt (Succubus_Badge06);
//		progress.Add(succubusProgress);
//		
//		ProgressState dancerProgress = new ProgressState();
//		dancerProgress.m_followerType = Follower.FollowerType.Dancer;
//		dancerProgress.m_isLocked = serializer.LoadBool(DancerLockState);
//		dancerProgress.m_level = serializer.LoadInt(DancerLevel);
//		dancerProgress.m_XP = serializer.LoadInt(DancerXP);
//		dancerProgress.m_XPBonus = serializer.LoadFloat(DancerXPBonus);
//		dancerProgress.m_badgeLevel1 = serializer.LoadInt (Dancer_Badge01);
//		dancerProgress.m_badgeLevel2 = serializer.LoadInt (Dancer_Badge02);
//		dancerProgress.m_badgeLevel3 = serializer.LoadInt (Dancer_Badge03);
//		dancerProgress.m_badgeLevel4 = serializer.LoadInt (Dancer_Badge04);
//		dancerProgress.m_badgeLevel5 = serializer.LoadInt (Dancer_Badge05);
//		dancerProgress.m_badgeLevel6 = serializer.LoadInt (Dancer_Badge06);
//		progress.Add(dancerProgress);
//		
//		ProgressState wrestlerProgress = new ProgressState();
//		wrestlerProgress.m_followerType = Follower.FollowerType.Wrestler;
//		wrestlerProgress.m_isLocked = serializer.LoadBool(WrestlerLockState);
//		wrestlerProgress.m_level = serializer.LoadInt(WrestlerLevel);
//		wrestlerProgress.m_XP = serializer.LoadInt(WrestlerXP);
//		wrestlerProgress.m_XPBonus = serializer.LoadFloat(WrestlerXPBonus);
//		wrestlerProgress.m_badgeLevel1 = serializer.LoadInt (Wrestler_Badge01);
//		wrestlerProgress.m_badgeLevel2 = serializer.LoadInt (Wrestler_Badge02);
//		wrestlerProgress.m_badgeLevel3 = serializer.LoadInt (Wrestler_Badge03);
//		wrestlerProgress.m_badgeLevel4 = serializer.LoadInt (Wrestler_Badge04);
//		wrestlerProgress.m_badgeLevel5 = serializer.LoadInt (Wrestler_Badge05);
//		wrestlerProgress.m_badgeLevel6 = serializer.LoadInt (Wrestler_Badge06);
//		progress.Add(wrestlerProgress);
//		
//		ProgressState psychicProgress = new ProgressState();
//		psychicProgress.m_followerType = Follower.FollowerType.Psychic;
//		psychicProgress.m_isLocked = serializer.LoadBool(PsychicLockState);
//		psychicProgress.m_level = serializer.LoadInt(PsychicLevel);
//		psychicProgress.m_XP = serializer.LoadInt(PsychicXP);
//		psychicProgress.m_XPBonus = serializer.LoadFloat(PsychicXPBonus);
//		psychicProgress.m_badgeLevel1 = serializer.LoadInt (Psychic_Badge01);
//		psychicProgress.m_badgeLevel2 = serializer.LoadInt (Psychic_Badge02);
//		psychicProgress.m_badgeLevel3 = serializer.LoadInt (Psychic_Badge03);
//		psychicProgress.m_badgeLevel4 = serializer.LoadInt (Psychic_Badge04);
//		psychicProgress.m_badgeLevel5 = serializer.LoadInt (Psychic_Badge05);
//		psychicProgress.m_badgeLevel6 = serializer.LoadInt (Psychic_Badge06);
//		progress.Add(psychicProgress);
//		
//		ProgressState monkProgress = new ProgressState();
//		monkProgress.m_followerType = Follower.FollowerType.Monk;
//		monkProgress.m_isLocked = serializer.LoadBool(MonkLockState);
//		monkProgress.m_level = serializer.LoadInt(MonkLevel);
//		monkProgress.m_XP = serializer.LoadInt(MonkXP);
//		monkProgress.m_XPBonus = serializer.LoadFloat(MonkXPBonus);
//		monkProgress.m_badgeLevel1 = serializer.LoadInt (Monk_Badge01);
//		monkProgress.m_badgeLevel2 = serializer.LoadInt (Monk_Badge02);
//		monkProgress.m_badgeLevel3 = serializer.LoadInt (Monk_Badge03);
//		monkProgress.m_badgeLevel4 = serializer.LoadInt (Monk_Badge04);
//		monkProgress.m_badgeLevel5 = serializer.LoadInt (Monk_Badge05);
//		monkProgress.m_badgeLevel6 = serializer.LoadInt (Monk_Badge06);
//		progress.Add(monkProgress);
//		
//		
//		
////		
////		
////		foreach (ProgressState thisFollower in progress)
////		{
////			switch(thisFollower.m_followerType)
////			{
////			case Follower.FollowerType.Brand:
////				serializer.SaveBool(BrandLockState, false);
////				serializer.SaveInt(BrandLevel, thisFollower.m_level);
////				serializer.SaveInt(BrandXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.August:
////				//Debug.Log("August Lock: " + thisFollower.m_isLocked);
////				serializer.SaveBool(AugustLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(AugustLevel, thisFollower.m_level);
////				serializer.SaveInt(AugustXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Telina:
////				//Debug.Log("Telina Lock: " + thisFollower.m_isLocked);
////				serializer.SaveBool(TelinaLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(TelinaLevel, thisFollower.m_level);
////				serializer.SaveInt(TelinaXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Jin:
////				serializer.SaveBool(JinLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(JinLevel, thisFollower.m_level);
////				serializer.SaveInt(JinXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Knight:
////				serializer.SaveBool(KnightLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(KnightLevel, thisFollower.m_level);
////				serializer.SaveInt(KnightXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Dragoon:
////				serializer.SaveBool(DragoonLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(DragoonLevel, thisFollower.m_level);
////				serializer.SaveInt(DragoonXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Elf:
////				serializer.SaveBool(ElfLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(ElfLevel, thisFollower.m_level);
////				serializer.SaveInt(ElfXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Mystic:
////				serializer.SaveBool(MysticLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(MysticLevel, thisFollower.m_level);
////				serializer.SaveInt(MysticXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Pyromage:
////				serializer.SaveBool(PyromageLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(PyromageLevel, thisFollower.m_level);
////				serializer.SaveInt(PyromageXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Ranger:
////				serializer.SaveBool(RangerLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(RangerLevel, thisFollower.m_level);
////				serializer.SaveInt(RangerXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Samurai:
////				serializer.SaveBool(SamuraiLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(SamuraiLevel, thisFollower.m_level);
////				serializer.SaveInt(SamuraiXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Seer:
////				serializer.SaveBool(SeerLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(SeerLevel, thisFollower.m_level);
////				serializer.SaveInt(SeerXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Barbarian:
////				serializer.SaveBool(BarbarianLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(BarbarianLevel, thisFollower.m_level);
////				serializer.SaveInt(BarbarianXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Berserker:
////				serializer.SaveBool(BerserkerLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(BerserkerLevel, thisFollower.m_level);
////				serializer.SaveInt(BerserkerXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Fencer:
////				serializer.SaveBool(FencerLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(FencerLevel, thisFollower.m_level);
////				serializer.SaveInt(FencerXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Succubus:
////				serializer.SaveBool(SuccubusLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(SuccubusLevel, thisFollower.m_level);
////				serializer.SaveInt(SuccubusXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Dancer:
////				serializer.SaveBool(DancerLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(DancerLevel, thisFollower.m_level);
////				serializer.SaveInt(DancerXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Wrestler:
////				serializer.SaveBool(WrestlerLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(WrestlerLevel, thisFollower.m_level);
////				serializer.SaveInt(WrestlerXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Psychic:
////				serializer.SaveBool(PsychicLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(PsychicLevel, thisFollower.m_level);
////				serializer.SaveInt(PsychicXP, thisFollower.m_XP);
////				break;
////			case Follower.FollowerType.Monk:
////				serializer.SaveBool(MonkLockState, thisFollower.m_isLocked);
////				serializer.SaveInt(MonkLevel, thisFollower.m_level);
////				serializer.SaveInt(MonkXP, thisFollower.m_XP);
////				break;
////			}
////		}
////		
////		
////		
////		
////		
////		
////		
//	
//		if (GameManager.m_gameManager.storageItems.Count > 0)
//		{
//			Item thisItem = GameManager.m_gameManager.storageItems[0];
//			serializer.SaveString(Storage01, thisItem.m_storageName);
//			Debug.Log("saving item " + thisItem.m_storageName);
//		} else {
//			serializer.SaveString(Storage01, "None");	
//		}
//		
//		if (GameManager.m_gameManager.storageItems.Count > 1)
//		{
//			Item thisItem = GameManager.m_gameManager.storageItems[1];
//			serializer.SaveString(Storage02, thisItem.m_storageName);
//			//Debug.Log("saving item " + thisItem.m_storageName);
//		} else {
//			serializer.SaveString(Storage02, "None");	
//		}
//		
//		if (GameManager.m_gameManager.storageItems.Count > 2)
//		{
//			Item thisItem = GameManager.m_gameManager.storageItems[2];
//			serializer.SaveString(Storage03, thisItem.m_storageName);
//			//Debug.Log("saving item " + thisItem.m_storageName);
//		} else {
//			serializer.SaveString(Storage03, "None");	
//		}
//
//		//save badge state
//		if (SettingsManager.m_settingsManager != null)
//		{
//			if (SettingsManager.m_settingsManager.badgeStates.Count > 0)
//			{
//				List<int> bs = SettingsManager.m_settingsManager.badgeStates;
//				
//				serializer.SaveInt (Badge_DungeonHeart, bs[0]);
//				serializer.SaveInt (Badge_BadgeBoost_01, bs[1]);
//				serializer.SaveInt (Badge_BadgeBoost_02, bs[2]);
//				serializer.SaveInt (Badge_BadgeBoost_03, bs[3]);
//				serializer.SaveInt (Badge_BadgeBoost_04, bs[4]);
//				serializer.SaveInt (Badge_BadgeBoost_05, bs[5]);
//				serializer.SaveInt (Badge_BPBoost_01, bs[6]);
//				serializer.SaveInt (Badge_EnergyRecover_01, bs[7]);
//				serializer.SaveInt (Badge_EnergyRecover_02, bs[8]);
//				serializer.SaveInt (Badge_HealthRecover_01, bs[9]);
//				serializer.SaveInt (Badge_ItemBoost_01, bs[10]);
//				serializer.SaveInt (Badge_ItemBoost_02, bs[11]);
//				serializer.SaveInt (Badge_PartyBoost_01, bs[12]);
//				serializer.SaveInt (Badge_Shop, bs[13]);
//				serializer.SaveInt (Badge_ShopPortal, bs[14]);
//				serializer.SaveInt (Badge_Storage, bs[15]);
//				serializer.SaveInt (Badge_StorageBoost_01, bs[16]);
//				serializer.SaveInt (Badge_StorageBoost_02, bs[17]);
//				
//			}
//
//			if (SettingsManager.m_settingsManager.trialStates.Count > 0)
//			{
//				List<int> ts = SettingsManager.m_settingsManager.badgeStates;
//				
//				serializer.SaveInt (Trial01State, ts[0]);
//				serializer.SaveInt (Trial02State, ts[1]);
//				serializer.SaveInt (Trial03State, ts[2]);
//				serializer.SaveInt (Trial04State, ts[3]);
//				serializer.SaveInt (Trial05State, ts[4]);
//				serializer.SaveInt (Trial06State, ts[5]);
//				serializer.SaveInt (Trial07State, ts[6]);
//				serializer.SaveInt (Trial08State, ts[7]);
//				serializer.SaveInt (Trial09State, ts[8]);
//				serializer.SaveInt (Trial10State, ts[9]);
//			}
//		}
//		
//		serializer.SerializeDataToFile(SaveFileName);
//		serializer.ClearSerializedData();
	}
	
	public List<ProgressState> loadState()
	{
		Debug.Log ("LOADING GAME STATE");
		GameSerializer serializer = GameSerializer.Instance;
		//serializer.CompressData = false;
		serializer.ClearSerializedData();
		serializer.DeserializeDataFromFile(SaveFileName);


		SettingsManager.m_settingsManager.gold = serializer.LoadInt(Gold);
		SettingsManager.m_settingsManager.xp = serializer.LoadInt (XP);
		//SettingsManager.m_settingsManager.gold = 1000;
		
		List<ProgressState> progress = new List<ProgressState>();
		
		ProgressState brandProgress = new ProgressState();
		brandProgress.m_followerType = Follower.FollowerType.Brand;
		brandProgress.m_isLocked = serializer.LoadBool(BrandLockState);
		brandProgress.m_level = serializer.LoadInt(BrandLevel);
		brandProgress.m_XP = serializer.LoadInt(BrandXP);
		brandProgress.m_XPBonus = serializer.LoadFloat(BrandXPBonus);
		brandProgress.m_badgeLevel1 = serializer.LoadInt (Brand_Badge01);
		brandProgress.m_badgeLevel2 = serializer.LoadInt (Brand_Badge02);
		brandProgress.m_badgeLevel3 = serializer.LoadInt (Brand_Badge03);
		brandProgress.m_badgeLevel4 = serializer.LoadInt (Brand_Badge04);
		brandProgress.m_badgeLevel5 = serializer.LoadInt (Brand_Badge05);
		brandProgress.m_badgeLevel6 = serializer.LoadInt (Brand_Badge06);
		progress.Add(brandProgress);

		ProgressState augustProgress = new ProgressState();
		augustProgress.m_followerType = Follower.FollowerType.August;
		augustProgress.m_isLocked = serializer.LoadBool(AugustLockState);
		//Debug.Log("August Lock: + " + augustProgress.m_isLocked);
		augustProgress.m_level = serializer.LoadInt(AugustLevel);
		augustProgress.m_XP = serializer.LoadInt(AugustXP);
		augustProgress.m_XPBonus = serializer.LoadFloat(AugustXPBonus);
		augustProgress.m_badgeLevel1 = serializer.LoadInt (August_Badge01);
		augustProgress.m_badgeLevel2 = serializer.LoadInt (August_Badge02);
		augustProgress.m_badgeLevel3 = serializer.LoadInt (August_Badge03);
		augustProgress.m_badgeLevel4 = serializer.LoadInt (August_Badge04);
		augustProgress.m_badgeLevel5 = serializer.LoadInt (August_Badge05);
		augustProgress.m_badgeLevel6 = serializer.LoadInt (August_Badge06);
		progress.Add(augustProgress);

		ProgressState telinaProgress = new ProgressState();
		telinaProgress.m_followerType = Follower.FollowerType.Telina;
		telinaProgress.m_isLocked = serializer.LoadBool(TelinaLockState);
		//Debug.Log("Telina Lock: + " + telinaProgress.m_isLocked);
		telinaProgress.m_level = serializer.LoadInt(TelinaLevel);
		telinaProgress.m_XP = serializer.LoadInt(TelinaXP);
		telinaProgress.m_XPBonus = serializer.LoadFloat(TelinaXPBonus);
		telinaProgress.m_badgeLevel1 = serializer.LoadInt (Telina_Badge01);
		telinaProgress.m_badgeLevel2 = serializer.LoadInt (Telina_Badge02);
		telinaProgress.m_badgeLevel3 = serializer.LoadInt (Telina_Badge03);
		telinaProgress.m_badgeLevel4 = serializer.LoadInt (Telina_Badge04);
		telinaProgress.m_badgeLevel5 = serializer.LoadInt (Telina_Badge05);
		telinaProgress.m_badgeLevel6 = serializer.LoadInt (Telina_Badge06);
		progress.Add(telinaProgress);

		ProgressState jinProgress = new ProgressState();
		jinProgress.m_followerType = Follower.FollowerType.Jin;
		jinProgress.m_isLocked = serializer.LoadBool(JinLockState);
		jinProgress.m_level = serializer.LoadInt(JinLevel);
		jinProgress.m_XP = serializer.LoadInt(JinXP);
		jinProgress.m_XPBonus = serializer.LoadFloat(JinXPBonus);
		jinProgress.m_badgeLevel1 = serializer.LoadInt (Jin_Badge01);
		jinProgress.m_badgeLevel2 = serializer.LoadInt (Jin_Badge02);
		jinProgress.m_badgeLevel3 = serializer.LoadInt (Jin_Badge03);
		jinProgress.m_badgeLevel4 = serializer.LoadInt (Jin_Badge04);
		jinProgress.m_badgeLevel5 = serializer.LoadInt (Jin_Badge05);
		jinProgress.m_badgeLevel6 = serializer.LoadInt (Jin_Badge06);
		progress.Add(jinProgress);
		
		ProgressState knightProgress = new ProgressState();
		knightProgress.m_followerType = Follower.FollowerType.Knight;
		knightProgress.m_isLocked = serializer.LoadBool(KnightLockState);
		knightProgress.m_level = serializer.LoadInt(KnightLevel);
		knightProgress.m_XP = serializer.LoadInt(KnightXP);
		knightProgress.m_XPBonus = serializer.LoadFloat(KnightXPBonus);
		knightProgress.m_badgeLevel1 = serializer.LoadInt (Knight_Badge01);
		knightProgress.m_badgeLevel2 = serializer.LoadInt (Knight_Badge02);
		knightProgress.m_badgeLevel3 = serializer.LoadInt (Knight_Badge03);
		knightProgress.m_badgeLevel4 = serializer.LoadInt (Knight_Badge04);
		knightProgress.m_badgeLevel5 = serializer.LoadInt (Knight_Badge05);
		knightProgress.m_badgeLevel6 = serializer.LoadInt (Knight_Badge06);
		progress.Add(knightProgress);

		ProgressState dragoonProgress = new ProgressState();
		dragoonProgress.m_followerType = Follower.FollowerType.Dragoon;
		dragoonProgress.m_isLocked = serializer.LoadBool(DragoonLockState);
		dragoonProgress.m_level = serializer.LoadInt(DragoonLevel);
		dragoonProgress.m_XP = serializer.LoadInt(DragoonXP);
		dragoonProgress.m_XPBonus = serializer.LoadFloat(DragoonXPBonus);
		dragoonProgress.m_badgeLevel1 = serializer.LoadInt (Dragoon_Badge01);
		dragoonProgress.m_badgeLevel2 = serializer.LoadInt (Dragoon_Badge02);
		dragoonProgress.m_badgeLevel3 = serializer.LoadInt (Dragoon_Badge03);
		dragoonProgress.m_badgeLevel4 = serializer.LoadInt (Dragoon_Badge04);
		dragoonProgress.m_badgeLevel5 = serializer.LoadInt (Dragoon_Badge05);
		dragoonProgress.m_badgeLevel6 = serializer.LoadInt (Dragoon_Badge06);
		progress.Add(dragoonProgress);
		
		ProgressState elfProgress = new ProgressState();
		elfProgress.m_followerType = Follower.FollowerType.Elf;
		elfProgress.m_isLocked = serializer.LoadBool(ElfLockState);
		elfProgress.m_level = serializer.LoadInt(ElfLevel);
		elfProgress.m_XP = serializer.LoadInt(ElfXP);
		elfProgress.m_XPBonus = serializer.LoadFloat(ElfXPBonus);
		elfProgress.m_badgeLevel1 = serializer.LoadInt (Elf_Badge01);
		elfProgress.m_badgeLevel2 = serializer.LoadInt (Elf_Badge02);
		elfProgress.m_badgeLevel3 = serializer.LoadInt (Elf_Badge03);
		elfProgress.m_badgeLevel4 = serializer.LoadInt (Elf_Badge04);
		elfProgress.m_badgeLevel5 = serializer.LoadInt (Elf_Badge05);
		elfProgress.m_badgeLevel6 = serializer.LoadInt (Elf_Badge06);
		progress.Add(elfProgress);
		
		ProgressState mysticProgress = new ProgressState();
		mysticProgress.m_followerType = Follower.FollowerType.Mystic;
		mysticProgress.m_isLocked = serializer.LoadBool(MysticLockState);
		mysticProgress.m_level = serializer.LoadInt(MysticLevel);
		mysticProgress.m_XP = serializer.LoadInt(MysticXP);
		mysticProgress.m_XPBonus = serializer.LoadFloat(MysticXPBonus);
		mysticProgress.m_badgeLevel1 = serializer.LoadInt (Mystic_Badge01);
		mysticProgress.m_badgeLevel2 = serializer.LoadInt (Mystic_Badge02);
		mysticProgress.m_badgeLevel3 = serializer.LoadInt (Mystic_Badge03);
		mysticProgress.m_badgeLevel4 = serializer.LoadInt (Mystic_Badge04);
		mysticProgress.m_badgeLevel5 = serializer.LoadInt (Mystic_Badge05);
		mysticProgress.m_badgeLevel6 = serializer.LoadInt (Mystic_Badge06);
		progress.Add(mysticProgress);
		
		ProgressState pyromageProgress = new ProgressState();
		pyromageProgress.m_followerType = Follower.FollowerType.Pyromage;
		pyromageProgress.m_isLocked = serializer.LoadBool(PyromageLockState);
		pyromageProgress.m_level = serializer.LoadInt(PyromageLevel);
		pyromageProgress.m_XP = serializer.LoadInt(PyromageXP);
		pyromageProgress.m_XPBonus = serializer.LoadFloat(PyromageXPBonus);
		pyromageProgress.m_badgeLevel1 = serializer.LoadInt (Pyromage_Badge01);
		pyromageProgress.m_badgeLevel2 = serializer.LoadInt (Pyromage_Badge02);
		pyromageProgress.m_badgeLevel3 = serializer.LoadInt (Pyromage_Badge03);
		pyromageProgress.m_badgeLevel4 = serializer.LoadInt (Pyromage_Badge04);
		pyromageProgress.m_badgeLevel5 = serializer.LoadInt (Pyromage_Badge05);
		pyromageProgress.m_badgeLevel6 = serializer.LoadInt (Pyromage_Badge06);
		progress.Add(pyromageProgress);
		
		ProgressState rangerProgress = new ProgressState();
		rangerProgress.m_followerType = Follower.FollowerType.Ranger;
		rangerProgress.m_isLocked = serializer.LoadBool(RangerLockState);
		rangerProgress.m_level = serializer.LoadInt(RangerLevel);
		rangerProgress.m_XP = serializer.LoadInt(RangerXP);
		rangerProgress.m_XPBonus = serializer.LoadFloat(RangerXPBonus);
		rangerProgress.m_badgeLevel1 = serializer.LoadInt (Ranger_Badge01);
		rangerProgress.m_badgeLevel2 = serializer.LoadInt (Ranger_Badge02);
		rangerProgress.m_badgeLevel3 = serializer.LoadInt (Ranger_Badge03);
		rangerProgress.m_badgeLevel4 = serializer.LoadInt (Ranger_Badge04);
		rangerProgress.m_badgeLevel5 = serializer.LoadInt (Ranger_Badge05);
		rangerProgress.m_badgeLevel6 = serializer.LoadInt (Ranger_Badge06);
		progress.Add(rangerProgress);
		
		ProgressState samuraiProgress = new ProgressState();
		samuraiProgress.m_followerType = Follower.FollowerType.Samurai;
		samuraiProgress.m_isLocked = serializer.LoadBool(SamuraiLockState);
		samuraiProgress.m_level = serializer.LoadInt(SamuraiLevel);
		samuraiProgress.m_XP = serializer.LoadInt(SamuraiXP);
		samuraiProgress.m_XPBonus = serializer.LoadFloat(SamuraiXPBonus);
		samuraiProgress.m_badgeLevel1 = serializer.LoadInt (Samurai_Badge01);
		samuraiProgress.m_badgeLevel2 = serializer.LoadInt (Samurai_Badge02);
		samuraiProgress.m_badgeLevel3 = serializer.LoadInt (Samurai_Badge03);
		samuraiProgress.m_badgeLevel4 = serializer.LoadInt (Samurai_Badge04);
		samuraiProgress.m_badgeLevel5 = serializer.LoadInt (Samurai_Badge05);
		samuraiProgress.m_badgeLevel6 = serializer.LoadInt (Samurai_Badge06);
		progress.Add(samuraiProgress);

		ProgressState seerProgress = new ProgressState();
		seerProgress.m_followerType = Follower.FollowerType.Seer;
		seerProgress.m_isLocked = serializer.LoadBool(SeerLockState);
		seerProgress.m_level = serializer.LoadInt(SeerLevel);
		seerProgress.m_badgeLevel1 = serializer.LoadInt (Seer_Badge01);
		seerProgress.m_badgeLevel2 = serializer.LoadInt (Seer_Badge02);
		seerProgress.m_badgeLevel3 = serializer.LoadInt (Seer_Badge03);
		seerProgress.m_badgeLevel4 = serializer.LoadInt (Seer_Badge04);
		seerProgress.m_badgeLevel5 = serializer.LoadInt (Seer_Badge05);
		seerProgress.m_badgeLevel6 = serializer.LoadInt (Seer_Badge06);
		seerProgress.m_XP = serializer.LoadInt(SeerXP);
		seerProgress.m_XPBonus = serializer.LoadFloat(SeerXPBonus);
		progress.Add(seerProgress);

		ProgressState barbarianProgress = new ProgressState();
		barbarianProgress.m_followerType = Follower.FollowerType.Barbarian;
		barbarianProgress.m_isLocked = serializer.LoadBool(BarbarianLockState);
		barbarianProgress.m_level = serializer.LoadInt(BarbarianLevel);
		barbarianProgress.m_XP = serializer.LoadInt(BarbarianXP);
		barbarianProgress.m_XPBonus = serializer.LoadFloat(BarbarianXPBonus);
		barbarianProgress.m_badgeLevel1 = serializer.LoadInt (Barbarian_Badge01);
		barbarianProgress.m_badgeLevel2 = serializer.LoadInt (Barbarian_Badge02);
		barbarianProgress.m_badgeLevel3 = serializer.LoadInt (Barbarian_Badge03);
		barbarianProgress.m_badgeLevel4 = serializer.LoadInt (Barbarian_Badge04);
		barbarianProgress.m_badgeLevel5 = serializer.LoadInt (Barbarian_Badge05);
		barbarianProgress.m_badgeLevel6 = serializer.LoadInt (Barbarian_Badge06);
		progress.Add(barbarianProgress);

		ProgressState berserkerProgress = new ProgressState();
		berserkerProgress.m_followerType = Follower.FollowerType.Berserker;
		berserkerProgress.m_isLocked = serializer.LoadBool(BerserkerLockState);
		berserkerProgress.m_level = serializer.LoadInt(BerserkerLevel);
		berserkerProgress.m_XP = serializer.LoadInt(BerserkerXP);
		berserkerProgress.m_XPBonus = serializer.LoadFloat(BerserkerXPBonus);
		berserkerProgress.m_badgeLevel1 = serializer.LoadInt (Berserker_Badge01);
		berserkerProgress.m_badgeLevel2 = serializer.LoadInt (Berserker_Badge02);
		berserkerProgress.m_badgeLevel3 = serializer.LoadInt (Berserker_Badge03);
		berserkerProgress.m_badgeLevel4 = serializer.LoadInt (Berserker_Badge04);
		berserkerProgress.m_badgeLevel5 = serializer.LoadInt (Berserker_Badge05);
		berserkerProgress.m_badgeLevel6 = serializer.LoadInt (Berserker_Badge06);
		progress.Add(berserkerProgress);
		
		ProgressState fencerProgress = new ProgressState();
		fencerProgress.m_followerType = Follower.FollowerType.Fencer;
		fencerProgress.m_isLocked = serializer.LoadBool(FencerLockState);
		fencerProgress.m_level = serializer.LoadInt(FencerLevel);
		fencerProgress.m_badgeLevel1 = serializer.LoadInt (Fencer_Badge01);
		fencerProgress.m_badgeLevel2 = serializer.LoadInt (Fencer_Badge02);
		fencerProgress.m_badgeLevel3 = serializer.LoadInt (Fencer_Badge03);
		fencerProgress.m_badgeLevel4 = serializer.LoadInt (Fencer_Badge04);
		fencerProgress.m_badgeLevel5 = serializer.LoadInt (Fencer_Badge05);
		fencerProgress.m_badgeLevel6 = serializer.LoadInt (Fencer_Badge06);
		fencerProgress.m_XP = serializer.LoadInt(FencerXP);
		fencerProgress.m_XPBonus = serializer.LoadFloat(FencerXPBonus);
		progress.Add(fencerProgress);
		
		ProgressState succubusProgress = new ProgressState();
		succubusProgress.m_followerType = Follower.FollowerType.Succubus;
		succubusProgress.m_isLocked = serializer.LoadBool(SuccubusLockState);
		succubusProgress.m_level = serializer.LoadInt(SuccubusLevel);
		succubusProgress.m_XP = serializer.LoadInt(SuccubusXP);
		succubusProgress.m_XPBonus = serializer.LoadFloat(SuccubusXPBonus);
		succubusProgress.m_badgeLevel1 = serializer.LoadInt (Succubus_Badge01);
		succubusProgress.m_badgeLevel2 = serializer.LoadInt (Succubus_Badge02);
		succubusProgress.m_badgeLevel3 = serializer.LoadInt (Succubus_Badge03);
		succubusProgress.m_badgeLevel4 = serializer.LoadInt (Succubus_Badge04);
		succubusProgress.m_badgeLevel5 = serializer.LoadInt (Succubus_Badge05);
		succubusProgress.m_badgeLevel6 = serializer.LoadInt (Succubus_Badge06);
		progress.Add(succubusProgress);
		
		ProgressState dancerProgress = new ProgressState();
		dancerProgress.m_followerType = Follower.FollowerType.Dancer;
		dancerProgress.m_isLocked = serializer.LoadBool(DancerLockState);
		dancerProgress.m_level = serializer.LoadInt(DancerLevel);
		dancerProgress.m_XP = serializer.LoadInt(DancerXP);
		dancerProgress.m_XPBonus = serializer.LoadFloat(DancerXPBonus);
		dancerProgress.m_badgeLevel1 = serializer.LoadInt (Dancer_Badge01);
		dancerProgress.m_badgeLevel2 = serializer.LoadInt (Dancer_Badge02);
		dancerProgress.m_badgeLevel3 = serializer.LoadInt (Dancer_Badge03);
		dancerProgress.m_badgeLevel4 = serializer.LoadInt (Dancer_Badge04);
		dancerProgress.m_badgeLevel5 = serializer.LoadInt (Dancer_Badge05);
		dancerProgress.m_badgeLevel6 = serializer.LoadInt (Dancer_Badge06);
		progress.Add(dancerProgress);
		
		ProgressState wrestlerProgress = new ProgressState();
		wrestlerProgress.m_followerType = Follower.FollowerType.Wrestler;
		wrestlerProgress.m_isLocked = serializer.LoadBool(WrestlerLockState);
		wrestlerProgress.m_level = serializer.LoadInt(WrestlerLevel);
		wrestlerProgress.m_XP = serializer.LoadInt(WrestlerXP);
		wrestlerProgress.m_XPBonus = serializer.LoadFloat(WrestlerXPBonus);
		wrestlerProgress.m_badgeLevel1 = serializer.LoadInt (Wrestler_Badge01);
		wrestlerProgress.m_badgeLevel2 = serializer.LoadInt (Wrestler_Badge02);
		wrestlerProgress.m_badgeLevel3 = serializer.LoadInt (Wrestler_Badge03);
		wrestlerProgress.m_badgeLevel4 = serializer.LoadInt (Wrestler_Badge04);
		wrestlerProgress.m_badgeLevel5 = serializer.LoadInt (Wrestler_Badge05);
		wrestlerProgress.m_badgeLevel6 = serializer.LoadInt (Wrestler_Badge06);
		progress.Add(wrestlerProgress);
		
		ProgressState psychicProgress = new ProgressState();
		psychicProgress.m_followerType = Follower.FollowerType.Psychic;
		psychicProgress.m_isLocked = serializer.LoadBool(PsychicLockState);
		psychicProgress.m_level = serializer.LoadInt(PsychicLevel);
		psychicProgress.m_XP = serializer.LoadInt(PsychicXP);
		psychicProgress.m_XPBonus = serializer.LoadFloat(PsychicXPBonus);
		psychicProgress.m_badgeLevel1 = serializer.LoadInt (Psychic_Badge01);
		psychicProgress.m_badgeLevel2 = serializer.LoadInt (Psychic_Badge02);
		psychicProgress.m_badgeLevel3 = serializer.LoadInt (Psychic_Badge03);
		psychicProgress.m_badgeLevel4 = serializer.LoadInt (Psychic_Badge04);
		psychicProgress.m_badgeLevel5 = serializer.LoadInt (Psychic_Badge05);
		psychicProgress.m_badgeLevel6 = serializer.LoadInt (Psychic_Badge06);
		progress.Add(psychicProgress);
		
		ProgressState monkProgress = new ProgressState();
		monkProgress.m_followerType = Follower.FollowerType.Monk;
		monkProgress.m_isLocked = serializer.LoadBool(MonkLockState);
		monkProgress.m_level = serializer.LoadInt(MonkLevel);
		monkProgress.m_XP = serializer.LoadInt(MonkXP);
		monkProgress.m_XPBonus = serializer.LoadFloat(MonkXPBonus);
		monkProgress.m_badgeLevel1 = serializer.LoadInt (Monk_Badge01);
		monkProgress.m_badgeLevel2 = serializer.LoadInt (Monk_Badge02);
		monkProgress.m_badgeLevel3 = serializer.LoadInt (Monk_Badge03);
		monkProgress.m_badgeLevel4 = serializer.LoadInt (Monk_Badge04);
		monkProgress.m_badgeLevel5 = serializer.LoadInt (Monk_Badge05);
		monkProgress.m_badgeLevel6 = serializer.LoadInt (Monk_Badge06);
		progress.Add(monkProgress);
		
		if (GameManager.m_gameManager != null)
		{
			for (int i=0; i<3; i++)
			{
				
				string itemName = serializer.LoadString(Storage01);	
				if (i == 1)
				{
					 itemName = serializer.LoadString(Storage02);	
				}else if (i == 2)
				{
					 itemName = serializer.LoadString(Storage03);	
				}
//				Debug.Log("creating " + itemName);
				GameManager.m_gameManager.LoadStorageItem(itemName);
			}
		}

		//load badge states
		List<int> badgeStates = new List<int> ();
		badgeStates.Add(serializer.LoadInt (Badge_DungeonHeart)); // 0: Not Owned, 1: Owned, Not Equipped, 2: Equipped

		badgeStates.Add(serializer.LoadInt (Badge_DamageBoost_01));
		badgeStates.Add(serializer.LoadInt (Badge_DamageBoost_02));
		badgeStates.Add(serializer.LoadInt (Badge_DamageBoost_03));
		badgeStates.Add(serializer.LoadInt (Badge_DamageBoost_04));

		badgeStates.Add(serializer.LoadInt (Badge_HealthBoost_01));
		badgeStates.Add(serializer.LoadInt (Badge_HealthBoost_02));
		badgeStates.Add(serializer.LoadInt (Badge_HealthBoost_03));
		badgeStates.Add(serializer.LoadInt (Badge_HealthBoost_04));

		badgeStates.Add(serializer.LoadInt (Badge_EnergyBoost_01));
		badgeStates.Add(serializer.LoadInt (Badge_EnergyBoost_02));
		badgeStates.Add(serializer.LoadInt (Badge_EnergyBoost_03));
		badgeStates.Add(serializer.LoadInt (Badge_EnergyBoost_04));

		badgeStates.Add(serializer.LoadInt (Badge_HandBoost_01));
		badgeStates.Add(serializer.LoadInt (Badge_HandBoost_02));

		badgeStates.Add(serializer.LoadInt (Badge_HealthRecover_01));

		badgeStates.Add(serializer.LoadInt (Badge_EnergyRecover_01));
		badgeStates.Add(serializer.LoadInt (Badge_EnergyRecover_02));

		badgeStates.Add(serializer.LoadInt (Badge_BadgeBoost_01));
		badgeStates.Add(serializer.LoadInt (Badge_BadgeBoost_02));
		badgeStates.Add(serializer.LoadInt (Badge_BadgeBoost_03));
		badgeStates.Add(serializer.LoadInt (Badge_BadgeBoost_04));
		badgeStates.Add(serializer.LoadInt (Badge_BadgeBoost_05));


		//load trial states
		List<int> trialStates = new List<int> ();
		trialStates.Add(serializer.LoadInt (Trial01State));
		trialStates.Add(serializer.LoadInt (Trial02State));
		trialStates.Add(serializer.LoadInt (Trial03State));
		trialStates.Add(serializer.LoadInt (Trial04State));
		trialStates.Add(serializer.LoadInt (Trial05State));
		trialStates.Add(serializer.LoadInt (Trial06State));
		trialStates.Add(serializer.LoadInt (Trial07State));
		trialStates.Add(serializer.LoadInt (Trial08State));
		trialStates.Add(serializer.LoadInt (Trial09State));
		trialStates.Add(serializer.LoadInt (Trial10State));

		//load shortcut states
		List<int> shortcutStates = new List<int> ();
		shortcutStates.Add(serializer.LoadInt (Shortcut01));
		shortcutStates.Add(serializer.LoadInt (Shortcut02));
		shortcutStates.Add(serializer.LoadInt (Shortcut03));
		shortcutStates.Add(serializer.LoadInt (Shortcut04));
		shortcutStates.Add(serializer.LoadInt (Shortcut05));

		if (SettingsManager.m_settingsManager != null)
		{
			SettingsManager.m_settingsManager.badgeStates = badgeStates;
			SettingsManager.m_settingsManager.trialStates = trialStates;
			SettingsManager.m_settingsManager.shortcutStates = shortcutStates;
		}

		
		serializer.SerializeDataToFile(SaveFileName);
		serializer.ClearSerializedData();
//			serializer.DeleteSerializedDataFromFile(SaveFileName);
		
		return progress;
	}
	
	public void clearState()
	{
		if (PlayerPrefs.HasKey(SaveFileName))
		{
			PlayerPrefs.DeleteKey(SaveFileName);
			PlayerPrefs.Save();
		}
	}
}

