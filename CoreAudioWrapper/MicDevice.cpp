// #include "stdafx.h"
#include <windows.h>
#include <mmdeviceapi.h>
#include <endpointvolume.h>
#include <Functiondiscoverykeys_devpkey.h>

#define DLLExport __declspec(dllexport)

struct DevDetails {
    LPWSTR name;
    LPWSTR id;
};

extern "C"
{
    DLLExport extern DevDetails* GetMicrophoneList(int* devcount) {
        HRESULT hr;
        CoInitialize(NULL);

        // Get device enumerator
        IMMDeviceEnumerator* deviceEnumerator = NULL;
        hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID*)&deviceEnumerator);

        // Get collection of all active capture devices
        IMMDeviceCollection* pCollection = NULL;
        hr = deviceEnumerator->EnumAudioEndpoints(eCapture, DEVICE_STATE_ACTIVE, &pCollection);

        // Iterate over the collection to work with devices
        UINT count;
        hr = pCollection->GetCount(&count);

        // Create an array for device details needed in c# code
        DevDetails* array = new DevDetails[count];
        for (UINT i = 0; i < count; i++)
        {
            IMMDevice* pDevice = nullptr;
            hr = pCollection->Item(i, &pDevice);

            if (SUCCEEDED(hr))
            {
                // DeviceID abfragen
                LPWSTR pwszID = nullptr;
                hr = pDevice->GetId(&pwszID);
                if (SUCCEEDED(hr))
                {
                    array[i].id = _wcsdup(pwszID);
                    CoTaskMemFree(pwszID);
                }

                // FriendlyName abfragen
                IPropertyStore* pProperties = nullptr;
                hr = pDevice->OpenPropertyStore(STGM_READ, &pProperties);
                if (SUCCEEDED(hr))
                {
                    PROPVARIANT var;
                    PropVariantInit(&var);
                    hr = pProperties->GetValue(PKEY_Device_FriendlyName, &var);

                    if (SUCCEEDED(hr) && var.vt == VT_LPWSTR)
                    {
                        array[i].name = _wcsdup(var.pwszVal);
                    }

                    PropVariantClear(&var);
                    pProperties->Release();
                }

                pDevice->Release();
            }
        }

        // Ressourcen freigeben
        pCollection->Release();
        deviceEnumerator->Release();

        *devcount = count;
        return array;
        CoUninitialize();
    }

    DLLExport LPCWSTR GetDefaultDeviceID() {
        HRESULT hr;
        CoInitialize(NULL);

        // Get device enumerator
        IMMDeviceEnumerator* deviceEnumerator = NULL;
        hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID*)&deviceEnumerator);

        // Get collection of all active capture devices
        IMMDevice* pDevice = NULL;
        hr = deviceEnumerator->GetDefaultAudioEndpoint(eCapture, eMultimedia, &pDevice);
        deviceEnumerator->Release();

        // DeviceID abfragen
        LPWSTR pwszID = nullptr;
        hr = pDevice->GetId(&pwszID);
        pDevice->Release();
        return pwszID;
        CoUninitialize();
    }

    DLLExport bool GetMute(LPCWSTR micDeviceID) {
        HRESULT hr;
        CoInitialize(NULL);
        IMMDeviceEnumerator* deviceEnumerator = NULL;
        hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID*)&deviceEnumerator);
        IMMDevice* activeDevice = NULL;

        hr = deviceEnumerator->GetDevice(micDeviceID, &activeDevice);
        deviceEnumerator->Release();
        deviceEnumerator = NULL;
        if (activeDevice == NULL) {
            return NULL;
        }

        IAudioEndpointVolume* endpointVolume = NULL;
        hr = activeDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID*)&endpointVolume);
        activeDevice->Release();
        activeDevice = NULL;

        BOOL muteState = NULL;
        hr = endpointVolume->GetMute(&muteState);
        if (SUCCEEDED(hr)) {
            return muteState;
        }
        else {
            return false;
        }
        CoUninitialize();
    }

    DLLExport void SetMute(BOOL mute, LPCWSTR micDeviceID) {
        HRESULT hr;
        CoInitialize(NULL);
        IMMDeviceEnumerator* deviceEnumerator = NULL;
        hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID*)&deviceEnumerator);
        IMMDevice* activeDevice = NULL;

        hr = deviceEnumerator->GetDevice(micDeviceID, &activeDevice);
        deviceEnumerator->Release();
        deviceEnumerator = NULL;
        if (activeDevice == NULL) {
            return;
        }

        IAudioEndpointVolume* endpointVolume = NULL;
        hr = activeDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID*)&endpointVolume);
        activeDevice->Release();
        activeDevice = NULL;

        hr = endpointVolume->SetMute(mute, NULL);

        CoUninitialize();
    }
}