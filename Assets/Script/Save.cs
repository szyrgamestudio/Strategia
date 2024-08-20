using System.IO;
using UnityEngine;
using System;
using Photon.Pun;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Save : MonoBehaviour
{
    public static Save zapis;
    public PhotonView photonView;


    // Lista do przechowywania otrzymanych informacji od graczy
    private List<int> receivedIPs = new List<int>();

    // TaskCompletionSource do sygnalizowania, że wszyscy gracze dostarczyli dane
    private TaskCompletionSource<bool> allPlayersReady = new TaskCompletionSource<bool>();


    void Awake()
    {
        zapis = GetComponent<Save>();
        photonView = this.GetComponent<PhotonView>();
    }

    [PunRPC]
    public void zasoby1()
    {
        photonView.RPC("zasoby2", RpcTarget.All, Ip.ip, Menu.zloto[Ip.ip], Menu.drewno[Ip.ip], Menu.magia[Ip.ip], Budowlaniec.punktyBudowyBonus[Ip.ip],
        Kuznia.update1[Ip.ip], Kuznia.update2[Ip.ip], Kuznia.update3[Ip.ip], Kuznia.update4[Ip.ip], Kuznia.update5[Ip.ip],
        Biblioteka.update1[Ip.ip], Biblioteka.update2[Ip.ip], Biblioteka.update3[Ip.ip], Biblioteka.update4[Ip.ip], Biblioteka.update5[Ip.ip], 
        Kbiblioteka.update1[Ip.ip], Kbiblioteka.update2[Ip.ip], Kbiblioteka.update3[Ip.ip], Kbiblioteka.update4[Ip.ip], Kbiblioteka.update5[Ip.ip], 
        Ebiblioteka.update1[Ip.ip], Ebiblioteka.update2[Ip.ip], Ebiblioteka.update3[Ip.ip], Ebiblioteka.update4[Ip.ip]);
    }

  [PunRPC]
    public void zasoby2(
    int ip, 
    int zloto, 
    int drewno, 
    int magia, 
    int punktyBudowyBonus,
    int kuzniaUpdate1, int kuzniaUpdate2, int kuzniaUpdate3, int kuzniaUpdate4, int kuzniaUpdate5,
    int bibliotekaUpdate1, int bibliotekaUpdate2, int bibliotekaUpdate3, int bibliotekaUpdate4, int bibliotekaUpdate5,
    int kbibliotekaUpdate1, int kbibliotekaUpdate2, int kbibliotekaUpdate3, int kbibliotekaUpdate4, int kbibliotekaUpdate5,
    int ebibliotekaUpdate1, int ebibliotekaUpdate2, int ebibliotekaUpdate3, int ebibliotekaUpdate4)
{
    // Przypisywanie zasobów
    Menu.zloto[ip] = zloto;
    Menu.drewno[ip] = drewno;
    Menu.magia[ip] = magia;
    Budowlaniec.punktyBudowyBonus[ip] = punktyBudowyBonus;

    // Przypisywanie wartości dla Kuznia
    Kuznia.update1[ip] = kuzniaUpdate1;
    Kuznia.update2[ip] = kuzniaUpdate2;
    Kuznia.update3[ip] = kuzniaUpdate3;
    Kuznia.update4[ip] = kuzniaUpdate4;
    Kuznia.update5[ip] = kuzniaUpdate5;

    // Przypisywanie wartości dla Biblioteka
    Biblioteka.update1[ip] = bibliotekaUpdate1;
    Biblioteka.update2[ip] = bibliotekaUpdate2;
    Biblioteka.update3[ip] = bibliotekaUpdate3;
    Biblioteka.update4[ip] = bibliotekaUpdate4;
    Biblioteka.update5[ip] = bibliotekaUpdate5;

    // Przypisywanie wartości dla Kbiblioteka
    Kbiblioteka.update1[ip] = kbibliotekaUpdate1;
    Kbiblioteka.update2[ip] = kbibliotekaUpdate2;
    Kbiblioteka.update3[ip] = kbibliotekaUpdate3;
    Kbiblioteka.update4[ip] = kbibliotekaUpdate4;
    Kbiblioteka.update5[ip] = kbibliotekaUpdate5;

    // Przypisywanie wartości dla Ebiblioteka
    Ebiblioteka.update1[ip] = ebibliotekaUpdate1;
    Ebiblioteka.update2[ip] = ebibliotekaUpdate2;
    Ebiblioteka.update3[ip] = ebibliotekaUpdate3;
    Ebiblioteka.update4[ip] = ebibliotekaUpdate4;

        if (!receivedIPs.Contains(ip))
        {
            receivedIPs.Add(ip);
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(i + " " + Menu.zloto[i]);
        }

        // Sprawdzenie, czy wszyscy gracze podali swoje informacje
        if (receivedIPs.Count >= Menu.IloscGraczy)
        {
            allPlayersReady.SetResult(true);  // Ustawienie TaskCompletionSource na zakończony
        }
    }



    public async void save()
    {
         if(MenuGlowne.multi)
            {
                photonView.RPC("zasoby1", RpcTarget.All);
                await allPlayersReady.Task;
                Debug.Log("Wszyscy gracze zapisali dane");
            }
        // Ustal ścieżkę do pliku MapLoad.nazwa
        string mapLoadPath = Path.Combine(Application.streamingAssetsPath, "Maps" , MapLoad.nazwa);

        // Ustal ścieżkę do pliku autosave.txt
        string autosavePath = Path.Combine(Application.streamingAssetsPath, "Save", MapLoad.nazwa);

        // Sprawdź, czy plik MapLoad.nazwa istnieje
        if (!File.Exists(mapLoadPath))
        {
            Debug.LogError("Plik " + mapLoadPath + " nie istnieje!");
            return;
        }

        // Otwórz plik MapLoad.nazwa do odczytu
        using (StreamReader reader = new StreamReader(mapLoadPath))
        {
            // Otwórz (lub stwórz) plik autosave.txt do zapisu
            using (StreamWriter writer = new StreamWriter(autosavePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if(line == "0")
                    {
                        if(Menu.tura != 0)
                            writer.WriteLine(Menu.tura);
                        else
                            writer.WriteLine("1");
                       
                        for(int i = 1; i <= 4; i++)
                            writer.WriteLine(Menu.zloto[i] + " " + Menu.drewno[i] + " " + Menu.magia[i]);
                        for(int i = 1; i <= 4; i++)
                            writer.Write(Budowlaniec.punktyBudowyBonus[i] + " ");
                        writer.WriteLine();
                        for(int i = 1; i <= 4; i++)
                        {
                            writer.Write(Kuznia.update1[i] + " " + Kuznia.update2[i] + " " + Kuznia.update3[i] + " " +Kuznia.update4[i] + " " +Kuznia.update5[i] + " ");
                            writer.Write(Biblioteka.update1[i] + " " + Biblioteka.update2[i] + " " + Biblioteka.update3[i] + " " +Biblioteka.update4[i] + " " +Biblioteka.update5[i] + " ");
                            writer.Write(Kbiblioteka.update1[i] + " " + Kbiblioteka.update2[i] + " " + Kbiblioteka.update3[i] + " " +Kbiblioteka.update4[i] + " " +Kbiblioteka.update5[i] + " ");
                            writer.Write(Ebiblioteka.update1[i] + " " + Ebiblioteka.update2[i] + " " + Ebiblioteka.update3[i] + " " +Ebiblioteka.update4[i]);
                            writer.WriteLine();
                        }
                    }
                    else
                        writer.WriteLine(line);
                    if (line == "01111010")
                    {
                        break;
                    }
                }
                for(int x = 0; x<Menu.BoardSizeX; x++)
                    for(int y = 0; y<Menu.BoardSizeY; y++)
                    {
                        if(Menu.kafelki[x][y].GetComponent<Pole>().zloto != 0)
                        {
                            writer.WriteLine(Menu.kafelki[x][y].GetComponent<Pole>().zloto + " " + x + " " + x + " "+ y + " "+ y);
                        }
                    }
                writer.WriteLine("01101010");
                for(int x = 0; x<Menu.BoardSizeX; x++)
                    for(int y = 0; y<Menu.BoardSizeY; y++)
                    {
                        if(Menu.kafelki[x][y].GetComponent<Pole>().magia == 2)
                            writer.WriteLine(x + " " + y);
                        if(Menu.kafelki[x][y].GetComponent<Pole>().trudnosc == 1)
                            writer.WriteLine(x + " " + y);

                    }
                writer.WriteLine("01100101");
                for(int x = 0; x<Menu.BoardSizeX; x++)
                    for(int y = 0; y<Menu.BoardSizeY; y++)
                    {
                        var postac = Menu.kafelki[x][y].GetComponent<Pole>().postac;

                        if (postac != null)
        
                        {
                            Jednostka jednostka = postac.GetComponent<Jednostka>();
                            Budynek budynek = postac.GetComponent<Budynek>();
                            int exp = 0;

                            if(postac.GetComponent<Heros>())
                            {
                                int level = postac.GetComponent<Heros>().level;
                                while(level != 1)
                                {
                                    exp += exp + 20;
                                    level--;
                                }
                                exp += postac.GetComponent<Heros>().exp;
                            }
                            if(jednostka)
                                writer.WriteLine("1 "  +  jednostka.typ + " " + x + " "+ y + " "+ jednostka.druzyna + " " + (int)jednostka.HP + " " + exp + " 0");
                            else
                                if(postac.GetComponent<Ratusz>())
                                    writer.WriteLine("2 "  + budynek.typ + " " + x + " "+ y + " " + budynek.druzyna + " " + (int)budynek.HP + " " + budynek.punktyBudowy + " " + (postac.GetComponent<Ratusz>().poziom-1));
                                else if(postac.GetComponent<nRatusz>())
                                    writer.WriteLine("2 "  + budynek.typ + " " + x + " "+ y + " " + budynek.druzyna + " " + (int)budynek.HP + " " + budynek.punktyBudowy + " " + (postac.GetComponent<nRatusz>().poziom-1));
                                else if(postac.GetComponent<KRatusz>())
                                    writer.WriteLine("2 "  + budynek.typ + " " + x + " "+ y + " " + budynek.druzyna + " " + (int)budynek.HP + " " + budynek.punktyBudowy + " " + (postac.GetComponent<KRatusz>().poziom-1));
                                else if(postac.GetComponent<eRatusz>())
                                    writer.WriteLine("2 "  + budynek.typ + " " + x + " "+ y + " " + budynek.druzyna + " " + (int)budynek.HP + " " + budynek.punktyBudowy + " " + (postac.GetComponent<eRatusz>().poziom-1));
                                else if(postac.GetComponent<Kopalnia>())
                                {
                                    int kopalnia = 0;
                                    for(int i = 0; i < 6;i++)
                                        if(postac.GetComponent<Kopalnia>().slot[i] != null)
                                            kopalnia++;
                                    
                                    writer.WriteLine("2 "  + budynek.typ + " " + x + " "+ y + " " + budynek.druzyna + " " + (int)budynek.HP + " " + budynek.punktyBudowy + " " + kopalnia);
                                }
                                else
                                    writer.WriteLine("2 "  + budynek.typ + " " + x + " "+ y + " " + budynek.druzyna + " " + (int)budynek.HP + " " + budynek.punktyBudowy + " -2");

                        }
                    }
                writer.WriteLine("01111011");
                for(int x = 0; x<Menu.BoardSizeX; x++)
                    for(int y = 0; y<Menu.BoardSizeY; y++)
                    {
                        if (Menu.kafelki[x][y].GetComponent<PoleFind>().rodzaj != 0)
                        {
                            writer.WriteLine(Menu.kafelki[x][y].GetComponent<PoleFind>().rodzaj  + " " + x + " "+ y);
                        }
                    }
                writer.WriteLine("7777");
                DateTime now = DateTime.Now;
                string dateTimeString = "Zapis z dnia " + now.ToString("yyyy-MM-dd HH:mm:ss");
                writer.WriteLine(dateTimeString);
            }
        }

        Debug.Log("Dane zostały zapisane do " + autosavePath);
    }
}
