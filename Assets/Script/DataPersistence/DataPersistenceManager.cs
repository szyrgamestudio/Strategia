// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DataPersistenceManager : MonoBehaviour
// {
//     public GameData gameData;

//     public static DataPersistenceManager instance {get; private set;}

//     private void Awake()
//     {
//         if(instance != null)
//         {
//             Debug.Log("wiecej niz 1");
//         }
//         instance = this;
//     }
//     public void NewGame()
//     {
//         this.gameData = new GameData();
//     }
//     public void LoadGame()
//     {
//         if(this.gameData == null)
//         {
//             Debug.Log("dyntka");
//         }
//         Menu.zloto[1] = data.zloto;
//     }
//     public void SaveGame()
//     {
//         data.zloto = Menu.zloto[1];
//     }
// }
