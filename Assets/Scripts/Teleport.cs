using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    [Header("Налаштування цілі")]
    public Transform destination; // точка, куди перемістити м'яч

    private static bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            if (destination != null)
            {
                StartCoroutine(DoTeleport(other.gameObject));
            }
        }
    }

    private IEnumerator DoTeleport(GameObject player)
    {
        isTeleporting = true;

        player.transform.position = destination.position;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Телепортовано до: " + destination.name);

        yield return new WaitForSeconds(0.1f);

        isTeleporting = false;
    }
}