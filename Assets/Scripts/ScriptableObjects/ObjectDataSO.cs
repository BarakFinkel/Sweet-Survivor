using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Data", menuName = "Scriptable Objects/New Object Data", order = 0)]
public class ObjectDataSO : ItemDataSO
{
    [field: Range(0,3)] [field: SerializeField] public int Rarity { get; private set; }
}
