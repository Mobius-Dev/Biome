using UnityEngine;

public enum EntityTypes
{
    Boar,
    Food,
    Fruit,
    Default
}

public class Entity : MonoBehaviour
{
    public EntityTypes entityType;

}
