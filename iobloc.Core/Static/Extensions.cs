namespace iobloc
{
    public static class Extensions
    {
        // Summary:
        //      Check if array contains value
        // Parameters: array: 
        // Parameters: val: 
        public static bool Contains<T>(this T[] array, T val)
        {
            foreach (T k in array)
                if (k.Equals(val))
                    return true;
            return false;
        }
    }
}
