using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour {

    [SerializeField]
    public List<HatObject> standardHatObjects = new List<HatObject>();
    public List<ColorObject> standardColorObjects = new List<ColorObject>();
    public List<PowerupObject> standardPowerupObjects = new List<PowerupObject>();

    public void SaveData(SavedData savedData) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, savedData);
        file.Close();
        Debug.Log("Game saved!");
    }

    public SavedData LoadData() {
        if ( File.Exists(Application.persistentDataPath + "/gamesave.save") ) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            SavedData savedData = (SavedData) bf.Deserialize(file);
            file.Close();
            Debug.Log("Game loaded!");
            return savedData;
        } else {
            Debug.Log("No game saved! Creating file with standard values...");
            SavedData savedData = new SavedData(standardHatObjects, standardColorObjects, standardPowerupObjects);
            SaveData(savedData);
            return savedData;
        }
    }
}
