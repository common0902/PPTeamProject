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
        [SerializeField] private EventChannelSO renderEvent;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;

        private void Awake()
        {
	        _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
			renderEvent.AddListener<TopViewEvent>(CombineMesh);
        }

        public void CombineMesh(TopViewEvent evt)
        {
	        if (evt.IsTopView)
	        {
		        _meshRenderer.enabled = true;
	        	var children = GetComponentsInChildren<FOVRendering>();
				foreach (FOVRendering child in children)
				{
					child.gameObject.SetActive(true);
					child.DrawFov();
				}
		    	       
				if(evt.IsTopView)
            		_meshFilter.mesh = MeshCombiner.CombineMesh(gameObject, children);
	        }
	        else
	        {
		        _meshRenderer.enabled = false;
	        }
	        
        }

        private void OnDestroy()
        {
	        renderEvent.RemoveListener<TopViewEvent>(CombineMesh);
        }
    }
}