using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem {

    public static readonly string SAVE_FOLDER = $"{Application.persistentDataPath}/Saves/";
    
    public static void Init() {
        //check if directory exists otherwise make it
        if(!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString, string fileName) {
        File.WriteAllText(SAVE_FOLDER + fileName + ".txt", saveString);
    }

    public static string Load(string fileName) {
        if(File.Exists(SAVE_FOLDER + fileName + ".txt")) {
            string saveString = File.ReadAllText(SAVE_FOLDER + fileName + ".txt");
            return saveString;
        } else return null;
    }

}
