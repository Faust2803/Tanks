using System;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class CharacterRotator2
    {
        private readonly float _rotationSpeed;

        private Transform _target;

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        private Tween _rotateTween;

        public CharacterRotator2(
            Transform target,
            float rotationSpeed)
        {
            _target = target;
            _rotationSpeed = rotationSpeed;
        }

        public void StartRotate()
        {
            if (_target == null)
                return;

            StopRotate();

            _rotateTween = _target
                .DORotate(
                    new Vector3(0f, 360f, 0f),
                    360f / _rotationSpeed,
                    RotateMode.FastBeyond360
                )
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

        public void StopRotate()
        {
            _rotateTween?.Kill();
            _rotateTween = null;
        }

        public void RotateTo(float targetY, Action onComplete = null)
        {
            if (_target == null)
                return;

            StopRotate();

            Vector3 rotation = _target.eulerAngles;
            rotation.y = targetY;

            _rotateTween = _target
                .DORotate(
                    rotation,
                    0.2f,
                    RotateMode.Fast
                )
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _rotateTween = null;
                    onComplete?.Invoke();
                });
        }

        public void Dispose()
        {
            StopRotate();
        }
    }
}