using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelViewer : MonoBehaviour {
 
  #region Atributos
 
  [Header("Level Panel")]
  [SerializeField] [Tooltip("")]
  public GameObject levelPanel;
  [SerializeField] [Tooltip("")]
  public GameObject levelPanelPrefab;
  [SerializeField] [Tooltip("")]
  public GameObject levelPanelParent;
    
  [Header("Search Box")]
  [SerializeField] [Tooltip("")]
  public TextMeshProUGUI searchInput;

  /// <summary>
  /// 
  /// </summary>
  private List<string> fullLevelList = new List<string>();
  /// <summary>
  /// 
  /// </summary>
  private List<string> filteredList = new List<string>();

  #endregion

  /// <summary>
  /// 
  /// </summary>
  /// <param name="list"></param>
  public void setLevels(List<string> list) {
    fullLevelList = list;
    refreshLevelViewer();
  }

  /// <summary>
  /// 
  /// </summary>
  public void search() {
    filteredList = new List<string>();                                  // Resetear la lista filtrada
    string cleanText = CleanInput(searchInput.text.ToLower().Trim());   // IMPORTANTE: limpiar caracteres ocultos del texto

    if (string.IsNullOrEmpty(searchInput.text)) {                       // Campo de busqueda vacio, mostramos todo
      filteredList = fullLevelList;
      refreshLevelViewer();
      return;
    }

    foreach (string level in fullLevelList) {                           // Metemos todo lo que empareja en la lista filtrada
      JObject json = JObject.Parse(level);
      string levelNameClean = json["environment"]["level_name"].ToString().ToLower().Trim();
      if (levelNameClean.Contains(cleanText)) filteredList.Add(level);
    }

    if (filteredList.Count == 0) {                                      // No hay nada que corresponde con la busqueda, mostramos nada
      cleanViewerSpace();
      return;
    }

    refreshLevelViewer();                                               // Actualizamos los niveles mostrados por pantalla
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  private string CleanInput(string input) {
    if (input == null) return "";

    // Remove common invisible Unicode characters
    string[] invisibleChars = {
      "\u200B", // Zero-width space
      "\u200C", // Zero-width non-joiner
      "\u200D", // Zero-width joiner
      "\u200E", // Left-to-right mark
      "\u200F", // Right-to-left mark
      "\uFEFF"  // Byte Order Mark (BOM)
    };

    foreach (string ch in invisibleChars) {
      input = input.Replace(ch, "");
    }

    return input.Trim();
  }

  /// <summary>
  /// 
  /// </summary>
  private void refreshLevelViewer() {
    cleanViewerSpace();
    List<string> levelList = (filteredList != null && filteredList.Count > 0) ? filteredList : fullLevelList;   // Si tenemos aplicado algun filtro, usamos la lista reducida 

    levelPanelParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, (100 * levelList.Count) + 20);
    
    for (int i = 0; i < levelList.Count; i++) {
      GameObject newObject = Instantiate(levelPanelPrefab);
      newObject.transform.SetParent(levelPanelParent.transform);
      newObject.transform.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
      newObject.transform.GetComponent<RectTransform>().localPosition = new Vector3(750f, -152.5f - (100f * i), 0f);

      JObject json = JObject.Parse(levelList[i]);

      newObject.transform.Find("Txt - Level Name").GetComponent<TextMeshProUGUI>().text = json["environment"]["level_name"].ToString();
      newObject.transform.Find("Txt - Creator Name").GetComponent<TextMeshProUGUI>().text = json["environment"]["user_name"].ToString();

      newObject.GetComponent<CustomLevelLoad>().setLevelCode(levelList[i]);
    }
  }

  /// <summary>
  /// 
  /// </summary>
  private void cleanViewerSpace() {
    foreach (Transform child in levelPanelParent.transform) {
      Destroy(child.gameObject);
    }
  }

  /// <summary>
  /// 
  /// </summary>
  public void ReturnToMenu() {
    SceneManager.LoadScene(1);
  }

}
