namespace LakhiPractice // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var inputArray = new int[] { 2, 3, 6, 6, 8, 9, 10, 10, 10, 12, 12 };

            var outputArray = RemoveDuplicates(inputArray);

            outputArray.ToList().ForEach(v => Console.WriteLine(v));

            ///Console.WriteLine(outputArray.Length);

            Console.ReadLine();
        }

        private static int[] RemoveDuplicates(int[] inputArray)
        {
            var j = 0;
            var i = 1;

            while (i < inputArray.Length)
            {
                if (inputArray.Take(j + 1).Contains(inputArray[i]))
                {
                    i++;
                }
                else
                {
                    inputArray[++j] = inputArray[i++];
                }
            }

            var outputArray = inputArray.Take(j + 1).ToArray();

            return outputArray;
        }
    }
}
