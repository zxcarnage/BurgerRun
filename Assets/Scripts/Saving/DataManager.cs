using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerPrefs = UnityEngine.PlayerPrefs;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string Language { get; private set; } = "ru";
    private string _path;
    private const string FILE_NAME = "BurgerRunPlayerData.json";

    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerSkin _playerSkin;
    [SerializeField] private ShopSkins _skins;
    [SerializeField] private ShopSkins _startSkins;
    [SerializeField] private Currensy _currensy;
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private Skin _defaultSkin;
    [SerializeField] private List<Skin> _allAvailableSkins;


    public int CurrentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + FILE_NAME;

        _currensy.ValueUpdated += OnCurrensyValueUpdated;
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        LoadSave(() => LoadScene());
        yield break;
#endif
        yield return YandexGamesSdk.Initialize(null, false);
        Language = YandexGamesSdk.Environment.i18n.tld;
        LoadSave(() => LoadScene());
    }

    private void OnCurrensyValueUpdated()
    {
        SaveData();
    }

    private void LoadScene()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene(1);
        return;
#endif
        Billing.GetPurchasedProducts((x) =>
        {
            AnalyzeProducts(x.purchasedProducts, () =>
            {
                Debug.Log("Save data");
                SaveData();
                SceneManager.LoadScene(1);
            });
        }, (x) =>
        {
            Debug.Log(x);
            SaveData();
            SceneManager.LoadScene(1);
        });
    }
    

    private void AnalyzeProducts(PurchasedProduct[] purchasedProducts, Action onSuccessCallback = null)
    {
        if(purchasedProducts == null || purchasedProducts.Length == 0)
            onSuccessCallback?.Invoke();
        foreach (var purchasedProduct in purchasedProducts)
        {
            Debug.Log("Purchased product:\n " + purchasedProduct + "\nid + " + purchasedProduct.productID);
            if(purchasedProduct.productID == "NoAds")
                Billing.ConsumeProduct(purchasedProduct.purchaseToken, () =>
                {
                    Debug.Log("In no ads");
                    _playerStats.SetNoAds(true);
                    onSuccessCallback?.Invoke();
                });
            else if(purchasedProduct.productID == "PunchUpgrade")
                Billing.ConsumeProduct(purchasedProduct.purchaseToken, () =>
                {
                    Debug.Log("In punch upgrade");
                    _playerStats.IncreasePushStrength(3);
                    onSuccessCallback?.Invoke();
                });
            else if(purchasedProduct.productID == "DistanceUpgrade")
                Billing.ConsumeProduct(purchasedProduct.purchaseToken, () =>
                {
                    Debug.Log("In distance upgrade");
                    _playerStats.IncreaseDistance(0.4f);
                    onSuccessCallback?.Invoke();
                });
            else if(purchasedProduct.productID == "UnlockAll")
                Billing.ConsumeProduct(purchasedProduct.purchaseToken, () =>
                {
                    Debug.Log("In unlock all");
                    _skins.UnlockAll();
                    onSuccessCallback?.Invoke();
                });
            else if(purchasedProduct.productID == "single")
                Billing.ConsumeProduct(purchasedProduct.purchaseToken, () =>
                {
                    Debug.Log("In unlock random");
                    _skins.UnlockRandom();
                    onSuccessCallback?.Invoke();
                });
            
        }
    }

    public void DebugResetSavings()
    {
        CreateNewSave();
        SaveData();
        SceneManager.LoadScene(1);
    }

    private void CreateNewSave()
    {
        PlayerData playerData = new PlayerData();
        _skins.CreateNew(_startSkins.SkinPages);
        Debug.Log("Created new save");
        SetData(playerData);
    }

    private void LoadSave(Action onSuccessCallback = null)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        LoadDataFromFile(onSuccessCallback);
#elif UNITY_WEBGL || !UNITY_EDITOR
        if(PlayerAccount.IsAuthorized == false)
            LoadDataFromPlayerPrefs(onSuccessCallback);
        else
            LoadDataFromCloud(onSuccessCallback);
