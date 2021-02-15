using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using CotcSdk;
using TMPro;

public class UIManager : MonoBehaviour
{
	public PostProcessProfile profilePostProcess;
	private ColorGrading pPColorGrading;
	private float brightness = 0;
	private float contrast = 0;

	private SoundManager soundManager;
	public Slider slid_volume_Music;
	public Slider slid_volume_SFX;
	public Slider slid_brightness;
	public Slider slid_contrast;
	
	public Slider slid_volume_Music_pause;
	public Slider slid_volume_SFX_pause;
	public Slider slid_brightness_pause;
	public Slider slid_contrast_pause;

	public Text playerHP;

	[SerializeField] private GameObject pause_options_panel;
	[SerializeField] private GameObject inGame_panel;
	[SerializeField] private Texture2D cursor;
	[SerializeField] private Texture2D cursor_NoModules;
	private GameObject player;



	private GameObject panelLogin;
	private GameObject panelStart;
	private GameObject cameraTitle;

	public TMP_InputField inputFLogin;
	public TMP_InputField inputFPassword;

	private Button signUpLogin;

	private Button resumeSession;

	private XtraLifeManager xtraLife;

	public List<TextMeshProUGUI> TimeNames;
	public List<TextMeshProUGUI> TimeValues;


	public List<TextMeshProUGUI> ScoreNames;
	public List<TextMeshProUGUI> ScoreValues;

	public List<GameObject> TimePanels;
	public List<GameObject> ScorePanels;

	public TextMeshProUGUI displayLogin;

	//private int 

	private void Start()
	{
		xtraLife = GameObject.Find("Manager").GetComponent<XtraLifeManager>();
		player = GameObject.Find("Player");

		panelLogin = GameObject.Find("Panel_Login");
		panelStart = GameObject.Find("Panel_Start");
		cameraTitle = GameObject.Find("Camera_TitleScreen");
		inputFLogin = GameObject.Find("if_Login").GetComponent<TMP_InputField>();
		inputFPassword = GameObject.Find("if_Password").GetComponent<TMP_InputField>();
		signUpLogin = GameObject.Find("btn_SignUplogin").GetComponent<Button>();
		resumeSession = GameObject.Find("btn_Resume").GetComponent<Button>();

		toggleResumeButton();

		player = GameObject.Find("Player");
		Cursor.lockState = CursorLockMode.Confined;
		
		// POST PROCESS
		profilePostProcess.TryGetSettings<ColorGrading>(out pPColorGrading);
		brightness = profilePostProcess.GetSetting<ColorGrading>().postExposure;
		slid_brightness.value = brightness;
		
		contrast = profilePostProcess.GetSetting<ColorGrading>().contrast;
		slid_contrast.value = contrast;
		
		// SOUNDS
		soundManager = GameObject.Find("Manager").GetComponent<SoundManager>();
		slid_volume_Music.value = 1.0f;
		slid_volume_SFX.value = 1.0f;
		soundManager.SetVolumeWind(slid_volume_Music.value);
		soundManager.SetVolumeSFX(slid_volume_SFX.value);


		xtraLife.CustomAwake();
	}

	private void Update()
	{
		if (player.GetComponent<CustomCharacterController>().isArenaStarted)
		{
			if (player.GetComponent<DetectionParcours>().suiveurs.Count == 0)
			{
				Cursor.SetCursor(cursor_NoModules, Vector2.zero, CursorMode.Auto);
			}
			else
			{
				Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
			}
		}
	}

	public void StartGame()
	{
		Debug.Log("Starting game...");
		slid_brightness_pause.value = slid_brightness.value;
		slid_contrast_pause.value = slid_contrast.value;
		slid_volume_SFX_pause.value = slid_volume_SFX.value;
		slid_volume_Music_pause.value = slid_volume_Music.value;
		
		
	}
	
