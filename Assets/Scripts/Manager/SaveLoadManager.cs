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

    private const string gameSavePath = "/gamesave.save";
    private const string soundStatusKey = "SoundStatus";
    private const string cameraStatusKey = "CameraStatus";

    public void SaveData(SavedData savedData) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + gameSavePath);
        bf.Serialize(file, savedData);
        file.Close();
    }

    public SavedData LoadData() {
        if ( File.Exists(Application.persistentDataPath + gameSavePath) ) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + gameSavePath, FileMode.Open);
            SavedData savedData = (SavedData) bf.Deserialize(file);
            file.Close();
            return savedData;
        } else {
            Debug.Log("No game saved! Creating file with standard values...");
            SavedData savedData = new SavedData(standardHatObjects, standardColorObjects, standardPowerupObjects);
            SaveData(savedData);
            return savedData;
        }
    }

    public int GetSoundStatus() {
        return PlayerPrefs.GetInt(soundStatusKey, (int)SoundStatus.SOUND_ON);
    }

    public void SetSoundStatus(int value) {
        PlayerPrefs.SetInt(soundStatusKey, value);
    }

    public int GetCameraStatus() {
        return PlayerPrefs.GetInt(cameraStatusKey, (int)CameraStatus.CAMERA_NO_ROTATION);
    }

    public void SetCameraStatus(int value) {
        PlayerPrefs.SetInt(cameraStatusKey, value);
    }
}
