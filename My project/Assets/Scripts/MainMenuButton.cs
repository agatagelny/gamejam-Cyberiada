using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenuButton : MonoBehaviour
{
    public int test;
    public GameObject button_target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadScene()
    {
        Debug.Log(test);
        SceneManager.LoadScene(button_target.name);
    }

    void LoadPopUp()
    {
        //chuj
    }
}
