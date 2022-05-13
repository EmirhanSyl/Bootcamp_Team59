using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {
	
	public float minIntensity = 2.0f;
	public float maxIntensity = 3.0f;
	public Light light;

	//Make the light flicker randomly
	void Update () {
		light.intensity = Random.Range (minIntensity, maxIntensity);
	}
}
