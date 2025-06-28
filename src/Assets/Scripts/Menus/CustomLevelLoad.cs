using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomLevelLoad : MonoBehaviour {
  public string levelCode = "";

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
      
  }

  // Update is called once per frame
  void Update()
  {
      
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
