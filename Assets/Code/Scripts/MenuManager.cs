using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Animator titlePageAnimator;
    [SerializeField] private Animator levelPageAnimator;

    [SerializeField] private GameObject[] levelButtons;

    [SerializeField] private GameObject blackbkg;
    [SerializeField] private CanvasGroup fadeToBlack;
    private float currVelocity;
    bool fading = false;
    bool transitioningToLevel = false;

    private void Start()
    {
        // TODO: UNCOMMENT FOR RELEASSE?
        // foreach (GameObject button in levelButtons)
        // {
        //     button.SetActive(false);
        // }

        StartCoroutine(WaitToFlip());

        AudioManager.instance.SwitchTheme(AudioManager.MusicType.baseTheme);
    }
    
    private void Update()
    {
        if (fading)
        {
            if (transitioningToLevel)
            {
                fadeToBlack.alpha = Mathf.SmoothDamp(fadeToBlack.alpha, 1, ref currVelocity, 0.75f);

                if ((1 - fadeToBlack.alpha) < 0.0001f)
                {
                    fading = false;
                }
            }
            else
            {
                fadeToBlack.alpha = Mathf.SmoothDamp(fadeToBlack.alpha, 0, ref currVelocity, 1.5f);

                if (fadeToBlack.alpha < 0.0001f)
                {
                    fading = false;
                    transitioningToLevel = true;
                }
            }
            
        }
    }

    private IEnumerator WaitToFlip()
    {

        fading = true;
        transitioningToLevel = false;
        fadeToBlack.alpha = 1;

        yield return new WaitForSeconds(1);


        // TODO: UNCOMMENT FOR RELEASSE?
        // int prevLevel = PlayerPrefs.GetInt("PrevLevel");
        // int currLevel = PlayerPrefs.GetInt("CurrLevel");

        // if (prevLevel < currLevel || currLevel == 0)
        // {
        //     // play calendar page flip anim
        //     levelPageAnimator.SetTrigger("Flip");

        //     // reveal button
        //     levelButtons[currLevel].SetActive(true);
        // }
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

    public void LoadBargaining()
    {
        SceneManager.LoadScene("Breakfast");
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
