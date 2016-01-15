using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
	public static bool ShowLevelSelection = false;

	Animator animator;

	EventSystem eventSystem;

	Button continueButton;
	Button newGameButton;
	Button optionsButton;
	Button exitButton;
	Button backButton;
	Button yesButton;
	Button noButton;

	Slider musicVolumeSlider;
	Slider sfxVolumeSlider;

	PlayerController granny;

	LevelEntry[] levelEntries;

	void Start()
	{
		animator = GetComponent<Animator>();

		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

		continueButton = GameObject.Find("Panel/Main/ContinueButton").GetComponent<Button>();
		continueButton.onClick.AddListener(ContinueGame);

		newGameButton = GameObject.Find("Panel/Main/NewGameButton").GetComponent<Button>();
		newGameButton.onClick.AddListener(NewGame);

		optionsButton = GameObject.Find("Panel/Main/OptionsButton").GetComponent<Button>();
		optionsButton.onClick.AddListener(EnterOptions);

		exitButton = GameObject.Find("Panel/Main/ExitButton").GetComponent<Button>();
		exitButton.onClick.AddListener(Exit);

		backButton = GameObject.Find("Panel/Options/BackButton").GetComponent<Button>();
		backButton.onClick.AddListener(LeaveOptions);

		yesButton = GameObject.Find("Panel/Override/YesButton").GetComponent<Button>();
		yesButton.onClick.AddListener(ConfirmOverride);

		noButton = GameObject.Find("Panel/Override/NoButton").GetComponent<Button>();
		noButton.onClick.AddListener(CancelOverride);

		musicVolumeSlider = GameObject.Find("Panel/Options/MusicSlider").GetComponent<Slider>();
		musicVolumeSlider.onValueChanged.AddListener(Utils.ChangeMusicVolume);

		sfxVolumeSlider = GameObject.Find("Panel/Options/SFXSlider").GetComponent<Slider>();
		sfxVolumeSlider.onValueChanged.AddListener(Utils.ChangeSfxVolume);

		granny = GameObject.Find("Granny").GetComponent<PlayerController>();

		PlayerController.IsAttacking = false; //quick fix

		levelEntries = GameObject.FindObjectsOfType<LevelEntry>();

		if (GameProgress.Instance.Level == 0)
		{
			continueButton.interactable = false;
		}

		if (ShowLevelSelection)
		{
			animator.SetBool("LevelSelect", true);
			animator.CrossFade("MainLevelSelect", 0.0f);

			granny.IsControllable = true;
		}
		else
		{
			granny.IsControllable = false;

			selectLeftButton();
		}
	}

	public void LoadGame(string level)
	{
		Application.LoadLevel(level);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void EnterOptions()
	{
		musicVolumeSlider.value = Options.Instance.MusicVolume;
		sfxVolumeSlider.value = Options.Instance.SfxVolume;

		animator.SetBool("Options", true);

		Utils.Select(backButton);
	}

	public void LeaveOptions()
	{
		SaveGame.Save();

		animator.SetBool("Options", false);

		Utils.Select(optionsButton);
	}

	public void ContinueGame()
	{
		InitLevelSelect();
	}

	public void NewGame()
	{
		bool gameExists = GameProgress.Instance.Level > 0;

		if (gameExists)
		{
			animator.SetBool("Override", true);
			Utils.Select(noButton);
		}
		else
		{
			initNewgame();
			InitLevelSelect();
		}
	}

	void initNewgame()
	{
		GameProgress.Instance.Level = 1;

		SaveGame.Save();
	}

	void InitLevelSelect()
	{
		animator.SetBool("LevelSelect", true);

		granny.IsControllable = true;

		updateLevelEntries();
	}

	void updateLevelEntries()
	{
		foreach (var levelEntry in levelEntries)
		{
			levelEntry.UpdateEnabled();
		}
	}

	public void ConfirmOverride()
	{
		animator.SetBool("LevelSelect", true);

		initNewgame();
		InitLevelSelect();
	}

	public void CancelOverride()
	{
		animator.SetBool("Override", false);

		Utils.Select(newGameButton);
	}

	public void CancelLevelSelect()
	{
		animator.SetBool("LevelSelect", false);
		animator.SetBool("Override", false);

		granny.IsControllable = false;

		if (GameProgress.Instance.Level > 0)
		{
			continueButton.interactable = true;
		}

		Utils.Select(continueButton);
	}

	void handleBackKey()
	{
		var currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (currentState.IsName("MainOptions"))
		{
			LeaveOptions();
		}
		else if (currentState.IsName("MainOverride"))
		{
			CancelOverride();
		}
		else if (currentState.IsName("Main"))
		{
			if (eventSystem.currentSelectedGameObject == exitButton.gameObject)
			{
				Exit();
			}
			else
			{
				Utils.Select(exitButton);
			}
		}
		else if (currentState.IsName("MainLevelSelect"))
		{
			CancelLevelSelect();
		}
	}

	void selectLeftButton()
	{
		var currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (currentState.IsName("MainOptions"))
		{
			Utils.Select(backButton);
		}
		else if (currentState.IsName("MainOverride"))
		{
			Utils.Select(yesButton); 
		}
		else if (currentState.IsName("Main"))
		{
			if (GameProgress.Instance.Level == 0)
			{
				Utils.Select(newGameButton);
			}
			else
			{
				Utils.Select(continueButton);
			}
		}
	}

	void selectRightButton()
	{
		var currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (currentState.IsName("MainOptions"))
		{
			Utils.Select(backButton);
		}
		else if (currentState.IsName("MainOverride"))
		{
			Utils.Select(noButton); 
		}
		else if (currentState.IsName("Main"))
		{
			Utils.Select(exitButton);
		}
	}

	void handleLostFocus()
	{
		float verticalAxis = Input.GetAxisRaw("Horizontal");

		if (verticalAxis > 0)
		{
			selectLeftButton();
		}
		else if (verticalAxis < 0)
		{
			selectRightButton();
		}
	}

	void Update()
	{
		if (Input.GetButtonUp("Cancel"))
		{
			handleBackKey();
		}

		if (eventSystem.currentSelectedGameObject == null)
		{
			handleLostFocus();   
		}

		#if UNITY_EDITOR
		if (Input.GetKeyUp(KeyCode.S))
		{
			SaveGame.Save();
		}

		if (Input.GetKeyUp(KeyCode.L))
		{
			SaveGame.Load();
		}

		if (Input.GetKeyUp(KeyCode.R))
		{
			GameProgress.Instance.Level = 0;
		}

		if (Input.GetKeyUp(KeyCode.T))
		{
			GameProgress.Instance.Level = 1;
		}
		#endif
	}
}