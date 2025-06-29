/**
* Universidad de La Laguna
* Proyecto: Roblockly-Android
* Autor: Thomas Edward Bradley
* Email: alu0101408248@ull.edu.es
* Fecha: 28/06/2025
* Descripcion: 
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomLevelLoad : MonoBehaviour {
  private string levelCode = "";

  public void setLevelCode(string code) {
    levelCode = code;
  }

  public void LoadLevel() {
    PlayerPrefs.SetString("Code", levelCode);
    PlayerPrefs.SetInt("Robot count", 1);
    PlayerPrefs.SetInt("Level", 6);
    SceneManager.LoadScene(2);
  }

  public void EditLevel() {
    PlayerPrefs.SetString("Code", levelCode);
    SceneManager.LoadScene(4);
  }

}
