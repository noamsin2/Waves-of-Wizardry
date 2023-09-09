using UnityEngine;

public class CameraController : MonoBehaviour
{//room camera
    
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // follow player
    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    private float lookAheadHorizontal;
    //private float lookAheadVertical;

    private void Update()
    {
        //Room camera: 
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed * Time.deltaTime);
     
        //follow player
        transform.position = new Vector3(player.position.x + lookAheadHorizontal, transform.position.y, transform.position.z);
        lookAheadHorizontal = Mathf.Lerp(lookAheadHorizontal, (aheadDistance * player.localScale.x), Time.deltaTime * speed);
        //lookAheadVertical = Mathf.Lerp(lookAheadVertical, (aheadDistance * player.localScale.y), Time.deltaTime * speed);
    }

}