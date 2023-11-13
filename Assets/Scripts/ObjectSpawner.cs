using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] float spawnArea_height = 1f;
    [SerializeField] float spawnArea_width = 1f;

    [SerializeField] GameObject[] spawn;
    int lenght;
    [SerializeField] float probability = 0.1f;
    [SerializeField] int spawnCount = 1;
    [SerializeField] int objectSpawnLimit = -1;

    [SerializeField] bool oneTime = false;

    List<SpawnedObject> spawnedObjects;
    [SerializeField] JSONStringList targetSaveJSONList;
    [SerializeField] int idInList = -1;

    private void Start()
    {
        lenght = spawn.Length;

        if (oneTime == false)
        {
            TimeAgent timeAgent = GetComponent<TimeAgent>();
            timeAgent.onTimeTick += Spawn;
            spawnedObjects = new List<SpawnedObject>();

            LoadData();
        }
        else
        {
            Spawn(null);
            //Destroy(gameObject);
        }
    }

    void Spawn(DayTimeController dayTimeController)
    {
        if (Random.value > probability) { return; }
        if(spawnedObjects != null)
        {
            if (objectSpawnLimit <= spawnedObjects.Count && objectSpawnLimit != -1) { return; }
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int id = Random.Range(0, lenght);
            GameObject go = Instantiate(spawn[id]);
            Transform t = go.transform;

            t.SetParent(transform);

            if (oneTime == false)
            {
                SpawnedObject spawnedObject = go.AddComponent<SpawnedObject>();
                spawnedObjects.Add(t.GetComponent<SpawnedObject>());
                spawnedObject.objId = id;
            }

            Vector3 position = transform.position;
            position.x += UnityEngine.Random.Range(-spawnArea_width, spawnArea_width);
            position.y += UnityEngine.Random.Range(-spawnArea_height, spawnArea_height);

            t.position = position;
        }
    }

    public void SpawnedObjectDestroyed(SpawnedObject spawnedObject)
    {
        spawnedObjects.Remove(spawnedObject);
    }

    public class ToSave
    {
        public List<SpawnedObject.SaveSpawnedObjectData> spawnedObjectDatas;

        public ToSave()
        {
            spawnedObjectDatas = new List<SpawnedObject.SaveSpawnedObjectData>();
        }
    }

    string Read()
    {
        ToSave toSave = new ToSave();

        for(int i =0; i < spawnedObjects.Count; i++) 
        {
            toSave.spawnedObjectDatas.Add(new SpawnedObject.SaveSpawnedObjectData(spawnedObjects[i].objId, spawnedObjects[i].transform.position));
        }

        return JsonUtility.ToJson(toSave);
    }

    public void Load(string json)
    {
        if(json == "" || json == "{}" || json == null) { return; }

        ToSave toLoad = JsonUtility.FromJson<ToSave>(json);

        for(int i=0; i < toLoad.spawnedObjectDatas.Count; i++)
        {
            SpawnedObject.SaveSpawnedObjectData data = toLoad.spawnedObjectDatas[i];
            GameObject go = Instantiate(spawn[data.objectId]);
            go.transform.position = data.worldPosition;
            go.transform.SetParent(transform);
            SpawnedObject so = go.AddComponent<SpawnedObject>();
            so.objId = data.objectId;
            spawnedObjects.Add(so);
        }
    }

    private void OnDestroy()
    {
        if (oneTime == true) { return; }
        SaveData();
    }

    private void SaveData()
    {
        if (CheckJSON() == false) { return; }

        string jsonString = Read();
        targetSaveJSONList.SetString(jsonString, idInList);
    }

    private void LoadData()
    {
        if(CheckJSON() == false) { return; }

        Load(targetSaveJSONList.GetString(idInList));
    }

    private bool CheckJSON()
    {
        if (oneTime == true) { return false; }
        if (targetSaveJSONList == null) { Debug.LogError("target json scriptable object to save data on spawnable is null"); return false; }
        if (idInList == -1) { Debug.LogError("id in list assigned data can't be saved"); return false; }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea_width * 2, spawnArea_height * 2));
    }
}
