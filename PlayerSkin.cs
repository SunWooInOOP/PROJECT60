using Photon.Pun;
using UnityEngine;

public class PlayerSkin : MonoBehaviourPun
{
    public PlayerSkinData playerSkinData;
    public SkinnedMeshRenderer playerSkin;
    public MaterialManager materialManager;

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            if (playerSkinData.playerSkinMaterial != null)
            {
                Material skinMaterial = playerSkinData.playerSkinMaterial;
                Material[] matArray = playerSkin.materials;
                matArray[0] = playerSkinData.playerSkinMaterial;
                playerSkin.materials = matArray;
                photonView.RPC("ChangeMaterial", RpcTarget.Others, skinMaterial.name);
            }
        }
    }

    [PunRPC]
    public void ChangeMaterial(string materialName)
    {
        Material newMaterial = materialManager.GetMaterialByName(materialName);
        Material[] matArray = playerSkin.materials;
        matArray[0] = newMaterial;
        playerSkin.materials = matArray;
    }



}
