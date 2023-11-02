using UnityEngine;

public class RouletteSegment : MonoBehaviour
{
    protected float _value;
    private CircleCollider2D boxCollider;

    public float Value => _value;

    private void Start()
    {
        boxCollider = GetComponent<CircleCollider2D>();
        UpdateColliderSize();
    }
    
    private void OnRectTransformDimensionsChange()
    {
        UpdateColliderSize();
    }
    
    private void UpdateColliderSize()
    {
        if (boxCollider != null)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 segmentSize = rectTransform.sizeDelta;
            
            boxCollider.radius = segmentSize.x;
        }
    }
}
