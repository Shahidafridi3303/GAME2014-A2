using System.Collections.Generic;
using UnityEngine;

public class DynamicChainController : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;  // Static point (e.g., wall)
    [SerializeField] private Transform _endPoint;    // Moving platform
    [SerializeField] private GameObject _chainLinkPrefab;  // Prefab of a single chain link
    [SerializeField] private float _linkSpacing = 0.5f;    // Spacing between chain links

    private List<GameObject> _chainLinks = new List<GameObject>();  // List to manage chain links

    void Update()
    {
        // Calculate the distance and direction between the start and end points
        Vector3 direction = (_endPoint.position - _startPoint.position).normalized;
        float distance = Vector3.Distance(_startPoint.position, _endPoint.position);

        // Calculate the required number of links
        int requiredLinks = Mathf.CeilToInt(distance / _linkSpacing);

        // Adjust the chain links to match the required length
        AdjustChainLength(requiredLinks);

        // Position the chain links
        for (int i = 0; i < _chainLinks.Count; i++)
        {
            _chainLinks[i].transform.position = _startPoint.position + direction * (i * _linkSpacing);
            _chainLinks[i].transform.up = direction; // Align with the direction of the chain
        }
    }

    private void AdjustChainLength(int requiredLinks)
    {
        // Add chain links if there aren't enough
        while (_chainLinks.Count < requiredLinks)
        {
            GameObject newLink = Instantiate(_chainLinkPrefab, transform);
            _chainLinks.Add(newLink);
        }

        // Remove chain links if there are too many
        while (_chainLinks.Count > requiredLinks)
        {
            GameObject linkToRemove = _chainLinks[_chainLinks.Count - 1];
            _chainLinks.RemoveAt(_chainLinks.Count - 1);
            Destroy(linkToRemove);
        }
    }
}