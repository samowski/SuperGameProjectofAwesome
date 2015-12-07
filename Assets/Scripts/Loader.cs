using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loader : MonoBehaviour
{
    Animator animator;

    Button continueButton;
    Button newGameButton;
    Button optionsButton;
    Button backButton;
    Button noButton;

    void Start()
    {        
    }

    void Awake()
    {
		if (Application.loadedLevelName == "menu") {
			animator = GameObject.Find ("Menu").GetComponent<Animator> ();

			continueButton = GameObject.Find ("Panel/Main/ContinueButton").GetComponent<Button> ();
			newGameButton = GameObject.Find ("Panel/Main/NewGameButton").GetComponent<Button> ();
			optionsButton = GameObject.Find ("Panel/Main/OptionsButton").GetComponent<Button> ();
			backButton = GameObject.Find ("Panel/Options/BackButton").GetComponent<Button> ();
			noButton = GameObject.Find ("Panel/Override/NoButton").GetComponent<Button> ();

			continueButton.Select ();
		}
    }

    public void LoadGame()
    {
        Application.LoadLevel("game");
    }

    public void LoadMenu()
    {
        Application.LoadLevel("menu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnOptionsClick()
    {
        animator.SetBool("Options", true);
        backButton.Select();
    }

    public void OnBackClick()
    {
        animator.SetBool("Options", false);
        optionsButton.Select();
    }

    public void OnContinueClick()
    {
        animator.SetBool("LevelSelect", true);
    }

    public void OnNewGameClick()
    {
        bool gameExists = true; //change later

        if (gameExists)
        {
            animator.SetBool("Override", true);
            noButton.Select();
        }
        else
        {
            animator.SetBool("LevelSelect", true);
        }

    }

    public void OnYesClick()
    {
        animator.SetBool("LevelSelect", true);
    }

    public void OnNoClick()
    {
        animator.SetBool("Override", false);
        newGameButton.Select();
    }

    void Update()
    {

        if (Input.GetButtonUp("Jump"))
        {
            if (Application.loadedLevelName == "game")
            {
                LoadMenu();
            }
            else
            {
                LoadGame();
            }
        }
    }
}
