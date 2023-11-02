using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class LevelVizualization : MonoBehaviour
{
    [SerializeField] protected TMP_Text _text;
    [SerializeField] protected Image _image;
    protected SpriteState _state = SpriteState.Disabled;

    public SpriteState State => _state;

    public abstract void InitializeText(int elementOrder);
    public abstract void InitializeImage();

    public virtual void Activate()
    {
        _state = SpriteState.Active;
    }

    public virtual void Deactivate()
    {
        _state = SpriteState.Disabled;
    }
}

public enum SpriteState
{
    Active,
    Disabled
}
