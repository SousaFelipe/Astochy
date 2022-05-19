


namespace VadenStock.Tools
{
    public static class Clc
    {
        public static double PercentFromVal(double subject, double value)
        {
            return subject * 100 / value;
        }



        public static double ValFromPercent(double percent, double value)
        {
            return percent * value / 100;
        }
    }
}
