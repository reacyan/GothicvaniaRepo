using UnityEngine;

public enum EquipmentType
{
    weapon,
    Armor,
    Amulet,
    flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]

public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
}
