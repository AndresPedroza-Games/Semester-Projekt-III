using UnityEngine;
using System.Collections.Generic;

public class ToiletDetector : MonoBehaviour
{
    public static ToiletDetector Instance;

    private List<GameObject> _DetectedObjects = new List<GameObject>();

    private Vector3 _ScissorsStartPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _ScissorsStartPos = FindFirstObjectByType<Scissors>().gameObject.transform.position;
    }

    public void Flush()
    {
        if (_DetectedObjects.Count <= 0)
            return;

        foreach (GameObject obj in _DetectedObjects)
        {
            IFlushable isFlushable = obj.GetComponent<IFlushable>();

            isFlushable.Flush();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Scissors>() != null)
            other.transform.position = _ScissorsStartPos;

        if (Toilet._IsOpen && other.GetComponent<IFlushable>() != null && other.gameObject.GetComponent<Scissors>() == null)
        {
            _DetectedObjects.Add(other.gameObject);

            Debug.Log($"Add: {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Toilet._IsOpen)
        {
            _DetectedObjects.Remove(other.gameObject);

            Debug.Log($"Remove: {other.gameObject.name}");
        }
    }
}
