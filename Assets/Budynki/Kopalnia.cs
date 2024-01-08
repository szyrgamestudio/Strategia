using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Dodano using dla UnityEngine.UI

public class Kopalnia : MonoBehaviour
{
    public GameObject pole;
    public GameObject polePomoc;
    public GameObject budynek;
    
    // public int miejsc;
    public GameObject[] slot;
    public Image[] kopacze;

    public Sprite puste;
    private int kolejnatura;

    private int dhelp;
    //private bool isZbieraczVisible = false;

    void Start()
    {
     budynek.GetComponent<Budynek>().poZniszczeniu = 1;
     for(int i = 0; i<6;i++)
        kopacze[i].enabled = false;
    }

    void Update()
    {
        if(budynek.GetComponent<BudynekRuch>().wybudowany)
            budynek.GetComponent<Budynek>().zdolnosci = Menu.kafelki[(int)budynek.transform.position.x][(int)budynek.transform.position.y].GetComponent<Pole>().zloto;
        if(budynek == Jednostka.Select)
        {
            pole = budynek.GetComponent<BudynekRuch>().pole;
            for(int i = 0; i<6;i++)
            {
            kopacze[i].enabled = false;
            if(slot[i] != null)
                kopacze[i].enabled = true;
            }
            //////////////////////////////////////////
        GameObject[] polaDookola = new GameObject[5];
            int k = 0;

            for (int x = (int)budynek.transform.position.x - 1; x <= (int)budynek.transform.position.x + 1; x++)
            {
                for (int y = (int)budynek.transform.position.y - 1; y <= (int)budynek.transform.position.y + 1; y++)
                {
                    if ((x - budynek.transform.position.x) * (y - budynek.transform.position.y) == 0 && Menu.istnieje(x, y))
                    {
                        polaDookola[k] = Menu.kafelki[x][y];
                        k++;
                    }
                }
            }

            bool foundZbieracz = false; // Dodaj nową zmienną

            foreach (GameObject pole in polaDookola)
            {
                if (pole != null)
                {
                    Pole poleComponent = pole.GetComponent<Pole>();

                    if (poleComponent != null && poleComponent.postac != null)
                    {
                        Zbieracz jednostkaComponent = poleComponent.postac.GetComponent<Zbieracz>();

                        if (jednostkaComponent != null)
                        {
                            foundZbieracz = true;

                            SpriteRenderer spriteRenderer = poleComponent.postac.GetComponent<SpriteRenderer>();
                            Sprite sprite = spriteRenderer.sprite;
                            InterfaceBuild.obrazkikopalnia[0].GetComponent<Image>().sprite = sprite;
                            InterfaceBuild.paskikopalnia[0].maxValue = poleComponent.postac.GetComponent<Jednostka>().maxHP;
                            InterfaceBuild.paskikopalnia[0].value = poleComponent.postac.GetComponent<Jednostka>().HP;
                            polePomoc = pole;
                        }
                    }
                }
            }

    // Zaktualizuj stan aktywności tylko jeśli znaleziono Zbieracza
    InterfaceBuild.obrazkikopalnia[0].SetActive(foundZbieracz);

    // Zresetuj flagę, aby przygotować się na kolejną aktualizację
    foundZbieracz = false;

            



            if(Przycisk.budynek[0] == true)
            {
                Przycisk.budynek[0] = false;
                if(InterfaceBuild.obrazkikopalnia[0].activeSelf && polePomoc.GetComponent<Pole>().postac.GetComponent<Jednostka>().zbieracz) //niepotrzebne w sumie
                {
                    for(int i=0;i<budynek.GetComponent<Budynek>().zdolnosci;i++)
                    {
                        if(slot[i]==null)
                        {
                            slot[i] = polePomoc.GetComponent<Pole>().postac;
                            slot[i].SetActive(false);
                            polePomoc.GetComponent<Pole>().Zajete=false;
                            polePomoc.GetComponent<Pole>().postac=null;
                            break;
                        }
                    }
                }
            }
            if(Przycisk.budynek[1] == true)
            {
                Przycisk.budynek[1] = false;
                if(slot[0]!=null && !pole.GetComponent<Pole>().Zajete) 
                {
                    slot[0].SetActive(true);
                    pole.GetComponent<Pole>().Zajete = true;
                    slot[0].transform.position = new Vector3(pole.transform.position.x,pole.transform.position.y,-2f);
                    Debug.Log(pole.name);
                    pole.GetComponent<Pole>().postac = slot[0];
                    InterfaceBuild.obrazkikopalnia[1].GetComponent<Image>().sprite = puste;
                    slot[0] = null;
                }
            }
            if(Przycisk.budynek[2] == true)
            {
                Przycisk.budynek[2] = false;
                if(slot[1]!=null && !pole.GetComponent<Pole>().Zajete) 
                {
                    slot[1].SetActive(true);
                    pole.GetComponent<Pole>().Zajete = true;
                    pole.GetComponent<Pole>().postac = slot[1];
                    slot[1].transform.position = new Vector3(pole.transform.position.x,pole.transform.position.y,-2f);
                    InterfaceBuild.obrazkikopalnia[2].GetComponent<Image>().sprite = puste;
                    slot[1] = null;
                }
            }
            if(Przycisk.budynek[3] == true)
            {
                Przycisk.budynek[3] = false;
                if(slot[2]!=null && !pole.GetComponent<Pole>().Zajete) 
                {
                    slot[2].SetActive(true);
                    pole.GetComponent<Pole>().Zajete = true;
                    pole.GetComponent<Pole>().postac = slot[2];
                    slot[2].transform.position = new Vector3(pole.transform.position.x,pole.transform.position.y,-2f);
                    InterfaceBuild.obrazkikopalnia[3].GetComponent<Image>().sprite = puste;
                    slot[2] = null;
                }
            }
            if(Przycisk.budynek[4] == true)
            {
                Przycisk.budynek[4] = false;
                if(slot[3]!=null && !pole.GetComponent<Pole>().Zajete) 
                {
                    slot[3].SetActive(true);
                    pole.GetComponent<Pole>().Zajete = true;
                    pole.GetComponent<Pole>().postac = slot[3];
                    slot[3].transform.position = new Vector3(pole.transform.position.x,pole.transform.position.y,-2f);
                    InterfaceBuild.obrazkikopalnia[4].GetComponent<Image>().sprite = puste;
                    slot[3] = null;
                }
            }
            if(Przycisk.budynek[5] == true)
            {
                Przycisk.budynek[5] = false;
                if(slot[4]!=null && !pole.GetComponent<Pole>().Zajete) 
                {
                    slot[4].SetActive(true);
                    pole.GetComponent<Pole>().Zajete = true;
                    pole.GetComponent<Pole>().postac = slot[4];
                    slot[4].transform.position = new Vector3(pole.transform.position.x,pole.transform.position.y,-2f);
                    InterfaceBuild.obrazkikopalnia[5].GetComponent<Image>().sprite = puste;
                    slot[4] = null;
                }
            }
            if(Przycisk.budynek[6] == true)
            {
                Przycisk.budynek[6] = false;
                if(slot[5]!=null && !pole.GetComponent<Pole>().Zajete) 
                {
                    slot[5].SetActive(true);
                    pole.GetComponent<Pole>().Zajete = true;
                    pole.GetComponent<Pole>().postac = slot[5];
                    slot[5].transform.position = new Vector3(pole.transform.position.x,pole.transform.position.y,-2f);
                    InterfaceBuild.obrazkikopalnia[6].GetComponent<Image>().sprite = puste;
                    slot[5] = null;
                }
            }


            for(int i=0;i<6;i++)
            {
                if(slot[i]!=null)
                {
                    SpriteRenderer spriteRenderer = slot[i].GetComponent<SpriteRenderer>();
                    Sprite sprite = spriteRenderer.sprite;
                    InterfaceBuild.obrazkikopalnia[i+1].GetComponent<Image>().sprite = sprite;
                    
                    InterfaceBuild.paskikopalnia[i+1].maxValue = slot[i].GetComponent<Jednostka>().maxHP;
                    InterfaceBuild.paskikopalnia[i+1].value = slot[i].GetComponent<Jednostka>().HP;
                }
                else
                {
                    InterfaceBuild.obrazkikopalnia[i+1].GetComponent<Image>().sprite = puste;
                    InterfaceBuild.paskikopalnia[i+1].maxValue = 1f;
                    InterfaceBuild.paskikopalnia[i+1].value = 0f;
                }
                
            }
            
        }
        if(budynek.GetComponent<Budynek>().poZniszczeniu == 2)
        {
            for(int i=0;i<6;i++)
                if(slot[i] != null)
                    slot[i].GetComponent<Jednostka>().umieranie();
            Destroy(budynek);
        }
        if(budynek.GetComponent<Budynek>().damage != 0)
        {
            float damagehelp = budynek.GetComponent<Budynek>().damage;
            budynek.GetComponent<Budynek>().damage = 0;
            Debug.Log(budynek.GetComponent<Budynek>().damage);
            float help = 99;
            
            GameObject bity = null;
            for(int i=0;i<6;i++)
            {
                if(slot[i] != null)
                {
                    if(slot[i].GetComponent<Jednostka>().HP < help)
                    {
                        help = slot[i].GetComponent<Jednostka>().HP;
                        bity = slot[i];
                        dhelp = i;
                    }
                }
            }
            if(bity != null)
            {
                bity.GetComponent<Jednostka>().HP -= damagehelp; 
                if(bity.GetComponent<Jednostka>().HP<=0)
                {
                    int druzyna_oponenta = bity.GetComponent<Jednostka>().druzyna;
                    int nr_jednostki =  bity.GetComponent<Jednostka>().nr_jednostki;
                    while(Menu.jednostki[druzyna_oponenta,nr_jednostki+1] != null)
                    {
                        Menu.jednostki[druzyna_oponenta,nr_jednostki] = Menu.jednostki[druzyna_oponenta,nr_jednostki+1];
                        Menu.jednostki[druzyna_oponenta,nr_jednostki].GetComponent<Jednostka>().nr_jednostki -= 1;
                        nr_jednostki++;
                    }
                    Menu.jednostki[druzyna_oponenta,nr_jednostki] = null;
                    Menu.ludnosc[druzyna_oponenta]--;
                    Destroy(bity); 
                    Debug.Log(dhelp);
                    slot[dhelp] = null;
                }
                        
            }
                

        }
        if(Menu.Next)
            kolejnatura = 2;
        if(!Menu.Next && kolejnatura == 2 && Menu.tura == budynek.GetComponent<Budynek>().druzyna)
        {
            int sumaGolda = 0;
            for(int i=0;i<budynek.GetComponent<Budynek>().zdolnosci;i++)
            {
                if(slot[i]!=null)
                    sumaGolda++;
                    
            }
            Menu.zloto[Menu.tura]+=sumaGolda;
            budynek.GetComponent<Budynek>().ShowDMG(sumaGolda, new Color(1.0f, 1.0f, 0.0f, 1.0f));
            DodawanieStat.zloto += sumaGolda;
            kolejnatura = 0;
        }
    }
    void OnMouseDown()
    {
        if(Jednostka.Select.GetComponent<Zbieracz>() != null && Walka.odleglosc(Jednostka.Select,budynek) == 1)
        {
            polePomoc = Menu.kafelki[(int)Jednostka.Select.transform.position.x][(int)Jednostka.Select.transform.position.y];
            for(int i=0;i<budynek.GetComponent<Budynek>().zdolnosci;i++)
            {
                if(slot[i]==null)
                {
                    slot[i] = polePomoc.GetComponent<Pole>().postac;
                    slot[i].SetActive(false);
                    polePomoc.GetComponent<Pole>().Zajete=false;
                    polePomoc.GetComponent<Pole>().postac=null;
                    break;
                }
            }

        }
    }
}
