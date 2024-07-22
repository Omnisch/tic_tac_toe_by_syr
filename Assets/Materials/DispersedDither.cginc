float2 DispersedDither(float frequency, int frameCount)
{
    float distort = (uint)(_Time.y / frequency) % frameCount;
    float2 cycle = float2(cos(distort), sin(distort + 3.14));
    return float2(cycle.x, cycle.y);
}
