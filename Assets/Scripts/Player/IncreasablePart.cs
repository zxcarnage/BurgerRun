using UnityEngine;

public class IncreasablePart : MonoBehaviour
{
    private Vector3 _minAssSize;

    private void Start()
    {
        _minAssSize = transform.localScale;
    }

    public void IncreasePart(Vector3 maxAssSize, float currentHealth, float maxHealth)
    {
        var newAssSize = Mathf.Lerp(_minAssSize.x, maxAssSize.x, currentHealth/maxHealth);
        transform.localScale = new Vector3(newAssSize, newAssSize, newAssSize);
    }
}
