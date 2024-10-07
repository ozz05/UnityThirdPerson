using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _drag = 0.3f;
    private Vector3 _impact;
    private Vector3 _dampingVelocity;
    private float _verticalVelocity;

    public Vector3 Movement => _impact + (Vector3.up * _verticalVelocity);
    void Update()
    {
        if (_verticalVelocity < 0 && _controller.isGrounded)
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);
        if(_impact.sqrMagnitude < 0.2f * 0.2f && _agent != null)
        {
            _impact = Vector3.zero;
            _agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        _impact += force;
        if(_agent != null)
        {
            _agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        _verticalVelocity += jumpForce;
    }
}
