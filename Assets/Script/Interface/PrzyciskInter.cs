using UnityEngine;
using UnityEngine.UI;

public class PrzyciskInter : MonoBehaviour
{
    public Text CenaZloto;
    public Text CenaDrewno;
    public Text CenaMagic;
    public Image IconZloto;
    public Image IconDrewno;
    public Image IconMagic;


    public GameObject ShowOpis;
    public Text Opis;

    public void Enter()
    {
        ShowOpis.SetActive(true);
    }
    public void Exit()
    {
        ShowOpis.SetActive(false);
    }
}
