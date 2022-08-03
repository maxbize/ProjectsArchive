using UnityEngine;
using System.Collections;

public class SkidMark : MonoBehaviour {

    public enum side {left, right};

	Car m_parentCar;
	TrailRenderer m_tr;

	bool initialized = false; // Need this or we'll get an exception first frame

	float phaseThreshold; // Min angle required for skid

    float timeAlive = 0; // Total time since birth
    float timeToFade;    // Total time skid will live for before starting to fade
	
	// Update is called once per frame
	void Update () {
		if (initialized) {
            timeAlive += Time.deltaTime;
            if (!m_tr)
            {
                Destroy(gameObject);
            }
			if (m_parentCar.phase < phaseThreshold || m_parentCar.phase > 90) {
				transform.parent = null;
			}
            if (timeAlive > timeToFade) {
                Color newColor = m_tr.material.GetColor("_TintColor");
                newColor.a = (m_tr.time - timeAlive) / (m_tr.time - timeToFade);
                m_tr.material.SetColor("_TintColor", newColor);
            }
		}
	}

	public void Init(TrailRenderer myRenderer, Car myParentCar, side s) {
		m_tr = myRenderer;
		m_parentCar = myParentCar;
		phaseThreshold = myParentCar.skidPhaseThreshold;
		transform.localScale = Vector3.one;
		float yOffset = myRenderer.transform.parent.gameObject.renderer.bounds.size.y * 0.8F;
        float xOffset = myRenderer.transform.parent.gameObject.renderer.bounds.size.x * 0.48F;
        xOffset = s == side.left ? xOffset : -xOffset;
		transform.localPosition = new Vector3(xOffset, -yOffset, 0);
        timeToFade = m_tr.time * 0.8f;
		initialized = true;
		m_tr.autodestruct = true;
	}
}
