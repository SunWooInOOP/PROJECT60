using Photon.Pun;
using UnityEngine;

public class PlayerSkin : MonoBehaviourPun
{
    public PlayerSkinData playerSkinData;
    public SkinnedMeshRenderer playerSkin;
    public MaterialManager materialManager;

    private void OnEnable()
    {
        playerSkin = GetComponent<SkinnedMeshRenderer>();
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeMaterial(playerSkinData.playerSkinMaterial.name);
        }
    }

    public void ChangeMaterial(string materialName)
    {
        photonView.RPC("SyncMaterial", RpcTarget.All, materialName);
    }

    [PunRPC]
    public void SyncMaterial(string materialName)
    {
        Material newMaterial = materialManager.GetMaterialByName(materialName);
        playerSkin.materials[0] = newMaterial;
    }
}
