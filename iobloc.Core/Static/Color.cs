namespace iobloc
{
    public enum NativeColor : short
    {
        Black = 0,
        ForegroundBlue = 0x1,
        ForegroundGreen = 0x2,
        ForegroundRed = 0x4,
        ForegroundYellow = 0x6,
        ForegroundIntensity = 0x8,
        BackgroundBlue = 0x10,
        BackgroundGreen = 0x20,
        BackgroundRed = 0x40,
        BackgroundYellow = 0x60,
        BackgroundIntensity = 0x80,

        ForegroundMask = 0xf,
        BackgroundMask = 0xf0,
        ColorMask = 0xff
    }

}
