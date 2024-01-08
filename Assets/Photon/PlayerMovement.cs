using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    private Vector3 targetPosition;

    void Update()
    {
        if (photonView.IsMine)
        {
            // Lokalny ruch gracza
            HandleMovementInput();
        }
    }

    void HandleMovementInput()
    {
        // Przykładowy kod obsługujący ruch gracza
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                // Lokalnie aktualizuj pozycję gracza
                transform.position = targetPosition;
                // Synchronizuj pozycję z innymi graczami przy użyciu Photon
                photonView.RPC("SyncPosition", RpcTarget.Others, targetPosition);
            }
        }
    }

    [PunRPC]
    void SyncPosition(Vector3 newPosition)
    {
        // Aktualizuj pozycję gracza na innych urządzeniach
        transform.position = newPosition;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Implementuj w przypadku, gdy wymagane są dodatkowe dane do przesyłania
    }
}
