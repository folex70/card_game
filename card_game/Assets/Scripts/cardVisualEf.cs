using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardVisualEf : MonoBehaviour {

	public GameObject cardGameObject;
	//public GameObject buttomSelectCard;

	public void OnMouseOver(){
	
		transform.localScale = new Vector3 (1.1f,1.1f,1.0f);

	}

	public void OnMouseExit(){
		transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		//buttomSelectCard.SetActive (false);
	}

	public void OnMouseDown(){
		//transform.position = new Vector3 (transform.position.x,-2.0f,transform.position.z);
	}

}
