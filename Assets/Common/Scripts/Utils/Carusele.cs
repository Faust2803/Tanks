using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class Carusele
    {
        private float _spacing = 10f;
        private float _duration = 0.5f;
        private bool _isMoving;
        private List<Transform> _items;
        private int _visibleOnLeft = 1;
        private int _visibleOnRight = 1;
        private int _currentIndex;
        
        public int CurrentIndex => _currentIndex;
        public List<Transform> Items
        {
            get => _items;
            set => _items = value;
        }

        public void Start()
        {
            UpdatePositions(false);
        }

        public void Next()
        {
            if (_isMoving || _items.Count <= 1)
                return;

            _isMoving = true;

            _currentIndex++;

            if (_currentIndex >= _items.Count)
                _currentIndex = 0;

            UpdatePositions(true);
        }

        public void Previous()
        {
            if (_isMoving || _items.Count <= 1)
                return;

            _isMoving = true;

            _currentIndex--;

            if (_currentIndex < 0)
                _currentIndex = _items.Count - 1;

            UpdatePositions(true);
        }

        public void SetCurrentIndex(int newIndex)
        {
            _currentIndex = newIndex;
        }

        private void UpdatePositions(bool animate)
        {
            int count = _items.Count;

            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < count; i++)
            {
                Transform item = _items[i];

                if (item == null)
                    continue;

                int offset = GetOffset(i);

                // Если объект слишком далеко от центра,
                // ставим его сразу на противоположную сторону.
                bool visible =
                    offset >= -_visibleOnLeft &&
                    offset <= _visibleOnRight;

                Vector3 targetPosition = GetPosition(offset);

                if (!visible)
                {
                    // Объект находится за пределами видимой области.
                    // Ставим его в позицию, соответствующую циклу.
                    item.localPosition = targetPosition;
                    continue;
                }

                if (animate)
                {
                    sequence.Join(
                        item.DOLocalMove(
                            targetPosition,
                            _duration
                        )
                        .SetEase(Ease.OutCubic)
                    );
                }
                else
                {
                    item.localPosition = targetPosition;
                }
            }

            if (animate)
            {
                sequence.OnComplete(() =>
                {
                    _isMoving = false;
                });
            }
            else
            {
                _isMoving = false;
            }
        }

        private int GetOffset(int index)
        {
            int offset = index - _currentIndex;

            int count = _items.Count;

            // Бесконечный цикл справа → слева
            if (offset > count / 2)
                offset -= count;

            // Бесконечный цикл слева → справа
            if (offset < -count / 2)
                offset += count;

            return offset;
        }

        private Vector3 GetPosition(int offset)
        {
            return new Vector3(
                offset * _spacing,
                0f,
                0f
            );
        }

        public void AddItem(Transform item)
        {
            Items.Add(item);
            UpdatePositions(false);
        }
    }
}