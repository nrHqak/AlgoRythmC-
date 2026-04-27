using System.Collections.Generic;
using System.Linq;

namespace AlgoRythmDesktop
{
    public class LinearSearch : Algorithm
    {
        public LinearSearch()
        {
            Name = "Linear Search";
            Complexity = "O(n)";
            Hint = "Linear Search последовательно проверяет каждый элемент массива, пока не найдет искомое значение или не достигнет конца массива.";
        }

        public override List<TraceStep> GetTrace(int[] array)
        {
            List<TraceStep> trace = new List<TraceStep>();
            int[] currentArray = (int[])array.Clone();

            for (int i = 0; i < currentArray.Length; i++)
            {
                trace.Add(new TraceStep(currentArray, i, -1, StepType.Compare, $"Шаг {trace.Count + 1}: сравниваем {currentArray[i]} с искомым значением {SearchValue}"));
                if (currentArray[i] == SearchValue)
                {
                    trace.Add(new TraceStep(currentArray, i, -1, StepType.Found, $"Шаг {trace.Count + 1}: найдено значение {SearchValue} по индексу {i}"));
                    break;
                }
            }
            if (!trace.Any(s => s.Type == StepType.Found))
            {
                trace.Add(new TraceStep(currentArray, -1, -1, StepType.Done, $"Шаг {trace.Count + 1}: значение {SearchValue} не найдено."));
            }
            else
            {
                trace.Add(new TraceStep(currentArray, -1, -1, StepType.Done, "Поиск завершен."));
            }
            return trace;
        }
    }
}
