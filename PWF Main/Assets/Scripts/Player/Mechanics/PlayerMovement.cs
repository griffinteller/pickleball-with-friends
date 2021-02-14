using System;
using Player.Networking;
using UnityEngine;

namespace Player.Mechanics
{
    public class PlayerMovement : MonoBehaviour
    {
        public Paddle paddle;
        public float servingRadius;
        public Vector3 servingPivot;
        public float baseRadius;
        public Vector3 basePivot;
        public float baseSpeed;
        public float lerpTime;

        private GameDirector _gameDirector;
        
        private Vector3 _actualPlayerPos;
        private Vector3 _actualPaddlePivot;
        private float _actualPivotRadius;
        
        private Vector3 _targetPlayerPos;
        private Vector3 _targetPaddlePivot;
        private float _targetPivotRadius;

        private Vector3 _playerPosVel;
        private Vector3 _paddlePivotVel;
        private float _paddleRadiusVel;

        public void Start()
        {
            _gameDirector = FindObjectOfType<GameDirector>();
        }

        private void SetActualPaddlePosition(Vector3 pivot, float radius)
        {
            paddle.transform.localPosition = pivot;
            paddle.PivotRadius = radius;
        }

        public void FixedUpdate()
        {
            UpdatePos();
        }

        private void UpdatePos()
        {
            _actualPaddlePivot = Vector3.SmoothDamp(
                _actualPaddlePivot,
                _targetPaddlePivot,
                ref _paddlePivotVel,
                lerpTime,
                Mathf.Infinity,
                Time.fixedDeltaTime);

            _actualPivotRadius = Mathf.SmoothDamp(
                _actualPivotRadius,
                _targetPivotRadius,
                ref _paddleRadiusVel,
                lerpTime,
                Mathf.Infinity,
                Time.fixedDeltaTime);
            
            _actualPlayerPos = Vector3.SmoothDamp(
                _actualPlayerPos,
                _targetPlayerPos,
                ref _playerPosVel,
                lerpTime,
                Mathf.Infinity,
                Time.fixedDeltaTime);
            
            SetActualPaddlePosition(_actualPaddlePivot, _actualPivotRadius);
        }

        public void GoToServingPos()
        {
            _targetPlayerPos = _gameDirector.thisPlayerNum == 1
                ? _gameDirector.playerOneStartingPos
                : _gameDirector.playerTwoStartingPos;

            _targetPaddlePivot = servingPivot;
            _targetPivotRadius = servingRadius;
        }
    }
}