namespace rhythm
{
    public class RhythmResult
    {
        public readonly int HitTime;
        public readonly int Offset;
        public readonly TimingResult Quality;

        public RhythmResult(int hitTime, int offset, TimingResult quality)
        {
            HitTime = hitTime;
            Offset = offset;
            Quality = quality;
        }
    }
}