using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

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

    Sprite defaultPillSprite;
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

        defaultPillSprite = currentPill.sprite;
        defaultPillColor = currentPill.color;
        defaultChocolateColor = currentChocolate.color;

        var go = GameObject.Find("LevelReward");
        unlockLevel = go != null  ? go.GetComponent<UnlockLevel>() : null;

        go = GameObject.Find("Granny");
        granny = go != null ? go.GetComponent<PlayerController>() : null;
    }

    void enterMenu(string animatorState)
    {
        animator.SetBool(animatorState, true);

        if (granny != null)
            granny.isControllable = false;

        Utils.Select(continueButton);
    }

    public void PauseGame()
    {
        enterMenu("Pause");
    }

    public void EnterLevelFailed()
    {
        enterMenu("Failed");
    }

    public void EnterLevelFinished()
    {
        if (unlockLevel != null)
            unlockLevel.Unlock();

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
                granny.isControllable = true;
        }
        else if (currentState.IsName("LevelFailed"))
        {
            Application.LoadLevel(Application.loadedLevel); 
        }
        else if (currentState.IsName("LevelFinished"))
        {
            Exit(true);
        }
    }

    public void EnterOptions()
    {
        musicVolumeSlider.value = Options.instance.MusicVolume;
        sfxVolumeSlider.value = Options.instance.SfxVolume;

        animator.SetBool("Options", true);

        Utils.Select(backButton);
    }

    public void LeaveOptions()
    {
        SaveGame.Save();

        animator.SetBool("Options", false);

        Utils.Select(optionsButton);
    }

    public void SetHUDPill(Sprite pillSprite)
    {
        if (pillSprite != null)
        {
            currentPill.sprite = pillSprite;
            currentPill.color = Color.white;
        }
        else
        {
            currentPill.sprite = defaultPillSprite;
            currentPill.color = defaultPillColor;
        }
    }

    public void SetHUDChocolate(bool hasChocolate)
    {
        currentChocolate.color = hasChocolate ? Color.white : defaultChocolateColor;
    }

    public void Exit(bool levelFinished)
    {
        MainMenu.showLevelSelection = levelFinished;

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
        float verticalAxis = Input.GetAxis("Horizontal");

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