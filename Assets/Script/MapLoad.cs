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
    [SerializeField]
    public static string nazwa;// = "mmap1.txt";
    public GameObject kafelek;
    public GameObject[][] enemyList;
    public GameObject kafelekFake;
    public bool wysokoscStart;
    public bool zlotoStart;
    public bool jednostkiStart;
    public bool enemyStart;

    public bool itemStart;

    public GameObject[] jednostki;
    public GameObject[] budyneki;

    public List<GameObject> enemyLatwy;
    public List<GameObject> enemySredni;
    public List<GameObject> enemyTrudny;
    public List<GameObject> enemyLata;
    public List<GameObject> enemyStrzela;
    public List<GameObject> enemyStrzelaTrudny;
    public GameObject boss;


    [PunRPC]
    public void manaZwieksz(int ip, int i)
    {
        if (Ip.ip != ip)
            Menu.magia[1 + (i)] += 6;
    }

    public GameObject RandomEnemy(int poziom)
    {
        switch (poziom)
        {
            case 1:
                return enemyLatwy[Random.Range(0, enemyLatwy.Count)];
            case 2:
                return enemySredni[Random.Range(0, enemySredni.Count)];
            case 3:
                return enemyTrudny[Random.Range(0, enemyTrudny.Count)];
            case 6:
                return enemyLata[Random.Range(0, enemyLata.Count)];
            case 4:
                return enemyStrzela[Random.Range(0, enemyStrzela.Count)];
            case 5:
                return enemyStrzelaTrudny[Random.Range(0, enemyStrzelaTrudny.Count)];
            default:
                return null;
        }
    }
    public static void rozdziel(string nazwa)
    {
        if (nazwa.EndsWith(";"))
        {
            nazwa = nazwa.TrimEnd(';');
        }

        string[] tablica = nazwa.Split(';');

        if (tablica[0] == "0")
        {
            End.pvp = false;
        }
        else
        {
            End.pvp = true;
        }

        if (int.TryParse(tablica[1], out int tureKontroli) && tureKontroli != 0)
        {
            End.control = true;
            End.tureKontroli = tureKontroli;
        }
        else
        {
            End.control = false;
        }
        if (int.TryParse(tablica[2], out int poziomRatusza) && poziomRatusza != 0)
        {
            End.economy = true;
            End.poziomRatusza = poziomRatusza;
        }
        else
        {
            End.economy = false;
        }
        if (tablica[3] != "0")
        {
            End.boss = true;
            string[] dane = tablica[3].Split(',');
            int.TryParse(dane[0], out int x);
            int.TryParse(dane[1], out int y);
            int.TryParse(dane[2], out int z);
            End.bossPosition = new Vector3((float)x, (float)y, 0);
            End.tureDoKonca = z;
        }
        else
        {
            End.boss = false;
        }
    }

    [PunRPC]
    public void updateAktywny(bool[] aktywne)
    {
        WyburRas.aktywny = aktywne;
    }

    public void LoadMapData()
    {
        // Łączenie ścieżki pliku z katalogiem "Maps" i nazwą pliku "map1.txt"
        string filePath = Path.Combine(Application.dataPath, "StreamingAssets/Maps", nazwa);

        // Lista przechowująca dane mapy (listy liczb całkowitych)
        List<List<int>> tempMapData = new List<List<int>>();
        List<List<int>> tempMapHigh = new List<List<int>>();
        List<List<int>> tempMapGold = new List<List<int>>();
        List<List<int>> tempMapUnit = new List<List<int>>();
        List<List<int>> tempMapEnemy = new List<List<int>>();
        List<List<int>> tempMapItem = new List<List<int>>();

        // Sprawdzenie, czy plik istnieje
        if (File.Exists(filePath))
        {
            // Utworzenie obiektu do odczytu danych z pliku
            using (StreamReader reader = new StreamReader(filePath))
            {
                string firstLine = reader.ReadLine();
                //rozdziel(firstLine);
                // Odczytanie i przypisanie szerokości planszy z pierwszej linii pliku
                Menu.BoardSizeX = int.Parse(reader.ReadLine().Trim());

                // Odczytanie i przypisanie wysokości planszy z drugiej linii pliku
                Menu.BoardSizeY = int.Parse(reader.ReadLine().Trim());


                // Odczytywanie kolejnych linii pliku
                char piatyOdKoncaZnak = nazwa[nazwa.Length - 6];
                int.TryParse(piatyOdKoncaZnak.ToString(), out int maxGraczy);
                End.maxGraczy = maxGraczy;
                if (Menu.IloscGraczy > End.maxGraczy)
                    Menu.IloscGraczy = End.maxGraczy;
                for (int i = 0; i < 4; i++)
                {
                    if (i >= End.maxGraczy)
                        WyburRas.aktywny[i] = false;

                }
                if (MenuGlowne.multi)
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("updateAktywny", RpcTarget.All, WyburRas.aktywny);
                }
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
                    if (line == "7777")
                        break;
                    if (itemStart)
                        tempMapItem.Add(row);
                    if (line == "01111011")
                        itemStart = true;
                    if (enemyStart && !itemStart)
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


            int[,] kafelekObraz = new int[100, 100];
            int[,] kafelekWysokosc = new int[100, 100];
            int[,] kafelekGold = new int[5000, 5];
            int[,] kafelekUnit = new int[20, 2];
            int[,] kafelekEnemy = new int[5000, 5];
            int[,] kafelekItem = new int[5000, 5];

            for (int i = 0; i < Menu.BoardSizeY; i++)
            {
                for (int j = 0; j < Menu.BoardSizeX; j++)
                {
                    kafelekObraz[j, i] = tempMapData[i][j];
                }
            }
            for (int i = 0; i < Menu.BoardSizeY; i++)
            {
                for (int j = 0; j < Menu.BoardSizeX; j++)
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
            for (int l = 0; l < tempMapItem.Count; l++)
                for (int j = 0; j < 3; j++)
                {
                    kafelekItem[l, j] = tempMapItem[l][j];
                }

            for (int x = 0; x < Menu.BoardSizeX; x++)
                for (int y = 0; y < Menu.BoardSizeY; y++)
                {
                    Vector3 TilePosition = new Vector3(x, y, 3);
                    GameObject newUnit = null;
                    if (MenuGlowne.multi)
                    {
                        newUnit = PhotonNetwork.Instantiate(kafelek.name, TilePosition, Quaternion.identity);
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
                        {
                            newUnit.GetComponent<SpriteRenderer>().sprite = obraz[31];
                            newUnit.GetComponent<Pole>().spriteName = 31;
                        }
                    }
                    if (kafelekObraz[x, y] == 5)
                    {
                        if (Random.Range(0, 7) == 0)
                        {
                            newUnit.GetComponent<SpriteRenderer>().sprite = obraz[35];
                            newUnit.GetComponent<Pole>().spriteName = 35;
                        }
                    }
                    if (kafelekObraz[x, y] == 32)
                    {
                        staty.las = true;
                        staty.Zajete = true;
                        if (Random.Range(0, 2) == 0)
                        {
                            newUnit.GetComponent<SpriteRenderer>().sprite = obraz[52];
                            newUnit.GetComponent<Pole>().spriteName = 52;
                        }
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
                        {
                            newUnit.GetComponent<SpriteRenderer>().sprite = obraz[kafelekObraz[x, y] + 9];
                            newUnit.GetComponent<Pole>().spriteName += 9;
                        }
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
            for (int i = 0; i < End.maxGraczy; i++)
            {
                if (WyburRas.aktywny[i] == true)
                {
                    StartCoroutine(ludzik(0 + 11 * WyburRas.rasa[i], kafelekUnit[0 + (i * 5), 0], kafelekUnit[0 + (i * 5), 1], 1 + i));
                    StartCoroutine(ludzik(0 + 11 * WyburRas.rasa[i], kafelekUnit[1 + (i * 5), 0], kafelekUnit[1 + (i * 5), 1], 1 + i));
                    StartCoroutine(ludzik(7 + 11 * WyburRas.rasa[i], kafelekUnit[2 + (i * 5), 0], kafelekUnit[2 + (i * 5), 1], 1 + i));
                    if (WyburRas.heros[i] == 0)
                    {
                        StartCoroutine(ludzik(1 + 11 * WyburRas.rasa[i], kafelekUnit[3 + (i * 5), 0], kafelekUnit[3 + (i * 5), 1], 1 + i));
                        StartCoroutine(ludzik(9 + 11 * WyburRas.rasa[i], kafelekUnit[4 + (i * 5), 0], kafelekUnit[4 + (i * 5), 1], 1 + i));
                    }
                    if (WyburRas.heros[i] == 1)
                    {
                        Menu.magia[1 + (i)] += 6;
                        if (MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("manaZwieksz", RpcTarget.All, Ip.ip, i);
                        }
                        StartCoroutine(ludzik(8 + 11 * WyburRas.rasa[i], kafelekUnit[3 + (i * 5), 0], kafelekUnit[3 + (i * 5), 1], 1 + i));
                        StartCoroutine(ludzik(10 + 11 * WyburRas.rasa[i], kafelekUnit[4 + (i * 5), 0], kafelekUnit[4 + (i * 5), 1], 1 + i));
                    }
                }
                else//TUTTAJ KOMBIJNUJE
                {
                    if (End.maxGraczy >= i + 1)
                        StartCoroutine(ludzik(3, kafelekUnit[4 + (i * 5), 0], kafelekUnit[4 + (i * 5), 1], 0));
                }
            }
            k = 0;
            while (kafelekEnemy[k, 0] != 0)
            {
                if (kafelekEnemy[k, 0] == 1)
                    StartCoroutine(ludzik(RandomEnemy(kafelekEnemy[k, 1]), kafelekEnemy[k, 2], kafelekEnemy[k, 3], kafelekEnemy[k, 4]));
                if (kafelekEnemy[k, 0] == 2)
                    StartCoroutine(budynek(kafelekEnemy[k, 1], kafelekEnemy[k, 2], kafelekEnemy[k, 3], kafelekEnemy[k, 4]));
                k++;
            }
            k = 0;
            while (kafelekItem[k, 0] != 0)
            {
                PoleFind pole = Menu.kafelki[kafelekItem[k, 1]][kafelekItem[k, 2]].GetComponent<PoleFind>();
                if (MenuGlowne.multi)
                {
                    pole.updateMultiWywolaj(kafelekItem[k, 0], 0);
                }
                else
                {
                    pole.rodzaj = kafelekItem[k, 0];
                    pole.Start();
                }
                k++;
            }
            for (int i = -1; i < Menu.BoardSizeX; i++)
            {
                GameObject newUnit = null;


                Vector3 TilePosition = new Vector3(i, -1, 3);
                if (MenuGlowne.multi)
                    newUnit = PhotonNetwork.Instantiate(kafelekFake.name, TilePosition, Quaternion.identity);
                else
                    newUnit = Instantiate(kafelekFake, TilePosition, Quaternion.identity);
                newUnit.name = i.ToString() + "  -1";
                TilePosition = new Vector3(i, Menu.BoardSizeY + 1, 3);
                if (MenuGlowne.multi)
                    newUnit = PhotonNetwork.Instantiate(kafelekFake.name, TilePosition, Quaternion.identity);
                else
                    newUnit = Instantiate(kafelekFake, TilePosition, Quaternion.identity);
                newUnit.name = Menu.BoardSizeY + 1.ToString() + "  " + Menu.BoardSizeX + 1.ToString();
                TilePosition = new Vector3(i, Menu.BoardSizeY, 3);
                if (MenuGlowne.multi)
                    newUnit = PhotonNetwork.Instantiate(kafelekFake.name, TilePosition, Quaternion.identity);
                else
                    newUnit = Instantiate(kafelekFake, TilePosition, Quaternion.identity);
                newUnit.name = Menu.BoardSizeY.ToString() + "  " + Menu.BoardSizeX.ToString();
            }
            for (int i = -1; i < Menu.BoardSizeY + 2; i++)
            {
                GameObject newUnit = null;


                Vector3 TilePosition = new Vector3(-1, i, 3);
                if (MenuGlowne.multi)
                    newUnit = PhotonNetwork.Instantiate(kafelekFake.name, TilePosition, Quaternion.identity);
                else
                    newUnit = Instantiate(kafelekFake, TilePosition, Quaternion.identity);
                newUnit.name = "-1 " + i.ToString();
                TilePosition = new Vector3(Menu.BoardSizeX, i, 3);
                if (MenuGlowne.multi)
                    newUnit = PhotonNetwork.Instantiate(kafelekFake.name, TilePosition, Quaternion.identity);
                else
                    newUnit = Instantiate(kafelekFake, TilePosition, Quaternion.identity);
                newUnit.name = Menu.BoardSizeX.ToString() + " " + Menu.BoardSizeY.ToString();
            }
            if (End.boss)
            {
                StartCoroutine(ludzik(boss, End.bossPosition.x, End.bossPosition.y, 0));
            }
            // Przykład użycia wczytanych danych (możesz dostosować do swoich potrzeb)
            Debug.Log($"Wczytano dane z pliku. BoardSizeX: {Menu.BoardSizeX}, BoardSizeY: {Menu.BoardSizeY}");
        }
        else
        {
            Debug.LogError($"Plik {nazwa} w folderze Maps nie istnieje.");
        }
    }
    IEnumerator ludzik(GameObject id, float x, float y, int team)
    {
        if (team == 0 || WyburRas.aktywny[team - 1] == true)
        {
            yield return new WaitForSeconds(0.015f);
            GameObject nowy = null;
            if (MenuGlowne.multi)
            {
                nowy = PhotonNetwork.Instantiate(id.name, new Vector3(x, y, -2f), Quaternion.identity);
            }
            else
                nowy = Instantiate(id, new Vector3(x, y, -2f), Quaternion.identity);
            //PhotonView photonView = nowy.AddComponent<PhotonView>();
            nowy.GetComponent<Jednostka>().druzyna = team;
            if (nowy.GetComponent<Jednostka>().lata)
                Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().ZajeteLot = true;
            else
                Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().Zajete = true;
            Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().postac = nowy;
            // if (ip == 10 || ip == 9)
            // {
            //     Menu.heros[team] = nowy;
            // }
            if (team == 0)
            {
                Menu.NPC.Add(nowy);
                nowy.GetComponent<Jednostka>().spanie = true;
            }
            if (MenuGlowne.multi)
            {
                nowy.GetComponent<Jednostka>().Aktualizuj();
                if (Ip.ip != 1)
                    nowy.GetComponent<Jednostka>().Start();
                nowy.GetComponent<Jednostka>().rozlozenie();
            }
        }
    }
    IEnumerator ludzik(int ip, float x, float y, int team)
    {
        StartCoroutine(ludzik(jednostki[ip], x, y, team));
        yield return null;
    }

    IEnumerator budynek(int ip, float x, float y, int team)
    {
        if (WyburRas.aktywny[team - 1] == true)
        {
            yield return new WaitForSeconds(0.015f);
            GameObject nowy = null;
            if (MenuGlowne.multi)
            {
                nowy = PhotonNetwork.Instantiate(budyneki[ip + 11 * WyburRas.rasa[team - 1]].name, new Vector3(x, y, -2f), Quaternion.identity);
            }
            else
                nowy = Instantiate(budyneki[ip + 11 * WyburRas.rasa[team - 1]], new Vector3(x, y, -2f), Quaternion.identity);
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

            if (MenuGlowne.multi)
            {
                nowy.GetComponent<Budynek>().Aktualizuj();
                nowy.GetComponent<Budynek>().Start();
                nowy.GetComponent<BudynekRuch>().startMultiMap();
            }
        }
    }
    private string currentScene;

    void Start()
    {
        obrazStatic = obraz;
        // Pobierz początkową scenę
        currentScene = SceneManager.GetActiveScene().name;

        // Wyślij informacje o scenie do innych graczy

        if (MenuGlowne.multi)
            GetComponent<PhotonView>().RPC("UpdateSceneInfo", RpcTarget.OthersBuffered, currentScene);
        if (!End.boss)
            End.tureDoKonca = 0;

        string selectedMap = nazwa;
        selectedMap = selectedMap.Replace(".txt", "");

        if (DiscordManager.Instance != null)
        {
            DiscordManager.Instance.Details = "W trakcie rozgrywki";
            DiscordManager.Instance.State = "Mapa: " + selectedMap;
            Debug.Log("Updated Discord state");
        }
        else
        {
            Debug.LogError("DiscordManager instance is null.");
        }

    }

    [PunRPC]
    void UpdateSceneInfo(string sceneName)
    {
        // Otrzymaj informacje o scenie od innego gracza
        currentScene = sceneName;

        // Zaktualizuj liczbę graczy na danej scenie na swoim graczu
        int playerCountOnScene = CountPlayersOnScene(currentScene);
        if (Ip.ip == 1)
            LoadMapData();
    }

    int CountPlayersOnScene(string sceneName)
    {
        // Tutaj możesz zaimplementować logikę, która zlicza graczy na danej scenie
        // Przykład: przejście przez PhotonNetwork.PlayerList i sprawdzenie ich aktualnej sceny
        int count = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            // Przykładowe założenie: każdy gracz ma zmienną przechowującą aktualną scenę
            count++;
        }
        return count;
    }
}
