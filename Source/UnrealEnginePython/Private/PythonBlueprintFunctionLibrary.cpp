

#include "PythonBlueprintFunctionLibrary.h"
#include "UEPyModule.h"

void UPythonBlueprintFunctionLibrary::ExecutePythonScript(FString script)
{
	FUnrealEnginePythonModule &PythonModule = FModuleManager::GetModuleChecked<FUnrealEnginePythonModule>("UnrealEnginePython");
	PythonModule.RunFile(TCHAR_TO_UTF8(*script));
}

void UPythonBlueprintFunctionLibrary::ExecutePythonString(const FString& PythonCmd)
{
	FUnrealEnginePythonModule &PythonModule = FModuleManager::GetModuleChecked<FUnrealEnginePythonModule>("UnrealEnginePython");
	PythonModule.RunString(TCHAR_TO_UTF8(*PythonCmd));
}

void UPythonBlueprintFunctionLibrary::CallSpecificFunctionWithArgs(const FString& Module, const FString& FunctionToCall, const TArray<FString>& FunctionArgs)
{
    if (FunctionToCall.IsEmpty() || Module.IsEmpty())
    {
        UE_LOG(LogPython, Error, TEXT("Empty function or module passed"), *FunctionToCall);
        return;
    }
    
    //PyObject *funcModule = ue_py_register_module(TCHAR_TO_UTF8(*Module));
    //if (!funcModule)
    //{
    //    unreal_engine_py_log_error();
    //    UE_LOG(LogPython, Error, TEXT("unable to load %s module"), *Module);
    //    // return the original string to avoid losing data
    //    return;
    //}

    FUnrealEnginePythonModule& PythonModule = FModuleManager::GetModuleChecked<FUnrealEnginePythonModule>("UnrealEnginePython");
    const FString importModule = FString::Printf(TEXT("import %s"), *Module);
    PythonModule.RunString(TCHAR_TO_UTF8(*importModule));
    const FString funcString = FString::Printf(TEXT("%s.%s(%s)"), *Module, *FunctionToCall, *FString::Join(FunctionArgs, TEXT(",")));
    PythonModule.RunString(TCHAR_TO_UTF8(*funcString));

    //PyObject *function_to_call = PyObject_GetAttrString(funcModule, TCHAR_TO_UTF8(*FunctionToCall));
    //if (!function_to_call)
    //{
    //    UE_LOG(LogPython, Error, TEXT("unable to find function %s"), *FunctionToCall);
    //    return;
    //}
    //
    //int n = FunctionArgs.Num();
    //PyObject *args = nullptr;
    //
    //if (n > 0)
    //    args = PyTuple_New(n);
    //
    //for (int i = 0; i < n; i++)
    //{
    //    PyTuple_SetItem(args, i, PyUnicode_FromString(TCHAR_TO_UTF8(*FunctionArgs[i])));
    //}
    //
    //PyObject *ret = PyObject_Call(function_to_call, args, nullptr);
    //Py_DECREF(args);
    //if (!ret)
    //{
    //    unreal_engine_py_log_error();
    //}
    //else
    //{
    //    Py_DECREF(ret);
    //}
}
