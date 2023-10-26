using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    public float unitSquareSize = 10.0f; 

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            mousePos.x = Mathf.Clamp(mousePos.x, -unitSquareSize / 1, unitSquareSize / 1);
            mousePos.y = Mathf.Clamp(mousePos.y, -unitSquareSize / 3, unitSquareSize / 3);

            transform.position = mousePos + offset;
        }
    }
}