	public void LogoutGame()
	{
		
		GameObject.Find("Player").GetComponent<CustomCharacterController>().pauseText.SetActive(false);
	    GameObject.Find("Manager").GetComponent<XtraLifeManager>().SendLogout();
		GameObject.Find("Manager").GetComponent<SoundManager>().StopAllSounds(true);

		Unpause();
		Time.timeScale = 1f;
		GameObject.Find("Manager").GetComponent<GameManager>().enabled = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		
		//GameObject.Find("Manager").GetComponent<GameManager>().mustReload = true;
	}

	public void QuitGame()
	{
		Debug.Log("Quitting game...");
		Application.Quit();
	}

	public void Unpause()
	{
		if (pause_options_panel.activeSelf)
		{
			pause_options_panel.SetActive(false);
		}
		inGame_panel.SetActive(true);
	}

	public void EditVolumeMusic()
	{
		if (GameObject.Find("Player").GetComponent<CharacterInputDetector>().isPauseOn)
		{
			soundManager.SetVolumeWind(slid_volume_Music_pause.value);
		}
		else
		{
			soundManager.SetVolumeWind(slid_volume_Music.value);
		}
	}
	
	public void EditVolumeSFX()
	{
		if (GameObject.Find("Player").GetComponent<CharacterInputDetector>().isPauseOn)
		{
			soundManager.SetVolumeSFX(slid_volume_SFX_pause.value);
		}
		else
		{
			soundManager.SetVolumeSFX(slid_volume_SFX.value);
		}
	}

	public void EditBrightness()
	{
		if (GameObject.Find("Player").GetComponent<CharacterInputDetector>().isPauseOn)
		{
			pPColorGrading.postExposure.value = slid_brightness_pause.value;
		}
		else
		{
			pPColorGrading.postExposure.value = slid_brightness.value;
		}
		
		profilePostProcess.GetSetting<ColorGrading>().brightness = pPColorGrading.postExposure;
	}
	
	public void EditContrast()
	{
		if (GameObject.Find("Player").GetComponent<CharacterInputDetector>().isPauseOn)
		{
			pPColorGrading.contrast.value = slid_contrast_pause.value;
		}
		else
		{
			pPColorGrading.contrast.value = slid_contrast.value;
		}
		
		profilePostProcess.GetSetting<ColorGrading>().contrast = pPColorGrading.contrast;
	}




	public void SignUpLogin()
	{
		xtraLife.CheckUserDuringLogin(inputFLogin.text, inputFPassword.text);

		
	}

	public void enableTransition()
    {
		panelLogin.SetActive(false);
		panelStart.SetActive(true);
		cameraTitle.GetComponent<Fading>().enabled = true;
	}

	public void toggleResumeButton()
    {
		resumeSession.gameObject.SetActive(!resumeSession.gameObject.activeSelf);
    }


	
	public void RefreshTimeLeaderBoard(List<string> names, List<long> values)
    {
		ScoreManager sc = GameObject.Find("Manager").GetComponent<ScoreManager>();

		for(int i = 0; i < names.Count; i++)
        {
			TimeNames[i].text = names[i];
			TimeValues[i].text = sc.FormatTime((float)values[i]);
			TimePanels[i].gameObject.SetActive(true);
		}
    }

	public void RefreshScoreLeaderBoard(List<string> names, List<string> values)
	{
		for (int i = 0; i < names.Count; i++)
		{
			ScoreNames[i].text = names[i];
			ScoreValues[i].text = values[i];
			ScorePanels[i].gameObject.SetActive(true);
		}
	}

	public void ToggleScoreLBPanelVisibility(bool activate)
    {
		foreach(GameObject go in ScorePanels)
        {
			go.SetActive(activate);
        }
    }

	public void ToggleTimeLBPanelVisibility(bool activate)
	{
		foreach (GameObject go in TimePanels)
		{
			go.SetActive(activate);
		}
	}

	public void RefreshDisplayLogin(string name)
	{
		displayLogin.text = name;
	}
}
