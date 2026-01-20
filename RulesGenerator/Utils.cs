namespace RulesGenerator
{
    internal class Utils
    {
        public static string ToCommentLine(string comment)
        {
            if (comment == "") return "";
            string[] split = comment.Split('\n');
            string compiled = "";
            foreach (string line in split)
            {
                compiled += "# " + line + "\n";
            }
            return compiled;
        }

        public static string ToFeetAndInches(double meters)
        {
            int inches = (int)Math.Round(meters * 39.3700787);
            //TODO maybe make this be better?
            return (inches / 12).ToString() + "ft " + (inches % 12).ToString() + "in";
        }
    }
}
