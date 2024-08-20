using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using System;

public class MapCheck : MonoBehaviour
{
    public Dropdown mapDropdown;
    private string mapsFolderPath;
    private List<string> mapFiles;
    public Text mapNameText;
    public GameObject[] przyciski;
    public Text opisText;
    public static string opis;
    PhotonView photonView;

    public static bool save;
    public Image mapyButton;
    public Image saveButton;

    public GameObject usun;

    public void usunMape()
    {
        string mapsFolderPath = Path.Combine(Application.streamingAssetsPath, "Save");
        
        // Ustawienie ścieżki do pliku, który ma być usunięty
         string filePath = Path.Combine(mapsFolderPath,  MapLoad.nazwa);

          if (File.Exists(filePath))
        {
            try
            {
                // Usunięcie pliku
                File.Delete(filePath);
                
                Debug.Log("Plik został usunięty: " + filePath);
            }
            catch (Exception e)
            {
                Debug.LogError("Błąd podczas usuwania pliku: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Plik nie istnieje: " + filePath);
        }

        LoadMapFiles();
        PopulateDropdown();
        if(!MenuGlowne.multi || Ip.ip == 1)
            SelectFirstItem();
        mapDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(mapDropdown); });
    }

    public void saveChanges(bool zmiana)
    {
        // if(save != false)
        // {
            save = zmiana;
            if(save)
            {
                usun.SetActive(true);
                saveButton.color = Color.grey;
                mapyButton.color = Color.white;
                mapsFolderPath = Path.Combine(Application.streamingAssetsPath, "Save");
                LoadMapFiles();
                PopulateDropdown();
                if(!MenuGlowne.multi || Ip.ip == 1)
                    SelectFirstItem();
                mapDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(mapDropdown); });
            }
            else
            {
                usun.SetActive(false);
                saveButton.color = Color.white;
                mapyButton.color = Color.grey;
                mapsFolderPath = Path.Combine(Application.streamingAssetsPath, "Maps");
                LoadMapFiles();
                PopulateDropdown();
                if(!MenuGlowne.multi || Ip.ip == 1)
                    SelectFirstItem();
                mapDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(mapDropdown); });
            }
        //}
    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        mapsFolderPath = Path.Combine(Application.streamingAssetsPath, "Maps");
        LoadMapFiles();
        PopulateDropdown();
        if(!MenuGlowne.multi || Ip.ip == 1)
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
                przyciskiControl();
                mapDropdown.gameObject.SetActive(false);
                
                if(MapLoad.nazwa != null)
                    mapNameText.text = "Mapa: " + (MapLoad.nazwa.Substring(0, MapLoad.nazwa.Length - 4));
                if(opis != null)
                    opisText.text = "Opis:\n" + opis;
                
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
    string LoadUntil7777(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            bool found7777 = false;
            string opis = string.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                if (found7777)
                {
                    opis += line + "\n";
                }
                else if (line == "7777")
                {
                    found7777 = true;
                }
            }

            return opis;
        }
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        string selectedMap = mapFiles[index];
        MapLoad.nazwa = selectedMap + ".txt";
        mapNameText.text = "Mapa: " + selectedMap; // Update the map name text
        Debug.Log("Selected map: " + selectedMap);

        string filePath;
        if(save)
            filePath = Path.Combine(Application.streamingAssetsPath, "Save", MapLoad.nazwa);
        else
            filePath = Path.Combine(Application.streamingAssetsPath, "Maps", MapLoad.nazwa);
        string firstLine = LoadFirstLine(filePath);
        opis = LoadUntil7777(filePath);

        //niech wczyta drugą linie i przypisze ją do wartości Menu.BoardSizeX
        string secondLine = LoadSecondLine(filePath);
        string trzeciaLine = LoadTrzeciaLine(filePath);
        Menu.BoardSizeX = int.Parse(secondLine);
        Menu.BoardSizeY = int.Parse(trzeciaLine);
        opisText.text = "Opis:\n" + opis;
        MapLoad.rozdziel(firstLine);
        przyciskiControl();
        
        if (MenuGlowne.multi)
        {
            if(End.end != null)
                End.end.updateStats();
            photonView.RPC("updateMulti", RpcTarget.All, selectedMap, opis, Menu.BoardSizeX, Menu.BoardSizeY);
        }
    }

    string LoadFirstLine(string filePath)
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return reader.ReadLine();
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }
    }

    private string LoadSecondLine(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Skip the first line
                return reader.ReadLine(); // Read the second line
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading second line: " + ex.Message);
            return string.Empty;
        }
    }

    private string LoadTrzeciaLine(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Skip the first line
                reader.ReadLine(); // Skip the first line
                return reader.ReadLine(); // Read the second line
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading second line: " + ex.Message);
            return string.Empty;
        }
    }

    public void pvp()
    {
        if(!MenuGlowne.multi || Ip.ip == 1)
            End.pvp = !End.pvp;
        if (MenuGlowne.multi && Ip.ip == 1)
        {
            End.end.updateStats();
            photonView.RPC("updateTogler", RpcTarget.All, Ip.ip);
        }
    }
    public void control()
    {
        if(!MenuGlowne.multi || Ip.ip == 1)
            End.control = !End.control;
        if (MenuGlowne.multi && Ip.ip == 1)
        {
            End.end.updateStats();
            photonView.RPC("updateTogler", RpcTarget.All, Ip.ip);
        }
    }
    public void economy()
    {
        if(!MenuGlowne.multi || Ip.ip == 1)
            End.economy =!End.economy;
        if (MenuGlowne.multi && Ip.ip == 1)
        {
            End.end.updateStats();
            photonView.RPC("updateTogler", RpcTarget.All, Ip.ip);
        }
    }
    public void boss()
    {
        if(!MenuGlowne.multi || Ip.ip == 1)
            End.boss =!End.boss;
        if (MenuGlowne.multi && Ip.ip == 1)
        {
            End.end.updateStats();
            photonView.RPC("updateTogler", RpcTarget.All, Ip.ip);
        }
    }
    public void przyciskiControl()
    {
        if (End.pvp)
        {
            przyciski[0].SetActive(true);
            przyciski[0].GetComponent<Toggle>().isOn = true;
        }
        else
        {
            przyciski[0].SetActive(false);
        }

        if (End.control)
        {
            przyciski[1].SetActive(true);
            przyciski[1].GetComponent<Toggle>().isOn = true;
        }
        else
        {
            przyciski[1].SetActive(false);
        }

        if (End.economy)
        {
            przyciski[2].SetActive(true);
            przyciski[2].GetComponent<Toggle>().isOn = true;
        }
        else
        {
            przyciski[2].SetActive(false);
        }

        if (End.boss)
        {
            przyciski[3].SetActive(true);
            przyciski[3].GetComponent<Toggle>().isOn = true;
        }
        else
        {
            przyciski[3].SetActive(false);
        }
    }

    [PunRPC]
    void updateTogler(int ip)
    {
        if(ip != Ip.ip )
        {
            przyciski[0].GetComponent<Toggle>().isOn = End.pvp;
            przyciski[1].GetComponent<Toggle>().isOn = End.control;
            przyciski[2].GetComponent<Toggle>().isOn = End.economy;
            przyciski[3].GetComponent<Toggle>().isOn = End.boss;
        }
    }

    [PunRPC]
    void updateMulti(string name, string opisOld, int BoardSizeX, int BoardSizeY)
    {
        opis = opisOld;
        opisText.text = "Opis:\n" + opis;
        MapLoad.nazwa = name + ".txt";
        Menu.BoardSizeX = BoardSizeX;
        Menu.BoardSizeY = BoardSizeY;
        przyciskiControl();
    }
}
