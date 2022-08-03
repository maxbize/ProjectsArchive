using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    public Int3 levelPosition { get; private set; }

    public enum powerUp { none, multiBall };
    public powerUp power;

    void Start()
    {
        
        gameObject.renderer.material.shader = Shader.Find("Specular");
        BoxCollider bc = (BoxCollider)gameObject.AddComponent<BoxCollider>();
        //bc.material = (PhysicMaterial)Resources.Load("Ball PMat");
    }

    public void setParameters(powerUp powerUp, Color color, Int3 levelPos)
    {
        power = powerUp;
        levelPosition = levelPos;
        gameObject.renderer.material.color = color;
        gameObject.transform.position = new Vector3(
            levelPosition.X * gameObject.renderer.bounds.size.x,
            levelPosition.Y * gameObject.renderer.bounds.size.y,
            levelPosition.Z * gameObject.renderer.bounds.size.z);
    }
}
