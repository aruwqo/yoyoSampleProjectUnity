using System.Collections;
using UnityEngine;

public class yoyoControl : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private float animationSpeed = 2f;

    [SerializeField] private GameObject startPosition;
    
    private LineRenderer lineRenderer;

    private float animationTimer = 0;

    
    
    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.positionCount = 2; // thread thickness

        transform.position = startPosition.transform.position; // setting start position for yoyo
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StopAllCoroutines();

            Vector3 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // possibility of moving
            float distance = Vector3.Distance(mousePosition, startPosition.transform.position); // distance between mouse and start position of yoyo
            if (distance > radius) // if mouse is out of range
            {
                Vector3 fromOriginToObject = mousePosition - startPosition.transform.position;
                fromOriginToObject *= radius / distance;
                mousePosition = startPosition.transform.position + fromOriginToObject;
            }

            // drawing a thread
            drawThread(new Vector3(startPosition.transform.position.x, startPosition.transform.position.y),
                       new Vector3(transform.position.x, transform.position.y));

            // smooth yoyo movement
            transform.position = Vector3.Lerp(transform.position, mousePosition, animationTimer / animationSpeed);
            animationTimer += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            
            animationTimer = 0;

            // moving yoyo back through coroutine
            StartCoroutine(returnYoyo(startPosition.transform.position));
        }
    }

    private IEnumerator returnYoyo(Vector3 targetPosition)
    {
        for (float i = 0; i < 1; i += Time.deltaTime / animationSpeed)
        {
            // drawing a thread
            drawThread(targetPosition, new Vector3(transform.position.x, transform.position.y));

            // smooth yoyo movement
            transform.position = Vector3.Lerp(transform.position, targetPosition, i);

            yield return null;
        }
    }

    private void drawThread(Vector3 firstPosition, Vector3 secondPosition)
    {
        lineRenderer.SetPosition(0, firstPosition);
        lineRenderer.SetPosition(1, secondPosition);
    }
}
