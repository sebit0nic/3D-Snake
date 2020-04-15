using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Handles persistent saving and loading of all relevant data.
/// </summary>
public class SaveLoadManager : MonoBehaviour {

    [SerializeField]
    public List<HatObject> standardHatObjects = new List<HatObject>();
    public List<ColorObject> standardColorObjects = new List<ColorObject>();
    public List<PowerupObject> standardPowerupObjects = new List<PowerupObject>();

    private const string gameSavePath = "/gamesave.save";
    private const string soundStatusKey = "SoundStatus";
    private const string cameraStatusKey = "CameraStatus";
    private const string tutorialStatusKey = "TutorialStatus";
    private const string lastRewardTimeKey = "LastRewardTime";
    private const string defaultLastRewardTime = "01.01.2000 00:00:00";

    /// <summary>
    /// Save data persistently on device.
    /// </summary>
    public void SaveData( SavedData savedData ) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create( Application.persistentDataPath + gameSavePath );
        bf.Serialize( file, savedData );
        file.Close();
    }

    /// <summary>
    /// Load data from device or create new data if no save file exists yet.
    /// </summary>
    public SavedData LoadData() {
        if( File.Exists( Application.persistentDataPath + gameSavePath ) ) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open( Application.persistentDataPath + gameSavePath, FileMode.Open );
            SavedData savedData = (SavedData) bf.Deserialize( file );
            file.Close();
            return savedData;
        } else {
            SavedData savedData = new SavedData( standardHatObjects, standardColorObjects, standardPowerupObjects );
            SaveData( savedData );
            return savedData;
        }
    }

    public int GetSoundStatus() {
        return PlayerPrefs.GetInt( soundStatusKey, (int) SoundStatus.SOUND_ON );
    }

    public void SetSoundStatus( int value ) {
        PlayerPrefs.SetInt( soundStatusKey, value );
    }

    public int GetCameraStatus() {
        return PlayerPrefs.GetInt( cameraStatusKey, (int) CameraStatus.CAMERA_NO_ROTATION );
    }

    public void SetCameraStatus( int value ) {
        PlayerPrefs.SetInt( cameraStatusKey, value );
    }

    public bool GetTutorialStatus() {
        return PlayerPrefs.GetInt( tutorialStatusKey, (int) TutorialStatus.TUTORIAL_OPEN ) != 0;
    }

    public void SetTutorialStatus( int value ) {
        PlayerPrefs.SetInt( tutorialStatusKey, value );
    }

    public System.DateTime GetLastRewardTime() {
        return System.DateTime.Parse( PlayerPrefs.GetString( lastRewardTimeKey, defaultLastRewardTime ) );
    }

    public void SetLastRewardTime( string lastRewardTime ) {
        PlayerPrefs.SetString( lastRewardTimeKey, lastRewardTime );
    }
}
