using UnityEngine;
using System;

public class ClockAnimator : MonoBehaviour {
	
	private const float
		hoursToDegrees = 360f / 12f,
		minutesToDegrees = 360f / 60f,
		secondsToDegrees = 360f / 60f;
	
	public Transform Hours, Minutes, Seconds;
	
	public bool analog;
	 
	void Update() {
		TimeSpan timeSpan = DateTime.Now.TimeOfDay;
		DateTime time = DateTime.Now;
		if (analog) {
			Hours.localRotation = Quaternion.Euler(0f, 0f, (float)timeSpan.TotalHours * -hoursToDegrees);
			Minutes.localRotation = Quaternion.Euler(0f, 0f, (float)timeSpan.TotalMinutes * -minutesToDegrees);
			Seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
		}
		else {
			Hours.localRotation = Quaternion.Euler(0f, 0f, time.Hour * -hoursToDegrees);
			Minutes.localRotation = Quaternion.Euler(0f, 0f, time.Minute * -minutesToDegrees);
			Seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
		}
	}
}