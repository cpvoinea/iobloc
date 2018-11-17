namespace iobloc
{
    public interface ITableAI
    {
        // Summary:
        //      Get move for current game configuration and dice
        // Parameters:
        //   lines:
        //      array of length=28,
        //      0-23 are game lines from player perspective, 0 being the smallest pip count,
        //      24 is the line with captured player pieces, 25 with captured opponent pieces,
        //      26 is the line with taken out player pices, 27 with opponent taken out pices
        //   dice:
        //      array of length 1..4 with available dice,
        //      for doubles there is a value for each available double to a maximum of 4
        // Returns:
        //      an array of length 3: [from_line_index, to_line_index, used_dice_value]
        int[] NextMove(int[] lines, int[] dice);
    }
}