#endif
    }

    private void LoadDataFromPlayerPrefs(Action onSuccessCallback = null)
    { 
        Debug.Log($"PlayerPrefs.GetString({FILE_NAME}): {PlayerPrefs.GetString(FILE_NAME)}");
        LoadData(PlayerPrefs.GetString(FILE_NAME), onSuccessCallback);
    }
    
    private void LoadDataFromCloud(Action onSuccessCallback = null)
    {
        PlayerAccount.GetCloudSaveData((data) => LoadData(data, onSuccessCallback));
    }
    
    private void LoadData(string data, Action onSuccessCallback = null)
    {
        Debug.Log("Updloaded data " + data);
        var json = data;
        if (Encoder.IsBase64String(data))
            json = Encoder.Base64Decode(data);
        Debug.Log("JSON = " + json);
        if (string.IsNullOrEmpty(json) || json == "{}")
            CreateNewSave();
        else
            DecodeJson(json); 
        Debug.Log(_skins.SkinPages.PagesList.Count);
        Debug.Log(_skins.SkinPages.PagesList[0].Skins.Count);
        onSuccessCallback?.Invoke();
    }
    
    private void LoadDataFromFile(Action onSuccessCallback = null)
    {
        Debug.Log("Loading from Path + " + _path);
        if (File.Exists(_path))
            LoadData(File.ReadAllText(_path));
        else
            CreateNewSave();
        onSuccessCallback?.Invoke();
    }

    private string EncodeAllInJson()
    {
        var skinPagesJson = _skins.SkinPages.ToJson();
        PlayerData data = new PlayerData(_playerStats, _currensy, CurrentLevel, _playerSkin.Skin, _playerStats.MoneyProgression);
        var playerStatsJson = JsonUtility.ToJson(data);
        var jsonsss = JsonHelper.ToJson<string>(new []{skinPagesJson,playerStatsJson});
        return jsonsss;
    }

    private void DecodeJson(string json)
    {
        Debug.Log("Decoding");
        string[] fromJson = JsonHelper.FromJson<string>(json);
        var skinPagesJson = fromJson[0];
        var playerStats = fromJson[1];
        _skins.SkinPages.FromJson(skinPagesJson);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerStats);
        SetData(playerData);
        Debug.Log("Decoded");
    }
    

    public void SaveData(Action onSuccessCallback = null)
    {
        Debug.Log(onSuccessCallback);
#if !UNITY_WEBGL || UNITY_EDITOR
        SaveDataInFile();
#elif !UNITY_EDITOR || UNITY_WEBGL
        if(PlayerAccount.IsAuthorized == false)
            SaveDataInPlayerPrefs(onSuccessCallback);
        else
            SaveDataOnCloud(onSuccessCallback);
#endif
    }

    private void SaveDataInPlayerPrefs(Action onSuccessCallback = null)
    {
        var json = EncodeAllInJson();
        string encodedJson = Encoder.Base64Encode(json);
        Debug.Log("Key: " + FILE_NAME + " Encoded json: " + encodedJson);
        PlayerPrefs.SetString(FILE_NAME, encodedJson);
        PlayerPrefs.Save();
    }
    
    private void SaveDataInFile()
    {
        Debug.Log("Saving Data in file");
        Debug.Log("Path + " + _path);
        var json = EncodeAllInJson();
        File.WriteAllText(_path, json);
    }
    
    private void SaveDataOnCloud(Action onSuccessCallback = null)
    {
        var json = EncodeAllInJson();
        PlayerAccount.SetCloudSaveData(json, () =>
        {
            Debug.Log("INVOKING INVOKE !!!!!!!!!!!!\n\n\n\n\n\n\n" + onSuccessCallback);
            onSuccessCallback?.Invoke();
            Debug.Log("Succesfully saved data " + json);
        }, (x) => Debug.Log("Failed to save " + x));
    }
    

    private void SetData(PlayerData data)
    {
        _playerStats.SetMaxDistance(data.MaxDistance);
        _playerStats.SetPushStrenght(data.PushStrenght);
        _playerStats.SetNoAds(data.NoAds);
        _playerStats.SetMoneyProgression(data.MoneyProgresion);
        Debug.Log("Is saved skin empty ");
        Debug.Log(data.PlayerSkin == null);
        //Debug.Log("Saved skin instance ID" + data.PlayerSkin.GetInstanceID());
        if (CheckIfSkinIsIntact(data.PlayerSkin))
        {
            Debug.Log("Skin ID intact");
            _playerSkin.ChangeSkin(data.PlayerSkin);
        }
        else
        {
            Debug.Log("Skin ID is not intact");
            _playerSkin.ChangeSkin(_defaultSkin);
        }
        _currensy.SetStartingMoney(data.Money);
        CurrentLevel = data.CurrentLevel;
    }

    private bool CheckIfSkinIsIntact(Skin skin)
    {
        if (skin == null) return false;
        if (_allAvailableSkins.FirstOrDefault(n => n.GetInstanceID() == skin.GetInstanceID()) == null) return false;
        return true;
    }
}
