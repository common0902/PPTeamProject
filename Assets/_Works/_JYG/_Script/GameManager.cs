using _Script.Tools.Utility;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [field: SerializeField] public GameObject Player { get; private set; } 
}
