using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Prędkość poruszania postaci
    [SerializeField] private float cameraSpeed = 25f; // Prędkość poruszania kamery przy użyciu scrolla

    public static Vector3 lewyDolnyRogSwiata;
    public static Vector3 prawyGornyRogSwiata;

    private bool lewoRuchBool, prawoRuchBool, goraRuchBool, dolRuchBool;
    private bool isDragging = false;
    private Vector3 dragOrigin;

    void Update()
    {
        if (!Menu.NIERUSZAC)
        {
            // Odczytywanie wejścia od gracza
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Obliczanie kierunku ruchu
            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0);

            // Normalizacja wektora ruchu (aby zachować stałą prędkość na skos)
            movement.Normalize();
            cameraPosition = Menu.kamera.transform.position;

            // Przesunięcie postaci
            if (!(movement.y < 0 && Menu.kamera.GetComponent<Camera>().orthographicSize * 1.0f - 0.5f > cameraPosition.y) && !(movement.y > 0 && Menu.BoardSizeY - Menu.kamera.GetComponent<Camera>().orthographicSize * 1f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.2f - 0.5f < cameraPosition.y))
            {
                transform.Translate(new Vector3(0, movement.y, 0) * speed * Time.deltaTime);
            }
            if (!(movement.x < 0 && Menu.kamera.GetComponent<Camera>().orthographicSize * 1.8f - 0.6f > cameraPosition.x) && !(movement.x > 0 && Menu.BoardSizeX - Menu.kamera.GetComponent<Camera>().orthographicSize * 1.75f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.6f - 0.5f < cameraPosition.x))
            {
                transform.Translate(new Vector3(movement.x, 0, 0) * speed * Time.deltaTime);
            }

            // Zoom przy użyciu myszy
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            Camera mainCamera = GetComponent<Camera>();
            if (scrollDelta > 0 && mainCamera.orthographicSize > 2.5f)
            {
                mainCamera.orthographicSize -= 0.1f;

            }
            else if (scrollDelta < 0 && mainCamera.orthographicSize < 6.2f)
            {
                GetComponent<Camera>().orthographicSize += 0.1f;
                UstawKamere();
            }

            // Przesuwanie kamery przy trzymaniu myszy
            if (Input.GetMouseButtonDown(2))
            {
                isDragging = true;
                dragOrigin = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(2))
            {
                isDragging = false;
            }
            if (isDragging)
            {
                Vector3 difference = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
                Vector3 move = new Vector3(difference.x * 20, difference.y * 20, 0);
                transform.Translate(move * cameraSpeed * Time.deltaTime);
                dragOrigin = Input.mousePosition;
            }

            // Przesuwanie kamery za pomocą przycisków
            if (lewoRuchBool && !(Menu.kamera.GetComponent<Camera>().orthographicSize * 1.8f - 0.6f > cameraPosition.x))
                transform.Translate(new Vector3(-1f, 0, 0) * speed * Time.deltaTime);
            if (prawoRuchBool && !(Menu.BoardSizeX - Menu.kamera.GetComponent<Camera>().orthographicSize * 1.75f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.6f - 0.5f < cameraPosition.x))
                transform.Translate(new Vector3(+1f, 0, 0) * speed * Time.deltaTime);
            if (dolRuchBool && !(Menu.kamera.GetComponent<Camera>().orthographicSize * 1.0f - 0.5f > cameraPosition.y))
                transform.Translate(new Vector3(0, -1f, 0) * speed * Time.deltaTime);
            if (goraRuchBool && !(Menu.BoardSizeY - Menu.kamera.GetComponent<Camera>().orthographicSize * 1f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.2f - 0.5f < cameraPosition.y))
                transform.Translate(new Vector3(0, +1f, 0) * speed * Time.deltaTime);
        }
    }
    public static Vector3 cameraPosition;
    public static void UstawKamere()
    {
        cameraPosition = Menu.kamera.transform.position;
        if (Menu.kamera.GetComponent<Camera>().orthographicSize * 1.8f - 0.6f > cameraPosition.x)
            Menu.kamera.transform.position = new Vector3(Menu.kamera.GetComponent<Camera>().orthographicSize * 1.8f - 0.6f, Menu.kamera.transform.position.y, Menu.kamera.transform.position.z);
        if (Menu.kamera.GetComponent<Camera>().orthographicSize * 1.0f - 0.5f > cameraPosition.y)
            Menu.kamera.transform.position = new Vector3(Menu.kamera.transform.position.x, Menu.kamera.GetComponent<Camera>().orthographicSize * 1.0f - 0.5f, Menu.kamera.transform.position.z);
        float x = Menu.BoardSizeX - Menu.kamera.GetComponent<Camera>().orthographicSize * 1.75f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.6f - 0.5f;
        if (x < cameraPosition.x)
            Menu.kamera.transform.position = new Vector3(x, Menu.kamera.transform.position.y, Menu.kamera.transform.position.z);
        float y = Menu.BoardSizeY - Menu.kamera.GetComponent<Camera>().orthographicSize * 1f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.2f - 0.5f;
        if (y < cameraPosition.y)
            Menu.kamera.transform.position = new Vector3(Menu.kamera.transform.position.x, y, Menu.kamera.transform.position.z);
    }
    public void ruchLewoEnter()
    {
        lewoRuchBool = true;
    }
    public void ruchLewoExit()
    {
        lewoRuchBool = false;
    }
    public void ruchPrawoEnter()
    {
        prawoRuchBool = true;
    }
    public void ruchPrawoExit()
    {
        prawoRuchBool = false;
    }
    public void ruchGoraEnter()
    {
        goraRuchBool = true;
    }
    public void ruchGoraExit()
    {
        goraRuchBool = false;
    }
    public void ruchDolEnter()
    {
        dolRuchBool = true;
    }
    public void ruchDolExit()
    {
        dolRuchBool = false;
    }
}

