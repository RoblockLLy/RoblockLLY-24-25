/**
* Universidad de La Laguna
* Proyecto: Roblockly
* Autor: Thomas Edward Bradley
* Email: alu0101408248@ull.edu.es
* Fecha: 29/06/2025
* Descripcion: Activa el boton de busqueda cuando se detecta al usuario pulsar la tecla 'Enter'
*/

using UnityEngine;
using UnityEngine.UI;

public class EnterKeyListener : MonoBehaviour {
  /// <summary>
  /// Boton a pulsar cuando se detecte la tecla pulsada
  /// </summary>
  public Button targetButton;

  /// <summary>
  /// Revisa cada frame si la tecla se ha pulsado
  /// </summary>
  void Update() {
    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
      targetButton.onClick.Invoke();
    }
  }
}
