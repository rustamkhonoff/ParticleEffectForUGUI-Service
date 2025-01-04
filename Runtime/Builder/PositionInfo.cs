using System;
using UnityEngine;

namespace UIParticle.Service
{
    [Serializable]
    public class PositionInfo
    {
        public Func<Vector3> PositionFunc { get; }
        public SpaceType Space { get; }
        public bool UpdatePositionOnUpdate { get; }

        public enum SpaceType
        {
            World = 0,
            UI = 1
        }

        public PositionInfo(Vector3 vector3, SpaceType space = SpaceType.UI)
        {
            Space = space;
            PositionFunc = () => vector3;
        }

        public PositionInfo(Transform transform, SpaceType space = SpaceType.UI, bool updatePositionOnUpdate = false)
        {
            PositionFunc = () => transform.position;
            UpdatePositionOnUpdate = updatePositionOnUpdate;
            Space = space;
        }

        public PositionInfo(Func<Vector3> positionFunc, SpaceType space = SpaceType.UI, bool updatePositionOnUpdate = false)
        {
            PositionFunc = positionFunc;
            UpdatePositionOnUpdate = updatePositionOnUpdate;
            Space = space;
        }

        public override string ToString()
        {
            return $"Pos: {PositionFunc()}, Space: {Space}, Update: {UpdatePositionOnUpdate}";
        }

        public static PositionInfo ScreenCenter => new(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
    }
}