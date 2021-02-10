using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;     //gives acces to binary formatter
using System.IO;                                          // lets us use File. that gives us the option to let the script check a binary file and delete/create it

[System.Serializable]                        // we need to serialize our filestream and profile to save.
public class SavedProfile    //what we want to save.
{
    public float s_wood;         //resources
    public float s_stones;
    public float s_food;

    public List<BuildingInfo> buildingsSaveData = new List<BuildingInfo>();  // we want to save the ID, level, yRot en connected gridelement for the buildings
    //script in building


}

public class Save : MonoBehaviour
{

    public SavedProfile profile;

    private Resources resources;
    private Buildings buildings;
    private Build build;

    
    void Awake()
    {
        build = FindObjectOfType<Build>();
        resources = FindObjectOfType<Resources>();
        buildings = FindObjectOfType<Buildings>();

        //LoadGame();

    }

    private void SaveGame()
    {
        if(profile == null)
        {
            profile = new SavedProfile();
        }

        profile.s_wood = resources.wood;                                      //connects the resources script with the floats i made in savedProfile class
        profile.s_stones = resources.stones;
        profile.s_food = resources.food;

        foreach(GameObject g in buildings.builtObjects)                       //gets all build buildings and stores them in my list, same goes for their levels.
        {
            BuildingInfo b = g.GetComponent<Building>().info;                 // converts ID, level, yRot en connected gridelement for the buildings to my list
            profile.buildingsSaveData.Add(b);
        }

        BinaryFormatter bf = new BinaryFormatter();                           //syrializes/desyrializes an object/list of objects

        string path = Application.persistentDataPath + "/save.dat";          // path where we save and the name of the savefile : "/save.dat". persistantdatapath is the access to our game file

        if (File.Exists(path))                                               // deletes old save file after re saving.
            File.Delete(path);

        FileStream fs = File.Open(path, FileMode.OpenOrCreate);              // filestream lets us make changes in a external script. because we deleted the (old scripts) we make a new one here.
        bf.Serialize(fs, profile);                                           // serialize filestream and our object is profile. 

        fs.Close();                                                          // close the filestream because we dont want errors
        
    }
    
    private void LoadGame()
    {
        string pathToLoad = Application.persistentDataPath + "/save.dat";             //reference naar gemaakte save data

        if (!File.Exists(pathToLoad))                                                 // no save file means no loading possible
        {
            Debug.Log("No saved profile found! Have you saved yet?");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();                                  // new reference binary formatter
        FileStream fs = File.Open(pathToLoad, FileMode.Open);                        // search for made save file and open it
        SavedProfile loadedProfile = bf.Deserialize(fs) as SavedProfile;             // deserialize filestream as a  save profile
        fs.Close();                                                                  // we dont want errors so sclose filestream

        resources.wood = loadedProfile.s_wood;                                       // tell what we want from the made binary code / resources
        resources.stones = loadedProfile.s_stones;
        resources.food = loadedProfile.s_food;
         
       for (int i = 0; i < loadedProfile.buildingsSaveData.Count; i++)              // tell that we want the building info from the binary script
        {
            BuildingInfo buildingFromSave = loadedProfile.buildingsSaveData[i];     // were givin the save info we saved
            build.RebuildBuilding(buildingFromSave.id, buildingFromSave.connectedGridId, (int)buildingFromSave.level, buildingFromSave.yRot); // refer our build script method (rebuildfromsave)
            Debug.Log(buildingFromSave.id); 
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))          // lets us save
        {
            SaveGame();
            Debug.Log("Game Saved!");
        }

        if (Input.GetKeyDown(KeyCode.L))          // lets us load
        {
            LoadGame();
        }
    }
}

