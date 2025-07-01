/**
* Universidad de La Laguna
* Proyecto: Roblockly-Android
* Autor: Thomas Edward Bradley
* Email: alu0101408248@ull.edu.es
* Fecha: 01/07/2025
* Descripcion: Codigo que invoca plugin JS para habilitar boton de 'pegar' en WebGL
*/

using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class ClipboardManager : MonoBehaviour {
  public TMP_InputField coceInput;

  #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void PasteFromClipboard();
  #endif

  /// <summary>
  /// Invocado por el boton, tiene en cuenta si estamos en WebGL o editor
  /// </summary>
  public void TriggerPaste() {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PasteFromClipboard();
    #else
      coceInput.text = GUIUtility.systemCopyBuffer;
    #endif
  }

  /// <summary>
  /// Llamado por plugin JavaScript
  /// </summary>
  /// <param name="pastedText">Texto en clipboard</param>
  public void OnClipboardPaste(string pastedText) {
    coceInput.text = pastedText;
  }
}
