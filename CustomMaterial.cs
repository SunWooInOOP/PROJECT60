using UnityEngine.UI;
using UnityEngine;

public class CustomMaterial : MonoBehaviour
{
    public PlayerSkinData playerSkinData;
    public Material[] materials;
    public Button[] buttons;
    public GameObject lobbyCharacter;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    void Start()
    {
        skinnedMeshRenderer = lobbyCharacter.GetComponent<SkinnedMeshRenderer>();
    }
    private void OnEnable()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnClickMaterial(index));
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].onClick.RemoveAllListeners();
    }

    public void OnClickMaterial(int index)
    {
        playerSkinData.playerSkinMaterial = materials[index];
        Material[] matArray = skinnedMeshRenderer.materials;
        matArray[0] = playerSkinData.playerSkinMaterial;
        skinnedMeshRenderer.materials = matArray;
    }
}
