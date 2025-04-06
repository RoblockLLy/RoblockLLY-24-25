/**
* Universidad de La Laguna
* Proyecto: Roblockly
* Autor: Edwin Plasencia Hern�ndez
* Email: alu0101329888@ull.edu.es
* Fecha: 07/05/2024
* Descripci�n: MainMenuManager: Manager del men� principal para dar utilidad a los botones del canvas
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    void Start() {}

    void Update() {}

    public void OnClick_Play() { // Carga la siguiente escena (Men� de selecci�n de robot)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void OnClick_Exit() { // Cierra la aplicaci�n
        Application.Quit();
    }
}
