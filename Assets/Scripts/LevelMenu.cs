using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelMenu : MonoBehaviour
{
    Animator animator;

    Button continueButton;
    Button optionsButton;
    Button exitButton;
    Button backButton;

    Slider musicVolumeSlider;
    Slider sfxVolumeSlider;

    UnlockLevel unlockLevel;

    void Start()
    {
        animator = GetComponent<Animator>();

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

        unlockLevel = GameObject.Find("LevelReward").GetComponent<UnlockLevel>();
    }

    public void PauseGame()
    {
		animator.SetBool("Pause", true);

        Utils.Select(continueButton);
    }

    public void ContinueGame()
    {
		animator.SetBool("Pause", false);
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

    public void Exit(bool levelComplete)
    {
        MainMenu.showLevelSelection = levelComplete;

        if (levelComplete)
        {
            unlockLevel.Unlock();
        }

        Application.LoadLevel("MainMenu");
    }
        
    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
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
        }

        #if UNITY_EDITOR
        if (Input.GetButtonUp("Jump"))
        {
            Exit(true);
        }
        #endif
    }
}
