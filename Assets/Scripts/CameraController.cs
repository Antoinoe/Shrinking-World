using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool DoFollowPlayer { get; set; }
    [SerializeField][Range(0f, 1f)] private float cameraLerpDistance;
    [SerializeField] private float cameraZOffset = -10;

    private void Awake()
    {
        DoFollowPlayer = true;
    }

    private void Update()
    {
        if (!DoFollowPlayer)
            return;

        var lerpDistance = Vector2.Lerp(
            GameManager.Instance.PlayerController.transform.position, 
            GameManager.Instance.PlayerController.PlayerVisual.position, 
            cameraLerpDistance);

        transform.position = new Vector3(lerpDistance.x, lerpDistance.y, cameraZOffset);
    }
}
