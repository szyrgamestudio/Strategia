using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Szaman : MonoBehaviour
{
    public GameObject jednostka;
    public bool odrzut;
    public Texture2D customCursorBudowa;

    public int silaWiatru;
    public int silaWiatru2;
    // Start is called before the first frame update
    void Update()
    { 
        if(jednostka.GetComponent<Heros>().levelUp)
        {
            jednostka.GetComponent<Heros>().levelUp=false;
            levelUp(jednostka.GetComponent<Heros>().level);
        }
        
        if(jednostka == Jednostka.Select)
        {
            if(Przycisk.jednostka[0] == true && jednostka.GetComponent<Jednostka>().akcja)
            {
                Przycisk.jednostka[0] = false;
                Jednostka.wybieranie = true;
                Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                odrzut = true;
            }
            if(Przycisk.jednostka[1] == true && jednostka.GetComponent<Jednostka>().akcja)
            {
                Przycisk.jednostka[1] = false;
                List<GameObject> lista = new List<GameObject>();
                przeszukanie(0,jednostka,lista);
                foreach(GameObject ludek in lista)
                {
                    for(int i = 0; i <= silaWiatru2; i++)
                    {
                    Vector3 offset = ludek.transform.position - jednostka.transform.position;
                    int x = (int)ludek.transform.position.x;
                    int y = (int)ludek.transform.position.y;
                    
                    if(offset.x < 0 && Menu.istnieje(x-1, y) && Menu.kafelki[x-1][y].GetComponent<Pole>().postac == null)
                    {
                        MoveJednostka(x, y, x-1, y, ludek);
                    }
                    else if(offset.x > 0 && Menu.istnieje(x+1, y) && Menu.kafelki[x+1][y].GetComponent<Pole>().postac == null)
                    {
                        MoveJednostka(x, y, x+1, y, ludek);
                    }

                    x = (int)ludek.transform.position.x;
                    y = (int)ludek.transform.position.y;

                    if(offset.y < 0 && Menu.istnieje(x, y-1) && Menu.kafelki[x][y-1].GetComponent<Pole>().postac == null)
                    {
                        MoveJednostka(x, y, x, y-1, ludek);
                    }
                    else if(offset.y < 0 && Menu.istnieje(x, y+1) && Menu.kafelki[x][y+1].GetComponent<Pole>().postac == null)
                    {
                        MoveJednostka(x, y, x, y+1, ludek);
                    }

                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("odrzutMulti", RpcTarget.All, Ip.ip, ludek.GetComponent<Jednostka>().nr_jednostki, ludek.GetComponent<Jednostka>().druzyna, ludek.transform.position);
                    }
                    

                    Menu.usunSelect2();
                    }
                    ludek.GetComponent<Jednostka>().HP --;
                    if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("dmg", RpcTarget.All,Ip.ip, ludek.GetComponent<Jednostka>().nr_jednostki, 1f ,ludek.GetComponent<Jednostka>().druzyna);
                        }
                }
            }

            if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 4 && odrzut)
            {
                odrzut = false;

                for(int i = 0 ; i <= silaWiatru ; i++)
                {

                Vector3 offset = Jednostka.Select2.transform.position - jednostka.transform.position;
                int x = (int)Jednostka.Select2.transform.position.x;
                int y = (int)Jednostka.Select2.transform.position.y;
                
                if(offset.x < 0 && Menu.istnieje(x-1, y) && Menu.kafelki[x-1][y].GetComponent<Pole>().postac == null)
                {
                    MoveJednostka(x, y, x-1, y, Jednostka.Select2);
                }
                else if(offset.x > 0 && Menu.istnieje(x+1, y) && Menu.kafelki[x+1][y].GetComponent<Pole>().postac == null)
                {
                    MoveJednostka(x, y, x+1, y, Jednostka.Select2);
                }

                x = (int)Jednostka.Select2.transform.position.x;
                y = (int)Jednostka.Select2.transform.position.y;

                if(offset.y < 0 && Menu.istnieje(x, y-1) && Menu.kafelki[x][y-1].GetComponent<Pole>().postac == null)
                {
                    MoveJednostka(x, y, x, y-1, Jednostka.Select2);
                }
                else if(offset.y > 0 && Menu.istnieje(x, y+1) && Menu.kafelki[x][y+1].GetComponent<Pole>().postac == null)
                {
                    MoveJednostka(x, y, x, y+1, Jednostka.Select2);
                }

                if(MenuGlowne.multi)
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("odrzutMulti", RpcTarget.All, Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, Jednostka.Select2.GetComponent<Jednostka>().druzyna, Jednostka.Select2.transform.position);
                }

                
                }

                    Jednostka.Select2.GetComponent<Jednostka>().HP -= 2f;
                    if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, 2f ,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                        }
                        Menu.usunSelect2();
            }
        }
        else
        {
            odrzut = false;
        }
    }
    [PunRPC]
    public void dmg(int ip, int id, int dmg, int team)
    {
        if(ip != Ip.ip)
        {
            GameObject Oponenet = Menu.jednostki[team,id];
            Oponenet.GetComponent<Jednostka>().HP -= dmg;
        }
    }

    private List<GameObject> przeszukanie(int odleglosc, GameObject NPC, List<GameObject> lista)
    {
       // List<GameObject> lista = new List<GameObject>();
        GameObject help;

        for (int j = -odleglosc; j <= odleglosc; j++)
        {
            for (int i = -odleglosc; i <= odleglosc; i++)
            {
                int newX = (int)(NPC.transform.position.x + i);
                int newY = (int)(NPC.transform.position.y + j);

                if (Mathf.Abs(i) + Mathf.Abs(j) < odleglosc &&
                    newX >= 0 && newX <= (Menu.BoardSizeX - 1) &&
                    newY >= 0 && newY <= (Menu.BoardSizeY - 1))
                {
                    var pole = Menu.kafelki[newX][newY].GetComponent<Pole>();

                    if (pole.postac != null)
                    {
                        help = pole.postac;
                        Jednostka czek = help.GetComponent<Jednostka>();

                        if (czek != null  && czek.HP > 0.1 && !lista.Contains(help))
                        {
                            lista.Add(help);
                        }
                    }
                }
            }
        }


        if (odleglosc <= 3)
        {
            lista = przeszukanie(odleglosc + 1, NPC, lista);
        }

        return lista;
    }

    void MoveJednostka(int oldX, int oldY, int newX, int newY, GameObject ludek)
    {
        jednostka.GetComponent<Jednostka>().akcja = false;
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().postac = null;
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().Zajete = false; 
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().ZajeteLot = false;
        
        Menu.kafelki[newX][newY].GetComponent<Pole>().postac = ludek;

        if(!ludek.GetComponent<Jednostka>().lata)
            Menu.kafelki[newX][newY].GetComponent<Pole>().Zajete = true;
        else
            Menu.kafelki[newX][newY].GetComponent<Pole>().ZajeteLot = true;
        Debug.Log("siema");
        Debug.Log(ludek.name);
        ludek.transform.position = new Vector3(newX, newY, ludek.transform.position.z);
    }

    [PunRPC]
    void odrzutMulti(int ip, int id, int team, Vector3 pozycja)
    {
        if(ip != Ip.ip)
        {
            Menu.jednostki[team, id].transform.position = pozycja;
        }
    }

    private void levelUp(int level)
    {
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.HP += 2;
        staty.maxHP += 2;   
        switch (level){
            case 2:
                staty.zdolnosci += 1;
                break;
            case 3:
                silaWiatru++;

                break;
            case 4:
                staty.maxszybkosc += 2;
                staty.szybkosc += 2;
                staty.obrona += 3;
                staty.mindmg += 1;
                staty.maxdmg += 1;
                break;
            case 5:
                staty.zdolnosci += 1;
                silaWiatru++;
                silaWiatru2++;
                break;
        }
    }
}
