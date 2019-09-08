using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Main");
    }
}
