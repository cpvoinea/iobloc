namespace iobloc.Ascio
{
    static class WalkAnimation
    {
        internal static readonly string CLD =
        "  __________" +
        " /  -  -___/" +
        "<__  - /    " +
        "   \\__/     ";

        internal static readonly char[] GND = { '_', '.', ',', ';', '/', '\\', '|' };

        internal static readonly string[] ACT = {
            " ')) " +
            "  |  " +
           "\\/|\\ " +
            "  | /" +
            " (|  " +
            "  |\\ ",

            " ')) " +
            "  |  " +
            " /|\\ " +
            "/ | \\" +
            " ( \\ " +
            "  \\ \\",

            "('.')" +
            "  |  " +
            " /|\\ " +
            "/ | \\" +
            " / \\ " +
            "/   \\",

            " ((' " +
            "  |  " +
            " /|\\ " +
            "/ | \\" +
            " / ) " +
            "/ /  ",

            " ((' " +
            "  |  " +
            " /|\\/" +
            " \\|  " +
            "  |) " +
            " /|  ",
        };
    }
}
