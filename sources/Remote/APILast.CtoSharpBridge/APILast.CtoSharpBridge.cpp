#include "stdafx.h"

#include "APILast.CtoSharpBridge.h"
extern "C" __declspec(dllexport) void __stdcall InitAssembly(char *serviceHookData)
{
	auto str = gcnew String(serviceHookData);
	APILast::Adapter::Loader::DoorOpener::Inject(str);


	Console::WriteLine("InitAssembly");
	//cout << "FooBarzen" << Endline
	//InjectedDll::Program::EntryPoint("Welcome in Managed Context");
}