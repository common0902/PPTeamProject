using Reflex.Attributes;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    [Inject] private Test1 test;

    private void Awake()
    {
        test.DeLog();
    }
} 
