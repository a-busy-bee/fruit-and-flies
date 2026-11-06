using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Animator titlePageAnimator;
    [SerializeField] private Animator levelPageAnimator;

    [SerializeField] private GameObject[] levelButtons;


    private void Start()
    {
        foreach (GameObject button in levelButtons)
        {
            button.SetActive(false);
        }

        int prevLevel = PlayerPrefs.GetInt("PrevLevel");
        int currLevel = PlayerPrefs.GetInt("CurrLevel");

        if (prevLevel < currLevel)
        {
            // play calendar page flip anim
            levelPageAnimator.SetTrigger("Flip");

            // reveal button
            levelButtons[currLevel].SetActive(true);
        }
    }

    public void OnStart()
    {
        // play hide anim for title
        titlePageAnimator.SetTrigger("Hide");

        // play reveal anim for level 
        levelPageAnimator.SetTrigger("Reveal");
    }

    public void OnBack()
    {
        // play hide anim for level
        levelPageAnimator.SetTrigger("Hide");
        
        // play reveal anim for title
        titlePageAnimator.SetTrigger("Reveal");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    #region Levels
    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadAnger()
    {
        SceneManager.LoadScene("Anger");
    }

    public void LoadBargaining()
    {
        SceneManager.LoadScene("Bargaining");
    }

    public void LoadDenial()
    {
        SceneManager.LoadScene("Denial");
    }

    public void LoadDepression()
    {
        SceneManager.LoadScene("Depression");
    }

    public void LoadAcceptance()
    {
        SceneManager.LoadScene("Acceptance");
    }

    #endregion
}
