using System.Collections.Generic;
using System.Linq;

namespace AlgoRythmDesktop
{
    public class BinarySearch : Algorithm
    {
        public BinarySearch()
        {
            Name = "Binary Search";
            Complexity = "O(log n)";
            Hint = "Binary Search эффективно ищет элемент в отсортированном массиве, многократно деля диапазон поиска пополам.";
        }

        public override List<TraceStep> GetTrace(int[] array)
        {
            List<TraceStep> trace = new List<TraceStep>();
            int[] currentArray = (int[])array.Clone();

            // Check if the array is sorted for Binary Search
            bool isSorted = true;
            for (int i = 0; i < currentArray.Length - 1; i++)
            {
                if (currentArray[i] > currentArray[i + 1])
                {
                    isSorted = false;
                    break;
                }
            }

            if (!isSorted)
            {
                trace.Add(new TraceStep(currentArray, -1, -1, StepType.Done, "Ошибка: Массив не отсортирован для бинарного поиска."));
                return trace;
            }

            int left = 0;
            int right = currentArray.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                trace.Add(new TraceStep(currentArray, left, right, StepType.Compare, $"Шаг {trace.Count + 1}: сравниваем {currentArray[mid]} с искомым значением {SearchValue} (диапазон [{left}, {right}])"));

                if (currentArray[mid] == SearchValue)
                {
                    trace.Add(new TraceStep(currentArray, mid, -1, StepType.Found, $"Шаг {trace.Count + 1}: найдено значение {SearchValue} по индексу {mid}"));
                    trace.Add(new TraceStep(currentArray, -1, -1, StepType.Done, "Поиск завершен."));
                    return trace;
                }
                else if (currentArray[mid] < SearchValue)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            trace.Add(new TraceStep(currentArray, -1, -1, StepType.Done, $"Шаг {trace.Count + 1}: значение {SearchValue} не найдено."));
            return trace;
        }
    }
}
