﻿using Microsoft.JSInterop;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services;

public class CookieStorage
{
    private readonly IJSRuntime _jsRuntime;

    public CookieStorage(IJSRuntime jsRuntime)
    {
        this._jsRuntime = jsRuntime;
    }

    private const string GetCookieJs =
        "(function(name){const reg = new RegExp(`(^| )${name}=([^;]*)(;|$)`);const arr = document.cookie.match(reg);if (arr) {return unescape(arr[2]);}return null;})";
    private const string SetCookieJs =
        "(function(name,value){ var Days = 30;var exp = new Date();exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);document.cookie = `${name}=${escape(value.toString())};path=/;expires=${exp.toUTCString()}`;})";

    public async Task<string> GetCookieAsync(string key)
        {
            return await this._jsRuntime.InvokeAsync<string>("eval", $"{GetCookieJs}('{key}')");
        }

        public string? GetCookie(string key)
        {
            if (this._jsRuntime is IJSInProcessRuntime jsInProcess)
            {
                return jsInProcess.Invoke<string>("eval", $"{GetCookieJs}('{key}')");
            }

            return null;
        }

    public async Task SetItemAsync<T>(string key, T? value)
    {
        try
        {
                await this._jsRuntime.InvokeVoidAsync("eval", $"{SetCookieJs}('{key}','{value}')");
        }
        catch
        {
        }
    }
}
