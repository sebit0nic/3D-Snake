using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour {

    //private SaveData object loaded at the start of the script?
    //methods to only update certain values?
    //raywenderlich.com/418-how-to-save-and-load-a-game-in-unity

    public void SaveData() {
        //TODO: save all values needed in SavedData (parameters?)
    }

    public SavedData LoadData() {
        if ( File.Exists(Application.persistentDataPath + "/gamesave.save") ) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            SavedData savedData = (SavedData) bf.Deserialize(file);
            file.Close();
            return savedData;
        } else {
            Debug.Log("No game saved!");
            //TODO: return default values
            return null;
        }
    }
}
