using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new equip list", menuName = "ScriptableObjects/EquipsList")]
public class EquipmentItemsListSO : ScriptableObject
{
    public List<EquipmentItemSO> equipmentItems;
}
