using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavegacion : MonoBehaviour
{
    public void IrAlJuego()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
