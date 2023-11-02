using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonSkinPartsInitializer : MonoBehaviour
{
    [SerializeField] private Transform _hair;
    [SerializeField] private Transform _shoes;
    [SerializeField] private Transform _body;

    private List<GameObject> _hairList;
    private List<GameObject> _shoesList;

    public SkinnedMeshRenderer Body => _body.GetComponent<SkinnedMeshRenderer>();
    public List<GameObject> HairList => _hairList;
    public List<GameObject> ShoesList => _shoesList;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        _hairList = new List<GameObject>();
        _shoesList = new List<GameObject>();
        FullfillList(_hairList, _hair);
        FullfillList(_shoesList, _shoes);
    }

    private void FullfillList(List<GameObject> list, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            list.Add(parent.GetChild(i).gameObject);
        }
    }
}
