using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;

public class MapCheck : MonoBehaviour
{
    public Dropdown mapDropdown;
    public string mapsFolderPath = "Assets/Maps";
    private List<string> mapFiles;
    public Text mapNameText;

    void Start()
    {
        LoadMapFiles();
        PopulateDropdown();
        SelectFirstItem();
        mapDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(mapDropdown); });
        
    }

    void Update()
    {
        if(MenuGlowne.multi)
        {
            if(Ip.ip == 1)
                mapNameText.text = " ";
            else
            {
                mapDropdown.gameObject.SetActive(false);
                if(MapLoad.nazwa.Length > 0)
                    mapNameText.text = "Mapa: " + (MapLoad.nazwa.Substring(0, MapLoad.nazwa.Length - 4));
            }
        }
        else
            mapNameText.text = " ";
    }
    void LoadMapFiles()
    {
        mapFiles = new List<string>();
        string[] files = Directory.GetFiles(mapsFolderPath, "*.txt"); // Assuming the map files have .txt extension

        foreach (string file in files)
        {
            mapFiles.Add(Path.GetFileNameWithoutExtension(file));
        }
    }

    void PopulateDropdown()
    {
        List<Dropdown.OptionData> dropdownOptions = new List<Dropdown.OptionData>();

        foreach (string mapFile in mapFiles)
        {
            dropdownOptions.Add(new Dropdown.OptionData(mapFile));
        }

        mapDropdown.AddOptions(dropdownOptions);
    }

     void SelectFirstItem()
    {
        if (mapFiles.Count > 0)
        {
            mapDropdown.value = 0; // Select the first item
            DropdownItemSelected(mapDropdown); // Trigger the selection change
        }
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        string selectedMap = mapFiles[index];
        MapLoad.nazwa = selectedMap + ".txt";
        mapNameText.text = "Mapa: " + selectedMap; // Update the map name text
        Debug.Log("Selected map: " + selectedMap);
        if(MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("updateMulti", RpcTarget.All, selectedMap);
        }
    }

    [PunRPC]
    void updateMulti(string name)
    {
        MapLoad.nazwa = name + ".txt";
    }
}
