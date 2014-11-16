using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour
{
	public float speed = 10.0f;
	public AudioClip bulletSound;
	
	private Vector3 newTransform;
	private float xPos;
	
	void Start()
	{
		AudioSource.PlayClipAtPoint (bulletSound, transform.position);
	}
	
	// Update is called once per frame
	void Update()
	{
		newTransform = transform.position;
		newTransform += transform.forward * speed * Time.deltaTime;
		transform.position = newTransform;
	}
	
	
}
