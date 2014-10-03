using UnityEngine;
using System.Collections;

public class ArrowScroll : MonoBehaviour {
	
	private UIDraggablePanel panel;
	private Vector3 dragAmt = Vector3.zero;
	// Use this for initialization
	void Awake () {
		panel = (UIDraggablePanel)transform.GetComponent("UIDraggablePanel");
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (panel.gameObject.activeSelf)
		{
			Vector3 dragAmt = Vector3.zero;
			float scrollSpeed = 800;
			
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				dragAmt.x += scrollSpeed * Time.deltaTime;	
			}
			
			if  (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				dragAmt.x -= scrollSpeed * Time.deltaTime;
			}
			
			if (dragAmt != Vector3.zero)
			{
				panel.MoveRelative(dragAmt);
			}
		}

	}
}
