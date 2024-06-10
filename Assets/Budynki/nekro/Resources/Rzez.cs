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
    private bool dopisz = true;

    public int druzyna;
    public bool dostepny;

    public static float zysk = 0.5f;

    public bool koniec;
    // Start is called before the first frame update
    void Start()
    {
        druzyna = budynek.GetComponent<Budynek>().druzyna;
        OnMouseDown();
    }

    void Update()
    {
        if(budynek == Jednostka.Select)
        {
            pole = budynek.GetComponent<BudynekRuch>().pole;
            if(Przycisk.budynek[0] == true)
            {
                Przycisk.budynek[0] = false;
                if(budynek.GetComponent<Budynek>().punktyBudowy == 3 && InterfaceBuild.obrazkikopalnia[0].activeSelf && pole.GetComponent<Pole>().postac.GetComponent<Jednostka>()) //niepotrzebne w sumie
                {
                    pole.GetComponent<Pole>().postac.GetComponent<Jednostka>().zdolnosci = 0;
                    Menu.zloto[druzyna] += (int)(pole.GetComponent<Pole>().postac.GetComponent<Jednostka>().cena * zysk);
                    pole.GetComponent<Pole>().postac.GetComponent<Jednostka>().umieranie();
                }
            }

                if (pole != null)
                {
                    Pole poleComponent = pole.GetComponent<Pole>();

                    if (poleComponent != null && poleComponent.postac != null && poleComponent.postac.GetComponent<Jednostka>().druzyna == druzyna)
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
    }

    void OnMouseDown()
    {
        InterfaceBuild.obrazkikopalnia[1].SetActive(false);
        InterfaceBuild.obrazkikopalnia[2].SetActive(false);
        InterfaceBuild.obrazkikopalnia[3].SetActive(false);
        InterfaceBuild.obrazkikopalnia[4].SetActive(false);
        InterfaceBuild.obrazkikopalnia[6].SetActive(false);
        InterfaceBuild.obrazkikopalnia[5].SetActive(false);

         for(int i = 0 ; i < 1 ; i++)
            {
                
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = false;
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.Opis.text = teksty[i];  
            }  
    }
}
