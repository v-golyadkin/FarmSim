using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(TimeAgent))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Item toSpawn;
    [SerializeField] int count;

    [SerializeField] float spread = 2f;

    [SerializeField] float probability = 0.5f;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += Spawn;
    }

    void Spawn(DayTimeController dayTimeController)
    {
        if (UnityEngine.Random.value < probability)
        {
            Vector3 _position = transform.position;
            _position.x += spread * UnityEngine.Random.value - spread / 2;
            _position.y += spread * UnityEngine.Random.value - spread / 2;

            ItemSpawnManager.instance.SpawnItem(_position, toSpawn, count);
        }
    }
}
