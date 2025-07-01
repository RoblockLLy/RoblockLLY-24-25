/**
* Universidad de La Laguna
* Proyecto: Roblockly
* Autor: Thomas Edward Bradley
* Email: alu0101408248@ull.edu.es
* Fecha: 28/06/2025
* Descripcion: Clase encargada de cargar el nivel seleccionado de la manera apropiada
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomLevelLoad : MonoBehaviour {
  /// <summary>
  /// String que contiene el json del nivel asociado al objeto
  /// </summary>
  private string levelCode = "";

  /// <summary>
  /// Setter que asigna el valor de 'levelCode' (llamado para cada nivel personalizado cuando se construye en LevelViewer)
  /// </summary>
  /// <param name="code">String con el código json del nivel</param>
  public void setLevelCode(string code) {
    levelCode = code;
  }

  /// <summary>
  /// Carga el nivel que se ha guardado en la clase para que sea jugado por el usuario
  /// </summary>
  public void LoadLevel() {
    PlayerPrefs.SetString("Code", levelCode);
    PlayerPrefs.SetInt("Robot count", 1);
    PlayerPrefs.SetInt("Level", 6);
    SceneManager.LoadScene(2);
  }

  /// <summary>
  /// Carga el nivel que se ha guardado en la clase para que sea editado por el usuario
  /// </summary>
  public void EditLevel() {
    PlayerPrefs.SetString("Code", levelCode);
    SceneManager.LoadScene(4);
  }

}
