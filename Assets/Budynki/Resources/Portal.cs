using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Dodano using dla UnityEngine.UI
using Photon.Pun;

public class Portal : MonoBehaviour
{
    public GameObject pole;
    public GameObject budynek;
    public static GameObject[] portal = new GameObject[50];
    public static int[] nr_portal = new int[50];

    public Sprite[] budynki;
    public string[] teksty;

    public Sprite puste;

    private int dhelp;
    private bool dopisz = true;

    public int druzyna;
    public bool dostepny;
    public int id;

    void Start()
    {
      druzyna = budynek.GetComponent<Budynek>().druzyna;
      OnMouseDown();
    }

    void Update()
    {
        if(budynek == Jednostka.Select && budynek.GetComponent<BudynekRuch>().pole)
        {
            pole = budynek.GetComponent<BudynekRuch>().pole;
            Pole poleComponent = pole.GetComponent<Pole>();
            if(poleComponent.postac != null)
            {
                Jednostka jednostkaComponent = poleComponent.postac.GetComponent<Jednostka>();
                if (jednostkaComponent != null)
                {
                InterfaceBuild.obrazkikopalnia[0].SetActive(true);

                SpriteRenderer spriteRenderer = poleComponent.postac.GetComponent<SpriteRenderer>();
                Sprite sprite = spriteRenderer.sprite;
                InterfaceBuild.obrazkikopalnia[0].GetComponent<Image>().sprite = sprite;
                InterfaceBuild.paskikopalnia[0].maxValue = poleComponent.postac.GetComponent<Jednostka>().maxHP;
                InterfaceBuild.paskikopalnia[0].value = poleComponent.postac.GetComponent<Jednostka>().HP;
                }
            }
            else
            {
                InterfaceBuild.obrazkikopalnia[0].SetActive(false);
            }


            if(Przycisk.budynek[0] == true)
            {
                Przycisk.budynek[0] = false;
                if(InterfaceBuild.obrazkikopalnia[0].activeSelf) //niepotrzebne w sumie
                {
                    int j = 0;
                    for(int i = 0; i<50; i++)
                    {
                        if((portal[i]!=null && portal[i]!=budynek && portal[i].GetComponent<BudynekRuch>().pole.GetComponent<Pole>().Zajete == false) && 
                        ((!dostepny && (!portal[i].GetComponent<Portal>().dostepny || portal[i].GetComponent<Portal>().druzyna==druzyna)) || (dostepny && portal[i].GetComponent<Portal>().druzyna==druzyna)) &&
                        (id == portal[i].GetComponent<Portal>().id))
                        {
                            nr_portal[j] = i;
                            j++;
                        }
                    }
                    if(j>0)
                    {
                        int n = Random.Range(0, j);
                    teleport(portal[nr_portal[n]],pole.GetComponent<Pole>().postac);
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("trzyMulti", RpcTarget.All,Ip.ip,n,pole.GetComponent<Pole>().postac.GetComponent<Jednostka>().nr_jednostki);
                    }
                        for(int i = 0; i<50; i++)
                        {
                            nr_portal[j] = 0;    
                        }
                    }

                }
            }
            if(Przycisk.budynek[1] == true)
            {
                Przycisk.budynek[1] = false;
                jeden();
                if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("jedenMulti", RpcTarget.All,Ip.ip);
                    }
            }
            if(Przycisk.budynek[2] == true)
            {
                Przycisk.budynek[2] = false;
                dwa();
                if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("dwaMulti", RpcTarget.All,Ip.ip);
                    }
            }
        }
        if(budynek.GetComponent<Budynek>().punktyBudowy >= budynek.GetComponent<Budynek>().punktyBudowyMax && dopisz)
        {
            dopisz = false;
            for(int i = 0; i<50; i++)
            {
                if(portal[i]==null)
                {
                    portal[i] = budynek;
                    i=50;
                }
            }
        }
    }
    [PunRPC]
    public void jedenMulti(int ip)
    {
        if(ip != Ip.ip)
        {
            jeden();
        }
    }
        [PunRPC]
    public void dwaMulti(int ip)
    {
        if(ip != Ip.ip)
        {
            dwa();
        }
    }
    [PunRPC]
    public void trzyMulti(int ip, int n, int nr)
    {
        if(ip != Ip.ip)
        {
            teleport(portal[nr_portal[n]],Menu.jednostki[Menu.tura,nr]);
        }
    }
    public void jeden()
    {
                if(dostepny)
                    InterfaceBuild.przyciski[1].GetComponent<Image>().sprite = budynki[0];
                else
                    InterfaceBuild.przyciski[1].GetComponent<Image>().sprite = budynki[1];
                dostepny = !dostepny;
    }
    public void dwa()
    {
                if(id < 9)
                    id++;
                else
                    id=0;
                InterfaceBuild.przyciski[2].GetComponent<Image>().sprite = budynki[id];
    }


    public void teleport(GameObject portal2, GameObject jednostka)
    {
        budynek.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().Zajete = false;
        budynek.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().postac = null;

        jednostka.transform.position = portal2.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().transform.position;
        portal2.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().Zajete = true;
        portal2.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().postac = jednostka;
        jednostka.transform.position = new Vector3(jednostka.transform.position.x,jednostka.transform.position.y, -2f);

        Jednostka.Select = jednostka;
        jednostka.GetComponent<Jednostka>().OnMouseDown();
    }
        void OnMouseDown()
    {
        InterfaceBuild.obrazkikopalnia[1].SetActive(false);
        InterfaceBuild.obrazkikopalnia[2].SetActive(false);
        InterfaceBuild.obrazkikopalnia[3].SetActive(false);
        InterfaceBuild.obrazkikopalnia[4].SetActive(false);
        InterfaceBuild.obrazkikopalnia[6].SetActive(false);
        InterfaceBuild.obrazkikopalnia[5].SetActive(false);
            
            for(int i = 0 ; i < 3 ; i++)
            {
                
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = false;
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.Opis.text = teksty[i];  
            }  
        InterfaceBuild.przyciski[2].GetComponent<Image>().sprite = budynki[id];
        if(dostepny)
            InterfaceBuild.przyciski[1].GetComponent<Image>().sprite = budynki[1];     
        else
            InterfaceBuild.przyciski[1].GetComponent<Image>().sprite = budynki[0];    
        InterfaceBuild.przyciski[0].SetActive(false);
    }

}
