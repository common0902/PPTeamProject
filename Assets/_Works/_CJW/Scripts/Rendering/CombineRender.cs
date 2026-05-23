using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._CJW.Scripts.Rendering
{
    public class CombineRender : MonoBehaviour
    {
        [SerializeField] private EventChannelSO cameraEvent;
        private List<FOVRendering> _registeredFovs = new();
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;

        private void Awake()
        {
	        _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
			cameraEvent.AddListener<TopViewEvent>(CombineMesh);
			cameraEvent.AddListener<RegisterFovEvent>(HandleRegisterFov);
        }

        private void HandleRegisterFov(RegisterFovEvent obj)
        {
	        if (obj.IsRegistered)
	        {
		        _registeredFovs.Add(obj.FovRendering);
	        }
	        else
	        {
		        _registeredFovs.Remove(obj.FovRendering);
	        }
        }

        private void CombineMesh(TopViewEvent evt)
        {
	        Debug.Log(evt.IsTopView);
	        if (evt.IsTopView)
	        {
		        _meshRenderer.enabled = true;

		        foreach (FOVRendering child in _registeredFovs)
		        {
			        child.gameObject.SetActive(true);
			        child.DrawFov();
		        }

		        if (_meshFilter.mesh != null)
		        {
			        Destroy(_meshFilter.mesh);
			        _meshFilter.mesh = null;
		        }

		        _meshFilter.mesh =
			        MeshCombiner.CombineMesh(gameObject, _registeredFovs);
	        }
	        else
	        {
		        if (_meshFilter.mesh != null)
		        {
			        Destroy(_meshFilter.mesh);
			        _meshFilter.mesh = null;
		        }

		        _meshRenderer.enabled = false;
	        }
        }

        private void OnDestroy()
        {
	        cameraEvent.RemoveListener<TopViewEvent>(CombineMesh);
        }
    }
}