using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SelectPlayer : MonoBehaviour
{

    public int selectedPlayer = 0;
    [SerializeField] private GameObject playerBody;

    [SerializeField] private Image playerIcon;
    [SerializeField] GameObject playerCanvas;

    private PhotonView photonView;

    void Start()
    {
        photonView = this.GetComponent<PhotonView>();

        if (photonView.IsMine == false)
        {
            playerCanvas.SetActive(false);
        }
        SwitchPlayer();
    }

    private void SwitchPlayer()
    {
        
        photonView.RPC(nameof(SwitchPlayerRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void SwitchPlayerRPC() 
    {

        int i = 0;

        foreach (Transform child in playerBody.transform)
        {
            PlayerConfig item = child.GetComponent<PlayerConfig>();

            if (i == selectedPlayer)
            {
                item.gameObject.SetActive(true);
                playerIcon.sprite = item.playerData.playerSprite;
            }
            else
            {
                item.gameObject.SetActive(false);
            }

            i++;

        }
    }

    public void Backward()
    {
        if (photonView.IsMine == true)
        {
            photonView.RPC(nameof(BackwardRPC), RpcTarget.AllBuffered);
        }
        
    }

    [PunRPC]
    private void BackwardRPC()
    {
        selectedPlayer--;

        if (selectedPlayer < 0)
        {
            selectedPlayer = playerBody.transform.childCount - 1;
        }

        SwitchPlayer();
    }

    public void Forward()
    {
        if (photonView.IsMine == true)
        {
            photonView.RPC(nameof(ForwardRPC), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void ForwardRPC()
    {
        selectedPlayer++;

        if (selectedPlayer > playerBody.transform.childCount - 1)
        {
            selectedPlayer = 0;
        }

        SwitchPlayer();
    }
}
