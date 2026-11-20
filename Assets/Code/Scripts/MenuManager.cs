using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Animator titlePageAnimator;
    [SerializeField] private Animator levelPageAnimator;

    [SerializeField] private GameObject[] levelButtons;
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
        
        AudioManager.instance.SwitchTheme(AudioManager.MusicType.baseTheme);

        StartCoroutine(WaitToFlip());
    }
    
    private IEnumerator WaitToFlip()
    {
        FadeToBlack.instance.FadeOut();

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
        StartCoroutine(WaitThenQuit());
    }

    #region Levels
    public void LoadTutorial()
    {
        StartCoroutine(WaitThenLoadScene("Tutorial"));
    }

    public void LoadBargaining()
    {
        StartCoroutine(WaitThenLoadScene("Breakfast"));
    }

    public void LoadDepression()
    {
        StartCoroutine(WaitThenLoadScene("Depression"));
    }

    public void LoadAcceptance()
    {
        StartCoroutine(WaitThenLoadScene("Acceptance"));
    }

    private IEnumerator WaitThenLoadScene(string sceneName)
    {
        FadeToBlack.instance.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator WaitThenQuit()
    {
        FadeToBlack.instance.FadeIn();
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    #endregion
}
