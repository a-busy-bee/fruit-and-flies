using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
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
