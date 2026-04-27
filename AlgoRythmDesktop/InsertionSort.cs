using System.Collections.Generic;
using System.Linq;

namespace AlgoRythmDesktop
{
    public class InsertionSort : Algorithm
    {
        public InsertionSort()
        {
            Name = "Insertion Sort";
            Complexity = "O(n²)";
            Hint = "Insertion Sort строит отсортированный массив по одному элементу за раз, вставляя каждый элемент в правильное место.";
        }

        public override List<TraceStep> GetTrace(int[] array)
        {
            List<TraceStep> trace = new List<TraceStep>();
            int n = array.Length;
            int[] currentArray = (int[])array.Clone();

            for (int i = 1; i < n; ++i)
            {
                int key = currentArray[i];
                int j = i - 1;

                trace.Add(new TraceStep(currentArray, i, j, StepType.Compare, $"Шаг {trace.Count + 1}: берем {key} и сравниваем с предыдущими элементами"));

                while (j >= 0 && currentArray[j] > key)
                {
                    currentArray[j + 1] = currentArray[j];
                    trace.Add(new TraceStep(currentArray, j + 1, j, StepType.Shift, $"Шаг {trace.Count + 1}: сдвигаем {currentArray[j]} вправо"));
                    j = j - 1;
                }
                currentArray[j + 1] = key;
                trace.Add(new TraceStep(currentArray, j + 1, -1, StepType.Swap, $"Шаг {trace.Count + 1}: вставляем {key} на правильное место"));
            }
            trace.Add(new TraceStep(currentArray, -1, -1, StepType.Done, "Сортировка завершена."));
            return trace;
        }
    }
}
