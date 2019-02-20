
#include "PythonDelegate.h"
#include "UEPyModule.h"
#include "UEPyCallable.h"

UPythonDelegate::UPythonDelegate()
{
	py_callable = nullptr;
	signature_set = false;
}

void UPythonDelegate::SetPyCallable(PyObject *callable)
{
	// do not acquire the gil here as we set the callable in python call themselves
	py_callable = callable;
	Py_INCREF(py_callable);
}

void UPythonDelegate::SetSignature(UFunction *original_signature)
{
	signature = original_signature;
	signature_set = true;
}

void UPythonDelegate::ProcessEvent(UFunction *function, void *Parms)
{

	if (!py_callable)
		return;

	FScopePythonGIL gil;

	PyObject *py_args = nullptr;

	if (signature_set)
	{
		Py_ssize_t argn = 0;        
        for (TFieldIterator<UProperty> PArgs(signature); PArgs && argn < signature->NumParms && PArgs->HasAnyPropertyFlags(CPF_Parm); ++PArgs)
        { 
            if ((PArgs->PropertyFlags & (CPF_Parm|CPF_ReturnParm)) == CPF_Parm)
            { argn++; }
        }
        
		py_args = PyTuple_New(argn);
        argn = 0;
		for (TFieldIterator<UProperty> PArgs(signature); PArgs && argn < signature->NumParms && PArgs->HasAnyPropertyFlags(CPF_Parm); ++PArgs)
		{
            if ((PArgs->PropertyFlags & (CPF_Parm|CPF_ReturnParm)) == CPF_Parm)
            {
			    UProperty *prop = *PArgs;
			    PyObject *arg = ue_py_convert_property(prop, (uint8 *)Parms, 0);
			    if (!arg)
			    {
				    unreal_engine_py_log_error();
				    Py_DECREF(py_args);
				    return;
			    }
			    PyTuple_SetItem(py_args, argn, arg);
			    argn++;
            }
		}
	}

	PyObject *ret = PyObject_CallObject(py_callable, py_args);
	Py_XDECREF(py_args);
	if (!ret)
	{
		unreal_engine_py_log_error();
		return;
	}
	// currently useless as events do not return a value
	if (signature_set) {
		UProperty *return_property = signature->GetReturnProperty();
		if (return_property && signature->ReturnValueOffset != MAX_uint16) {
#if defined(UEPY_MEMORY_DEBUG)
            UE_LOG(LogPython, Warning, TEXT("FOUND RETURN VALUE"));
#endif
            if (ue_py_convert_pyobject(ret, return_property, (uint8 *)Parms, 0)) {
                // No need to copy value to stack result value
                //FMemory::Memcpy(RESULT_PARAM, Parms + signature->ReturnValueOffset, return_property->ElementSize);
            }
            else {
				UE_LOG(LogPython, Error, TEXT("Invalid return value type for delegate"));
			}
		}
	}
	Py_DECREF(ret);
}

void UPythonDelegate::PyFakeCallable()
{
}

void UPythonDelegate::PyInputHandler()
{
	FScopePythonGIL gil;
	PyObject *ret = PyObject_CallObject(py_callable, NULL);
	if (!ret)
	{
		unreal_engine_py_log_error();
		return;
	}
	Py_DECREF(ret);
}

void UPythonDelegate::PyInputAxisHandler(float value)
{
	FScopePythonGIL gil;
	PyObject *ret = PyObject_CallFunction(py_callable, (char *)"f", value);
	if (!ret)
	{
		unreal_engine_py_log_error();
		return;
	}
	Py_DECREF(ret);
}

bool UPythonDelegate::UsesPyCallable(PyObject *other)
{
    ue_PyCallable *other_callable = (ue_PyCallable*)other;
    ue_PyCallable *this_callable = (ue_PyCallable*)py_callable;
    return other_callable->u_function == this_callable->u_function && other_callable->u_target == this_callable->u_target;
}

UPythonDelegate::~UPythonDelegate()
{
	FScopePythonGIL gil;

	Py_XDECREF(py_callable);
#if defined(UEPY_MEMORY_DEBUG)
	UE_LOG(LogPython, Warning, TEXT("PythonDelegate %p callable XDECREF'ed"), this);
#endif
}
