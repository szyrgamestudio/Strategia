using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public GameObject info;
    public Text text;
    public int druzyna;
    public int rodzaj;

    void Update()
    {
        if(WyburRas.aktywny[druzyna] == false)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
    

    void OnMouseEnter()
    {
        info.SetActive(true);
        switch(rodzaj)
        {
            case 0:
                    switch(WyburRas.rasa[druzyna])
                    {
                        case 0: text.text = "Srebrny Sojusz: Frakcja złożona głównie z ludzi oraz wytresowanych zwierząt. Jest to dosyć podstawowa frakcja. Ulepszenie przyśpieszające budowę może być kupowane ponownie z każdym ulepszeniem ratusza. Zasięg znajdowania złota przez poszukiwacza skaluje się z poziomem ratusza."; break;
                        case 1: text.text = "Żniwiarze Cienia: Mroczna frakcja złożona z nieumarłych jednostek. Mogą zaskoczyć swoją liczebnością i posiadają sporo jednostek regenerujących zdrowie. Posiadają specialny budynek pozwalający poświęcać swoje jednostki w zamian za złoto."; break;
                        case 2: text.text = "Góskie klany to frakcja składająca się z krasnoludów oraz Golemów zrobionych przez nich. Wszystkie jednstki charakteryzują się wysoką przeżywalnością i małą ilością obrażeń. Kopalnie Górskiego Klasnu otrzymują jedno więcej wolne miejsce"; break;
                        case 3: text.text = "Elfickie przymieże to frakcja składająca się z elfów, druidów i drzewców. Wszystkie jednostki charakteruzyją się wysoką mobilnością. W przeciwieństwie do pozostałych frakcjim elfickie przymieże może mieć na starcie 7 jednsotek. Wszyskie jednostki regenerują zdrowie, gdy skończą ture na polu lasu. Przymierze nie może wybudować Tartaku."; break;
                        case 4: text.text = "Frakcja Losowa"; break;
                    }
                    break;
            case 1:
                if(WyburRas.heros[druzyna] == 2 || WyburRas.rasa[druzyna] == 4)
                    text.text = "Losowy bohater";
                else
                    switch(WyburRas.rasa[druzyna] * 2 + WyburRas.heros[druzyna])
                        {
                            case 0: text.text = "Palladyn to potężny bohater potrafiący uleczyć sojuszniczą jednostkę za punkty magii. Posiada solidne statystyki przez co dobrze sprawdza się w walce na pierwszej lini. Z czasem dostaje możliwość zwiększania statysyk przyjaznym jednostką. Poczatkowa jednostka: Bojownik"; break;
                            case 1: text.text = "Arcymag to potężny czarodziej. O ile na stacie rozgrywki nie powala ilością swoich zaklęć tak już na 3 poziomie Arcymag uczy się fireaballa. Z sporą ilością magii, Arcymag jest w stanie samodzielnie pokonać nie duży odział przeciwnika. Zaczyna z 6 punktami magii. Poczatkowa jednostka: Adept"; break;
                            case 2: text.text = "Wampirzy Lord to poteżna jednostka. Jego główną umiejętnością jest wysysanie krwi z przeciwnika, co leczy Wampira. Ilość wyssanej krwi rośnie proporcionalnie do poziomu. Dzięki tej umiejętności Wampirzy Lord bez problemu radzi z początkowymi przeciwnikami bez żadnego wsparcia. Początkowa jednostka: Zombi"; break;
                            case 3: text.text = "Nekormanta to dość nietypowa jednostka. Za pomocą mrocznej magii może przywoływać małe jednostki. Wraz z poziomem, przyzwane jednostki są coraz silniejsze. Nekromanta w dowolnym momencie może przywołać całą armie w dowolnym miejscu co może odmienić losy pojedynku. Zaczyna z 6 punktami magii. Poczatkowa jednostka: Adept"; break;
                            case 4: text.text = "Barbażyńca to potężny krasnolud nie bojący się zagrożenia. Bije, uderza i atakuje. Z poziomem otrzymuje jedynie statystyki oraz możliwość kolejnego ataku. Barbażyńca na wyższych poziomach jest nie do zatrzymania. Poczatkowa jednostka: Kwatermistrz"; break;
                            case 5: text.text = "Szaman to potężny czarodziej władający żwyiołem powietrza. Może odpychać przeciwników. Za pomocą kontrolowania terenu, może dostosować pole bitwy pod swoje potrzeby. Wraz z poziomem, Szaman ma możliwość odpechnięcia na dalszą odległość. Zaczyna z 6 punktami magii. Poczatkowa jednostka: Adept"; break;
                            case 6: text.text = "Leśny Dowódca to przywódca każdego elfa z łukiem w ręce. Swoimi umiejętnościami może zwiększać statystyki swoich łuczników. Pozwa wsparciem sam Leśny dowódca posiada solidne statystyki, dzięki czemu ma spore znaczenie podczas walki. Poczatkowa jednostka: Elfi Łucznik"; break;
                            case 7: text.text = "Arcydruid sam w sobie nie jest groźny, jednak groźne są zwierzęta, w które może się przemienić. Arcydruid co poziom otrzymuje nowe, coraz silniejsze, zwierzęta do przmienay. Zdrowie przemienieonych zwierząt rośnie proporcionalnie do poziomu druida. Zaczyna z 6 punktami magii. Poczatkowa jednostka: Zwiadowca"; break;
                        }
                    break;
                break;
            case 2:
                text.text = "Sojusz definiuje drużyny. Osoby w tym samym sojuszu nie mogą się atakować oraz dzielą ze sobą mgłe wojny";
                break;
            case 3:
                text.text = "By zwyciężyć należy zniszczyć wszystkie Ratusze przeciwnika";
                break;
            case 4:
                text.text = "By zwyciężyć należy stać jednostką, inną niż latającą, na polu zwycięstwa przez " + End.tureKontroli + " tur.";
                break;
            case 5:
                text.text = "By zwyciężyć należy jako pierwszy ulepszyć Ratusz na " + End.poziomRatusza + " poziom.";
                break;
            case 6:
                if(End.tureDoKonca != 0)
                    text.text = "By zwyciężyć należy zadać bossowi ostateczne obrażenia. Tryb pozwala na kolaboracje. Gdy w przeciągu " +  End.tureDoKonca + " tur, żadnej z graczy nie pokona bossa. Boss zwycięża";
                else
                    text.text = "By zwyciężyć należy zadać bossowi ostateczne obrażenia. Tryb pozwala na kolaboracje.";
                break;
        }
    }

    void OnMouseExit()
    {
        info.SetActive(false);
    }
}
