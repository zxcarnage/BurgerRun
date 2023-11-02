using UnityEngine;

public class TwerkBattle : MonoBehaviour, IEnding
{
    [SerializeField] private TwerkInsides _insides;
    
    private FinishLine _finishLine;

    private void Start()
    {
        _insides.Initialize();
        _finishLine.FinishLineCrossed += OnFinishLineCrossed;
    }

    private void OnDisable()
    {
        _finishLine.FinishLineCrossed -= OnFinishLineCrossed;
    }

    public void Spawn(Vector3 original)
    {
        _finishLine = _insides.GetFinishLine();
        PositionateEnding(original);
    }

    private void PositionateEnding(Vector3 original)
    {
        var delta = _finishLine.LineModel.position.z - transform.position.z;
        transform.position = new Vector3(transform.position.x,transform.position.y,original.z + Mathf.Abs(delta));
    }

    private void OnFinishLineCrossed(Player player)
    {
        DisableInGameUI();
        _insides.Spawn();
    }

    private void DisableInGameUI()
    {
        GameManager.Instance.DisableSlider();
        ServiceLocator.InGameUI.HideStat();
    }
}
