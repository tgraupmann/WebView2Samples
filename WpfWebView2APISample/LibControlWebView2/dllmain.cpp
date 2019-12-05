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

void SetupWebView2(HWND hWnd)
{
	IWebView2WebView* webviewWindow = nullptr;

	// Step 3 - Create a single WebView within the parent window
	// Locate the browser and set up the environment for WebView
	CreateWebView2EnvironmentWithDetails(nullptr, nullptr, nullptr,
		Callback<IWebView2CreateWebView2EnvironmentCompletedHandler>(
			[hWnd, &webviewWindow](HRESULT result, IWebView2Environment* env) -> HRESULT {

				// Create a WebView, whose parent is the main window hWnd
				env->CreateWebView(hWnd, Callback<IWebView2CreateWebViewCompletedHandler>(
					[hWnd, &webviewWindow](HRESULT result, IWebView2WebView* webview) -> HRESULT {
					if (webview != nullptr) {
						webviewWindow = webview;
					}

					// Add a few settings for the webview
					// this is a redundant demo step as they are the default settings values
					IWebView2Settings* Settings;
					webviewWindow->get_Settings(&Settings);
					Settings->put_IsScriptEnabled(TRUE);
					Settings->put_AreDefaultScriptDialogsEnabled(TRUE);
					Settings->put_IsWebMessageEnabled(TRUE);

					// Resize WebView to fit the bounds of the parent window
					RECT bounds;
					GetClientRect(hWnd, &bounds);
					webviewWindow->put_Bounds(bounds);

					// Schedule an async task to navigate to Bing
					webviewWindow->Navigate(L"https://www.bing.com/");

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

	EXPORT_API int PluginInit()
	{
		SetupWebView2(nullptr);
		return 123;
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

