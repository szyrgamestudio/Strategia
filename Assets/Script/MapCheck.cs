using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;

public class MapCheck : MonoBehaviour
{
    public Dropdown mapDropdown;
    private string mapsFolderPath;
    private List<string> mapFiles;
    public Text mapNameText;

    void Start()
    {
        // Ustawienie œcie¿ki do folderu Maps w StreamingAssets
        mapsFolderPath = Path.Combine(Application.streamingAssetsPath, "Maps");
        LoadMapFiles();
        PopulateDropdown();
        SelectFirstItem();
        mapDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(mapDropdown); });
    }

    void Update()
    {
        if (MenuGlowne.multi)
        {
            if (Ip.ip == 1)
            {
                mapNameText.text = " ";
            }
            else
            {
                mapDropdown.gameObject.SetActive(false);
                if (MapLoad.nazwa.Length > 0)
                {
                    mapNameText.text = "Mapa: " + (MapLoad.nazwa.Substring(0, MapLoad.nazwa.Length - 4));
                }
            }
        }
        else
        {
            mapNameText.text = " ";
        }
    }

    void LoadMapFiles()
    {
        mapFiles = new List<string>();

        if (Directory.Exists(mapsFolderPath))
        {
            string[] files = Directory.GetFiles(mapsFolderPath, "*.txt"); // Assuming the map files have .txt extension

            foreach (string file in files)
            {
                mapFiles.Add(Path.GetFileNameWithoutExtension(file));
            }
        }
        else
        {
            Debug.LogError("Maps folder not found at path: " + mapsFolderPath);
        }
    }

    void PopulateDropdown()
    {
        List<Dropdown.OptionData> dropdownOptions = new List<Dropdown.OptionData>();

        foreach (string mapFile in mapFiles)
        {
            dropdownOptions.Add(new Dropdown.OptionData(mapFile));
        }

        mapDropdown.ClearOptions();
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

        if (MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (photonView != null)
            {
                photonView.RPC("updateMulti", RpcTarget.All, selectedMap);
            }
            else
            {
                Debug.LogError("PhotonView component not found on this GameObject.");
            }
        }
    }

    [PunRPC]
    void updateMulti(string name)
    {
        MapLoad.nazwa = name + ".txt";
    }
}
