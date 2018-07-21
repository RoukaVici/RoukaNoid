using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class SkinnedMeshCollider : MonoBehaviour {
	[SerializeField]
   	private SkinnedMeshRenderer meshRenderer;
    private MeshCollider mCollider;

	void Start()
	{
		mCollider = GetComponent<MeshCollider>();
	}
	
	void Update ()
	{
		Mesh colliderMesh = new Mesh();
		meshRenderer.BakeMesh(colliderMesh);
        mCollider.sharedMesh = null;
        mCollider.sharedMesh = colliderMesh;
	}
}
