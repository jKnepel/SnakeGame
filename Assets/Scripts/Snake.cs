using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
	[SerializeField] private GameObject _snakeBodyPrefab;
	[SerializeField] private int _maxLength;
	[SerializeField] private float _speed = 1;
	[SerializeField] private float _minDistanceToLastPos = 0.5f;

	private readonly List<Transform> _bodyParts = new();
	private readonly List<Vector3> _bodyPositions = new();

	private LayerMask _layerMask;

	private void Start()
	{
		_layerMask = LayerMask.NameToLayer("Collectible");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
			transform.rotation = Quaternion.Euler(0, 0, 0);
		else if (Input.GetKeyDown(KeyCode.DownArrow))
			transform.rotation = Quaternion.Euler(0, -180, 0);
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			transform.rotation = Quaternion.Euler(0, -90, 0);
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			transform.rotation = Quaternion.Euler(0, 90, 0);

		transform.position += _speed * Time.deltaTime * transform.forward;
	}

	private void LateUpdate()
	{
		if (_bodyPositions.Count == 0)
			return;

		Vector3 lastPos = _bodyPositions[^1];
		if (Mathf.Abs(lastPos.x - transform.position.x) > _minDistanceToLastPos
			|| Mathf.Abs(lastPos.z - transform.position.z) > _minDistanceToLastPos)
		{
			_bodyPositions.Add(transform.position);
			if (_bodyPositions.Count > _bodyParts.Count + 1)
				_bodyPositions.RemoveAt(0);
		}

		for (int i = 0; i < _bodyParts.Count; i++)
			_bodyParts[i].position = _bodyPositions[i];
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.layer.Equals(_layerMask) && (_maxLength != 0 || _bodyParts.Count >= _maxLength))
			return;

		var bodyPart = Instantiate(_snakeBodyPrefab, transform.position, Quaternion.identity).transform;
		_bodyParts.Add(bodyPart);
		_bodyPositions.Add(transform.position);
		_speed += 0.5f;
		Destroy(other.gameObject);
	}
}
