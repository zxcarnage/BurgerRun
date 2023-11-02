using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LabelChanger : MonoBehaviour
{
    [SerializeField] private string[] _labelNames;
    [SerializeField] private string[] _englishLabelNames;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private PaginationScroll _paginationScroll;

    private void OnEnable()
    {
        _paginationScroll.PageChanged += OnPageChanged;
    }

    private void OnDisable()
    {
        _paginationScroll.PageChanged -= OnPageChanged;
    }

    private void OnPageChanged(int currentPage)
    {
        _text.text = DataManager.Instance.Language == "ru" ? _labelNames[currentPage - 1] : _englishLabelNames[currentPage - 1];
    }
}
