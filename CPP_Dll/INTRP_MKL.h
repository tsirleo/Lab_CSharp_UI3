#pragma once
#include "mkl.h"

extern "C"  _declspec(dllexport)
void InterpolateUni(MKL_INT nx, MKL_INT ny, double* x, double* y, double* bounds, double* derivBounds, double* scoeff, MKL_INT nsite, double* site, MKL_INT ndorder, MKL_INT * dorder, double* result, double* derives, double* left, double* right, double* integres, int& ret);
extern "C"  _declspec(dllexport)
void InterpolateNonUni(MKL_INT nx, MKL_INT ny, double* x, double* y, double* bounds, double* derivBounds, double* scoeff, MKL_INT nsite, double* site, MKL_INT ndorder, MKL_INT * dorder, double* result, double* derives, double* right, double* integres, int& ret)