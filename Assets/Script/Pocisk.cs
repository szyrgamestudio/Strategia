using UnityEngine;

public class Pocisk : MonoBehaviour
{
    public GameObject cel;
    public float predkosc = 5f;

    void Update()
    {
        if (cel != null)
        {
            // Kierunek do celu
            Vector3 kierunek = (cel.transform.position - transform.position).normalized;

            // Przesunięcie obiektu w kierunku celu z zadaną prędkością
            transform.Translate(kierunek * predkosc * Time.deltaTime, Space.World);

            // Sprawdzenie odległości do celu
            float odlegloscDoCelu = Vector3.Distance(transform.position, cel.transform.position);

            // Jeśli obiekt jest wystarczająco blisko celu, usuń go
            if (odlegloscDoCelu < 0.1f)
            {
                UsunObiekt();
            }
        }
        else
        {
            UsunObiekt();
        }
    }

    void UsunObiekt()
    {
        // Zniszcz obiekt
        Destroy(gameObject);
    }
}
