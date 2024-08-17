using System.IO;
using UnityEngine;

public class Save : MonoBehaviour
{
    public void save()
    {
        // Ustal ścieżkę do pliku MapLoad.nazwa
        string mapLoadPath = Path.Combine(Application.streamingAssetsPath, "Maps" , MapLoad.nazwa);

        // Ustal ścieżkę do pliku autosave.txt
        string autosavePath = Path.Combine(Application.streamingAssetsPath, "Save", "autosave(" + Menu.IloscGraczyStart + ").txt");

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
                writer.WriteLine("01100101");
                for(int x = 0; x<Menu.BoardSizeX; x++)
                    for(int y = 0; y<Menu.BoardSizeY; y++)
                    {
                        var postac = Menu.kafelki[x][y].GetComponent<Pole>().postac;

                        if (postac != null)
        
                        {
                            Jednostka jednostka = postac.GetComponent<Jednostka>();
                            Budynek budynek = postac.GetComponent<Budynek>();
                            if(jednostka)
                                writer.WriteLine("1 "  +  jednostka.typ + " " + x + " "+ y + " "+ jednostka.druzyna );
                            else
                                writer.WriteLine("2 "  + budynek.typ + " " + x + " "+ y + " "+ budynek.druzyna);

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
            }
        }

        Debug.Log("Dane zostały zapisane do " + autosavePath);
    }
}
