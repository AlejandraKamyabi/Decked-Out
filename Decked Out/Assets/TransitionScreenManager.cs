using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScreenManager : MonoBehaviour
{
    public static TransitionScreenManager Instance { get; private set; }
    
    [SerializeField] float _transitionDuration;
    [SerializeField] float _fadeOutDuration;

    [SerializeField] GameObject _title;
    [SerializeField] GameObject _leftParent;
    [SerializeField] Transform _leftTarget;
    [SerializeField] GameObject _northParent;
    [SerializeField] Transform _northTarget;
    [SerializeField] GameObject _rightParent;
    [SerializeField] Transform _rightTarget;
    [SerializeField] GameObject _southParent;
    [SerializeField] Transform _southTarget;
    [SerializeField]
    Image[] _images;

    [SerializeField] List<GameObject> _objectsToFade = new List<GameObject>();

    List<Image> _imagesToFade;
    List<Color> _colorOfImagesToFade;
    List<Image> _childImages;
    List<Color> _colorOfChildImages;

    Vector3 _startPostionNorth;
    Vector3 _startPostionLeft;
    Vector3 _startPostionRight;
    Vector3 _startPositionSouth;

    bool _loading;
    bool _fading;
    bool _twinkiling;
    string _previousScene;
    string _targetScene;
    float _transitionTimer = 0f;
    float _fadeOutTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject.transform.parent);
        }
        else
        {
            Destroy(gameObject.transform.parent);
        }
    }
    private void Start()
    {
        _startPostionLeft = _leftParent.transform.position;
        _startPostionNorth = _northParent.transform.position; 
        _startPositionSouth = _southParent.transform.position;
        _startPostionRight = _rightParent.transform.position;
        _colorOfImagesToFade = new List<Color>();
        _imagesToFade = new List<Image>();
        _childImages = new List<Image>();
        _colorOfChildImages = new List<Color>();
        AddImages();
    }
    public void StartTranistion(string loadingSceneName)
    {
        DontDestroyOnLoad(gameObject);
        _previousScene = SceneManager.GetActiveScene().name;
        _targetScene = loadingSceneName;
        for (int i = 0; i < _objectsToFade.Count; i++)
        {
            _imagesToFade.Add(_objectsToFade[i].GetComponent<Image>());
            if (_imagesToFade[i] == null)
            {
                Debug.LogError("Can't Find Image on Object to Fade");
            }
            else
            {
                _colorOfImagesToFade.Add(_imagesToFade[i].color);
            }
        }
        _loading = true;
    }
    private void Update()
    {
        if (_loading)
        {
            _transitionTimer += Time.deltaTime;
            float t = _transitionTimer / _transitionDuration;
            t = Mathf.Clamp01(t);

            _leftParent.transform.position = Vector3.Lerp(_startPostionLeft, _leftTarget.position, t);
            _rightParent.transform.position = Vector3.Lerp(_startPostionRight, _rightTarget.position, t);
            _northParent.transform.position = Vector3.Lerp(_startPostionNorth, _northTarget.position, t);
            _southParent.transform.position = Vector3.Lerp(_startPositionSouth, _southTarget.position, t);

            for (int i = 0; i < _imagesToFade.Count; i++)
            {
                float newAlpha = Mathf.Lerp(_colorOfImagesToFade[i].a, 0f, t);
                Color newColor = new Color(_colorOfImagesToFade[i].r, _colorOfImagesToFade[i].g, _colorOfImagesToFade[i].b, newAlpha);
                _imagesToFade[i].color = newColor;
            }

            if (t >= 1f)
            {
                _loading = false;
                if (!_title)
                {
                    _title.SetActive(true);
                }
                StartCoroutine(LoadSceneAsync(_targetScene));
            }
        }
        if (_fading)
        {
            Debug.Log("Starting Fade Out");
            _fadeOutTimer += Time.deltaTime;
            float t = _fadeOutTimer / _fadeOutDuration;
            t = Mathf.Clamp01(t);
            for (int i = 0; i < _childImages.Count; i++)
            {
                float newAlpha = Mathf.Lerp(_colorOfChildImages[i].a, 0f, t);
                Color newColor = new Color(_colorOfChildImages[i].r, _colorOfChildImages[i].g, _colorOfChildImages[i].b, newAlpha);
                _childImages[i].color = newColor;
            }

            if (t >= 1f)
            {
                _fading = false;
                _title.SetActive(false);
                _leftParent.transform.position = _startPostionLeft;
                _northParent.transform.position = _startPostionNorth;
                _rightParent.transform.position = _startPostionRight;
                _southParent.transform.position = _startPositionSouth;
                for (int i = 0; i < _childImages.Count; i++)
                {
                    _childImages[i].color = new Color(_colorOfChildImages[i].r, _colorOfChildImages[i].g, _colorOfChildImages[i].b, 1);
                }
                _fadeOutTimer = 0;
            }
        }
       
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            _twinkiling = true;
            yield return null;
        }
        _fading = true;

        _transitionTimer = 0f;
        _fadeOutTimer = 0f;
        _objectsToFade.Clear();
        _imagesToFade.Clear();
        _colorOfImagesToFade.Clear();
    }

    private void AddImages()
    {
        foreach (Image image in _images)
        {
            _childImages.Add(image);
        }
        for (int i = 0; i < _childImages.Count; i++)
        {
            _colorOfChildImages.Add(_childImages[i].color);
        }
    }
}
