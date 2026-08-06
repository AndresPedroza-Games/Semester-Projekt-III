using UnityEngine;

public class CameraLock : MonoBehaviour
{
    private void Awake()
    {
        LockInteractor.cameraLock = gameObject;
        gameObject.SetActive(false);
    }
}
