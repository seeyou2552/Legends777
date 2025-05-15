using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private Vector3 shakeOffset = Vector3.zero;
    private Coroutine shakeCoroutine;

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 pos = transform.position;
        Vector3 targetPos = target.position;
        pos.x = Mathf.Clamp(targetPos.x, -15.4f, 15.4f); // 카메라 움직임 제한
        pos.y = Mathf.Clamp(targetPos.y, -8f, 7.25f);

        transform.position = pos + shakeOffset;
    }

    public void StartShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(Shake(5f, 0.2f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            shakeOffset = new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}
