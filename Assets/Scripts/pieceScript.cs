using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceScript : MonoBehaviour {

    public Rigidbody rb;

    public int index;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setIndex(int set)
    {
        index = set;
    }

    void disableRagDoll()
    {
        rb.isKinematic = true;
    }
}
