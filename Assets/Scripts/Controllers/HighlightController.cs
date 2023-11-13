using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [SerializeField] GameObject _highlighter;

    private GameObject _currentTarget;
    public void HightlightGameObject(GameObject target)
    {
        if(_currentTarget == target)
        {
            return;
        }
        _currentTarget = target;
        Vector3 _position = target.transform.position + Vector3.up * 0.5f;
        HighlightVector(_position);
    }

    public void HighlightVector(Vector3 position)
    {
        _highlighter.SetActive(true);
        _highlighter.transform.position = position; 
    }

    public void Hide()
    {
        _currentTarget = null;
        _highlighter.SetActive(false);
    }
}
