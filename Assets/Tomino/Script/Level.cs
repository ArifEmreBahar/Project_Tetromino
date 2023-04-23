using System;

namespace Tomino
{
    public class Level
    {
        public int Number => Lines / 3 + 1;

        public float FallDelay => Math.Max(0.2f, 1.0f - ((Number - 1) * 0.1f));

        public int Lines { get; private set; }

        public void RowsCleared(int count) => Lines += count;
    }
}