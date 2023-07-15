using UnityEngine;

[CreateAssetMenu(fileName = "Skin Data", menuName = "Scriptable/PlayerSkinData")]
public class PlayerSkinData : ScriptableObject
{
    public Material playerSkinMaterial;
    public Material playerEmotionMaterial;
    public GameObject playerAccessory;
}
