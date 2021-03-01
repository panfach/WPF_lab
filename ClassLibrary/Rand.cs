using System;

public static class Rand
{
    static Random rnd = new Random();
    public static double Double(double min, double max) { return rnd.NextDouble() * (max - min) + min; }
    public static double Double(double max) { return rnd.NextDouble() * max; }
}
