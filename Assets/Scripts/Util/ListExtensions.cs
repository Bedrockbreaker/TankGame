using System;
using System.Collections.Generic;

/**
 * <summary>
 * Custom extensions for lists
 * </summary>
 */
public static class ListExtensions {
	/**
	 * <summary>
	 * Selects a random item from the list
	 * </summary>
	 */
	public static T Random<T>(this List<T> list, bool useUnityRandom = true) {
		return useUnityRandom
			? list[UnityEngine.Random.Range(0, list.Count)]
			: list.Random(new Random());
	}

	/**
	 * <summary>
	 * Selects a random item from the list
	 * </summary>
	 */
	public static T Random<T>(this List<T> list, Random random) {
		return list[random.Next(0, list.Count)];
	}

	/**
	 * <summary>
	 * Selects a specified number of random unique items from the list without mutation.
	 * </summary>
	 */
	public static List<T> TakeRandom<T>(this List<T> list, int count, bool useUnityRandom = true) {
		if (count > list.Count) throw new ArgumentException(
			"Count cannot be greater than the number of items in the list.",
			nameof(count)
		);

		if (!useUnityRandom) return list.TakeRandom(count, new Random());

		List<T> temp = new(list);

		for (int i = 0; i < count; i++) {
			int j = UnityEngine.Random.Range(i, temp.Count);
			(temp[i], temp[j]) = (temp[j], temp[i]);
		}

		return temp.GetRange(0, count);
	}

	/**
	 * <summary>
	 * Selects a specified number of random unique items from the list without mutation.
	 * </summary>
	 */
	public static List<T> TakeRandom<T>(this List<T> list, int count, Random random) {
		if (count > list.Count) throw new ArgumentException(
			"Count cannot be greater than the number of items in the list.",
			nameof(count)
		);

		List<T> temp = new(list);

		for (int i = 0; i < count; i++) {
			int j = random.Next(i, temp.Count);
			(temp[i], temp[j]) = (temp[j], temp[i]);
		}

		return temp.GetRange(0, count);
	}
}