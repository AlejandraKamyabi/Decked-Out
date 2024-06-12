using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScreenManager : MonoBehaviour
{
    public static TransitionScreenManager Instance { get; private set; }
    
    [SerializeField] float _transitionDuration;
    [SerializeField] float _fadeOutDuration;

    [SerializeField] GameObject _leftParent;
    [SerializeField] Transform _leftTarget;
    [SerializeField] GameObject _rightParent;
    [SerializeField] Transform _rightTarget;

    [SerializeField] List<GameObject> _objectsToFade = new List<GameObject>();
    [SerializeField] List<GameObject> _objectsToEnhance = new List<GameObject>();

    List<Image> _imagesToFade;
    List<Transform> _imagesToEnhance;
    List<Color> _colorOfImagesToFade;
    List<Image> _childImages;
    List<Color> _colorOfChildImages;

    Vector3 _startPostionLeft;
    Vector3 _startPostionRight;

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
        _startPostionRight = _rightParent.transform.position;
        _colorOfImagesToFade = new List<Color>();
        _imagesToFade = new List<Image>();
        _childImages = new List<Image>();
        _colorOfChildImages = new List<Color>();
        GetChildrenOfTransParents(_rightParent.transform);
        GetChildrenOfTransParents(_leftParent.transform);
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
            //_imagesToEnhance[i] = _objectsToEnhance[i].GetComponent<Transform>();
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

            for (int i = 0; i < _imagesToFade.Count; i++)
            {
                float newAlpha = Mathf.Lerp(_colorOfImagesToFade[i].a, 0f, t);
                Color newColor = new Color(_colorOfImagesToFade[i].r, _colorOfImagesToFade[i].g, _colorOfImagesToFade[i].b, newAlpha);
                _imagesToFade[i].color = newColor;
            }

            if (t >= 1f)
            {
                _loading = false;
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
                _leftParent.transform.position = _startPostionLeft;
                _rightParent.transform.position = _startPostionRight;
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

    private void GetChildrenOfTransParents(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                _childImages.Add(image);
            }
        }
        for (int i = 0; i < _childImages.Count; i++)
        {
            _colorOfChildImages.Add(_childImages[i].color);
        }
    }
}
