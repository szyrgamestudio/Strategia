using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Photon.Pun;

public class MapLoad : MonoBehaviour
{
    public Sprite[] obraz = new Sprite[66];
    public static Sprite[] obrazStatic = new Sprite[66];
    public string nazwa;// = "mmap1.txt";
    public GameObject kafelek;
    public bool wysokoscStart;
    public bool zlotoStart;
    public bool jednostkiStart;
    public bool enemyStart;

    public GameObject[] jednostki;
    public GameObject[] budyneki;

    public void LoadMapData()
    {
        Debug.Log("dziala");
        // Łączenie ścieżki pliku z katalogiem "Maps" i nazwą pliku "map1.txt"
        string filePath = Path.Combine(Application.dataPath, "Maps", nazwa);

        // Lista przechowująca dane mapy (listy liczb całkowitych)
        List<List<int>> tempMapData = new List<List<int>>();
        List<List<int>> tempMapHigh = new List<List<int>>();
        List<List<int>> tempMapGold = new List<List<int>>();
        List<List<int>> tempMapUnit = new List<List<int>>();
        List<List<int>> tempMapEnemy = new List<List<int>>();

        // Sprawdzenie, czy plik istnieje
        if (File.Exists(filePath))
        {
            // Utworzenie obiektu do odczytu danych z pliku
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Odczytanie i przypisanie szerokości planszy z pierwszej linii pliku
                Menu.BoardSizeX = int.Parse(reader.ReadLine().Trim());

                // Odczytanie i przypisanie wysokości planszy z drugiej linii pliku
                Menu.BoardSizeY = int.Parse(reader.ReadLine().Trim());

                // Odczytywanie kolejnych linii pliku
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Inicjalizacja listy na przechowywanie danych jednego wiersza
                    List<int> row = new List<int>();

                    // Podział odczytanej linii na poszczególne liczby
                    string[] values = line.Trim().Split(' ');

                    // Iteracja po wszystkich odczytanych wartościach
                    foreach (string value in values)
                    {
                        // Konwersja odczytanej wartości na liczbę całkowitą i dodanie do listy
                        row.Add(int.Parse(value));
                    }

                    // Wstawienie listy z danymi wiersza na początek listy mapy
                    // if(wysokoscStart && zlotoStart)
                    //     tempMapGold.Insert(0, row);
                    if (line == "7777")
                        break;
                    if (enemyStart)
                        tempMapEnemy.Add(row);
                    if (line == "01100101")
                        enemyStart = true;
                    if (jednostkiStart && !enemyStart)
                        tempMapUnit.Add(row);
                    if (line == "01101010")
                        jednostkiStart = true;
                    if (wysokoscStart && zlotoStart && !jednostkiStart)
                        tempMapGold.Insert(0, row);
                    if (line == "01111010")
                    {
                        zlotoStart = true;
                    }

                    if (wysokoscStart && !zlotoStart)
                        tempMapHigh.Insert(0, row);
                    if (line == "01110000")
                    {
                        wysokoscStart = true;
                    }
                    if (!wysokoscStart)
                        tempMapData.Insert(0, row);
                }
            } // Zakończenie bloku 'using', automatycznie zamyka obiekt StreamReader


            // Pozostała część kodu dla przypisania odwrotnie
            int[,] kafelekObraz = new int[tempMapData.Count, tempMapData[0].Count];
            int[,] kafelekWysokosc = new int[tempMapHigh.Count, tempMapHigh[0].Count];
            int[,] kafelekGold = new int[5000, 5];
            int[,] kafelekUnit = new int[20, 2];
            int[,] kafelekEnemy = new int[5000, 5];

