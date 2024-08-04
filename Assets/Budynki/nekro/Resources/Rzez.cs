using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Dodano using dla UnityEngine.UI
using Photon.Pun;

public class Rzez : MonoBehaviour
{
    public GameObject pole;
    public GameObject budynek;

    public Sprite[] budynki;
    public string[] teksty;

    public Sprite puste;

    private int dhelp;
    //private bool dopisz = true;

    public int druzyna;
    public bool dostepny;

    public Image wykrzyknik;

    private bool dodaj=false;

    public static float zysk = 0.8f;

    public bool koniec;


    public int regeneracja = 0;
    // Start is called before the first frame update
    void Start()
    {
        druzyna = budynek.GetComponent<Budynek>().druzyna;
        OnMouseDown();
        budynek.GetComponent<Budynek>().poZniszczeniu = 1;
    }

    void Update()
    {
        if(budynek == Jednostka.Select)
        {
            pole = budynek.GetComponent<BudynekRuch>().pole;
            if(Przycisk.budynek[0] == true)
            {
                Przycisk.budynek[0] = false;
                if(budynek.GetComponent<Budynek>().punktyBudowy >= 5 && InterfaceBuild.obrazkikopalnia[0].activeSelf && pole.GetComponent<Pole>().postac.GetComponent<Jednostka>() && !pole.GetComponent<Pole>().postac.GetComponent<Heros>()) //niepotrzebne w sumie
                {
                    pole.GetComponent<Pole>().postac.GetComponent<Jednostka>().zdolnosci = 0;
                    Menu.zloto[druzyna] += (int)(pole.GetComponent<Pole>().postac.GetComponent<Jednostka>().cena * zysk);
                    pole.GetComponent<Pole>().postac.GetComponent<Jednostka>().umieranie();
                }
            }
             if(Przycisk.budynek[1]==true && Menu.zloto[Menu.tura]>= Menu.heros[druzyna].GetComponent<Heros>().level && regeneracja < 5)
                {
                    Menu.zloto[Menu.tura]-=Menu.heros[druzyna].GetComponent<Heros>().level;
                    Przycisk.budynek[1]=false;
                    regeneracja++;
                }
                if(InterfaceBuild.zdrowieHerosa.gameObject.activeSelf)
                {
                    InterfaceBuild.zdrowieHerosa.value = regeneracja;
                }

                if (pole != null)
                {
                    Pole poleComponent = pole.GetComponent<Pole>();

                    if (poleComponent != null && poleComponent.postac != null && poleComponent.postac.GetComponent<Jednostka>().druzyna == druzyna && !poleComponent.postac.GetComponent<Heros>())
                    {
                            SpriteRenderer spriteRenderer = poleComponent.postac.GetComponent<SpriteRenderer>();
                            Sprite sprite = spriteRenderer.sprite;
                            InterfaceBuild.obrazkikopalnia[0].GetComponent<Image>().sprite = sprite;
                            InterfaceBuild.paskikopalnia[0].maxValue = poleComponent.postac.GetComponent<Jednostka>().maxHP;
                            InterfaceBuild.paskikopalnia[0].value = poleComponent.postac.GetComponent<Jednostka>().HP;
                    }
                    else
                    {
                        InterfaceBuild.obrazkikopalnia[0].GetComponent<Image>().sprite = puste;
                        InterfaceBuild.paskikopalnia[0].maxValue = 0;
                        InterfaceBuild.paskikopalnia[0].value = 0;
                    }
                }
        }
            if(Menu.Next)
            {
                koniec = true;
            }
            if(koniec && !Menu.Next && budynek.GetComponent<Budynek>().druzyna == Menu.tura)
            {
                koniec = false;
                if(!Menu.heros[druzyna].activeSelf && regeneracja < 5)
                {
                    regeneracja++;
                }
            }
            
        
        if(regeneracja >= 5)
                wykrzyknik.enabled = true;
            else
                wykrzyknik.enabled = false;
        if(!Menu.heros[druzyna].activeSelf && regeneracja >= 5 && 
        !budynek.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().Zajete
        && Menu.ludnosc[druzyna] < Menu.maxludnosc[druzyna])
        {
            if(MenuGlowne.multi)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("wskrzeszenieMulti", RpcTarget.All, (int)budynek.GetComponent<BudynekRuch>().pole.transform.position.x ,  (int)budynek.GetComponent<BudynekRuch>().pole.transform.position.y);
            }
            else
            {
                Jednostka staty = Menu.heros[druzyna].GetComponent<Jednostka>();
                regeneracja = 0;
                Menu.ludnosc[druzyna]++;
                staty.HP = staty.maxHP;
                budynek.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().Zajete = true;
                budynek.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().postac = Menu.heros[druzyna];
                Vector3 newPosition = budynek.GetComponent<BudynekRuch>().pole.transform.position;
                newPosition.z = -2f;
                Menu.heros[druzyna].transform.position = newPosition;
                Menu.heros[druzyna].SetActive(true);
                staty.ShowDMG(staty.maxHP, Color.green);
                OnMouseDown();
            }
        }
        if(!dodaj)
        {
            dodaj = true;
            Apteka.apteka[druzyna] = true;
        }
        if(budynek.GetComponent<Budynek>().poZniszczeniu == 2)
        {
            Apteka.apteka[druzyna] = false;
            Destroy(budynek);
        }
    }
        [PunRPC]
    public void wskrzeszenieMulti(int x, int y)
    {
        Jednostka staty = Menu.heros[druzyna].GetComponent<Jednostka>();
            regeneracja = 0;
            Menu.ludnosc[druzyna]++;
            staty.HP = staty.maxHP;
            Menu.kafelki[x][y].GetComponent<Pole>().Zajete = true;
            Menu.kafelki[x][y].GetComponent<Pole>().postac = Menu.heros[druzyna];
            Vector3 newPosition = Menu.kafelki[x][y].transform.position;
            newPosition.z = -2f;
            Menu.heros[druzyna].transform.position = newPosition;
            Menu.heros[druzyna].SetActive(true);
            staty.ShowDMG(staty.maxHP, Color.green);
            Debug.Log("kończymy");
    }

    void OnMouseDown()
    {
        if(budynek == Jednostka.Select)
        {
        InterfaceBuild.obrazkikopalnia[1].SetActive(false);
        InterfaceBuild.obrazkikopalnia[2].SetActive(false);
        InterfaceBuild.obrazkikopalnia[3].SetActive(false);
        InterfaceBuild.obrazkikopalnia[4].SetActive(false);
        InterfaceBuild.obrazkikopalnia[6].SetActive(false);
        InterfaceBuild.obrazkikopalnia[5].SetActive(false);
        InterfaceBuild.Czyszczenie(); 

         for(int i = 0; i < 2; i++){
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = false;
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.Opis.text = teksty[i]; 
            } 

            if(Menu.heros[druzyna].activeSelf)
            {
                InterfaceBuild.twarzHerosa.enabled = false;
                InterfaceBuild.zdrowieHerosa.gameObject.SetActive(false);
                budynek.GetComponent<Budynek>().zdolnosci = 1;
                budynek.GetComponent<Budynek>().OnMouseDown();
            }
            else
            {
                InterfaceBuild.twarzHerosa.enabled = true;
                InterfaceBuild.zdrowieHerosa.gameObject.SetActive(true);
                InterfaceBuild.twarzHerosa.sprite = Menu.heros[druzyna].GetComponent<SpriteRenderer>().sprite;
                PrzyciskInter Guzik = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = true;
                Guzik.CenaZloto.text = Menu.heros[budynek.GetComponent<Budynek>().druzyna].GetComponent<Heros>().level.ToString();
                InterfaceBuild.zdrowieHerosa.maxValue = 5;
                budynek.GetComponent<Budynek>().zdolnosci = 2;
                budynek.GetComponent<Budynek>().OnMouseDown();
            }

        }
    }
}
