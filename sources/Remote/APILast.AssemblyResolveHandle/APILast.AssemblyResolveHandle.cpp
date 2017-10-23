#include "stdafx.h"
#include "APILast.AssemblyResolveHandle.h"


public ref class Program
{

	private : 
		static String^ _basePath;
		//System::Reflection::Assembly ^ OnAssemblyResolve(System::Object ^sender, System::ResolveEventArgs ^args);

		static System::Reflection::Assembly ^ OnAssemblyResolve(System::Object ^sender, System::ResolveEventArgs ^args)
		{
			auto nameParts = args->Name->Split(',');
			auto name = nameParts[0];
			//"D:\\Develop\\KleinZeug\\InterProcessCommunication\\ListProcess\\REST\\bin\\Debug\\"
			String ^fullPath = _basePath + name + ".dll";
			Console::WriteLine("Native Hello :" + fullPath);
			Console::WriteLine("Location: " + args->RequestingAssembly->Location);
			auto assembly = System::Reflection::Assembly::LoadFile(fullPath);
			Console::WriteLine("Native Goodbye Assembly didnt throw");

			return assembly;
		}

	public : 
	
		void Init(char *basePath)
		{
			_basePath = gcnew String(basePath);
			AppDomain::CurrentDomain->AssemblyResolve += gcnew System::ResolveEventHandler(&OnAssemblyResolve);
		};
};

extern "C" __declspec(dllexport) void __stdcall RegisterHandler(char *filePath)
{
	auto prog = gcnew Program;
	prog->Init(filePath);
};
