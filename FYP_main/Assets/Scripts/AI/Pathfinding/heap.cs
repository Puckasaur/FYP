using UnityEngine;
using System.Collections;
using System;


public interface IHeapItem<T> : IComparable<T>
{
	int heapIndex 
	{
		get;
		set;
	}


}


public class heap<T> where T : IHeapItem<T>

{
	T[] items;
	int currentItemCount;


	public heap(int maxHeapSize)

	{
		items = new T[maxHeapSize];
	}


	public void add(T item)

	{
		item.heapIndex = currentItemCount;
		items[currentItemCount] = item;
		sortUp(item);
		currentItemCount++;
	}
	 
	public T removeFirst()
	{
	T firstItem = items[0];
	currentItemCount--;
	items[0] = items[currentItemCount];
	items[0].heapIndex = 0;
	sortDown(items[0]);
	return firstItem;
	}


	public bool contains(T item)
	{
		return Equals(items[item.heapIndex], item);
	}

	public void updateItem(T item)
	{
		sortUp (item);
	}

	public int count
	{
		get
		{
			return currentItemCount;
		}
	}

	void sortUp(T item)
	{
		int parentIndex = (item.heapIndex - 1)/2;
		
		while(true)
		{
			T parentItem = items[parentIndex];
			
			if(item.CompareTo(parentItem) > 0)
			{
				swap(item, parentItem);
			}
			else
			{
				break;
			}

			parentIndex = (item.heapIndex - 1)/2;
		}
		
	}

	void sortDown(T item)
	{
	while(true)
	{
		int childIndexLeft = item.heapIndex * 2 + 1;
		int childIndexRight = item.heapIndex * 2 + 2;
		int swapIndex = 0;

		if(childIndexLeft < currentItemCount)
		{
			swapIndex = childIndexLeft;

			if(childIndexRight < currentItemCount)
			{
				if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
				{
					swapIndex = childIndexRight;
				}
			}

			if(item.CompareTo(items[childIndexLeft]) < 0)
			{
				swap(item, items[swapIndex]);
			}
			else
			{
				return;
			}
		}
		else
		{
			return;
		}
	}
	}

	void swap(T itemA, T itemB)
	{
		items[itemA.heapIndex] = itemB;
		items[itemB.heapIndex] = itemA;
		int itemAIndex = itemA.heapIndex;
		itemA.heapIndex = itemB.heapIndex;
		itemB.heapIndex = itemAIndex;
	}
}