            for (int i = 0; i < Menu.BoardSizeX - 1; i++)
            {
                for (int j = 0; j < Menu.BoardSizeY - 1; j++)
                {
                    kafelekObraz[j, i] = tempMapData[i][j];
                }
            }
            for (int i = 0; i < Menu.BoardSizeX - 1; i++)
            {
                for (int j = 0; j < Menu.BoardSizeY - 1; j++)
                {
                    kafelekWysokosc[j, i] = tempMapHigh[i][j];
                }
            }
            for (int l = 0; l < tempMapGold.Count; l++)
                for (int j = 0; j < 5; j++)
                {
                    kafelekGold[l, j] = tempMapGold[l][j];
                }
            for (int l = 0; l < tempMapUnit.Count; l++)
                for (int j = 0; j < 2; j++)
                {
                    kafelekUnit[l, j] = tempMapUnit[l][j];
                }
            for (int l = 0; l < tempMapEnemy.Count; l++)
                for (int j = 0; j < 5; j++)
                {
                    kafelekEnemy[l, j] = tempMapEnemy[l][j];
                }
            for (int x = 0; x < Menu.BoardSizeX; x++)
                for (int y = 0; y < Menu.BoardSizeY; y++)
                {
                    Vector3 TilePosition = new Vector3(x, y, 3);
                    GameObject newUnit = null;
                    if (MenuGlowne.multi)
                    {
                        newUnit = PhotonNetwork.Instantiate(kafelek.name, TilePosition, Quaternion.identity);
                        Debug.Log(x + " " + y);
                    }
                    else
                        newUnit = Instantiate(kafelek, TilePosition, Quaternion.identity);
                    newUnit.transform.SetParent(transform);
                    newUnit.name = x.ToString() + " " + y.ToString();
                    newUnit.GetComponent<SpriteRenderer>().sprite = obraz[kafelekObraz[x, y]];
                    Pole staty = newUnit.GetComponent<Pole>();
                    staty.poziom = kafelekWysokosc[x, y];
                    staty.spriteName = kafelekObraz[x, y];
                    if (kafelekObraz[x, y] == 0)
                    {
                        staty.las = true;
                        if (Random.Range(0, 2) == 0)
                            newUnit.GetComponent<SpriteRenderer>().sprite = obraz[31];
                    }
                    if (kafelekObraz[x, y] == 5)
                    {
                        if (Random.Range(0, 7) == 0)
                            newUnit.GetComponent<SpriteRenderer>().sprite = obraz[35];
                    }
                    if (kafelekObraz[x, y] == 32)
                    {
                        staty.las = true;
                        staty.Zajete = true;
                        if (Random.Range(0, 2) == 0)
                            newUnit.GetComponent<SpriteRenderer>().sprite = obraz[52];
                    }
                    if (kafelekObraz[x, y] == 10)
                        staty.magia = 1;
                    if (kafelekObraz[x, y] == 20)
                        staty.magia = 2;
                    if (kafelekObraz[x, y] == 30)
                        staty.magia = 3;
                    if (kafelekObraz[x, y] >= 48 && kafelekObraz[x, y] <= 56)
                    {
                        staty.las = true;
                        if (Random.Range(0, 2) == 0)
                            newUnit.GetComponent<SpriteRenderer>().sprite = obraz[kafelekObraz[x, y] + 9];
                    }


                    Menu.kafelki[x][y] = newUnit;
                }
            int k = 0;
            while (kafelekGold[k, 0] != 0)
            {
                int x = Random.Range(kafelekGold[k, 1], kafelekGold[k, 2] + 1);
                int y = Random.Range(kafelekGold[k, 3], kafelekGold[k, 4] + 1);
                Menu.kafelki[x][y].GetComponent<Pole>().zloto = kafelekGold[k, 0];
                k++;
            }
            for (int i = 0; i < 4; i++)
            {
                if (WyburRas.aktywny[i] == true)
                {
                    StartCoroutine(ludzik(0, kafelekUnit[0 + (i * 5), 0], kafelekUnit[0 + (i * 5), 1], 1 + i));
                    StartCoroutine(ludzik(0, kafelekUnit[1 + (i * 5), 0], kafelekUnit[1 + (i * 5), 1], 1 + i));
                    StartCoroutine(ludzik(7, kafelekUnit[2 + (i * 5), 0], kafelekUnit[2 + (i * 5), 1], 1 + i));
                    if (WyburRas.heros[i] == 0)
                    {
                        StartCoroutine(ludzik(1, kafelekUnit[3 + (i * 5), 0], kafelekUnit[3 + (i * 5), 1], 1 + i));
                        StartCoroutine(ludzik(9, kafelekUnit[4 + (i * 5), 0], kafelekUnit[4 + (i * 5), 1], 1 + i));
                    }
                    if (WyburRas.heros[i] == 1)
                    {
                        Menu.magia[1 + (i)] += 6;
                        StartCoroutine(ludzik(8, kafelekUnit[3 + (i * 5), 0], kafelekUnit[3 + (i * 5), 1], 1 + i));
                        StartCoroutine(ludzik(10, kafelekUnit[4 + (i * 5), 0], kafelekUnit[4 + (i * 5), 1], 1 + i));
                    }
                }
            }
            k = 0;
            while (kafelekEnemy[k, 0] != 0)
            {
                if (kafelekEnemy[k, 0] == 1)
                    StartCoroutine(ludzik(kafelekEnemy[k, 1], kafelekEnemy[k, 2], kafelekEnemy[k, 3], kafelekEnemy[k, 4]));
                if (kafelekEnemy[k, 0] == 2)
                    StartCoroutine(budynek(kafelekEnemy[k, 1], kafelekEnemy[k, 2], kafelekEnemy[k, 3], kafelekEnemy[k, 4]));
                k++;
            }
            // Przykład użycia wczytanych danych (możesz dostosować do swoich potrzeb)
            Debug.Log($"Wczytano dane z pliku. BoardSizeX: {Menu.BoardSizeX}, BoardSizeY: {Menu.BoardSizeY}");
        }
        else
        {
            Debug.LogError($"Plik {nazwa} w folderze Maps nie istnieje.");
        }
    }
    IEnumerator ludzik(int ip, float x, float y, int team)
    {
        if (team == 0 || WyburRas.aktywny[team - 1] == true)
        {
            yield return new WaitForSeconds(0.015f);
            GameObject nowy = null;
            if (MenuGlowne.multi)
                nowy = PhotonNetwork.Instantiate(jednostki[ip].name, new Vector3(x, y, -2f), Quaternion.identity);
            else
                nowy = Instantiate(jednostki[ip], new Vector3(x, y, -2f), Quaternion.identity);
            //PhotonView photonView = nowy.AddComponent<PhotonView>();
            nowy.GetComponent<Jednostka>().druzyna = team;
            Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().Zajete = true;
            Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().postac = nowy;
            if (ip == 10 || ip == 9)
                Menu.heros[team] = nowy;
            if (team == 0)
            {
                Menu.NPC.Add(nowy);
                nowy.GetComponent<Jednostka>().spanie = true;
            }
        }
    }
    IEnumerator budynek(int ip, float x, float y, int team)
    {
        if (WyburRas.aktywny[team - 1] == true)
        {
            yield return new WaitForSeconds(0.015f);
            GameObject nowy = Instantiate(budyneki[ip], new Vector3(x, y, -1.5f), Quaternion.identity);
            nowy.GetComponent<Budynek>().druzyna = team;
            nowy.GetComponent<Budynek>().punktyBudowy = nowy.GetComponent<Budynek>().punktyBudowyMax;
            nowy.GetComponent<BudynekRuch>().wybudowany = true;
            Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().Zajete = true;
            Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().postac = nowy;
            if (team == 4)
                nowy.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 90.0f);
            if (team == 2)
                nowy.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 180.0f);
            if (team == 3)
                nowy.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 270.0f);
        }
    }
    private string currentScene;

    void Start()
    {
        obrazStatic = obraz;
        // Pobierz początkową scenę
        currentScene = SceneManager.GetActiveScene().name;

        // Wyślij informacje o scenie do innych graczy
        GetComponent<PhotonView>().RPC("UpdateSceneInfo", RpcTarget.OthersBuffered, currentScene);
    }

    [PunRPC]
    void UpdateSceneInfo(string sceneName)
    {
        // Otrzymaj informacje o scenie od innego gracza
        currentScene = sceneName;

        // Zaktualizuj liczbę graczy na danej scenie na swoim graczu
        int playerCountOnScene = CountPlayersOnScene(currentScene);
        Debug.Log("Liczba graczy na scenie " + currentScene + ": " + playerCountOnScene + "    " + Ip.ip);
        if (playerCountOnScene == 2 && Ip.ip == 1)
            LoadMapData();
    }

    int CountPlayersOnScene(string sceneName)
    {
        // Tutaj możesz zaimplementować logikę, która zlicza graczy na danej scenie
        // Przykład: przejście przez PhotonNetwork.PlayerList i sprawdzenie ich aktualnej sceny
        int count = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.Log(PhotonNetwork.PlayerList);
            // Przykładowe założenie: każdy gracz ma zmienną przechowującą aktualną scenę
            count++;
        }
        return count;
    }
}
