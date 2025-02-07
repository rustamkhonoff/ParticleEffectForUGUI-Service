using System;
using UnityEngine;

namespace UIParticle.Service
{
    [Serializable]
    public class PositionInfo
    {
        public Func<Vector3> PositionFunc { get; set; }
        public SpaceType Space { get; set; }
        public bool UpdatePositionOnUpdate { get; set; }

        public enum SpaceType
        {
            World = 0,
            UI = 1
        }

        public PositionInfo() { }

        public PositionInfo(Vector3 vector3, SpaceType space = SpaceType.UI)
        {
            Space = space;
            PositionFunc = () => vector3;
        }

        public PositionInfo(Transform transform, SpaceType space = SpaceType.World, bool updatePositionOnUpdate = true)
        {
            PositionFunc = () => transform.position;
            UpdatePositionOnUpdate = updatePositionOnUpdate;
            Space = space;
        }

        public PositionInfo(RectTransform rectTransform, SpaceType space = SpaceType.UI, bool updatePositionOnUpdate = true)
        {
            PositionFunc = () => rectTransform.position;
            UpdatePositionOnUpdate = updatePositionOnUpdate;
            Space = space;
        }

        public PositionInfo(Func<Vector3> positionFunc, SpaceType space = SpaceType.UI, bool updatePositionOnUpdate = true)
        {
            PositionFunc = positionFunc;
            UpdatePositionOnUpdate = updatePositionOnUpdate;
            Space = space;
        }

        public override string ToString()
        {
            return $"Pos: {PositionFunc()}, Space: {Space}, Update: {UpdatePositionOnUpdate}";
        }

        public static implicit operator PositionInfo(Vector3 vector3) => new(vector3);
        public static implicit operator PositionInfo(Func<Vector3> vector3) => new(vector3);
        public static implicit operator PositionInfo(RectTransform rectTransform) => new(rectTransform);
        public static implicit operator PositionInfo(Transform transform) => new(transform);
        public static PositionInfo ScreenCenter => new(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
    }
}