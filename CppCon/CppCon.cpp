// CppCon.cpp: 定义控制台应用程序的入口点。
//

#include "stdafx.h"

int sum(int a[], unsigned len) {
	int i, sum = 0;
	int len_1 = len - 1;
	for (i = 0;i <= len_1;i++) {
		sum += a[i];
	}
	return sum;
}

int main()
{
	/*int a[10];
	a[0] = 9;
	a[1] = 2;
	int temp = 0;
	temp= sum(a, 0);*/

	int x, y;
	x = 65535;
	y = x * x;

	return 0;
}

