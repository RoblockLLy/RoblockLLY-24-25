/**
* Universidad de La Laguna
* Proyecto: Roblockly
* Autor: Thomas Edward Bradley
* Email: alu0101408248@ull.edu.es
* Fecha: 28/06/2025
* Descripcion: Clases para la descarga de niveles personalizados del repositorio para los mismos
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region Clases Aux

[Serializable]
public class GitHubFileInfo {
  public string name;
}

[Serializable]
public class GitHubContentFile {
  public string content;
}

public static class JsonHelper {
  public static T[] GetJsonArray<T>(string json) {
    string newJson = "{ \"array\": " + json + "}";
    Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
    return wrapper.array;
  }

  [Serializable]
  private class Wrapper<T> {
    public T[] array;
  }
}

#endregion

public class GithubDownloader : MonoBehaviour {
  
  #region Atributos

  [Header("GitHub Settings")]
  [SerializeField] [Tooltip("Nombre del usuario propietario (u organización) que creo el repositorio a buscar")] 
  public string repoOwner = "your-username";
  [SerializeField] [Tooltip("Nombre del repositorio al que deseamos subir el nivel")]
  public string repoName = "your-repo";
  [SerializeField] [Tooltip("Dirección dentro del repositorio del que queremos pillar los ficheros .json")]
  public string levelsFolder = "Levels";
  [SerializeField] [Tooltip("Rama del repositorio de donde pillaremos los niveles")]
  public string branch = "main";

  [Header("Authentication")]
  [SerializeField] [Tooltip("Objeto que contiene el token encriptado y las funciones para desencriptarlo")]
  public TokenEncryptor encriptador;

  [Header("Level Viewer")]
  [SerializeField] [Tooltip("Manager para el browser de niveles, recibira los niveles cargados")]
  public LevelViewer levelViewer;

  [Header("Loading Panel")]
  [SerializeField] [Tooltip("Panel de carga, activa mientras corre la corutina")]
  public GameObject loadingPanel;

  /// <summary>
  /// Array que contendra los contenidos de cada fichero .json
  /// </summary>
  private List<string> levelJsonFiles;

  #endregion

  private void Start() {
    DownloadAllLevels();
  }

  #region Lectura Repo

  /// <summary>
  /// Comienza la corutina para descargar todos los niveles del repositorio
  /// </summary>
  public void DownloadAllLevels() {
    StartCoroutine(DownloadLevelsCoroutine());
  }

  /// <summary>
  /// Corutina encargado de descargar todos los niveles personalizados de nuestro repositorio GitHub
  /// </summary>
  private IEnumerator DownloadLevelsCoroutine() {
    loadingPanel.SetActive(true);
    string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/contents/{levelsFolder}?ref={branch}";

    UnityWebRequest request = UnityWebRequest.Get(url);
    request.SetRequestHeader("User-Agent", "UnityDownloader");
    if (!string.IsNullOrEmpty(encriptador.DecryptString())) {
      request.SetRequestHeader("Authorization", "Bearer " + encriptador.DecryptString());
    }

    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success) {
      Debug.LogError("Error al pillar la lista de niveles: " + request.error);
      loadingPanel.SetActive(false);
      yield break;
    }

    var jsonArray = JsonHelper.GetJsonArray<GitHubFileInfo>(FixJsonArray(request.downloadHandler.text));
    var jsonFiles = new List<string>();

    foreach (var file in jsonArray) {
      if (file.name.EndsWith(".json")) {
        yield return StartCoroutine(DownloadFileContent(file.name, jsonFiles));
      }
    }

    levelJsonFiles = jsonFiles;
    levelViewer.setLevels(levelJsonFiles);
    loadingPanel.SetActive(false);
    Debug.Log($"Se han descargado exitosamente {levelJsonFiles.Count} niveles de GitHub");
  }

  /// <summary>
  /// Corutina encargado de leer el contenido de un fichero especificado
  /// </summary>
  /// <param name="fileName">Nombre del fichero a leer</param>
  /// <param name="jsonFiles">Lista con el contenido de todos los niveles, se actualiza al final de la corutina</param>
  private IEnumerator DownloadFileContent(string fileName, List<string> jsonFiles) {
    string fileApiUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}/contents/{levelsFolder}/{fileName}?ref={branch}";

    UnityWebRequest fileRequest = UnityWebRequest.Get(fileApiUrl);
    fileRequest.SetRequestHeader("User-Agent", "UnityDownloader");
    if (!string.IsNullOrEmpty(encriptador.DecryptString())) {
      fileRequest.SetRequestHeader("Authorization", "Bearer " + encriptador.DecryptString());
    }

    yield return fileRequest.SendWebRequest();

    if (fileRequest.result != UnityWebRequest.Result.Success) {
      Debug.LogWarning($"Mo se ha podido descargar el fichero: {fileName}, Error: {fileRequest.error}");
      yield break;
    }

    try {
      string json = fileRequest.downloadHandler.text;
      var fileInfo = JsonUtility.FromJson<GitHubContentFile>(json);
      string decodedContent = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(fileInfo.content));
      jsonFiles.Add(decodedContent);
    } catch (Exception e) {
      Debug.LogWarning($"Error al decifrar el fichero {fileName}: {e.Message}");
    }
  }

  #endregion

  #region Metodos Aux

  /// <summary>
  /// Asegura que un json puede ser parseado como un json array
  /// </summary>
  /// <param name="rawJsonArray">string json de entrada</param>
  /// <returns>string json procesado, array vacio si se ha encontrado algun problema</returns>
  private string FixJsonArray(string rawJsonArray) => rawJsonArray.TrimStart().StartsWith("[") ? rawJsonArray : "[]";

  #endregion

}
