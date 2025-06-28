using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

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

  /// <summary>
  /// Array que contendra los contenidos de cada fichero .json
  /// </summary>
  private List<string> levelJsonFiles;

  #endregion

  private void Start() {
    DownloadAllLevels();
  }

  public void DownloadAllLevels() {
    StartCoroutine(DownloadLevelsCoroutine());
  }

  private IEnumerator DownloadLevelsCoroutine() {
    string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/contents/{levelsFolder}?ref={branch}";

    UnityWebRequest request = UnityWebRequest.Get(url);
    request.SetRequestHeader("Authorization", "Bearer " + encriptador.DecryptString());
    request.SetRequestHeader("User-Agent", "UnityDownloader");

    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success) {
      Debug.LogError("Failed to fetch file list: " + request.error);
      yield break;
    }

    // Parse file list
    var jsonArray = JsonHelper.GetJsonArray<GitHubFileInfo>(FixJsonArray(request.downloadHandler.text));
    var jsonFiles = new List<string>();

    foreach (var file in jsonArray) {
      // Debug.Log(file.name);
      if (file.name.EndsWith(".json")) {
        yield return StartCoroutine(DownloadFileContent(file.download_url, jsonFiles));
      }
    }

    levelJsonFiles = jsonFiles;
    levelViewer.setLevels(levelJsonFiles);
    Debug.Log($"✅ {levelJsonFiles.Count} archivos .json descargados desde {levelsFolder}/");
  }

  private IEnumerator DownloadFileContent(string fileUrl, List<string> jsonFiles) {
    UnityWebRequest fileRequest = UnityWebRequest.Get(fileUrl);
    fileRequest.SetRequestHeader("Authorization", "Bearer " + encriptador.DecryptString());
    fileRequest.SetRequestHeader("User-Agent", "UnityDownloader");

    yield return fileRequest.SendWebRequest();

    if (fileRequest.result == UnityWebRequest.Result.Success) {
      jsonFiles.Add(fileRequest.downloadHandler.text);
    } else {
      Debug.LogWarning($"⚠️ Falló la descarga de: {fileUrl}");
    }
  }

  [Serializable]
  public class GitHubFileInfo {
    public string name;
    public string download_url;
  }

  // Helper to parse JSON arrays (GitHub returns them without a wrapper)
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

  // GitHub returns raw JSON arrays. JsonUtility needs a wrapped object.
  private string FixJsonArray(string rawJsonArray) => rawJsonArray.TrimStart().StartsWith("[") ? rawJsonArray : "[]";

}
