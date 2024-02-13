using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float moveSpeed = 10f;
    public float panBorderThickness = 10f;
    public BoxCollider2D boundCollider;

    private Vector3 minBound;
    private Vector3 maxBound;
    private float halfWidth;
    private float halfHeight;
    private Camera mainCamera;

    private void Start()
    {
        minBound = boundCollider.bounds.min;
        maxBound = boundCollider.bounds.max;
        mainCamera = GetComponent<Camera>();
        halfHeight = mainCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width/ Screen.height;
    }

    // Update is called once per frame
    void LateUpdate () {
        Vector3 pos = transform.position;
		if(Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += moveSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        transform.position = pos;

    }


}
