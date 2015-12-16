using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    public static bool showLevelSelection = false;

    Animator animator;

    EventSystem eventSystem;

    // Test
    Button level1;
    Button level2;
    //

    Button continueButton;
    Button newGameButton;
    Button optionsButton;
    Button exitButton;
    Button backButton;
    Button yesButton;
    Button noButton;

    Slider musicVolumeSlider;
    Slider sfxVolumeSlider;

    void Start()
    {
        SaveGame.Load(); // only on startup ?

        animator = GetComponent<Animator>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        // Test
        level1 = GameObject.Find("Level/Level1").GetComponent<Button>();
        level1.onClick.AddListener(delegate {
            Application.LoadLevel("game");
        });
        level2 = GameObject.Find("Level/Level2").GetComponent<Button>();
        level2.onClick.AddListener(delegate {
            Application.LoadLevel("game2");
        });
        //

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

        if (GameProgress.instance.level == 0)
        {
            continueButton.interactable = false;
        }

        if (showLevelSelection)
        {
            animator.SetBool("LevelSelect", true);
            animator.CrossFade("MainLevelSelect", 0.0f);
        }
        else
        {
            if (GameProgress.instance.level == 0)
            {
                Utils.Select(newGameButton);
            }
            else
            {
                Utils.Select(continueButton);
            }
        }
    }
        
    // obsolete ???
    public void LoadGame()
    {
        Application.LoadLevel("game"); // based on level selection
    }
        
    public void Exit()
    {
        Application.Quit();
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

    public void ContinueGame()
    {
        InitLevelSelect();
    }

    public void NewGame()
    {
        bool gameExists = GameProgress.instance.level > 0;

        if (gameExists)
        {
            animator.SetBool("Override", true);
            Utils.Select(noButton);
        }
        else
        {
            InitNewgame();
            InitLevelSelect();
        }

    }

    void InitNewgame()
    {
        GameProgress.instance.level = 1;

        SaveGame.Save();
    }

    void InitLevelSelect()
    {
        animator.SetBool("LevelSelect", true);

        level2.interactable = GameProgress.instance.level >= 2;
    }

    public void ConfirmOverride()
    {
        animator.SetBool("LevelSelect", true);

        InitNewgame();
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

        if (GameProgress.instance.level > 0)
        {
            continueButton.interactable = true;
        }

        Utils.Select(continueButton);
    }

    void Update()
    {
        #if UNITY_EDITOR
        /*if (Input.GetButtonUp("Jump"))
        {
            LoadGame();
        }*/
        #endif

        if (Input.GetButtonUp("Cancel"))
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
            GameProgress.instance.level = 0;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            GameProgress.instance.level = 1;
        }
        #endif
    }
}
