using UnityEngine;

public class MP_Obstacles : MonoBehaviour
{
    [SerializeField, Range(0, 100)] float speedMovement = 10;

    private void Update() => MoveTo();
    void MoveTo() => transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right, Time.deltaTime * speedMovement);
}
