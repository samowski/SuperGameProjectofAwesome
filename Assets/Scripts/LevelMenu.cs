﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelMenu : MonoBehaviour
{
	Animator animator;

	EventSystem eventSystem;

	Button continueButton;
	Button optionsButton;
	Button exitButton;
	Button backButton;

	Slider musicVolumeSlider;
	Slider sfxVolumeSlider;

	Image currentPill;
	Image currentChocolate;

	Material defaultPillMaterial;
	Color defaultPillColor;
	Color defaultChocolateColor;

	UnlockLevel unlockLevel;

	PlayerController granny;

	void Start()
	{
		animator = GetComponent<Animator>();

		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

		continueButton = GameObject.Find("LevelMenu/PauseMenu/Main/ContinueButton").GetComponent<Button>();
		continueButton.onClick.AddListener(ContinueGame);

		optionsButton = GameObject.Find("LevelMenu/PauseMenu/Main/OptionsButton").GetComponent<Button>();
		optionsButton.onClick.AddListener(EnterOptions);

		exitButton = GameObject.Find("LevelMenu/PauseMenu/Main/ExitButton").GetComponent<Button>();
		exitButton.onClick.AddListener(delegate
			{
				Exit(false);
			});

		backButton = GameObject.Find("LevelMenu/PauseMenu/Options/BackButton").GetComponent<Button>();
		backButton.onClick.AddListener(LeaveOptions);

		musicVolumeSlider = GameObject.Find("LevelMenu/PauseMenu/Options/MusicSlider").GetComponent<Slider>();
		musicVolumeSlider.onValueChanged.AddListener(Utils.ChangeMusicVolume);

		sfxVolumeSlider = GameObject.Find("LevelMenu/PauseMenu/Options/SFXSlider").GetComponent<Slider>();
		sfxVolumeSlider.onValueChanged.AddListener(Utils.ChangeSfxVolume);

		currentPill = GameObject.Find("LevelMenu/HUD/Pill/Image").GetComponent<Image>();
		currentChocolate = GameObject.Find("LevelMenu/HUD/Chocolate/Image").GetComponent<Image>();

		defaultPillMaterial = currentPill.material;
		defaultPillColor = currentPill.color;
		defaultChocolateColor = currentChocolate.color;

		var gameObject = GameObject.Find("LevelReward");
		unlockLevel = gameObject != null ? gameObject.GetComponent<UnlockLevel>() : null;

		gameObject = GameObject.Find("Granny");
		granny = gameObject != null ? gameObject.GetComponent<PlayerController>() : null;
	}

	void enterMenu(string animatorState)
	{
		animator.SetBool(animatorState, true);

		if (granny != null)
		{
			granny.IsControllable = false;
		}

		Utils.Select(continueButton);
	}

	public void PauseGame()
	{
		enterMenu("Pause");

		Time.timeScale = 0.0f;
	}

	public void EnterLevelFailed()
	{
		enterMenu("Failed");
	}

	public void EnterLevelFinished()
	{
		if (unlockLevel != null)
		{
			unlockLevel.Unlock();
		}

		SaveGame.Save();

		enterMenu("Finished");
	}

	public void ContinueGame()
	{
		var currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (currentState.IsName("Pause"))
		{
			animator.SetBool("Pause", false);

			if (granny != null)
			{
				granny.IsControllable = true;
			}
		}
		else if (currentState.IsName("LevelFailed"))
		{
			Application.LoadLevel(Application.loadedLevel); 
		}
		else if (currentState.IsName("LevelFinished"))
		{
			Exit(true);
		}

		Time.timeScale = 1.0f;
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

	public void SetHUDPill(Pill.Effect effect)
	{
		if (effect != Pill.Effect.Nothing)
		{
			currentPill.material = Pill.getMaterial(effect);
			currentPill.color = Color.white;
		}
		else
		{
			currentPill.material = defaultPillMaterial;
			currentPill.color = defaultPillColor;
		}
	}

	public void SetHUDChocolate(bool hasChocolate)
	{
		currentChocolate.color = hasChocolate ? Color.white : defaultChocolateColor;
	}

	public void Exit(bool levelFinished)
	{
		MainMenu.ShowLevelSelection = levelFinished;

		Time.timeScale = 1.0f;

		Application.LoadLevel("MainMenu");
	}

	void handleBackKey()
	{
		var currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (currentState.IsName("LevelMain"))
		{
			PauseGame();
		}
		else if (currentState.IsName("Pause"))
		{
			ContinueGame();
		}
		else if (currentState.IsName("Options"))
		{
			LeaveOptions();
		}
		else if (currentState.IsName("LevelFailed"))
		{
			if (eventSystem.currentSelectedGameObject == exitButton.gameObject)
			{
				Exit(false);
			}
			else
			{
				Utils.Select(exitButton);
			}
		}
		else if (currentState.IsName("LevelFinished"))
		{
			if (eventSystem.currentSelectedGameObject == exitButton.gameObject)
			{
				Exit(true);
			}
			else
			{
				Utils.Select(exitButton);
			}
		}
	}

	void selectLeftButton()
	{
		var currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (currentState.IsName("Pause") || currentState.IsName("LevelFailed") || currentState.IsName("LevelFinished"))
		{
			Utils.Select(continueButton);
		}
		else if (currentState.IsName("Options"))
		{
			Utils.Select(backButton); 
		}
	}

	void selectRightButton()
	{
		var currentState = animator.GetCurrentAnimatorStateInfo(0);

		if (currentState.IsName("Pause") || currentState.IsName("LevelFailed") || currentState.IsName("LevelFinished"))
		{
			Utils.Select(exitButton);
		}
		else if (currentState.IsName("Options"))
		{
			Utils.Select(backButton); 
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
		if (Input.GetKeyUp(KeyCode.O))
		{
			EnterLevelFinished();
		}
		if (Input.GetKeyUp(KeyCode.P))
		{
			EnterLevelFailed();
		}
		#endif
	}
}