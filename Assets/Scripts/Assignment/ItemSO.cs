using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game Data/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int score = 10;
}
