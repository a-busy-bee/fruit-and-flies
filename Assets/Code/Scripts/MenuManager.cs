using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<Animator> lvSelectObjects = new List<Animator>();
    [SerializeField] private List<Animator> titleObjects = new List<Animator>();
    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void SwitchToLevelSelect()
    {
        foreach (Animator animator in titleObjects)
        {
            animator.SetTrigger("Exit");
        }
        foreach (Animator animator in lvSelectObjects)
        {
            animator.SetTrigger("Enter");
        }
    }

     public void SwitchToTitle()
    {
        foreach (Animator animator in lvSelectObjects)
        {
            animator.SetTrigger("Exit");
        }
        foreach (Animator animator in titleObjects)
        {
            animator.SetTrigger("Enter");
        }
    }

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
}
