/**
* Universidad de La Laguna
* Proyecto: Roblockly
* Autor: Edwin Plasencia Hern�ndez, Thomas Edward Bradley
* Email: alu0101329888@ull.edu.es
* Fecha: 13/05/2024, 01/07/2025
* Descripci�n: RobotCameraHandleRotator: Script simple para rotar la c�mara con el rat�n sobre el robot
*/

using UnityEngine;

public class RobotCameraHandleRotator : MonoBehaviour {
  public float sensitivity = 100.0f;
  private float verticalRotation = 0f; // Store vertical (pitch) rotation in degrees
  public float upperLimit = 30f;
  public float lowerLimit = -80f;
  public GameObject yTransformHandler;

  /// <summary>
  /// Se obtiene la posicion en el eje 'x' e 'y' del raton y se rota la camara correspondientemente
  /// </summary>
  void Update() {
    if (Input.GetMouseButton(0)) {
      float rotationX = Input.GetAxis("Mouse X") * Mathf.Deg2Rad * sensitivity;
      transform.Rotate(0, rotationX, 0);

      float rotationY = Input.GetAxis("Mouse Y") * Mathf.Deg2Rad * sensitivity;
      verticalRotation -= rotationY;
      verticalRotation = Mathf.Clamp(verticalRotation, lowerLimit, upperLimit);
      
      // Apply pitch rotation with clamping
      yTransformHandler.transform.localEulerAngles = new Vector3(verticalRotation, 0, 0);
    }
  }
}
