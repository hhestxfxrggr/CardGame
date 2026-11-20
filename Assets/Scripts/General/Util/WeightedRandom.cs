using System;
using System.Collections.Generic;

public class WeightedRandom<T>
{
    private readonly List<T> values = new();
    private readonly List<int> weights = new();

    private int[] prefix;
    private int totalWeight;
    private bool dirty = true;

    public int Count => values.Count;

    public void AddElement(int weight, T value)
    {
        if (weight < 0)
            throw new ArgumentOutOfRangeException(nameof(weight));

        if (weight == 0)
            return;

        if (weight > int.MaxValue - totalWeight)
            throw new OverflowException("가중치 합계가 int 범위를 초과했습니다.");

        values.Add(value);
        weights.Add(weight);

        totalWeight += weight;
        dirty = true;
    }

    public bool TryRandom(out T value)
    {
        if (values.Count == 0)
        {
            value = default;
            return false;
        }

        if (dirty)
            Build();

        int target = UnityEngine.Random.Range(0, totalWeight) + 1;

        int index = LowerBound(prefix, target);
        value = values[index];

        return true;
    }

    public T Random()
    {
        if (!TryRandom(out T value))
            throw new InvalidOperationException("추첨 가능한 항목이 없습니다.");

        return value;
    }

    private void Build()
    {
        prefix = new int[weights.Count];

        int sum = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            sum += weights[i];
            prefix[i] = sum;
        }

        dirty = false;
    }

    private static int LowerBound(int[] array, int target)
    {
        int low = 0;
        int high = array.Length;

        while (low < high)
        {
            int mid = low + ((high - low) >> 1);

            if (array[mid] >= target)
                high = mid;
            else
                low = mid + 1;
        }

        return low;
    }
}
