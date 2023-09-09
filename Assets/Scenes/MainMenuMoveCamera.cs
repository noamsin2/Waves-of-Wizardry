using UnityEngine;

public class MainMenuMoveCamera : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector2(0, 0);
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y, transform.position.z);
    }
}
