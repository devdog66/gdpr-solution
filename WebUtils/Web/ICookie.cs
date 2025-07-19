using System;

namespace WebUtils.Web
{
    public interface ICookie
    {
        //
        // Summary:
        //     Gets or sets the domain to associate the cookie with.
        //
        // Returns:
        //     The domain to associate the cookie with.
        string Domain { get; set; }
        //
        // Summary:
        //     Gets or sets the cookie path.
        //
        // Returns:
        //     The cookie path.
        string Path { get; set; }
        //
        // Summary:
        //     Gets or sets the expiration date and time for the cookie.
        //
        // Returns:
        //     The expiration date and time for the cookie.
        DateTimeOffset Expires { get; set; }
        //
        // Summary:
        //     Gets or sets a value that indicates whether to transmit the cookie using Secure
        //     Sockets Layer (SSL)--that is, over HTTPS only.
        //
        // Returns:
        //     true to transmit the cookie only over an SSL connection (HTTPS); otherwise, false.
        bool Secure { get; set; }
        //
        // Summary:
        //     Gets or sets the value for the SameSite attribute of the cookie. The default
        //     value is Microsoft.AspNetCore.Http.SameSiteMode.Unspecified
        //
        // Returns:
        //     The Microsoft.AspNetCore.Http.SameSiteMode representing the enforcement mode
        //     of the cookie.
        SameSiteEnum SameSite { get; set; }
        //
        // Summary:
        //     Gets or sets a value that indicates whether a cookie is inaccessible by client-side
        //     script.
        //
        // Returns:
        //     true if a cookie must not be accessible by client-side script; otherwise, false.
        bool HttpOnly { get; set; }
        string Value { get; }
        string Name { get; }
    }

    public enum SameSiteEnum
    {
        /// <summary>No SameSite field will be set, the client should follow its default cookie policy.</summary>
        Unspecified = -1,
        /// <summary>Indicates the client should disable same-site restrictions.</summary>
        None = 0,
        /// <summary>Indicates the client should send the cookie with "same-site" requests, and with "cross-site" top-level navigations.</summary>
        Lax,
        /// <summary>Indicates the client should only send the cookie with "same-site" requests.</summary>
        Strict
    }
}
