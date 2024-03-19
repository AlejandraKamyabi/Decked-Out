using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusAnimationController : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] GameObject _burnEffect;
    [SerializeField] GameObject _slowEffect;
    [SerializeField] GameObject _charmEffect;
    [SerializeField] GameObject _poisonEffect;

    [Header("Enemy Script")]
    [SerializeField] Enemy _enemyScript;
    [SerializeField] Apostate _apostateScript;
    [SerializeField] KaboomEnemy _kaboomScript;
    [SerializeField] Necromancer _necromancerScript;

    bool _basic = false;
    bool _kaboom = false;
    bool _apostate = false;
    bool _necromancer = false;
    float _yPos;
    float _upscaleDuration = 0.5f;
    float _downscaleDuration = 0.25f;

    SpriteRenderer _burnRenderer;
    SpriteRenderer _slowRenderer;
    SpriteRenderer _charmRenderer;
    SpriteRenderer _poisonRenderer;

    [SerializeField] Vector3 _burnScale;
    [SerializeField] Vector3 _slowScale;
    [SerializeField] Vector3 _charmScale;
    [SerializeField] Vector3 _poisonScale;


    int frozenStateFrames;
    int frozenStateFrameThreshold = 10;

    private void Start()
    {
        if (_enemyScript != null)
        {
            _basic = true;
        }
        else if (_kaboomScript != null)
        {
            _kaboom = true;
        }
        else if (_apostateScript != null)
        {
            _apostate = true;
        }
        else if (_necromancerScript != null)
        {
            _necromancer = true;
        }
        else
        {
            Debug.LogError("No type of enemy script found.");
        }

        _burnRenderer = _burnEffect.GetComponent<SpriteRenderer>();
        ScaleDown(_burnEffect, _burnScale);
        _slowRenderer = _slowEffect.GetComponent<SpriteRenderer>();
        ScaleDown(_slowEffect, _slowScale);
        //_charmRenderer = _charmEffect.GetComponent<SpriteRenderer>();
       // ScaleDown(_charmEffect);
       // _poisonRenderer = _poisonEffect.GetComponent<SpriteRenderer>();
       // ScaleDown(_poisonEffect);
    }
    private void Update()
    {
        if (_basic)
        {
            if (_enemyScript.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
            }
            else if (_enemyScript.isBurning)
            {
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUp(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            if (_enemyScript.isFrozen)
            {
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUp(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!_enemyScript.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDown(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }
                
            }
           
        }
    }

    private void ScaleDown(GameObject effectObject, Vector3 effectScale)
    {
        StartCoroutine(ScaleDownCoroutine(effectObject, effectScale));
    }
    private IEnumerator ScaleDownCoroutine(GameObject effectObject, Vector3 effectScale)
    {
        Vector3 originalScale = effectScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0;

        while (elapsedTime < _downscaleDuration)
        {
            float ratio = elapsedTime / _downscaleDuration;
            effectObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, ratio);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectObject.transform.localScale = targetScale;
        effectObject.SetActive(false);
    }

    private void ScaleUp(GameObject effectObject, Vector3 scale)
    {
        effectObject.SetActive(true);
        StartCoroutine(ScaleUpCoroutine(effectObject, scale));
    }
    private IEnumerator ScaleUpCoroutine(GameObject effectObject, Vector3 targetScale)
    {
        Vector3 originalScale = effectObject.transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < _upscaleDuration)
        {
            float ratio = elapsedTime / _upscaleDuration;
            effectObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, ratio);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectObject.transform.localScale = targetScale;
    }

    private void UpdateSortingOrder(SpriteRenderer effectRenderer)
    {
        _yPos = transform.position.y;
        _yPos = -_yPos;
        if (_yPos < 0)
        {
            effectRenderer.sortingOrder = (int)(_yPos * 100) + 2;
        }
        else
        {
            effectRenderer.sortingOrder = (int)(_yPos * 100) + 1;
        }
    }

}
