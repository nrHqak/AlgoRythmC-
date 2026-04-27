using System.Collections.Generic;
using System.Linq;

namespace AlgoRythmDesktop
{
    public class BubbleSort : Algorithm
    {
        public BubbleSort()
        {
            Name = "Bubble Sort";
            Complexity = "O(n²)";
            Hint = "Bubble Sort сравнивает соседние элементы и меняет их местами, если они находятся в неправильном порядке. Процесс повторяется до тех пор, пока массив не будет отсортирован.";
        }

        public override List<TraceStep> GetTrace(int[] array)
        {
            List<TraceStep> trace = new List<TraceStep>();
            int n = array.Length;
            int[] currentArray = (int[])array.Clone();

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    trace.Add(new TraceStep(currentArray, j, j + 1, StepType.Compare, $"Шаг {trace.Count + 1}: сравниваем {currentArray[j]} и {currentArray[j + 1]}"));
                    if (currentArray[j] > currentArray[j + 1])
                    {
                        int temp = currentArray[j];
                        currentArray[j] = currentArray[j + 1];
                        currentArray[j + 1] = temp;
                        trace.Add(new TraceStep(currentArray, j, j + 1, StepType.Swap, $"Шаг {trace.Count + 1}: меняем {currentArray[j + 1]} и {currentArray[j]} местами"));
                    }
                }
            }
            trace.Add(new TraceStep(currentArray, -1, -1, StepType.Done, "Сортировка завершена."));
            return trace;
        }
    }
}
