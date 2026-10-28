using System;
using UnityEngine;

namespace Utils
{
    public class CharacterRotator
    {
        private readonly float _rotationSpeed;

        private Transform _target;

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        private bool _isRotating;
        private bool _isRotatingTo;

        private float _targetY;
        private Action _onComplete;

        private Quaternion _targetRotation;

        public CharacterRotator(
            Transform target,
            float rotationSpeed)
        {
            _target = target;
            _rotationSpeed = rotationSpeed;
        }

        /// <summary>
        /// Запускает бесконечное вращение вокруг Y.
        /// </summary>
        public void StartRotate()
        {
            if (_target == null)
                return;

            StopRotate();

            _isRotating = true;
        }

        /// <summary>
        /// Плавно поворачивает объект к указанному углу Y.
        /// </summary>
        public void RotateTo(float targetY, Action onComplete = null)
        {
            if (_target == null)
                return;

            StopRotate();

            _isRotatingTo = true;
            _targetY = targetY;
            _onComplete = onComplete;

            _targetRotation = Quaternion.Euler(0f, targetY, 0f);
        }

        /// <summary>
        /// Останавливает любое вращение.
        /// </summary>
        public void StopRotate()
        {
            _isRotating = false;
            _isRotatingTo = false;

            _onComplete = null;
        }

        /// <summary>
        /// Вызывать каждый кадр из MonoBehaviour.Update().
        /// </summary>
        public void Tick(float deltaTime)
        {
            if (_target == null)
                return;

            if (_isRotating)
            {
                Rotate(deltaTime);
            }
            else if (_isRotatingTo)
            {
                RotateToTarget(deltaTime);
            }
        }

        private void Rotate(float deltaTime)
        {
            float angle = _rotationSpeed * deltaTime;

            Quaternion rotation =
                Quaternion.AngleAxis(angle, Vector3.up);

            _target.rotation =
                _target.rotation * rotation;
        }

        private void RotateToTarget(float deltaTime)
        {
            float rotationSpeed = 180f / 0.2f;

            _target.rotation = Quaternion.RotateTowards(
                _target.rotation,
                _targetRotation,
                rotationSpeed * deltaTime
            );

            if (Quaternion.Angle(
                    _target.rotation,
                    _targetRotation) < 0.01f)
            {
                _target.rotation = _targetRotation;

                _isRotatingTo = false;

                Action callback = _onComplete;
                _onComplete = null;

                callback?.Invoke();
            }
        }

        public void Dispose()
        {
            StopRotate();
        }
    }
}