namespace StudioTGMinesweeperService.Additional
{
    public static class ArrayMethods
    {
        public static string[] CreateEmptyField(int width, int height)
        {
            string[] array = new string[height];
            for (int i = 0; i < height; i++)
            {
                array[i] = new string(' ', width);
            }

            return array;
        }

        public static char[,] ConvertTo2DCharArray(int width, int height, string[] field)
        {
            var charArray = new char[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    charArray[i, j] = field[i][j];
                }
            }
            return charArray;
        }

        public static string[] CreateStringArrayField(char[,] fieldChars, int width, int height)
        {
            string[] gameFieldString = new string[width];
            for (int i = 0; i < width; i++)
            {
                string row = string.Empty;
                for (int j = 0; j < height; j++)
                {
                    row += fieldChars[i, j];
                }
                gameFieldString[i] = row;
            }
            return gameFieldString;
        }
    }
}
