using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatformHalo : MonoBehaviour
{
    [SerializeField] float _fadeDuration;
    [Range(0.1f, 1f)]
    [SerializeField] float _maxAlpha = 1f;
    [Range(0.1f, 1f)]
    [SerializeField] float _minAlpha = 0.75f;
    [SerializeField] float _dragScale;
    [SerializeField] float _scaleDuration;

    float _scaleUpTimer;
    float _scaleDownTimer;
    float _currentScale;
    float _alpha;
    SpriteRenderer _spriteRenderer;
    bool _isDragging;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _alpha = _spriteRenderer.color.a;
    }

    public void IsDragging(bool isDragging)
    {
        _isDragging = isDragging;
    }

    private void Update()
    {
        if (_spriteRenderer != null)
        {
            float alphaSIN = Mathf.Sin((Time.time / _fadeDuration) * Mathf.PI * 2);
            float t = (alphaSIN + 1) / 2;
            _alpha = Mathf.Lerp(_maxAlpha, _minAlpha, t);
            Color newColor = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _alpha);
            _spriteRenderer.color = newColor;
        }
        if (_isDragging)
        {
            // Reset scale down timer when starting to drag
            _scaleDownTimer = 0.0f;
            if (_currentScale < _dragScale)
            {
                _scaleUpTimer += Time.deltaTime;
                float t = Mathf.Clamp01(_scaleUpTimer / _scaleDuration);
                _currentScale = Mathf.Lerp(1, _dragScale, t);
            }
        }
        else
        {
            // Reset scale up timer when stopping dragging
            _scaleUpTimer = 0.0f;
            if (_currentScale > 1.0f)
            {
                _scaleDownTimer += Time.deltaTime;
                float t = Mathf.Clamp01(_scaleDownTimer / _scaleDuration);
                _currentScale = Mathf.Lerp(_dragScale, 1, t);
            }
        }

        // Apply the scaling outside the conditions to ensure smooth transition
        _spriteRenderer.transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
    }
}
