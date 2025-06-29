/**
* Universidad de La Laguna
* Proyecto: Roblockly-Android
* Autor: Thomas Edward Bradley
* Email: alu0101408248@ull.edu.es
* Fecha: 28/06/2025
* Descripcion: Clase encargado del visualizador de niveles personalizados, también maneja los filtros asociados a ello
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelViewer : MonoBehaviour {
 
  #region Atributos
 
  [Header("Level Panel")]
  [SerializeField] [Tooltip("Panel donde se mostaran todos los niveles personalizados")]
  public GameObject levelPanel;
  [SerializeField] [Tooltip("Prefab para la visualización de cada nivel")]
  public GameObject levelPanelPrefab;
  [SerializeField] [Tooltip("GameObject donde se colocaran todos los GameObject correspondientes a cada nivel")]
  public GameObject levelPanelParent;
    
  [Header("Search Box")]
  [SerializeField] [Tooltip("Texto introducido por el usuario para busqueda/filtrado")]
  public TextMeshProUGUI searchInput;
  [SerializeField] [Tooltip("El tipo de busqueda que se esta llevando a cabo")]
  public TextMeshProUGUI selectedSearch;  // Hay texto vinculado pero es invisible, facilita averiguar la opción seleccionada

  /// <summary>
  /// Lista con todos los niveles almacenados en el repositorio
  /// </summary>
  private List<string> fullLevelList = new List<string>();
  /// <summary>
  /// Lista de todos los niveles que cumplen con las opciones de busqueda (en el caso de que haya uno)
  /// </summary>
  private List<string> filteredList = new List<string>();

  #endregion

  #region Visualización

  /// <summary>
  /// Rellena la lista de niveles filtrados cuando se pulsa sobre la opción de busqueda (mostrando todo si este es vacio, lo que
  /// encaja con lo buscado o nada, en el caso de que no encaje nada)
  /// </summary>
  public void search() {
    filteredList = new List<string>();                                  // Resetear la lista filtrada
    string cleanText = CleanInput(searchInput.text.ToLower().Trim());   // IMPORTANTE: limpiar caracteres ocultos del texto

    if (string.IsNullOrEmpty(cleanText)) {                              // Campo de busqueda vacio, mostramos todo
      refreshLevelViewer();
      return;
    }

    foreach (string level in fullLevelList) {                           // Metemos todo lo que empareja en la lista filtrada
      JObject json = JObject.Parse(level);
      string searchType = (selectedSearch.text == "Name") ? "level_name" : "user_name";   // Vemos si tenemos que buscar por nombre o creador
      string levelNameClean = json["environment"][searchType].ToString().ToLower().Trim();
      if (levelNameClean.Contains(cleanText)) filteredList.Add(level);
    }

    if (filteredList.Count == 0) {                                      // No hay nada que corresponde con la busqueda, mostramos nada
      cleanViewerSpace();
      return;
    }

    refreshLevelViewer();                                               // Actualizamos los niveles mostrados por pantalla
  }

  /// <summary>
  /// Vuelve a pintar todos los niveles (solo los filtrados en el caso de que haya una buisqueda activa) en el panel
  /// </summary>
  private void refreshLevelViewer() {
    cleanViewerSpace();   // Primero eliminamos todos los elementos actuales del panel
    List<string> levelList = (filteredList != null && filteredList.Count > 0) ? filteredList : fullLevelList;   // Si tenemos aplicado algun filtro, usamos la lista reducida 

    levelPanelParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, (100 * levelList.Count) + 20);   // Longitud del espacio de 'scroll'
    
    for (int i = 0; i < levelList.Count; i++) {
      GameObject newObject = Instantiate(levelPanelPrefab);                                                           // 1) Instanciamos el objeto nuevo
      newObject.transform.SetParent(levelPanelParent.transform);                                                      // 2) Lo colocamos en su sitio adecuado
      newObject.transform.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);                         // 3) Arreglamos su escala
      newObject.transform.GetComponent<RectTransform>().localPosition = new Vector3(750f, -152.5f - (100f * i), 0f);  // 4) Arreglamos su posición

      JObject json = JObject.Parse(levelList[i]);                                                                     // 5) Asignamos los campos de texto en el panel
      newObject.transform.Find("Txt - Level Name").GetComponent<TextMeshProUGUI>().text = json["environment"]["level_name"].ToString();
      newObject.transform.Find("Txt - Creator Name").GetComponent<TextMeshProUGUI>().text = json["environment"]["user_name"].ToString();

      newObject.GetComponent<CustomLevelLoad>().setLevelCode(levelList[i]);                                           // 6) Guardamos el json dentro del GameObject
    }
  }

  #endregion

  #region Limpiar

  /// <summary>
  /// Elimina todos los niveles cargados dentro del panel
  /// </summary>
  private void cleanViewerSpace() {
    foreach (Transform child in levelPanelParent.transform) {
      Destroy(child.gameObject);
    }
  }

  /// <summary>
  /// Elimina todos los caracteres ocultos asociados a una string, necesario para funcionamiento correcto de metodo 'Contains()'
  /// </summary>
  /// <param name="input">Cadena a procesar</param>
  /// <returns>Input con todos los caracteres ocultos eliminados</returns>
  private string CleanInput(string input) {
    if (input == null) return "";

    string[] invisibleChars = {
      "\u200B", // Espacio con 0 de ancho
      "\u200C", // Non-Joiner con 0 de ancho
      "\u200D", // Joiner con 0 de ancho
      "\u200E", // Marco izquierda a derecha
      "\u200F", // Marco derecha a izquierda
      "\uFEFF"  // Byte Order Mark (BOM)
    };

    foreach (string ch in invisibleChars) {
      input = input.Replace(ch, "");
    }

    return input.Trim();
  }

  #endregion

  #region Metodos Aux

  /// <summary>
  /// Setter para la lista completa de niveles, vuelve a pintar el panel tras guardar este
  /// </summary>
  /// <param name="list">Lista con los codigos de los niveles</param>
  public void setLevels(List<string> list) {
    fullLevelList = list;
    refreshLevelViewer();
  }

  /// <summary>
  /// Retorna a la escena para seleccionar niveles (Escena 01), usado por boton 'Return'
  /// </summary>
  public void ReturnToMenu() {
    SceneManager.LoadScene(1);
  }

  #endregion

}
