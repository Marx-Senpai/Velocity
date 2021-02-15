using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private bool lowpassFading;
	private float lowpassValue;
	private float basseValue = 1;
	[HideInInspector] public bool basseOn;
	[HideInInspector] public bool windFadingInSolo;
	[HideInInspector] public bool windFadingToBasse;
	[SerializeField] private float fadingSpeed = .25f;
	
	[SerializeField] private GameObject viseur;
	[SerializeField] private GameObject arena_core;
	
	// -----------------  DECLARATION INSPECTOR -----------------

	[Header("Avatar")]
	[FMODUnity.EventRef] public string damageTaken;
	[FMODUnity.EventRef] public string footsteps;
	[FMODUnity.EventRef] public string dash;
	[FMODUnity.EventRef] public string death;
	
	[Header("Stances & Modules")]
	[FMODUnity.EventRef] public string pickModule;
	[FMODUnity.EventRef] public string noModule;
	[FMODUnity.EventRef] public string stance_switch;
	[FMODUnity.EventRef] public string shoot;
	[FMODUnity.EventRef] public string reflect;
	[FMODUnity.EventRef] public string shield_deploy;
	[FMODUnity.EventRef] public string shield_destroy;
	
	[Header("Enemies")]
	[FMODUnity.EventRef] public string spawnEnemy;
	[FMODUnity.EventRef] public string losePart;
	[FMODUnity.EventRef] public string col_part_ground;
	[FMODUnity.EventRef] public string col_part_water;
	[FMODUnity.EventRef] public string shoot_traqueur;
	[FMODUnity.EventRef] public string shoot_infanterie;
	[FMODUnity.EventRef] public string shoot_sniper;
	[FMODUnity.EventRef] public string charging_sniper;
	[FMODUnity.EventRef] public string moving_chargeur;

	[Header("General")]
	[FMODUnity.EventRef] public string wind;
	[FMODUnity.EventRef] public string storm;
	[FMODUnity.EventRef] public string lightningStrike;
	[FMODUnity.EventRef] public string doorOpening;
	[FMODUnity.EventRef] public string doorClosing;
	[FMODUnity.EventRef] public string endWave;

	// ----------------- DECLARATION EVENTS FMOD -----------------
	
	// Avatar
	public FMOD.Studio.EventInstance damageTakenFmod;
	public FMOD.Studio.EventInstance footstepsFmod;
	public FMOD.Studio.EventInstance dashFmod;
	public FMOD.Studio.EventInstance deathFmod;
	
	// Stances & Modules
	public FMOD.Studio.EventInstance pickModuleFmod;
	public FMOD.Studio.EventInstance noModuleFmod;
	public FMOD.Studio.EventInstance stance_switchFmod;
	public FMOD.Studio.EventInstance shootFmod;
	public FMOD.Studio.EventInstance reflectFmod;
	public FMOD.Studio.EventInstance shield_deployFmod;
	public FMOD.Studio.EventInstance shield_destroyFmod;
	
	// Enemies
	public FMOD.Studio.EventInstance spawnEnemyFmod;
	public FMOD.Studio.EventInstance losePartFmod;
	public FMOD.Studio.EventInstance col_part_groundFmod;
	public FMOD.Studio.EventInstance col_part_waterFmod;
	public FMOD.Studio.EventInstance shoot_TraqueurFmod;
	public FMOD.Studio.EventInstance shoot_InfanterieFmod;
	public FMOD.Studio.EventInstance shoot_SniperFmod;
	public FMOD.Studio.EventInstance charging_SniperFmod;
	public FMOD.Studio.EventInstance moving_chargeurFmod;
	
	// General
	public FMOD.Studio.EventInstance windFmod;
	public FMOD.Studio.EventInstance stormFmod;
	public FMOD.Studio.EventInstance lightningStrikeFmod;
	public FMOD.Studio.EventInstance doorOpeningFmod;
	public FMOD.Studio.EventInstance doorClosingFmod;
	public FMOD.Studio.EventInstance endWaveFmod;
	

	// ----------------- DECLARATION PARAMETERS EVENTS FMOD -----------------

	// Avatar
	public FMOD.Studio.ParameterInstance footstepsTextureParameter;
	
	// Stances & Modules
	//
	
	// Enemies
	public FMOD.Studio.ParameterInstance movingChargeur_Speed;
	
	// General
	public FMOD.Studio.ParameterInstance wind_enemyWaveParameter;
	public FMOD.Studio.ParameterInstance wind_outsideArenaParameter;
	
	
	// Lowpass
	public FMOD.Studio.ParameterInstance damageTaken_lowpassParameter;
	public FMOD.Studio.ParameterInstance footsteps_lowpassParameter;
	public FMOD.Studio.ParameterInstance dash_lowpassParameter;

	public FMOD.Studio.ParameterInstance pickModule_lowpassParameter;
	public FMOD.Studio.ParameterInstance noModule_lowpassParameter;
	public FMOD.Studio.ParameterInstance stance_switch_lowpassParameter;
	public FMOD.Studio.ParameterInstance shoot_lowpassParameter;
	public FMOD.Studio.ParameterInstance reflect_lowpassParameter;
	public FMOD.Studio.ParameterInstance shield_deploy_lowpassParameter;
	public FMOD.Studio.ParameterInstance shield_destroy_lowpassParameter;
	
	public FMOD.Studio.ParameterInstance spawnEnemy_lowpassParameter;
	public FMOD.Studio.ParameterInstance losePart_lowpassParameter;
	public FMOD.Studio.ParameterInstance col_part_ground_lowpassParameter;
	public FMOD.Studio.ParameterInstance col_part_water_lowpassParameter;
	public FMOD.Studio.ParameterInstance shootTraqueur_lowpassParameter;
	public FMOD.Studio.ParameterInstance shootInfanterie_lowpassParameter;
	public FMOD.Studio.ParameterInstance shootSniper_lowpassParameter;
	public FMOD.Studio.ParameterInstance chargingSniper_lowpassParameter;
	public FMOD.Studio.ParameterInstance movingChargeur_lowpassParameter;
	
	public FMOD.Studio.ParameterInstance wind_lowpassParameter;
	
	void Start()
	{
		// ----------------- DECLARATION SOUNDS -----------------
		
		// Avatar
		damageTakenFmod = FMODUnity.RuntimeManager.CreateInstance(damageTaken);
		footstepsFmod = FMODUnity.RuntimeManager.CreateInstance(footsteps);
		//dashFmod = FMODUnity.RuntimeManager.CreateInstance(dash);
		deathFmod = FMODUnity.RuntimeManager.CreateInstance(death);
		
		// Stances & Modules
		//pickFollowerFmod = FMODUnity.RuntimeManager.CreateInstance(pickFollower);
		noModuleFmod = FMODUnity.RuntimeManager.CreateInstance(noModule);
		stance_switchFmod = FMODUnity.RuntimeManager.CreateInstance(stance_switch);
		//shootFmod = FMODUnity.RuntimeManager.CreateInstance(shoot);
		reflectFmod = FMODUnity.RuntimeManager.CreateInstance(reflect);
		
		// Enemies
		losePartFmod = FMODUnity.RuntimeManager.CreateInstance(losePart);
		col_part_groundFmod = FMODUnity.RuntimeManager.CreateInstance(col_part_ground);
		col_part_waterFmod = FMODUnity.RuntimeManager.CreateInstance(col_part_water);
		//shoot_TraqueurFmod = FMODUnity.RuntimeManager.CreateInstance(shoot_traqueur);
		//shoot_InfanterieFmod = FMODUnity.RuntimeManager.CreateInstance(shoot_infanterie);
		//shoot_SniperFmod = FMODUnity.RuntimeManager.CreateInstance(shoot_sniper);
		//charging_SniperFmod = FMODUnity.RuntimeManager.CreateInstance(charging_sniper);
		
		// General
		windFmod = FMODUnity.RuntimeManager.CreateInstance(wind);
		stormFmod = FMODUnity.RuntimeManager.CreateInstance(storm);
		lightningStrikeFmod = FMODUnity.RuntimeManager.CreateInstance(lightningStrike);
		endWaveFmod = FMODUnity.RuntimeManager.CreateInstance(endWave);

		
		// ----------------- DECLARATION SOUNDS PARAMETERS -----------------
		footstepsFmod.getParameter("Pas", out footstepsTextureParameter);
		windFmod.getParameter("FermeturePorte1", out wind_enemyWaveParameter);
		windFmod.getParameter("sortie_arène", out wind_outsideArenaParameter);
		
		
		// Lowpass
		damageTakenFmod.getParameter("PertePV", out damageTaken_lowpassParameter);
		footstepsFmod.getParameter("PertePV", out footsteps_lowpassParameter);
		//dashFmod.getParameter("PertePV", out dash_lowpassParameter);
		
		noModuleFmod.getParameter("PertePV", out noModule_lowpassParameter);
		stance_switchFmod.getParameter("PertePV", out stance_switch_lowpassParameter);
		//shootFmod.getParameter("PertePV", out shoot_lowpassParameter);
		reflectFmod.getParameter("PertePV", out reflect_lowpassParameter);
		
		losePartFmod.getParameter("PertePV", out losePart_lowpassParameter);
		col_part_groundFmod.getParameter("PertePV", out col_part_ground_lowpassParameter);
		col_part_waterFmod.getParameter("PertePV", out col_part_water_lowpassParameter);
		//shoot_TraqueurFmod.getParameter("PertePV", out shootTraqueur_lowpassParameter);
		//shoot_InfanterieFmod.getParameter("PertePV", out shootInfanterie_lowpassParameter);
		//shoot_SniperFmod.getParameter("PertePV", out shootSniper_lowpassParameter);
		//charging_SniperFmod.getParameter("PertePV", out chargingSniper_lowpassParameter);
		
		windFmod.getParameter("PertePV", out wind_lowpassParameter);
		
		
		// ----------------- START SOUNDS -----------------
		wind_outsideArenaParameter.setValue(1);
		PlayWindSound();
		PlayStormSound(arena_core);
		StopSound(footstepsFmod);
	}

	
	void Update()
	{
		//StartCoroutine(Wind_Volume_FadeIn());

		if (lowpassFading)
		{
			lowpassValue -= fadingSpeed * Time.deltaTime;
			lowpassValue = Mathf.Clamp(lowpassValue, 0, 1);
			SetAllLowpass(lowpassValue);
		}

		if (lowpassValue <= 0)
		{
			lowpassFading = false;
		}

		if (windFadingInSolo)
		{
			wind_enemyWaveParameter.setValue(0);
			basseValue += fadingSpeed * Time.deltaTime;
			basseValue = Mathf.Clamp(basseValue, 0, 1);
			wind_outsideArenaParameter.setValue(basseValue);
		}

		if (basseValue >= 1)
		{
			windFadingInSolo = false;
			basseOn = false;
		}
		
		if (windFadingToBasse)
		{
			basseValue -= fadingSpeed * Time.deltaTime;
			basseValue = Mathf.Clamp(basseValue, 0, 1);
			wind_outsideArenaParameter.setValue(basseValue);
		}
		
		if (basseValue <= 0)
		{
			windFadingToBasse = false;
			basseOn = true;
		}

		// Pour les sons 3D :
		//sthFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GameObject.Find("myGameObject"));
	}

	// --------------------------------- AVATAR ---------------------------------
	public void PlayFootstepsSound()
	{
		footstepsFmod.start();
	}
	
	public void PlayDashSound()
	{
		dashFmod = FMODUnity.RuntimeManager.CreateInstance(dash);
		dashFmod.getParameter("PertePV", out dash_lowpassParameter);
		dashFmod.start();
	}
	
	public void PlayDeathSound()
	{
		deathFmod.start();
	}
	
	public void PlayDamageTakenSound()
	{
		damageTakenFmod.start();
		lowpassValue = 1;
		SetAllLowpass(lowpassValue);
		StartCoroutine(LowpassFadeOut());
	}

	private IEnumerator LowpassFadeOut()
	{
		yield return new WaitForSecondsRealtime(1);
		lowpassFading = true;
	}
	
	// --------------------------------- STANCES & MODULES ---------------------------------
	public void PlayPickModuleSound()
	{
		pickModuleFmod = FMODUnity.RuntimeManager.CreateInstance(pickModule);
		pickModuleFmod.getParameter("PertePV", out pickModule_lowpassParameter);
		pickModuleFmod.start();
	}
	
	public void PlayNoModuleSound()
	{
		noModuleFmod.start();
	}
	
	public void PlayStanceSwitchSound()
	{
		stance_switchFmod.start();
	}
	
	public void PlayShootSound()
	{
		shootFmod = FMODUnity.RuntimeManager.CreateInstance(shoot);
		shootFmod.getParameter("PertePV", out shoot_lowpassParameter);
		shootFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(viseur));
		shootFmod.start();
	}
	
	public void PlayReflectSound(GameObject obj)
	{
		reflectFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		reflectFmod.start();
	}
	
	public void PlayShieldDeploySound(GameObject obj)
	{
		shield_deployFmod = FMODUnity.RuntimeManager.CreateInstance(shield_deploy);
		shield_deployFmod.getParameter("PertePV", out shield_deploy_lowpassParameter);
		shield_deployFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		shield_deployFmod.start();
	}
	
	public void PlayShieldDestroySound(GameObject obj)
	{
		shield_destroyFmod = FMODUnity.RuntimeManager.CreateInstance(shield_destroy);
		shield_destroyFmod.getParameter("PertePV", out shield_destroy_lowpassParameter);
		shield_destroyFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		shield_destroyFmod.start();
	}
	
	
	// --------------------------------- ENEMIES ---------------------------------
	public void PlaySpawnEnemySound(GameObject obj)
	{
		spawnEnemyFmod = FMODUnity.RuntimeManager.CreateInstance(spawnEnemy);
		spawnEnemyFmod.getParameter("PertePV", out spawnEnemy_lowpassParameter);
		spawnEnemyFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		spawnEnemyFmod.start();
	}
	
	public void PlayLosePartSound(GameObject obj)
	{
		losePartFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		losePartFmod.start();
	}
	
	public void PlayCollisionGroundSound(GameObject obj)
	{
		col_part_groundFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		col_part_groundFmod.start();
	}
	
	public void PlayCollisionWaterSound(GameObject obj)
	{
		col_part_waterFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		col_part_waterFmod.start();
	}
	
	public EventInstance PlayChargingSniperSound(GameObject obj)
	{
		charging_SniperFmod = FMODUnity.RuntimeManager.CreateInstance(charging_sniper);
		charging_SniperFmod.getParameter("PertePV", out chargingSniper_lowpassParameter);
		charging_SniperFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		charging_SniperFmod.start();
		return charging_SniperFmod;
	}
	
	// --- For UI only
	public void PlayChargingSniperSoundSolo(GameObject obj)
	{
		charging_SniperFmod = FMODUnity.RuntimeManager.CreateInstance(charging_sniper);
		charging_SniperFmod.getParameter("PertePV", out chargingSniper_lowpassParameter);
		charging_SniperFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		charging_SniperFmod.start();
	}
	// ---
	
	public void PlayShootSniperSound(GameObject obj)
	{
		shoot_SniperFmod = FMODUnity.RuntimeManager.CreateInstance(shoot_sniper);
		shoot_SniperFmod.getParameter("PertePV", out shootSniper_lowpassParameter);
		shoot_SniperFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		shoot_SniperFmod.start();
	}
	
	public void PlayShootTraqueurSound(GameObject obj)
	{
		shoot_TraqueurFmod = FMODUnity.RuntimeManager.CreateInstance(shoot_traqueur);
		shoot_TraqueurFmod.getParameter("PertePV", out shootTraqueur_lowpassParameter);
		shoot_TraqueurFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		shoot_TraqueurFmod.start();
	}
	
	public void PlayShootInfanterieSound(GameObject obj)
	{
		shoot_InfanterieFmod = FMODUnity.RuntimeManager.CreateInstance(shoot_infanterie);
		shoot_InfanterieFmod.getParameter("PertePV", out shootInfanterie_lowpassParameter);
		shoot_InfanterieFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		shoot_InfanterieFmod.start();
	}
	
	public EventInstance PlayMovingChargeurSound(GameObject obj)
	{
		moving_chargeurFmod = FMODUnity.RuntimeManager.CreateInstance(moving_chargeur);
		moving_chargeurFmod.getParameter("PertePV", out movingChargeur_lowpassParameter);
		moving_chargeurFmod.getParameter("Vitesse deplacement fonceur", out movingChargeur_Speed);
		movingChargeur_Speed.setValue(0.5f);
		moving_chargeurFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		moving_chargeurFmod.start();
		return moving_chargeurFmod;
	}
	
	
	// --------------------------------- GENERAL ---------------------------------
	public void PlayWindSound()
	{
		windFmod.start();
	}
	
	public void PlayStormSound(GameObject obj)
	{
		stormFmod = FMODUnity.RuntimeManager.CreateInstance(storm);
		stormFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		stormFmod.start();
	}
	
	public void PlayDoorOpeningSound(GameObject obj)
	{
		doorOpeningFmod = FMODUnity.RuntimeManager.CreateInstance(doorOpening);
		doorOpeningFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		doorOpeningFmod.start();
	}
	
	public void PlayDoorClosingSound(GameObject obj)
	{
		doorClosingFmod = FMODUnity.RuntimeManager.CreateInstance(doorClosing);
		doorClosingFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		doorClosingFmod.start();
	}
	
	public void PlayLightningStrikeSound(GameObject obj)
	{
		lightningStrikeFmod = FMODUnity.RuntimeManager.CreateInstance(lightningStrike);
		lightningStrikeFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		lightningStrikeFmod.start();
	}

	public void PlayEndWaveSound(GameObject obj)
	{
		endWaveFmod.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
		endWaveFmod.start();
	}

	public void SetEnemyKickParameter(float value)
	{
		wind_enemyWaveParameter.setValue(value);
	}

	public void StopAllSounds(bool withWind)
	{
		damageTakenFmod.stop(STOP_MODE.ALLOWFADEOUT);
		footstepsFmod.stop(STOP_MODE.ALLOWFADEOUT);
		dashFmod.stop(STOP_MODE.ALLOWFADEOUT);

		pickModuleFmod.stop(STOP_MODE.ALLOWFADEOUT);
		noModuleFmod.stop(STOP_MODE.ALLOWFADEOUT);
		stance_switchFmod.stop(STOP_MODE.ALLOWFADEOUT);
		shootFmod.stop(STOP_MODE.ALLOWFADEOUT);
		reflectFmod.stop(STOP_MODE.ALLOWFADEOUT);
		shield_deployFmod.stop(STOP_MODE.ALLOWFADEOUT);

		spawnEnemyFmod.stop(STOP_MODE.ALLOWFADEOUT);
		losePartFmod.stop(STOP_MODE.ALLOWFADEOUT);
		col_part_groundFmod.stop(STOP_MODE.ALLOWFADEOUT);
		col_part_waterFmod.stop(STOP_MODE.ALLOWFADEOUT);
		shoot_TraqueurFmod.stop(STOP_MODE.ALLOWFADEOUT);
		shoot_InfanterieFmod.stop(STOP_MODE.ALLOWFADEOUT);
		shoot_SniperFmod.stop(STOP_MODE.ALLOWFADEOUT);
		charging_SniperFmod.stop(STOP_MODE.ALLOWFADEOUT);
		moving_chargeurFmod.stop(STOP_MODE.ALLOWFADEOUT);

		if (withWind)
		{
			windFmod.stop(STOP_MODE.ALLOWFADEOUT);
		}

		stormFmod.stop(STOP_MODE.ALLOWFADEOUT);
		lightningStrikeFmod.stop(STOP_MODE.ALLOWFADEOUT);
		doorOpeningFmod.stop(STOP_MODE.ALLOWFADEOUT);
		doorClosingFmod.stop(STOP_MODE.ALLOWFADEOUT);
		endWaveFmod.stop(STOP_MODE.ALLOWFADEOUT);
	}

	public void StopSound(FMOD.Studio.EventInstance sound)
	{
		sound.stop(STOP_MODE.ALLOWFADEOUT);
	}
	
	void SetAllLowpass(float value)
	{
		damageTaken_lowpassParameter.setValue(value);
		footsteps_lowpassParameter.setValue(value);
		dash_lowpassParameter.setValue(value);

		pickModule_lowpassParameter.setValue(value);
		noModule_lowpassParameter.setValue(value);
		stance_switch_lowpassParameter.setValue(value);
		shoot_lowpassParameter.setValue(value);
		reflect_lowpassParameter.setValue(value);
		shield_deploy_lowpassParameter.setValue(value);

		spawnEnemy_lowpassParameter.setValue(value);
		losePart_lowpassParameter.setValue(value);
		col_part_ground_lowpassParameter.setValue(value);
		col_part_water_lowpassParameter.setValue(value);
		shootTraqueur_lowpassParameter.setValue(value);
		shootInfanterie_lowpassParameter.setValue(value);
		shootSniper_lowpassParameter.setValue(value);
		chargingSniper_lowpassParameter.setValue(value);
		movingChargeur_lowpassParameter.setValue(value);
		
		wind_lowpassParameter.setValue(value);
	}

	public void SetVolumeWind(float volume)
	{
		windFmod.setVolume(volume);
		stormFmod.setVolume(volume);
		lightningStrikeFmod.setVolume(volume);
		doorOpeningFmod.setVolume(volume);
		doorClosingFmod.setVolume(volume);
		endWaveFmod.setVolume(volume);
	}
	
	public void SetVolumeSFX(float volume)
	{
		footstepsFmod.setVolume(volume);
		damageTakenFmod.setVolume(volume);
		dashFmod.setVolume(volume);
		deathFmod.setVolume(volume);
		
		pickModuleFmod.setVolume(volume);
		noModuleFmod.setVolume(volume);
		stance_switchFmod.setVolume(volume);
		shootFmod.setVolume(volume);
		reflectFmod.setVolume(volume);
		shield_deployFmod.setVolume(volume);

		spawnEnemyFmod.setVolume(volume);
		losePartFmod.setVolume(volume);
		col_part_groundFmod.setVolume(volume);
		col_part_waterFmod.setVolume(volume);
		shoot_TraqueurFmod.setVolume(volume);
		shoot_InfanterieFmod.setVolume(volume);
		shoot_SniperFmod.setVolume(volume);
		charging_SniperFmod.setVolume(volume);
		moving_chargeurFmod.setVolume(volume);
	}
}