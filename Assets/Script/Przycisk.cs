using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Przycisk : MonoBehaviour
{
    public static bool[] budynek; 
    public static bool[] jednostka; 

    void Start()
    {
        budynek = new bool[16]; 
        jednostka = new bool[16]; 
    }

    public void b1() {budynek[0]=true;}
    public void b2() {budynek[1]=true;}
    public void b3() {budynek[2]=true;}
    public void b4() {budynek[3]=true;}
    public void b5() {budynek[4]=true;}
    public void b6() {budynek[5]=true;}
    public void b7() {budynek[6]=true;}
    public void b8() {budynek[7]=true;}
    public void b9() {budynek[8]=true;}
    public void b10() {budynek[9]=true;}
    public void b11() {budynek[10]=true;}
    public void b12() {budynek[11]=true;}
    public void b13() {budynek[12]=true;}
    public void b14() {budynek[13]=true;}
    public void b15() {budynek[14]=true;}
    public void b16() {budynek[15]=true;}

    
    public void j1() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[0]=true;}
    public void j2() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[1]=true;}
    public void j3() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[2]=true;}
    public void j4() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[3]=true;}
    public void j5() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[4]=true;}
    public void j6() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[5]=true;}
    public void j7() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[6]=true;}
    public void j8() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[7]=true;}
    public void j9() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[8]=true;}
    public void j10() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[9]=true;}
    public void j11() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[10]=true;}
    public void j12() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[11]=true;}
    public void j13() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[12]=true;}
    public void j14() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[13]=true;}
    public void j15() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[14]=true;}
    public void j16() {if(Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura) jednostka[15]=true;}
}
