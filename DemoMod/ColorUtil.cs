namespace DemoMod
{
    public static class ColorUtil
    {
        public static System.Drawing.Color ToSys(this Color ccColor)
        {
            int argb;
            unchecked
            {
                argb = (int)ccColor.ToInt();
            }
            return System.Drawing.Color.FromArgb(argb);
        }
    }
}