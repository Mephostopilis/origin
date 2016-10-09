using UnityEngine;

namespace Bacon
{
    class PCBall : Ball
    {
        public PCBall(GameObject o, float radis) : base(o, radis)
        {
        }

        public void SyncPosition(Vector3 position)
        {
            _ball.transform.position = position;
            CalArrowPosition();   
        }

        public void SyncDirection(Vector3 direction)
        {
            _direction = direction;
            CalArrowDirection();
        }

        public void Sync(Vector3 position, Vector3 direction)
        {
            SyncPosition(position);
            SyncDirection(direction);
        }
    }
}
