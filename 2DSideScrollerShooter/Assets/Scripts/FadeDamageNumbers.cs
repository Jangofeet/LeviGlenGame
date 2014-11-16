using UnityEngine;
using System.Collections;

public class FadeDamageNumbers : MonoBehaviour {

	private float time_;
	private float time_to_fade = 1.0f;
	public Vector3 pos;
	public float speed = 2.0f; 

	void Start ()
	{
		time_ = Time.time;
		pos = Camera.main.ViewportToWorldPoint(gameObject.transform.position);
	}
	
	void Update () 
	{

		transform.position = Camera.main.WorldToViewportPoint (pos) ;
		pos.y += speed * Time.deltaTime;
		//gameObject.transform.Translate (0.0f,0.006f,0.0f);

		//guiText.material.color.a = Mathf.Cos((Time.time - time_)*((Mathf.PI/2)/time_to_fade));
		Destroy (gameObject,time_to_fade);
	}
}
