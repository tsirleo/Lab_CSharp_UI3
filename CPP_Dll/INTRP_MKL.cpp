#include "pch.h"
#include "mkl.h"

extern "C"  _declspec(dllexport)
void InterpolateUni(MKL_INT nx, MKL_INT ny, double* x, double* y, double* bounds, double* derivBounds, double* scoeff, MKL_INT nsite, double* site, MKL_INT ndorder, MKL_INT * dorder, double* result, double* derives, double* left, double* right, double* integres, int& ret)
{
	try
	{
		int status;
		DFTaskPtr task;

		status = dfdNewTask1D(&task, nx, x, DF_UNIFORM_PARTITION, ny, y, DF_MATRIX_STORAGE_ROWS);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER, derivBounds, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nsite, site, DF_UNIFORM_PARTITION, ndorder, dorder, NULL, result, DF_MATRIX_STORAGE_ROWS, NULL);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, 2, bounds, DF_UNIFORM_PARTITION, 2, new int[2]{ 1, 1 }, NULL, derives, DF_MATRIX_STORAGE_ROWS, NULL);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdIntegrate1D(task, DF_METHOD_PP, 1, left, DF_UNIFORM_PARTITION, right, DF_UNIFORM_PARTITION, NULL, NULL, integres, DF_MATRIX_STORAGE_ROWS);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfDeleteTask(&task);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		ret = 0;
	}
	catch (...)
	{
		ret = -1;
	}
}

extern "C"  _declspec(dllexport)
void InterpolateNonUni(MKL_INT nx, MKL_INT ny, double* x, double* y, double* bounds, double* derivBounds, double* scoeff, MKL_INT nsite, double* site, MKL_INT ndorder, MKL_INT * dorder, double* result, double* derives, double* left, double* right, double* integres, int& ret)
{
	try
	{
		int status;
		DFTaskPtr task;

		status = dfdNewTask1D(&task, nx, x, DF_NON_UNIFORM_PARTITION, ny, y, DF_MATRIX_STORAGE_ROWS);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER, derivBounds, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nsite, site, DF_UNIFORM_PARTITION, ndorder, dorder, NULL, result, DF_MATRIX_STORAGE_ROWS, NULL);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, 2, bounds, DF_UNIFORM_PARTITION, 2, new int[2]{ 1, 1 }, NULL, derives, DF_MATRIX_STORAGE_ROWS, NULL);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfdIntegrate1D(task, DF_METHOD_PP, 1, left, DF_UNIFORM_PARTITION, right, DF_UNIFORM_PARTITION, NULL, NULL, integres, DF_MATRIX_STORAGE_ROWS);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		status = dfDeleteTask(&task);
		if (status != DF_STATUS_OK) { ret = -1; return; }

		ret = 0;
	}
	catch (...)
	{
		ret = -1;
	}
}