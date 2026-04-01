using System;
using System.Diagnostics.Tracing;
using _Script.ScriptableObject.Event;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class AbstractSabotage : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private EventChannelSO sabotageOpenEvent;
        [SerializeField] private EventChannelSO sabotageCloseEvent;
        // private readonly int _hash = Shader.PropertyToID("_OutlineSize");
        // private SpriteRenderer _renderer;
        // private MaterialPropertyBlock _matPropertyBlock;

        private Outline _outline;
        private Rigidbody _rigid;
        private bool _isUsed;
        
        private void Awake()
        {
            sabotageOpenEvent.AddListener<GameEvent>(HandleOpen);
            sabotageCloseEvent.AddListener<GameEvent>(HandleClose);
            _rigid = GetComponent<Rigidbody>();
            _outline = GetComponent<Outline>();
            // _renderer = GetComponent<SpriteRenderer>();
            // _matPropertyBlock = new MaterialPropertyBlock();
            // _renderer.GetPropertyBlock(_matPropertyBlock);
            
            _outline.enabled = false;
            gameObject.SetActive(false);
        }

        private void HandleClose(GameEvent obj)
        {
            gameObject.SetActive(false);   
        }

        private void HandleOpen(GameEvent obj)
        {
            gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_isUsed) return;
            Debug.Log("OPEN");
            _rigid.useGravity = true;
            _isUsed = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_isUsed) return;
            
            _outline.enabled = true;
            // _matPropertyBlock.SetFloat(_hash, 0.001f);
            // _renderer.SetPropertyBlock(_matPropertyBlock);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _outline.enabled = false;
            // _matPropertyBlock.SetFloat(_hash, 0f);
            // _renderer.SetPropertyBlock(_matPropertyBlock);
        }
    }
}
