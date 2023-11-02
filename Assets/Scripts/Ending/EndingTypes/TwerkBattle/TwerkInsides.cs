using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class TwerkInsides : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private FinishLine _finish;
    [SerializeField] private PlayerTwerkBattle _playerTwerkBattle;
    [SerializeField] private EnemyTwerkBattle _enemyTwerkBattle;
    [SerializeField] private TwerkBattleUI _twerkBattleUI;
    
    
    public void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void OnEnable()
    {
        _playerTwerkBattle.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        _playerTwerkBattle.Died -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        _twerkBattleUI.gameObject.SetActive(false);
    }
    
    public void Initialize()
    {
        _playerTwerkBattle.Initialize();
    }

    private void SpawnParticipants()
    {
        _playerTwerkBattle.GiveEnemy(_enemyTwerkBattle);
        _playerTwerkBattle.Positionate();
        _enemyTwerkBattle.GiveEnemy(_playerTwerkBattle);
        _enemyTwerkBattle.Positionate();
    }

    private IEnumerator SpawnRoutine()
    {
        ChangeCamera();
        yield return new WaitForSeconds(2f);
        SpawnParticipants();
        _twerkBattleUI.Show();
    }

    private void ChangeCamera()
    {
        _playerCamera.Priority = 500;
    }

    public FinishLine GetFinishLine()
    {
        return _finish;
    }
}
