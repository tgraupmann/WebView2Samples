// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

#include <wrl.h>
#include <wil/com.h>
// include WebView2 header
#include "WebView2.h"

using namespace Microsoft::WRL;

#if _MSC_VER // this is defined when compiling with Visual Studio
#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this
#else
#define EXPORT_API // XCode does not need annotating exported functions, so define is empty
#endif

IWebView2WebView* g_WebviewWindow = nullptr;

void SetupWebView2(HWND hWnd)
{
	// Step 3 - Create a single WebView within the parent window
	// Locate the browser and set up the environment for WebView
	CreateWebView2EnvironmentWithDetails(nullptr, nullptr, nullptr,
		Callback<IWebView2CreateWebView2EnvironmentCompletedHandler>(
			[hWnd](HRESULT result, IWebView2Environment* env) -> HRESULT {

				// Create a WebView, whose parent is the main window hWnd
				env->CreateWebView(hWnd, Callback<IWebView2CreateWebViewCompletedHandler>(
					[hWnd](HRESULT result, IWebView2WebView* webview) -> HRESULT {
					if (webview != nullptr) {
						g_WebviewWindow = webview;
					}

					// Add a few settings for the webview
					// this is a redundant demo step as they are the default settings values
					IWebView2Settings* Settings;
					g_WebviewWindow->get_Settings(&Settings);
					Settings->put_IsScriptEnabled(TRUE);
					Settings->put_AreDefaultScriptDialogsEnabled(TRUE);
					Settings->put_IsWebMessageEnabled(TRUE);

					// Resize WebView to fit the bounds of the parent window
					RECT bounds;
					GetClientRect(hWnd, &bounds);
					g_WebviewWindow->put_Bounds(bounds);

					// Schedule an async task to navigate to Bing
					g_WebviewWindow->Navigate(L"https://www.bing.com/");

					// Step 4 - Navigation events


					// Step 5 - Scripting


					// Step 6 - Communication between host and web content


					return S_OK;
				}).Get());
			return S_OK;
		}).Get());
}

extern "C"
{

	EXPORT_API int PluginInit(HWND hWnd)
	{
		SetupWebView2(hWnd);
		return 123;
	}

	EXPORT_API HWND PluginGetControl()
	{
		return nullptr;
	}

	EXPORT_API HWND PluginGetHost()
	{
		return nullptr;
	}
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

