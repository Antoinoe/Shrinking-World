using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float cameraLerpDistance;
    [SerializeField] private float cameraZOffset = -10;

    private void Update()
    {
        var lerpDistance = Vector2.Lerp(
            GameManager.Instance.PlayerController.transform.position, 
            GameManager.Instance.PlayerController.PlayerVisual.position, 
            cameraLerpDistance);

        //transform.SetPositionAndRotation(
        //    new Vector3(lerpDistance.x, lerpDistance.y, cameraZOffset),
        //    GameManager.Instance.PlayerController.transform.rotation);
        transform.position = new Vector3(lerpDistance.x, lerpDistance.y, cameraZOffset);
    }
}
