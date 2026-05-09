using System;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    [CreateAssetMenu(fileName = "Sabotage data", menuName = "Sabotage", order = 0)]
    public class SabotageDataSo : ScriptableObject
    {
        [field: SerializeField] public string SabotageName { get; private set; }
        [field: TextArea]
        [field: SerializeField] public string SabotageDesc { get; private set; } 
        // 일단은 사보타지 설명이 필요할 것 같아서 넣어봄
        // + 이걸로 사보타지 구별 가능
    }
}