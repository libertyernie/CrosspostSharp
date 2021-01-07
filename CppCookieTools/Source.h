#pragma once

#pragma comment(lib, "wininet.lib")

#define WIN32_LEAN_AND_MEAN

#include <windows.h>

#include <WinInet.h>
#include <Winineti.h>
#include <vcclr.h>

using System::DateTimeOffset;
using System::FlagsAttribute;
using System::String;
using System::Collections::Generic::IEnumerable;
using System::ComponentModel::Win32Exception;

namespace CppCookieTools {
    [Flags]
    public enum class SuppressBehavior {
        ResetAll = INTERNET_SUPPRESS_RESET_ALL,
        CookiePolicy = INTERNET_SUPPRESS_COOKIE_POLICY,
        CookiePolicyReset = INTERNET_SUPPRESS_COOKIE_POLICY_RESET,
        CookiePersist = INTERNET_SUPPRESS_COOKIE_PERSIST,
        CookiePersistReset = INTERNET_SUPPRESS_COOKIE_PERSIST_RESET,
    };

    [Flags]
    public enum class CookieFlags {
        IsSecure = INTERNET_COOKIE_IS_SECURE,
        IsSession = INTERNET_COOKIE_IS_SESSION,
        IsRestricted = INTERNET_COOKIE_IS_RESTRICTED,
        HttpOnly = INTERNET_COOKIE_HTTPONLY,
        HostOnly = INTERNET_COOKIE_HOST_ONLY,
        HostOnlyApplied = INTERNET_COOKIE_HOST_ONLY_APPLIED,
        SameSiteStrict = INTERNET_COOKIE_SAME_SITE_STRICT,
        SameSiteLax = INTERNET_COOKIE_SAME_SITE_LAX
    };

    public value struct Cookie {
        String^ Name;
        String^ Value;
        String^ Domain;
        String^ Path;
        CookieFlags Flags;
        DateTimeOffset Expires;
        bool ExpiresSet;
    };

    public ref struct Cookies abstract sealed {
        static IEnumerable<Cookie>^ GetCookies(String^ url) {
            INTERNET_COOKIE2* pCookies;
            DWORD dwCookieCount;

            pin_ptr<const wchar_t> pcwszUrl = PtrToStringChars(url);

            DWORD err = InternetGetCookieEx2(pcwszUrl, NULL, INTERNET_COOKIE_NON_SCRIPT, &pCookies, &dwCookieCount);

            if (err != ERROR_SUCCESS) {
                throw gcnew Win32Exception(err);
            }

            array<Cookie>^ arr = gcnew array<Cookie>(dwCookieCount);
            pin_ptr<Cookie> arr_ptr = &arr[0];

            for (DWORD i = 0; i < dwCookieCount; i++) {
                INTERNET_COOKIE2* from = pCookies + i;
                Cookie* to = arr_ptr + i;

                to->Name = gcnew String(from->pwszName);
                to->Value = gcnew String(from->pwszValue);
                to->Domain = gcnew String(from->pwszDomain);
                to->Path = gcnew String(from->pwszPath);

                to->Flags = (CookieFlags)from->dwFlags;

                ULARGE_INTEGER ticks{};
                ticks.LowPart = from->ftExpires.dwLowDateTime;
                ticks.HighPart = from->ftExpires.dwHighDateTime;
                to->Expires = DateTimeOffset::FromFileTime(ticks.QuadPart);

                to->ExpiresSet = from->fExpiresSet;
            }

            InternetFreeCookies(pCookies, dwCookieCount);
            return arr;
        }

        static bool SetSuppressBehaviorForProcess(SuppressBehavior value) {
            int option = (int)value;
            return InternetSetOption(NULL, INTERNET_OPTION_SUPPRESS_BEHAVIOR, &option, sizeof(int));
        }
    };
};