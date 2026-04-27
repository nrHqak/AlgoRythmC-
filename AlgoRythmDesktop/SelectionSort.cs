using System.Collections.Generic;
using System.Linq;

namespace AlgoRythmDesktop
{
    public class SelectionSort : Algorithm
    {
        public SelectionSort()
        {
            Name = "Selection Sort";
            Complexity = "O(n²)";
            Hint = "Selection Sort находит минимальный элемент в неотсортированной части массива и помещает его в начало.";
        }

        public override List<TraceStep> GetTrace(int[] array)
        {
            List<TraceStep> trace = new List<TraceStep>();
            int n = array.Length;
            int[] currentArray = (int[])array.Clone();

            for (int i = 0; i < n - 1; i++)
            {
                int min_idx = i;
                for (int j = i + 1; j < n; j++)
                {
                    trace.Add(new TraceStep(currentArray, min_idx, j, StepType.Compare, $"Шаг {trace.Count + 1}: сравниваем {currentArray[min_idx]} и {currentArray[j]}"));
                    if (currentArray[j] < currentArray[min_idx])
                    {
                        min_idx = j;
                    }
                }

                if (min_idx != i)
                {
                    int temp = currentArray[min_idx];
                    currentArray[min_idx] = currentArray[i];
                    currentArray[i] = temp;
                    trace.Add(new TraceStep(currentArray, min_idx, i, StepType.Swap, $"Шаг {trace.Count + 1}: меняем {currentArray[min_idx]} и {currentArray[i]} местами"));
                }
            }
            trace.Add(new TraceStep(currentArray, -1, -1, StepType.Done, "Сортировка завершена."));
            return trace;
        }
    }
}
