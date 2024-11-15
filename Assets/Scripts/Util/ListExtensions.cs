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
	public static T Random<T>(this List<T> list) {
		return list[new Random().Next(0, list.Count)];
	}

	/**
	 * <summary>
	 * Selects a specified number of random unique items from the list without mutation.
	 * </summary>
	 */
	public static List<T> TakeRandom<T>(this List<T> list, int count) {
		if (count > list.Count) throw new ArgumentException(
			"Count cannot be greater than the number of items in the list.",
			nameof(count)
		);

		List<T> temp = new(list);

		Random random = new();

		for (int i = 0; i < count; i++) {
			int j = random.Next(i, temp.Count);
			(temp[i], temp[j]) = (temp[j], temp[i]);
		}

		return temp.GetRange(0, count);
	}
}