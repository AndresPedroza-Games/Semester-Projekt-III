using UnityEngine;
using System.Collections.Generic;

public class TutorialRoomManager : MonoBehaviour
{
    [SerializeField] private List<Light> lights = new List<Light>();
    [SerializeField] private int minDistance = 3;

    private GameObject player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerTest>().gameObject;
    }

    private void Update()
    {
        foreach (Light lightSource in lights)
        {
            if (DistanceFromPlayer(lightSource.gameObject.transform.position) < minDistance)
                TurnOnLight(true, lightSource);
            else
                TurnOnLight(false, lightSource);
        }
    }

    private void TurnOnLight(bool status, Light lightSource)
    {
        lightSource.enabled = status;
    }

    private float DistanceFromPlayer(Vector3 lightSourcePos)
    {
        float distance = Vector3.Distance(player.transform.position, lightSourcePos);

        return distance;
    }
}
